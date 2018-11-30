// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.DotNet.Docker.Tests.ImageVersion;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageTests
    {
        private static readonly string s_repoName = Environment.GetEnvironmentVariable("REPO") ?? GetManifestRepoName();
        private static readonly bool s_isHttpVerificationDisabled =
            Environment.GetEnvironmentVariable("DISABLE_HTTP_VERIFICATION") != null;
        private static readonly bool s_isLocalRun =
            Environment.GetEnvironmentVariable("LOCAL_RUN") != null;
        private static readonly bool s_isNightlyRepo = s_repoName.Contains("nightly");
        private static readonly bool s_isRunningInContainer =
            Environment.GetEnvironmentVariable("RUNNING_TESTS_IN_CONTAINER") != null;

        private static readonly ImageData[] s_linuxTestData =
        {
            new ImageData { Version = V1_0, OS = OS.Jessie,       Arch = Arch.Amd64,    SdkVersion = V1_1 },
            new ImageData { Version = V1_1, OS = OS.Jessie,       Arch = Arch.Amd64,    RuntimeDepsVersion = V1_0 },
            new ImageData { Version = V1_1, OS = OS.Stretch,      Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Alpine37,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.Alpine37,     Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Alpine38,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.Alpine38,     Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Alpine38,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.Alpine38,     Arch = Arch.Amd64,    IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Arm,      IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm,      IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Arm64,    IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm64,    IsWeb = true },
        };
        private static readonly ImageData[] s_windowsTestData =
        {
            new ImageData { Version = V1_0, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64,  SdkVersion = V1_1 },
            new ImageData { Version = V1_1, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1709,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1803,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.NanoServer1709,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.NanoServer1803,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_1, OS = OS.NanoServer1809,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1709,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1803,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.NanoServer1709,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.NanoServer1803,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1709,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1803,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.NanoServer1709,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.NanoServer1803,     Arch = Arch.Amd64,  IsWeb = true },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64,  IsWeb = true },
        };

        private readonly DockerHelper _dockerHelper;
        private readonly ITestOutputHelper _outputHelper;

        public ImageTests(ITestOutputHelper outputHelper)
        {
            _dockerHelper = new DockerHelper(outputHelper);
            _outputHelper = outputHelper;
        }

        public static IEnumerable<object[]> GetVerifyImagesData()
        {
            string archFilterPattern = GetFilterRegexPattern("IMAGE_ARCH_FILTER");
            string osFilterPattern = GetFilterRegexPattern("IMAGE_OS_FILTER");
            string versionFilterPattern = GetFilterRegexPattern("IMAGE_VERSION_FILTER");

            // Filter out test data that does not match the active architecture and version filters.
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxTestData : s_windowsTestData)
                .Where(imageData => archFilterPattern == null
                    || Regex.IsMatch(Enum.GetName(typeof(Arch), imageData.Arch), archFilterPattern, RegexOptions.IgnoreCase))
                .Where(imageData => osFilterPattern == null
                    || Regex.IsMatch(imageData.OS, osFilterPattern, RegexOptions.IgnoreCase))
                .Where(imageData => versionFilterPattern == null
                    || Regex.IsMatch(imageData.VersionString, versionFilterPattern, RegexOptions.IgnoreCase))
                .Select(imageData => new object[] { imageData });
        }

        private static string GetFilterRegexPattern(string filterEnvName)
        {
            string filter = Environment.GetEnvironmentVariable(filterEnvName);
            return filter != null ? $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$" : null;
        }

        [Theory]
        [MemberData(nameof(GetVerifyImagesData))]
        public async Task VerifyImages(ImageData imageData)
        {
            if (!DockerHelper.IsLinuxContainerModeEnabled && imageData.IsArm)
            {
                _outputHelper.WriteLine("Tests are blocked on https://github.com/dotnet/corefx/issues/33563");
                return;
            }
            else if (imageData.Version == V3_0 && imageData.OS == OS.StretchSlim)
            {
                _outputHelper.WriteLine("Intermittent compile failure");
                return;
            }

            string appSdkImage = GetIdentifier(imageData, "app-sdk");

            try
            {
                CreateTestAppWithSdkImage(imageData, appSdkImage);

                if (!imageData.HasNoSdk)
                {
                    VerifySdkImage_PackageCache(imageData);

                    // TODO: Skip running app in arm + web configuration to workaround https://github.com/dotnet/cli/issues/9162
                    if (!(imageData.IsArm && imageData.IsWeb))
                    {
                        await VerifySdkImage_RunApp(imageData, appSdkImage);
                    }
                }

                await VerifyRuntimeImage_FrameworkDependentApp(imageData, appSdkImage);

                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    await VerifyRuntimeDepsImage_SelfContainedApp(imageData, appSdkImage);
                }
            }
            finally
            {
                _dockerHelper.DeleteImage(appSdkImage);
            }
        }

        private void CreateTestAppWithSdkImage(ImageData imageData, string appSdkImage)
        {
            // dotnet new, restore, build a new app using the sdk image
            List<string> buildArgs = new List<string>();
            buildArgs.Add($"netcoreapp_version={imageData.VersionString}");

            if (s_isNightlyRepo)
            {
                string dotnetCoreKey = "dotnet-core";
                string dotnetCoreUrl = "https://dotnet.myget.org/F/dotnet-core/api/v3/index.json";
                string packageSourceValue;

                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    packageSourceValue = $"<add key=\\\"{dotnetCoreKey}\\\" value=\\\"{dotnetCoreUrl}\\\" />";
                }
                else
                {
                    packageSourceValue = $"^<add key=^\\\"{dotnetCoreKey}^\\\" value=^\\\"{dotnetCoreUrl}^\\\" /^>";
                }

                buildArgs.Add($"optional_package_sources=\" {packageSourceValue}\"");
            }

            AddOptionalRestoreArgs(imageData, buildArgs);

            if (imageData.SdkVersion.Major > 1)
            {
                buildArgs.Add("optional_new_args=--no-restore");
            }

            buildArgs.Add("template_name=" + GetTestTemplateName(imageData.IsWeb));

            _dockerHelper.Build(
                dockerfile: $"Dockerfile.{DockerHelper.DockerOS.ToLower()}.testapp",
                tag: appSdkImage,
                fromImage: GetImage(DotNetImageType.SDK, imageData),
                buildArgs: buildArgs.ToArray());
        }

        private async Task VerifySdkImage_RunApp(ImageData imageData, string appSdkImage)
        {
            try
            {
                // dotnet run the new app using the sdk image
                _dockerHelper.Run(
                    image: appSdkImage,
                    command: "dotnet run --no-launch-profile",
                    detach: imageData.IsWeb,
                    containerName: appSdkImage);

                if (imageData.IsWeb && !s_isHttpVerificationDisabled)
                {
                    await VerifyHttpResponseFromContainer(appSdkImage);
                }
            }
            finally
            {
                _dockerHelper.DeleteContainer(appSdkImage);
            }
        }

        private void VerifySdkImage_PackageCache(ImageData imageData)
        {
            string verifyCacheCommand = null;
            if (imageData.Version.Major == 1)
            {
                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    verifyCacheCommand = "test -d /root/.nuget/packages";
                }
                else
                {
                    verifyCacheCommand = "CMD /S /C PUSHD \"C:\\Users\\ContainerAdministrator\\.nuget\\packages\"";
                }
            }
            else if (imageData.Version.Major == 2)
            {
                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    verifyCacheCommand = "test -d /usr/share/dotnet/sdk/NuGetFallbackFolder";
                }
                else
                {
                    verifyCacheCommand = "CMD /S /C PUSHD \"C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder\"";
                }
            }
            // else imageData.DotNetVersion >= 3.0 doesn't include the NuGetFallbackFolder

            if (verifyCacheCommand != null)
            {
                // Simple check to verify the NuGet package cache was created
                _dockerHelper.Run(
                    image: GetImage(DotNetImageType.SDK, imageData),
                    command: verifyCacheCommand,
                    containerName: GetIdentifier(imageData, "PackageCache"));
            }
        }

        private async Task VerifyRuntimeImage_FrameworkDependentApp(ImageData imageData, string appSdkImage)
        {
            string frameworkDepAppId = GetIdentifier(imageData, "framework-dependent-app");
            bool isRunAsContainerAdministrator = !DockerHelper.IsLinuxContainerModeEnabled && imageData.OS != OS.NanoServerSac2016;
            string publishCmd = GetPublishArgs(imageData);

            try
            {
                // Publish the app to a Docker volume using the app's sdk image
                _dockerHelper.Run(
                    image: appSdkImage,
                    command: publishCmd,
                    containerName: frameworkDepAppId,
                    volumeName: frameworkDepAppId,
                    runAsContainerAdministrator: isRunAsContainerAdministrator);

                // Run the app in the Docker volume to verify the runtime image
                string runtimeImage = GetImage(
                    imageData.IsWeb ? DotNetImageType.AspNetCore_Runtime : DotNetImageType.Runtime, imageData);
                string appDllPath = _dockerHelper.GetContainerWorkPath("testApp.dll");
                _dockerHelper.Run(
                    image: runtimeImage,
                    command: $"dotnet {appDllPath}",
                    containerName: frameworkDepAppId,
                    volumeName: frameworkDepAppId,
                    detach: imageData.IsWeb,
                    runAsContainerAdministrator: isRunAsContainerAdministrator);

                if (imageData.IsWeb && !s_isHttpVerificationDisabled)
                {
                    await VerifyHttpResponseFromContainer(frameworkDepAppId);
                }
            }
            finally
            {
                _dockerHelper.DeleteContainer(frameworkDepAppId);
                _dockerHelper.DeleteVolume(frameworkDepAppId);
            }
        }

        private async Task VerifyRuntimeDepsImage_SelfContainedApp(ImageData imageData, string appSdkImage)
        {
            string selfContainedAppId = GetIdentifier(imageData, "self-contained-app");

            try
            {
                // Build a self-contained app
                List<string> buildArgs = new List<string>();
                buildArgs.Add($"rid={imageData.Rid}");
                AddOptionalRestoreArgs(imageData, buildArgs);

                _dockerHelper.Build(
                    dockerfile: "Dockerfile.linux.testapp.selfcontained",
                    tag: selfContainedAppId,
                    fromImage: appSdkImage,
                    buildArgs: buildArgs.ToArray());

                try
                {
                    // Publish the self-contained app to a Docker volume using the app's sdk image
                    string publishCmd = GetPublishArgs(imageData, imageData.Rid);
                    _dockerHelper.Run(
                        image: selfContainedAppId,
                        command: publishCmd,
                        containerName: selfContainedAppId,
                        volumeName: selfContainedAppId);

                    // Run the self-contained app in the Docker volume to verify the runtime-deps image
                    string runtimeDepsImage = GetImage(DotNetImageType.Runtime_Deps, imageData);
                    string appExePath = _dockerHelper.GetContainerWorkPath("testApp");
                    _dockerHelper.Run(
                        image: runtimeDepsImage,
                        command: appExePath,
                        containerName: selfContainedAppId,
                        detach: imageData.IsWeb,
                        volumeName: selfContainedAppId);

                    if (imageData.IsWeb && !s_isHttpVerificationDisabled)
                    {
                        await VerifyHttpResponseFromContainer(selfContainedAppId);
                    }
                }
                finally
                {
                    _dockerHelper.DeleteContainer(selfContainedAppId);
                    _dockerHelper.DeleteVolume(selfContainedAppId);
                }
            }
            finally
            {
                _dockerHelper.DeleteImage(selfContainedAppId);
            }
        }

        private async Task VerifyHttpResponseFromContainer(string containerName)
        {
            var retries = 30;
            var url = !s_isRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
                ? $"http://localhost:{_dockerHelper.GetContainerHostPort(containerName)}"
                : $"http://{_dockerHelper.GetContainerAddress(containerName)}";

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
                            _outputHelper.WriteLine($"HTTP {result.StatusCode}\n{(await result.Content.ReadAsStringAsync())}");
                            result.EnsureSuccessStatusCode();
                        }

                        return;
                    }
                    catch (Exception ex)
                    {
                        _outputHelper.WriteLine($"Request to {url} failed - retrying: {ex.ToString()}");
                    }
                }
            }

            throw new TimeoutException($"Timed out attempting to access the endpoint {url} on container {containerName}");
        }

        private static string GetTestTemplateName(bool isWeb)
        {
            return isWeb ? "web" : "console";
        }

        private static void AddOptionalRestoreArgs(ImageData imageData, List<string> buildArgs)
        {
            if (imageData.Version == V1_1)
            {
                buildArgs.Add($"optional_restore_args=\"/p:RuntimeFrameworkVersion=1.1.*\"");
            }
        }

        public string GetImage(DotNetImageType imageType, ImageData imageData)
        {
            string imageName = $"{s_repoName}:{imageData.GetTag(imageType)}";

            if (s_isLocalRun)
            {
                Assert.True(DockerHelper.ImageExists(imageName), $"`{imageName}` could not be found on disk.");
            }
            else
            {
                _dockerHelper.Pull(imageName);
            }

            return imageName;
        }

        private static string GetIdentifier(ImageData imageData, string type)
        {
            return $"{imageData.VersionString}-{type}-{DateTime.Now.ToFileTime()}";
        }

        private static string GetPublishArgs(ImageData imageData, string rid = null)
        {
            string optionalArgs = string.IsNullOrEmpty(rid) ? "" : $" -r {rid}";
            return $"dotnet publish -c Release -o {DockerHelper.ContainerWorkDir}{optionalArgs}";
        }

        private static string GetManifestRepoName()
        {
            string manifestJson = File.ReadAllText("manifest.json");
            JObject manifest = JObject.Parse(manifestJson);
            return (string)manifest["repos"][0]["name"];
        }
    }
}
