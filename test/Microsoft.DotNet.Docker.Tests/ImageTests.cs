// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageTests
    {
        private static readonly string s_repoName = GetRepoName();
        private static readonly bool s_isNightlyRepo = s_repoName.EndsWith("nightly");
        private static readonly bool s_isRunningInContainer =
            Environment.GetEnvironmentVariable("RUNNING_TESTS_IN_CONTAINER") != null;
        private static readonly string s_repoOwner = Environment.GetEnvironmentVariable("REPO_OWNER") ?? "microsoft";

        private static readonly ImageData[] s_linuxTestData =
        {
            new ImageData { DotNetVersion = "1.0", SdkVersion = "1.1" },
            new ImageData { DotNetVersion = "1.1", RuntimeDepsVersion = "1.0" },
            new ImageData { DotNetVersion = "2.0" },
            new ImageData { DotNetVersion = "2.0", OsVariant = OS.Jessie },
            new ImageData { DotNetVersion = "2.1" },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.Bionic },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.Alpine },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.StretchSlim, SdkOsVariant = OS.Stretch, Architecture = "arm" },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.Bionic, Architecture = "arm" },
            new ImageData { DotNetVersion = "2.1", IsWeb = true },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.Bionic, IsWeb = true },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.Alpine, IsWeb = true },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.StretchSlim, SdkOsVariant = OS.Stretch, Architecture = "arm", IsWeb = true },
            new ImageData { DotNetVersion = "2.1", OsVariant = OS.Bionic, Architecture = "arm", IsWeb = true },
        };
        private static readonly ImageData[] s_windowsTestData =
        {
            new ImageData { DotNetVersion = "1.0", PlatformOS = OS.NanoServerSac2016, SdkVersion = "1.1" },
            new ImageData { DotNetVersion = "1.1", PlatformOS = OS.NanoServerSac2016 },
            new ImageData { DotNetVersion = "2.0", PlatformOS = OS.NanoServerSac2016 },
            new ImageData { DotNetVersion = "2.0", PlatformOS = OS.NanoServer1709 },
            new ImageData { DotNetVersion = "2.0", PlatformOS = OS.NanoServer1803 },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServerSac2016 },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServer1709 },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServer1803 },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServerSac2016, IsWeb = true },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServer1709, IsWeb = true },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServer1803, IsWeb = true },
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
                    || Regex.IsMatch(imageData.Architecture, archFilterPattern, RegexOptions.IgnoreCase))
                .Where(imageData => osFilterPattern == null
                    || (imageData.PlatformOS != null
                        && Regex.IsMatch(imageData.PlatformOS, osFilterPattern, RegexOptions.IgnoreCase)))
                .Where(imageData => versionFilterPattern == null
                    || Regex.IsMatch(imageData.DotNetVersion, versionFilterPattern, RegexOptions.IgnoreCase))
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
            string appSdkImage = GetIdentifier(imageData.DotNetVersion, "app-sdk");

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
            buildArgs.Add($"netcoreapp_version={imageData.DotNetVersion}");
            AddOptionalRestoreArgs(buildArgs);

            if (!imageData.SdkVersion.StartsWith("1."))
            {
                buildArgs.Add($"optional_new_args=--no-restore");
            }

            buildArgs.Add("template_name=" + GetTestTemplateName(imageData.IsWeb));

            _dockerHelper.Build(
                dockerfile: $"Dockerfile.{DockerHelper.DockerOS.ToLower()}.testapp",
                tag: appSdkImage,
                fromImage: GetDotNetImage(DotNetImageType.SDK, imageData),
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

                if (imageData.IsWeb)
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
            string verifyCacheCommand;
            if (imageData.DotNetVersion.StartsWith("1."))
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
            else
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

            // Simple check to verify the NuGet package cache was created
            _dockerHelper.Run(
                image: GetDotNetImage(DotNetImageType.SDK, imageData),
                command: verifyCacheCommand,
                containerName: GetIdentifier(imageData.DotNetVersion, "PackageCache"));
        }

        private async Task VerifyRuntimeImage_FrameworkDependentApp(ImageData imageData, string appSdkImage)
        {
            string frameworkDepAppId = GetIdentifier(imageData.DotNetVersion, "framework-dependent-app");
            bool isRunAsContainerAdministrator = 
                String.Equals("nanoserver-1709", imageData.PlatformOS, StringComparison.OrdinalIgnoreCase)
                || String.Equals("nanoserver-1803", imageData.PlatformOS, StringComparison.OrdinalIgnoreCase);
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
                string runtimeImage = GetDotNetImage(
                    imageData.IsWeb ? DotNetImageType.AspNetCore_Runtime : DotNetImageType.Runtime, imageData);
                string appDllPath = _dockerHelper.GetContainerWorkPath("testApp.dll");
                _dockerHelper.Run(
                    image: runtimeImage,
                    command: $"dotnet {appDllPath}",
                    containerName: frameworkDepAppId,
                    volumeName: frameworkDepAppId,
                    detach: imageData.IsWeb,
                    runAsContainerAdministrator: isRunAsContainerAdministrator);

                if (imageData.IsWeb)
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
            string selfContainedAppId = GetIdentifier(imageData.DotNetVersion, "self-contained-app");
            string rid = GetRuntimeIdentifier(imageData);

            try
            {
                // Build a self-contained app
                List<string> buildArgs = new List<string>();
                buildArgs.Add($"rid={rid}");
                AddOptionalRestoreArgs(buildArgs);

                _dockerHelper.Build(
                    dockerfile: "Dockerfile.linux.testapp.selfcontained",
                    tag: selfContainedAppId,
                    fromImage: appSdkImage,
                    buildArgs: buildArgs.ToArray());

                try
                {
                    // Publish the self-contained app to a Docker volume using the app's sdk image
                    string publishCmd = GetPublishArgs(imageData, rid);
                    _dockerHelper.Run(
                        image: selfContainedAppId,
                        command: publishCmd,
                        containerName: selfContainedAppId,
                        volumeName: selfContainedAppId);

                    // Run the self-contained app in the Docker volume to verify the runtime-deps image
                    string runtimeDepsImage = GetDotNetImage(DotNetImageType.Runtime_Deps, imageData);
                    string appExePath = _dockerHelper.GetContainerWorkPath("testApp");
                    _dockerHelper.Run(
                        image: runtimeDepsImage,
                        command: appExePath,
                        containerName: selfContainedAppId,
                        detach: imageData.IsWeb,
                        volumeName: selfContainedAppId);

                    if (imageData.IsWeb)
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

        private static void AddOptionalRestoreArgs(List<string> buildArgs)
        {
            if (s_isNightlyRepo)
            {
                buildArgs.Add("optional_restore_args=\"-s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json -s https://api.nuget.org/v3/index.json\"");
            }
        }

        public static string GetDotNetImage(DotNetImageType imageType, ImageData imageData)
        {
            string imageVersion;
            string osVariant;
            string variantName = Enum.GetName(typeof(DotNetImageType), imageType).ToLowerInvariant().Replace('_', '-');

            switch (imageType)
            {
                case DotNetImageType.Runtime:
                case DotNetImageType.AspNetCore_Runtime:
                    imageVersion = imageData.DotNetVersion;
                    osVariant = imageData.OsVariant;
                    break;
                case DotNetImageType.Runtime_Deps:
                    imageVersion = imageData.RuntimeDepsVersion;
                    osVariant = imageData.OsVariant;
                    break;
                case DotNetImageType.SDK:
                    imageVersion = imageData.SdkVersion;
                    osVariant = imageData.SdkOsVariant;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported image type '{variantName}'");
            }

            string imageName = $"{s_repoOwner}/{s_repoName}:{imageVersion}-{variantName}";
            if (!string.IsNullOrEmpty(osVariant))
            {
                imageName += $"-{osVariant}";
            }

            if (imageData.IsArm)
            {
                imageName += $"-arm32v7";
            }

            Assert.True(DockerHelper.ImageExists(imageName), $"`{imageName}` could not be found on disk.");

            return imageName;
        }

        private static string GetIdentifier(string version, string type)
        {
            return $"{version}-{type}-{DateTime.Now.ToFileTime()}";
        }

        private static string GetPublishArgs(ImageData imageData, string rid = null)
        {
            string optionalArgs = imageData.DotNetVersion.StartsWith("1.") ? "" : " --no-restore";
            optionalArgs += string.IsNullOrEmpty(rid) ? "" : $" -r {rid}";
            return $"dotnet publish -c Release -o {DockerHelper.ContainerWorkDir}{optionalArgs}";
        }

        private static string GetRepoName()
        {
            string manifestJson = File.ReadAllText("manifest.json");
            JObject manifest = JObject.Parse(manifestJson);
            string qualifiedRepoName = (string)manifest["repos"][0]["name"];
            return qualifiedRepoName.Split('/')[1];
        }

        private static string GetRuntimeIdentifier(ImageData imageData)
        {
            string rid;

            if (imageData.IsArm)
            {
                rid = "linux-arm";
            }
            else if (imageData.IsAlpine)
            {
                rid = "linux-musl-x64";
            }
            else if (imageData.DotNetVersion.StartsWith("1."))
            {
                rid = "debian.8-x64";
            }
            else
            {
                rid = "linux-x64";
            }

            return rid;
        }
    }
}
