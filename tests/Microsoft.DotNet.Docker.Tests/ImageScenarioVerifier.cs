// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageScenarioVerifier
    {
        private readonly DockerHelper _dockerHelper;
        private readonly ProductImageData _imageData;
        private readonly bool _isWeb;
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _adminUser = DockerHelper.IsLinuxContainerModeEnabled ? "root" : "ContainerAdministrator";
        private readonly string _nonRootUser = DockerHelper.IsLinuxContainerModeEnabled ? "app" : "ContainerUser";
        private readonly bool _nonRootUserSupported;
        private readonly bool _isAot;

        public ImageScenarioVerifier(
            ProductImageData imageData,
            DockerHelper dockerHelper,
            ITestOutputHelper outputHelper,
            bool isWeb = false)
        {
            _dockerHelper = dockerHelper;
            _imageData = imageData;
            _isWeb = isWeb;
            _outputHelper = outputHelper;
            _nonRootUserSupported = DockerHelper.IsLinuxContainerModeEnabled && _imageData.Version.Major > 7;
            _isAot = _imageData.ImageVariant.HasFlag(DotNetImageVariant.AOT);
        }

        public async Task Execute()
        {
            string appType = _isAot && _isWeb
                ? "webapiaot"
                : _isWeb
                    ? "web"
                    : "console";

            string solutionDir = CreateTestSolutionWithSdkImage(appType);
            List<string> tags = new List<string>();
            InjectCustomTestCode(Path.Combine(solutionDir, "app"));

            try
            {
                // Need to include the RID for all build stages because they all rely on "dotnet restore". We should
                // always provide RID when running restore because it's RID-dependent. If we don't then a call to the
                // publish command with a different RID than the default would end up restoring images. This is not
                // what we'd want and plus it would fail in that case if it was targeting a private NuGet feed because
                // the password isn't necessarily provided in that stage.
                string customBuildArgs = $"rid={_imageData.Rid}";

                string buildStageTarget = _isAot ? "publish_aot" : "build";
                string appStageTarget = _isAot ? "aot_app" : "self_contained_app";

                if (!_imageData.HasCustomSdk)
                {
                    // Use `sdk` image to build and run test app
                    string buildTag = BuildTestAppImage(buildStageTarget, solutionDir, customBuildArgs);
                    tags.Add(buildTag);
                    string dotnetRunArgs = _isWeb && _imageData.Version.Major <= 7 ? $" --urls http://0.0.0.0:{_imageData.DefaultPort}" : string.Empty;
                    await RunTestAppImage(buildTag, command: $"dotnet run ${dotnetRunArgs}");
                }

                // Running a scenario of unit testing within the sdk container is identical between a console app and web app,
                // so we only want to execute it for one of those app types.
                if (!_isWeb)
                {
                    string unitTestTag = BuildTestAppImage("test", solutionDir, customBuildArgs);
                    tags.Add(unitTestTag);
                    await RunTestAppImage(unitTestTag);
                }

                // AOT compiled apps don't need the .NET runtime.
                if (!_isAot)
                {
                    // Use `sdk` image to publish FX dependent app and run with `runtime` or `aspnet` image
                    string fxDepTag = BuildTestAppImage("fx_dependent_app", solutionDir, customBuildArgs);
                    tags.Add(fxDepTag);
                    // If we're a web app on Windows, use the ContainerAdministrator account
                    string fxDepUser = (_isWeb && !DockerHelper.IsLinuxContainerModeEnabled) ? _adminUser : null;
                    await RunTestAppImage(fxDepTag, user: fxDepUser);

                    // For distroless, run another test that explicitly runs the container as a root user to verify
                    // the root user is defined.
                    if (!_isWeb && DockerHelper.IsLinuxContainerModeEnabled && _imageData.IsDistroless &&
                        (!_imageData.OS.StartsWith(OS.Mariner) || _imageData.Version.Major > 6))
                    {
                        await RunTestAppImage(fxDepTag, user: _adminUser);
                    }
                    // For non-distroless, which uses the root user by default, run the test as the non-root user
                    else if (_nonRootUserSupported && !_imageData.IsDistroless)
                    {
                        await RunTestAppImage(fxDepTag, user: _nonRootUser);
                    }
                }

                // There is no point in testing composite images here because there are no composite-specific
                // runtime-deps or SDK images
                if (DockerHelper.IsLinuxContainerModeEnabled && !_imageData.ImageVariant.HasFlag(DotNetImageVariant.Composite))
                {
                    // Use `sdk` image to publish self contained app and run with `runtime-deps` image
                    string selfContainedTag = BuildTestAppImage(appStageTarget, solutionDir, customBuildArgs);
                    tags.Add(selfContainedTag);

                    await RunTestAppImage(selfContainedTag, user: _adminUser);

                    if (_nonRootUserSupported)
                    {
                        await RunTestAppImage(selfContainedTag, user: _nonRootUser);
                    }
                }
            }
            finally
            {
                tags.ForEach(tag => _dockerHelper.DeleteImage(tag));
                Directory.Delete(solutionDir, true);
            }
        }

        private void InjectCustomTestCode(string appDir)
        {
            string programFilePath = Path.Combine(appDir, "Program.cs");

            SyntaxTree programTree = CSharpSyntaxTree.ParseText(File.ReadAllText(programFilePath));

            string newContent;

            MethodDeclarationSyntax mainMethod = programTree.GetRoot().DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(method => method.Identifier.ValueText == "Main");

            if (mainMethod is null)
            {
                // Handles project templates that use top-level statements instead of a Main method
                IEnumerable<SyntaxNode> nodes = programTree.GetRoot().ChildNodes();

                IEnumerable<UsingDirectiveSyntax> usingDirectives = nodes.OfType<UsingDirectiveSyntax>();

                IEnumerable<SyntaxNode> otherNodes = nodes.Except(usingDirectives);

                StringBuilder builder = new();
                foreach (UsingDirectiveSyntax usingDir in usingDirectives)
                {
                    builder.Append(usingDir.ToFullString());
                }

                // Verify a web request succeeds
                builder.AppendLine("System.Console.WriteLine(\"Verifying a web request succeeds\");");
                builder.AppendLine("var response = await new System.Net.Http.HttpClient().GetAsync(\"https://www.microsoft.com\");");
                builder.AppendLine("response.EnsureSuccessStatusCode();");

                // Verify write access is allowed to the user directory
                builder.AppendLine(GetUserDirectoryWriteAccessValidationCode());

                foreach (SyntaxNode otherNode in otherNodes)
                {
                    builder.Append(otherNode.ToFullString());
                }

                newContent = builder.ToString();
            }
            else
            {
                StatementSyntax testHttpsConnectivityStatement = SyntaxFactory.ParseStatement(
                    // Verify a web request succeeds
                    "System.Console.WriteLine(\"Verifying a web request succeeds\");" +
                    "var task = new System.Net.Http.HttpClient().GetAsync(\"https://www.microsoft.com\");" +
                    "task.Wait();" +
                    "task.Result.EnsureSuccessStatusCode();" +
                    // Verify write access is allowed to the user directory
                    GetUserDirectoryWriteAccessValidationCode());

                MethodDeclarationSyntax newMainMethod = mainMethod.InsertNodesBefore(
                    mainMethod.Body.ChildNodes().First(),
                    new SyntaxNode[] { testHttpsConnectivityStatement });

                SyntaxNode newRoot = programTree.GetRoot().ReplaceNode(mainMethod, newMainMethod);
                newContent = newRoot.ToFullString();
            }

            File.WriteAllText(programFilePath, newContent);
        }

        private string GetUserDirectoryWriteAccessValidationCode()
        {
            if (_imageData.IsDistroless && _imageData.Version.Major == 6 && _imageData.OS.StartsWith(OS.Mariner))
            {
                return string.Empty;
            }

            string userDirEnvVarName = DockerHelper.IsLinuxContainerModeEnabled ? "HOME" : "USERPROFILE";
            return
                "System.Console.WriteLine(\"Verifying write access to user directory\");" +
                $"System.IO.File.WriteAllText(System.Environment.GetEnvironmentVariable(\"{userDirEnvVarName}\") + \"/test.txt\", \"test\");";
        }

        private string BuildTestAppImage(string stageTarget, string contextDir, params string[] customBuildArgs)
        {
            string tag = _imageData.GetIdentifier(stageTarget);

            DotNetImageRepo runtimeImageRepo = _isAot
                ? DotNetImageRepo.Runtime_Deps
                : _isWeb
                    ? DotNetImageRepo.Aspnet
                    : DotNetImageRepo.Runtime;

            List<string> buildArgs = new()
            {
                $"sdk_image={_imageData.GetImage(DotNetImageRepo.SDK, _dockerHelper)}",
                $"runtime_image={_imageData.GetImage(runtimeImageRepo, _dockerHelper)}",
                $"port={_imageData.DefaultPort}"
            };

            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                buildArgs.Add($"runtime_deps_image={_imageData.GetImage(DotNetImageRepo.Runtime_Deps, _dockerHelper)}");
            }

            if (customBuildArgs != null)
            {
                buildArgs.AddRange(customBuildArgs);
            }

            const string NuGetFeedPasswordVar = "NuGetFeedPassword";

            if (!string.IsNullOrEmpty(Config.NuGetFeedPassword))
            {
                buildArgs.Add(NuGetFeedPasswordVar);
                Environment.SetEnvironmentVariable(NuGetFeedPasswordVar, Config.NuGetFeedPassword);
            }

            try
            {
                _dockerHelper.Build(
                    tag: tag,
                    target: stageTarget,
                    contextDir: contextDir,
                    platform: _imageData.Platform,
                    buildArgs: buildArgs.ToArray());
            }
            finally
            {
                if (!string.IsNullOrEmpty(Config.NuGetFeedPassword))
                {
                    Environment.SetEnvironmentVariable(NuGetFeedPasswordVar, null);
                }
            }

            return tag;
        }

        private string CreateTestSolutionWithSdkImage(string appType)
        {
            string solutionDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), $"{appType}App{DateTime.Now.ToFileTime()}")).FullName;
            string appProjectContainerName = _imageData.GetIdentifier($"create-{appType}");
            string testProjectContainerName = _imageData.GetIdentifier("create-test");

            try
            {
                CreateProjectWithSdkImage(appType, Path.Combine(solutionDir, "app"), appProjectContainerName);

                string testDirectory = Path.Combine(solutionDir, "tests");
                CreateProjectWithSdkImage("xunit", testDirectory, testProjectContainerName);
                File.Copy(Path.Combine(DockerHelper.TestArtifactsDir, "UnitTests.cs"), Path.Combine(testDirectory, "UnitTests.cs"));

                string sourceDockerfileName = $"Dockerfile.{DockerHelper.DockerOS.ToLower()}";

                File.Copy(
                    Path.Combine(DockerHelper.TestArtifactsDir, sourceDockerfileName),
                    Path.Combine(solutionDir, "Dockerfile"));

                string nuGetConfigFileName = "NuGet.config";
                if (Config.IsNightlyRepo)
                {
                    nuGetConfigFileName += ".nightly";
                }

                File.Copy(Path.Combine(DockerHelper.TestArtifactsDir, nuGetConfigFileName), Path.Combine(solutionDir, "NuGet.config"));
                File.Copy(Path.Combine(DockerHelper.TestArtifactsDir, ".dockerignore"), Path.Combine(solutionDir, ".dockerignore"));
            }
            catch (Exception)
            {
                if (Directory.Exists(solutionDir))
                {
                    Directory.Delete(solutionDir, true);
                }

                throw;
            }
            finally
            {
                _dockerHelper.DeleteContainer(appProjectContainerName);
                _dockerHelper.DeleteContainer(testProjectContainerName);
            }

            return solutionDir;
        }

        private void CreateProjectWithSdkImage(string templateName, string destinationPath, string containerName)
        {
            IEnumerable<string> args = new List<string>
            {
                templateName,
                $"--framework net{_imageData.Version}",
                "--no-restore"
            };

            if (templateName.Contains("web"))
            {
                args = args.Append("--exclude-launch-settings");
            }

            const string ProjectContainerDir = "/app";

            _dockerHelper.Run(
                image: _imageData.GetImage(DotNetImageRepo.SDK, _dockerHelper),
                name: containerName,
                command: $"dotnet new {String.Join(' ', args)}",
                workdir: ProjectContainerDir,
                optionalRunArgs: $"--platform {_imageData.Platform}",
                skipAutoCleanup: true);

            _dockerHelper.Copy($"{containerName}:{ProjectContainerDir}", destinationPath);
        }

        private async Task RunTestAppImage(string image, string user = null, string command = null)
        {
            string containerName = _imageData.GetIdentifier("app-run");

            try
            {
                _dockerHelper.Run(
                    image: image,
                    name: containerName,
                    detach: _isWeb,
                    optionalRunArgs: _isWeb ? $"-p {_imageData.DefaultPort}" : string.Empty,
                    runAsUser: user,
                    command: command);

                if (_isWeb && !Config.IsHttpVerificationDisabled)
                {
                    await VerifyHttpResponseFromContainerAsync(
                        containerName,
                        _dockerHelper,
                        _outputHelper,
                        _imageData.DefaultPort,
                        pathAndQuery: _isAot ? "todos" : null);
                }
            }
            finally
            {
                _dockerHelper.DeleteContainer(containerName);
            }
        }

        public static async Task<HttpResponseMessage> GetHttpResponseFromContainerAsync(string containerName, DockerHelper dockerHelper, ITestOutputHelper outputHelper, int containerPort, string pathAndQuery = null, Action<HttpResponseMessage> validateCallback = null, AuthenticationHeaderValue authorizationHeader = null)
        {
            int retries = 32;

            // Can't use localhost when running inside containers or Windows.
            string url = !Config.IsRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
                ? $"http://localhost:{dockerHelper.GetContainerHostPort(containerName, containerPort)}/{pathAndQuery}"
                : $"http://{dockerHelper.GetContainerAddress(containerName)}:{containerPort}/{pathAndQuery}";

            using (HttpClient client = new HttpClient())
            {
                if (null != authorizationHeader)
                {
                    client.DefaultRequestHeaders.Authorization = authorizationHeader;
                }

                while (retries > 0)
                {
                    retries--;
                    await Task.Delay(TimeSpan.FromSeconds(2));

                    HttpResponseMessage result = null;
                    try
                    {
                        result = await client.GetAsync(url);
                        outputHelper.WriteLine($"HTTP {result.StatusCode}\n{(await result.Content.ReadAsStringAsync())}");

                        if (null == validateCallback)
                        {
                            result.EnsureSuccessStatusCode();
                        }
                        else
                        {
                            validateCallback(result);
                        }

                        // Store response in local that will not be disposed
                        HttpResponseMessage returnResult = result;
                        result = null;
                        return returnResult;
                    }
                    catch (Exception ex)
                    {
                        outputHelper.WriteLine($"Request to {url} failed - retrying: {ex}");
                    }
                    finally
                    {
                        result?.Dispose();
                    }
                }
            }

            throw new TimeoutException($"Timed out attempting to access the endpoint {url} on container {containerName}");
        }

        public static async Task VerifyHttpResponseFromContainerAsync(string containerName, DockerHelper dockerHelper, ITestOutputHelper outputHelper, int containerPort, string pathAndQuery = null, Action<HttpResponseMessage> validateCallback = null)
        {
            (await GetHttpResponseFromContainerAsync(
                containerName,
                dockerHelper,
                outputHelper,
                containerPort,
                pathAndQuery,
                validateCallback)).Dispose();
        }
    }
}
