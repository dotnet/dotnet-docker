// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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

        private string BuildTestAppImage(string stageTarget, string contextDir, params string[] customBuildArgs)
        {
            string tag = _imageData.GetIdentifier(stageTarget);

            List<string> buildArgs = new List<string>();
            buildArgs.Add($"sdk_image={_imageData.GetImage(DotNetImageType.SDK, _dockerHelper)}");

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
                _dockerHelper.Run(
                    image: _imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                    name: containerName,
                    command: $"dotnet new {appType} --framework netcoreapp{_imageData.Version} --no-restore",
                    workdir: "/app",
                    skipAutoCleanup: true);

                _dockerHelper.Copy($"{containerName}:/app", appDir);

                string sourceDockerfileName = $"Dockerfile.{DockerHelper.DockerOS.ToLower()}";

                // TODO: Remove Windows arm workaround once underlying Windows/Docker issue is resolved
                // https://github.com/dotnet/dotnet-docker/issues/1054
                if (!DockerHelper.IsLinuxContainerModeEnabled && _imageData.Arch == Arch.Arm)
                {
                    sourceDockerfileName += $".{Enum.GetName(typeof(Arch), _imageData.Arch).ToLowerInvariant()}";
                }

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

        public static async Task VerifyHttpResponseFromContainerAsync(string containerName, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        {
            var retries = 30;

            // Can't use localhost when running inside containers or Windows.
            var url = !Config.IsRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
                ? $"http://localhost:{dockerHelper.GetContainerHostPort(containerName)}"
                : $"http://{dockerHelper.GetContainerAddress(containerName)}";

            using (HttpClient client = new HttpClient())
            {
                while (retries > 0)
                {
                    retries--;
                    await Task.Delay(TimeSpan.FromSeconds(2));

                    try
                    {
                        using (HttpResponseMessage result = await client.GetAsync(url))
                        {
                            outputHelper.WriteLine($"HTTP {result.StatusCode}\n{(await result.Content.ReadAsStringAsync())}");
                            result.EnsureSuccessStatusCode();
                        }

                        return;
                    }
                    catch (Exception ex)
                    {
                        outputHelper.WriteLine($"Request to {url} failed - retrying: {ex.ToString()}");
                    }
                }
            }

            throw new TimeoutException($"Timed out attempting to access the endpoint {url} on container {containerName}");
        }
    }
}
