// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageTests
    {
        private static string ArchFilter => Environment.GetEnvironmentVariable("IMAGE_ARCH_FILTER");
        private static string VersionFilter => Environment.GetEnvironmentVariable("IMAGE_VERSION_FILTER");

        private DockerHelper DockerHelper { get; set; }

        public ImageTests(ITestOutputHelper output)
        {
            DockerHelper = new DockerHelper(output);
        }

        public static IEnumerable<object[]> GetVerifyImagesData()
        {
            List<ImageDescriptor> testData = new List<ImageDescriptor>
            {
                new ImageDescriptor {DotNetCoreVersion = "1.0", SdkVersion = "1.1"},
                new ImageDescriptor {DotNetCoreVersion = "1.1", RuntimeDepsVersion = "1.0"},
                new ImageDescriptor {DotNetCoreVersion = "2.0"},
            };

            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                testData.AddRange(new List<ImageDescriptor>
                    {
                        new ImageDescriptor {DotNetCoreVersion = "2.0", OsVariant = "jessie"},
                        new ImageDescriptor
                        {
                            DotNetCoreVersion = "2.0",
                            OsVariant = "stretch",
                            SdkOsVariant = "",
                            Architecture = "arm"
                        },
                    });
            }

            string versionFilterPattern = null;
            if (VersionFilter != null)
            {
                versionFilterPattern = "^" + Regex.Escape(VersionFilter).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            }

            // Filter out test data that does not match the active architecture and version filters.
            return testData
                .Where(imageDescriptor => ArchFilter == null
                    || string.Equals(imageDescriptor.Architecture, ArchFilter, StringComparison.OrdinalIgnoreCase))
                .Where(imageDescriptor => VersionFilter == null
                    || Regex.IsMatch(imageDescriptor.DotNetCoreVersion, versionFilterPattern, RegexOptions.IgnoreCase))
                .Select(imageDescriptor => new object[] { imageDescriptor });
        }

        [Theory]
        [MemberData(nameof(GetVerifyImagesData))]
        public void VerifyImages(ImageDescriptor imageDescriptor)
        {
            string appSdkImage = GetIdentifier(imageDescriptor.DotNetCoreVersion, "app-sdk");

            try
            {
                CreateTestAppWithSdkImage(imageDescriptor, appSdkImage);

                if (!imageDescriptor.IsArm)
                {
                    VerifySdkImage_RunApp(imageDescriptor, appSdkImage);
                }

                VerifyRuntimeImage_FrameworkDependentApp(imageDescriptor, appSdkImage);

                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    VerifyRuntimeDepsImage_SelfContainedApp(imageDescriptor, appSdkImage);
                }
            }
            finally
            {
                DockerHelper.DeleteImage(appSdkImage);
            }
        }

        private void CreateTestAppWithSdkImage(ImageDescriptor imageDescriptor, string appSdkImage)
        {
            // dotnet new, restore, build a new app using the sdk image
            List<string> buildArgs = new List<string>();
            buildArgs.Add($"netcoreapp_version={imageDescriptor.DotNetCoreVersion}");
            if (!imageDescriptor.SdkVersion.StartsWith("1."))
            {
                buildArgs.Add($"optional_new_args=--no-restore");
            }

            string sdkImage = GetDotNetImage(
                imageDescriptor.SdkVersion, DotNetImageType.SDK, imageDescriptor.SdkOsVariant);

            DockerHelper.Build(
                dockerfile: $"Dockerfile.{DockerHelper.DockerOS.ToLower()}.testapp",
                tag: appSdkImage,
                fromImage: sdkImage,
                buildArgs: buildArgs.ToArray());
        }

        private void VerifySdkImage_RunApp(ImageDescriptor imageDescriptor, string appSdkImage)
        {
            // dotnet run the new app using the sdk image
            DockerHelper.Run(
                image: appSdkImage,
                command: "dotnet run",
                containerName: appSdkImage);
        }

        private void VerifyRuntimeImage_FrameworkDependentApp(ImageDescriptor imageDescriptor, string appSdkImage)
        {
            string frameworkDepAppId = GetIdentifier(imageDescriptor.DotNetCoreVersion, "framework-dependent-app");

            try
            {
                // Publish the app to a Docker volume using the app's sdk image
                DockerHelper.Run(
                    image: appSdkImage,
                    command: $"dotnet publish -o {DockerHelper.ContainerWorkDir}",
                    containerName: frameworkDepAppId,
                    volumeName: frameworkDepAppId);

                // Run the app in the Docker volume to verify the runtime image
                string runtimeImage = GetDotNetImage(
                    imageDescriptor.DotNetCoreVersion,
                    DotNetImageType.Runtime,
                    imageDescriptor.OsVariant,
                    imageDescriptor.IsArm);
                string appDllPath = DockerHelper.GetContainerWorkPath("testApp.dll");
                DockerHelper.Run(
                    image: runtimeImage,
                    command: $"dotnet {appDllPath}",
                    containerName: frameworkDepAppId,
                    volumeName: frameworkDepAppId);
            }
            finally
            {
                DockerHelper.DeleteVolume(frameworkDepAppId);
            }
        }

        private void VerifyRuntimeDepsImage_SelfContainedApp(ImageDescriptor imageDescriptor, string appSdkImage)
        {
            string selfContainedAppId = GetIdentifier(imageDescriptor.DotNetCoreVersion, "self-contained-app");
            string rid = imageDescriptor.IsArm ? "linux-arm" : "debian.8-x64";

            try
            {
                // Build a self-contained app
                DockerHelper.Build(
                    dockerfile: "Dockerfile.linux.testapp.selfcontained",
                    tag: selfContainedAppId,
                    fromImage: appSdkImage,
                    buildArgs: $"rid={rid}");

                try
                {
                    // Publish the self-contained app to a Docker volume using the app's sdk image
                    string optionalPublishArgs = imageDescriptor.DotNetCoreVersion.StartsWith("1.") ? "" : "--no-restore";
                    string dotNetCmd = $"dotnet publish -r {rid} -o {DockerHelper.ContainerWorkDir} {optionalPublishArgs}";
                    DockerHelper.Run(
                        image: selfContainedAppId,
                        command: dotNetCmd,
                        containerName: selfContainedAppId,
                        volumeName: selfContainedAppId);

                    // Run the self-contained app in the Docker volume to verify the runtime-deps image
                    string runtimeDepsImage = GetDotNetImage(
                        imageDescriptor.RuntimeDepsVersion,
                        DotNetImageType.Runtime_Deps,
                        imageDescriptor.OsVariant,
                        imageDescriptor.IsArm);
                    string appExePath = DockerHelper.GetContainerWorkPath("testApp");
                    DockerHelper.Run(
                        image: runtimeDepsImage,
                        command: appExePath,
                        containerName: selfContainedAppId,
                        volumeName: selfContainedAppId);
                }
                finally
                {
                    DockerHelper.DeleteVolume(selfContainedAppId);
                }
            }
            finally
            {
                DockerHelper.DeleteImage(selfContainedAppId);
            }
        }

        public static string GetDotNetImage(
            string imageVersion, DotNetImageType imageType, string osVariant, bool isArm = false)
        {
            string variantName = Enum.GetName(typeof(DotNetImageType), imageType).ToLowerInvariant().Replace('_', '-');
            string imageName = $"microsoft/dotnet:{imageVersion}-{variantName}";
            if (!string.IsNullOrEmpty(osVariant))
            {
                imageName += $"-{osVariant}";
            }

            if (isArm)
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
    }
}
