// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private readonly string _testArtifactsDir = Path.Combine(Directory.GetCurrentDirectory(), "TestAppArtifacts");

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
        }

        public async Task Execute()
        {
            string appDir = CreateTestAppWithSdkImage(_isWeb ? "web" : "console");
            List<string> tags = new List<string>();
            InjectCustomTestCode(appDir);

            try
            {
                if (!_imageData.HasCustomSdk)
                {
                    // Use `sdk` image to build and run test app
                    string buildTag = BuildTestAppImage("build", appDir);
                    tags.Add(buildTag);
                    string dotnetRunArgs = _isWeb ? " --urls http://0.0.0.0:80" : string.Empty;
                    await RunTestAppImage(buildTag, command: $"dotnet run{dotnetRunArgs}");
                }

                // Use `sdk` image to publish FX dependent app and run with `runtime` or `aspnet` image
                string fxDepTag = BuildTestAppImage("fx_dependent_app", appDir);
                tags.Add(fxDepTag);
                bool runAsAdmin = _isWeb && !DockerHelper.IsLinuxContainerModeEnabled;
                await RunTestAppImage(fxDepTag, runAsAdmin: runAsAdmin);

                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    // Use `sdk` image to publish self contained app and run with `runtime-deps` image
                    string selfContainedTag = BuildTestAppImage("self_contained_app", appDir, customBuildArgs: $"rid={_imageData.Rid}");
                    tags.Add(selfContainedTag);
                    await RunTestAppImage(selfContainedTag, runAsAdmin: runAsAdmin);
                }
            }
            finally
            {
                tags.ForEach(tag => _dockerHelper.DeleteImage(tag));
                Directory.Delete(appDir, true);
            }
        }

        private static void InjectCustomTestCode(string appDir)
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

                builder.AppendLine("var response = await new System.Net.Http.HttpClient().GetAsync(\"https://www.microsoft.com\");");
                builder.AppendLine("response.EnsureSuccessStatusCode();");

                foreach (SyntaxNode otherNode in otherNodes)
                {
                    builder.Append(otherNode.ToFullString());
                }

                newContent = builder.ToString();
            }
            else
            {
                StatementSyntax testHttpsConnectivityStatement = SyntaxFactory.ParseStatement(
                    "var task = new System.Net.Http.HttpClient().GetAsync(\"https://www.microsoft.com\");" +
                    "task.Wait();" +
                    "task.Result.EnsureSuccessStatusCode();");

                MethodDeclarationSyntax newMainMethod = mainMethod.InsertNodesBefore(
                    mainMethod.Body.ChildNodes().First(),
                    new SyntaxNode[] { testHttpsConnectivityStatement });

                SyntaxNode newRoot = programTree.GetRoot().ReplaceNode(mainMethod, newMainMethod);
                newContent = newRoot.ToFullString();
            }
            
            File.WriteAllText(programFilePath, newContent);
        }

        private string BuildTestAppImage(string stageTarget, string contextDir, params string[] customBuildArgs)
        {
            string tag = _imageData.GetIdentifier(stageTarget);

            List<string> buildArgs = new List<string>
            {
                $"sdk_image={_imageData.GetImage(DotNetImageType.SDK, _dockerHelper)}"
            };

            DotNetImageType runtimeImageType = _isWeb ? DotNetImageType.Aspnet : DotNetImageType.Runtime;
            buildArgs.Add($"runtime_image={_imageData.GetImage(runtimeImageType, _dockerHelper)}");

            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                buildArgs.Add($"runtime_deps_image={_imageData.GetImage(DotNetImageType.Runtime_Deps, _dockerHelper)}");
            }

            if (customBuildArgs != null)
            {
                buildArgs.AddRange(customBuildArgs);
            }

            _dockerHelper.Build(
                tag: tag,
                target: stageTarget,
                contextDir: contextDir,
                buildArgs: buildArgs.ToArray());

            return tag;
        }

        private string CreateTestAppWithSdkImage(string appType)
        {
            string appDir = Path.Combine(Directory.GetCurrentDirectory(), $"{appType}App{DateTime.Now.ToFileTime()}");
            string containerName = _imageData.GetIdentifier($"create-{appType}");

            try
            {
                string targetFramework;
                if (_imageData.Version.Major < 5)
                {
                    targetFramework = $"netcoreapp{_imageData.Version}";
                }
                else
                {
                    targetFramework = $"net{_imageData.Version}";
                }

                _dockerHelper.Run(
                    image: _imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                    name: containerName,
                    command: $"dotnet new {appType} --framework {targetFramework} --no-restore",
                    workdir: "/app",
                    skipAutoCleanup: true);

                _dockerHelper.Copy($"{containerName}:/app", appDir);

                string sourceDockerfileName = $"Dockerfile.{DockerHelper.DockerOS.ToLower()}";

                File.Copy(
                    Path.Combine(_testArtifactsDir, sourceDockerfileName),
                    Path.Combine(appDir, "Dockerfile"));

                string nuGetConfigFileName = "NuGet.config";
                if (Config.IsNightlyRepo)
                {
                    nuGetConfigFileName += ".nightly";
                }

                File.Copy(Path.Combine(_testArtifactsDir, nuGetConfigFileName), Path.Combine(appDir, "NuGet.config"));
                File.Copy(Path.Combine(_testArtifactsDir, ".dockerignore"), Path.Combine(appDir, ".dockerignore"));
            }
            catch (Exception)
            {
                if (Directory.Exists(appDir))
                {
                    Directory.Delete(appDir, true);
                }

                throw;
            }
            finally
            {
                _dockerHelper.DeleteContainer(containerName);
            }

            return appDir;
        }

        private async Task RunTestAppImage(string image, bool runAsAdmin = false, string command = null)
        {
            string containerName = _imageData.GetIdentifier("app-run");

            try
            {
                _dockerHelper.Run(
                    image: image,
                    name: containerName,
                    detach: _isWeb,
                    optionalRunArgs: _isWeb ? "-p 80" : string.Empty,
                    runAsContainerAdministrator: runAsAdmin,
                    command: command);

                if (_isWeb && !Config.IsHttpVerificationDisabled)
                {
                    await VerifyHttpResponseFromContainerAsync(containerName, _dockerHelper, _outputHelper);
                }
            }
            finally
            {
                _dockerHelper.DeleteContainer(containerName);
            }
        }

        public static async Task<HttpResponseMessage> GetHttpResponseFromContainerAsync(string containerName, DockerHelper dockerHelper, ITestOutputHelper outputHelper, int containerPort = 80, string pathAndQuery = null, Action<HttpResponseMessage> validateCallback = null)
        {
            int retries = 30;

            // Can't use localhost when running inside containers or Windows.
            string url = !Config.IsRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
                ? $"http://localhost:{dockerHelper.GetContainerHostPort(containerName, containerPort)}/{pathAndQuery}"
                : $"http://{dockerHelper.GetContainerAddress(containerName)}:{containerPort}/{pathAndQuery}";

            using (HttpClient client = new HttpClient())
            {
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

        public static async Task VerifyHttpResponseFromContainerAsync(string containerName, DockerHelper dockerHelper, ITestOutputHelper outputHelper, int containerPort = 80, string pathAndQuery = null)
        {
            (await GetHttpResponseFromContainerAsync(
                containerName,
                dockerHelper,
                outputHelper,
                containerPort,
                pathAndQuery)).Dispose();
        }
    }
}
