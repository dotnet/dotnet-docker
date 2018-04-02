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
        private static string ArchFilter => Environment.GetEnvironmentVariable("IMAGE_ARCH_FILTER");
        private static bool IsNightlyRepo => RepoName.EndsWith("nightly");
        private static string OsFilter => Environment.GetEnvironmentVariable("IMAGE_OS_FILTER");
        private static string RepoName => GetRepoName();
        private static string RepoOwner => Environment.GetEnvironmentVariable("REPO_OWNER") ?? "microsoft";
        private static string VersionFilter => Environment.GetEnvironmentVariable("IMAGE_VERSION_FILTER");

        private static ImageData[] LinuxTestData => new[]
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
        };
        private static ImageData[] WindowsTestData => new[]
        {
            new ImageData { DotNetVersion = "1.0", PlatformOS = OS.NanoServerSac2016, SdkVersion = "1.1" },
            new ImageData { DotNetVersion = "1.1", PlatformOS = OS.NanoServerSac2016 },
            new ImageData { DotNetVersion = "2.0", PlatformOS = OS.NanoServerSac2016 },
            new ImageData { DotNetVersion = "2.0", PlatformOS = OS.NanoServer1709 },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServerSac2016, IsWeb = true },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServer1709, IsWeb = true },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServerSac2016 },
            new ImageData { DotNetVersion = "2.1", PlatformOS = OS.NanoServer1709 },
        };

        private DockerHelper DockerHelper { get; set; }

        private readonly ITestOutputHelper _outputHelper;

        public ImageTests(ITestOutputHelper outputHelper)
        {
            DockerHelper = new DockerHelper(outputHelper);
            _outputHelper = outputHelper;
        }

        public static IEnumerable<object[]> GetVerifyImagesData()
        {
            string versionFilterPattern = null;
            if (VersionFilter != null)
            {
                versionFilterPattern = GetFilterRegexPattern(VersionFilter);
            }

            string osFilterPattern = null;
            if (OsFilter != null)
            {
                osFilterPattern = GetFilterRegexPattern(OsFilter);
            }

            // Filter out test data that does not match the active architecture and version filters.
            return (DockerHelper.IsLinuxContainerModeEnabled ? LinuxTestData : WindowsTestData)
                .Where(imageData => ArchFilter == null
                    || string.Equals(imageData.Architecture, ArchFilter, StringComparison.OrdinalIgnoreCase))
                .Where(imageData => OsFilter == null
                    || (imageData.PlatformOS != null
                        && Regex.IsMatch(imageData.PlatformOS, osFilterPattern, RegexOptions.IgnoreCase)))
                .Where(imageData => VersionFilter == null
                    || Regex.IsMatch(imageData.DotNetVersion, versionFilterPattern, RegexOptions.IgnoreCase))
                .Select(imageData => new object[] { imageData });
        }

        private static string GetFilterRegexPattern(string filter)
        {
            return $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$";
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
                    await VerifySdkImage_RunApp(imageData, appSdkImage);
                }

                await VerifyRuntimeImage_FrameworkDependentApp(imageData, appSdkImage);

                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    await VerifyRuntimeDepsImage_SelfContainedApp(imageData, appSdkImage);
                }
            }
            finally
            {
                DockerHelper.DeleteImage(appSdkImage);
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

            DockerHelper.Build(
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
                DockerHelper.Run(
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
                DockerHelper.Kill(appSdkImage);
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
            DockerHelper.Run(
                image: GetDotNetImage(DotNetImageType.SDK, imageData),
                command: verifyCacheCommand,
                containerName: GetIdentifier(imageData.DotNetVersion, "PackageCache"));
        }

        private async Task VerifyRuntimeImage_FrameworkDependentApp(ImageData imageData, string appSdkImage)
        {
            string frameworkDepAppId = GetIdentifier(imageData.DotNetVersion, "framework-dependent-app");
            bool isRunAsContainerAdministrator = String.Equals(
                "nanoserver-1709", imageData.PlatformOS, StringComparison.OrdinalIgnoreCase);
            string optionalPublishArgs = GetOptionalPublishArgs(imageData);

            try
            {
                // Publish the app to a Docker volume using the app's sdk image
                DockerHelper.Run(
                    image: appSdkImage,
                    command: $"dotnet publish -o {DockerHelper.ContainerWorkDir} {optionalPublishArgs}",
                    containerName: frameworkDepAppId,
                    volumeName: frameworkDepAppId,
                    runAsContainerAdministrator: isRunAsContainerAdministrator);

                // Run the app in the Docker volume to verify the runtime image
                string runtimeImage = GetDotNetImage(imageData.IsWeb ? DotNetImageType.AspNetCore_Runtime : DotNetImageType.Runtime, imageData);
                string appDllPath = DockerHelper.GetContainerWorkPath("testApp.dll");
                DockerHelper.Run(
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
                DockerHelper.Kill(frameworkDepAppId);
                DockerHelper.DeleteVolume(frameworkDepAppId);
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
                buildArgs.Add("template_name=" + GetTestTemplateName(imageData.IsWeb));
                AddOptionalRestoreArgs(buildArgs);

                DockerHelper.Build(
                    dockerfile: "Dockerfile.linux.testapp.selfcontained",
                    tag: selfContainedAppId,
                    fromImage: appSdkImage,
                    buildArgs: buildArgs.ToArray());

                try
                {
                    // Publish the self-contained app to a Docker volume using the app's sdk image
                    string optionalPublishArgs = GetOptionalPublishArgs(imageData);
                    string dotNetCmd = $"dotnet publish -r {rid} -o {DockerHelper.ContainerWorkDir} {optionalPublishArgs}";
                    DockerHelper.Run(
                        image: selfContainedAppId,
                        command: dotNetCmd,
                        containerName: selfContainedAppId,
                        volumeName: selfContainedAppId);

                    // Run the self-contained app in the Docker volume to verify the runtime-deps image
                    string runtimeDepsImage = GetDotNetImage(DotNetImageType.Runtime_Deps, imageData);
                    string appExePath = DockerHelper.GetContainerWorkPath("testApp");
                    DockerHelper.Run(
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
                    DockerHelper.Kill(selfContainedAppId);
                    DockerHelper.DeleteVolume(selfContainedAppId);
                }
            }
            finally
            {
                DockerHelper.DeleteImage(selfContainedAppId);
            }
        }

        private async Task VerifyHttpResponseFromContainer(string containerName)
        {
            var retries = 60;
            var client = new HttpClient();
            var url = Environment.GetEnvironmentVariable("RUNNING_TESTS_IN_CONTAINER") == null && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                          ? "http://localhost:5000"
                          : "http://" + DockerHelper.GetContainerAddress(containerName);

            while (retries > 0)
            {
                retries--;
                try
                {
                    var result = await client.GetAsync(url);
                    _outputHelper.WriteLine($"HTTP {result.StatusCode}\n{(await result.Content.ReadAsStringAsync())}");
                    result.EnsureSuccessStatusCode();
                    return;
                }
                catch (Exception ex)
                {
                    _outputHelper.WriteLine($"Request to {url} failed : {ex.ToString()}");
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            throw new TimeoutException($"Timed out attempting to access the endpoint {url} on container {containerName}");
        }

        private static string GetTestTemplateName(bool isWeb)
        {
            return isWeb ? "web" : "console";
        }

        private static void AddOptionalRestoreArgs(List<string> buildArgs)
        {
            if (IsNightlyRepo)
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
                case DotNetImageType.AspNetCore_Runtime:
                    imageVersion = imageData.DotNetVersion;
                    osVariant = imageData.OsVariant;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported image type '{variantName}'");
            };

            string imageName = $"{RepoOwner}/{RepoName}:{imageVersion}-{variantName}";
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

        private static string GetOptionalPublishArgs(ImageData imageData)
        {
            return imageData.DotNetVersion.StartsWith("1.") ? "" : "--no-restore";
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
                rid = "alpine.3.6-x64";
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
