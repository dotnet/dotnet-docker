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
    public abstract class ProductImageTests
    {
        protected ProductImageTests(ITestOutputHelper outputHelper)
        {
            DockerHelper = new DockerHelper(outputHelper);
            OutputHelper = outputHelper;
        }
        
        protected DockerHelper DockerHelper { get; }
        protected ITestOutputHelper OutputHelper { get; }
        protected abstract DotNetImageType ImageType { get; }

        /// <summary>
        /// Verifies that a SAS token isn't accidentally leaked through the Docker history of the image.
        /// </summary>
        protected void VerifyCommonNoSasToken(ProductImageData imageData)
        {
            string imageTag = imageData.GetImage(ImageType, DockerHelper);
            string historyContents = DockerHelper.GetHistory(imageTag);
            Match match = Regex.Match(historyContents, @"=\?(sig|\S+&sig)=\S+");
            Assert.False(match.Success, $"A SAS token was detected in the Docker history of '{imageTag}'.");
        }

        protected void VerifyCommonInsecureFiles(ProductImageData imageData)
        {
            if (imageData.Version < new Version("3.1") ||
                (imageData.OS.Contains("alpine") && imageData.IsArm))
            {
                return;
            }

            string rootFsPath = imageData.IsDistroless ? "/rootfs" : "/";

            string worldWritableDirectoriesWithoutStickyBitCmd = $@"find {rootFsPath} -xdev -type d \( -perm -0002 -a ! -perm -1000 \)";
            string worldWritableFilesCmd = $"find {rootFsPath} -xdev -type f -perm -o+w";
            string noUserOrGroupFilesCmd;
            if (imageData.OS.Contains("alpine"))
            {
                // BusyBox in Alpine doesn't support the more convenient -nouser and -nogroup options for the find command
                noUserOrGroupFilesCmd = $@"find {rootFsPath} -xdev -exec stat -c %U-%n {{}} \+ | {{ grep ^UNKNOWN || true; }}";
            }
            else
            {
                noUserOrGroupFilesCmd = $@"find {rootFsPath} -xdev \( -nouser -o -nogroup \)";
            }

            string command = $"-c \"{worldWritableDirectoriesWithoutStickyBitCmd} && {worldWritableFilesCmd} && {noUserOrGroupFilesCmd}\"";

            string imageTag;
            if (imageData.IsDistroless)
            {
                imageTag = DockerHelper.BuildDistrolessHelper(ImageType, imageData, rootFsPath);
            }
            else
            {
                imageTag = imageData.GetImage(ImageType, DockerHelper);
            }

            string output = DockerHelper.Run(
                image: imageTag,
                name: imageData.GetIdentifier($"InsecureFiles-{ImageType}"),
                command: command,
                runAsUser: "root",
                optionalRunArgs: "--entrypoint /bin/sh"
            );

            Assert.Empty(output);
        }

        protected void VerifyCommonDefaultUser(ProductImageData imageData)
        {
            string imageTag = imageData.GetImage(ImageType, DockerHelper);
            string actualUser = DockerHelper.GetImageUser(imageTag);

            string expectedUser;
            if (imageData.IsDistroless && ImageType != DotNetImageType.SDK)
            {
                expectedUser = "app";
            }
            // For Windows, only Nano Server defines a user, which seems wrong.
            // I've logged https://dev.azure.com/microsoft/OS/_workitems/edit/40146885 for this.
            else if (imageData.OS.StartsWith(OS.NanoServer))
            {
                expectedUser = "ContainerUser";
            }
            else
            {
                expectedUser = string.Empty;
            }

            Assert.Equal(expectedUser, actualUser);
        }

        private IEnumerable<string> GetInstalledRpmPackages(ProductImageData imageData)
        {
            string rootPath = imageData.IsDistroless ? "/rootfs" : "/";
            // Get list of installed RPM packages
            string command = $"-c \"rpm -qa -r {rootPath} | sort\"";

            string imageTag;
            if (imageData.IsDistroless)
            {
                imageTag = DockerHelper.BuildDistrolessHelper(ImageType, imageData, rootPath);
            }
            else
            {
                imageTag = imageData.GetImage(ImageType, DockerHelper);
            }

            string installedPackages = DockerHelper.Run(
                image: imageTag,
                command: command,
                name: imageData.GetIdentifier("PackageInstallation"),
                optionalRunArgs: "--entrypoint /bin/sh");

            return installedPackages.Split(Environment.NewLine);
        }

        protected void VerifyExpectedInstalledRpmPackages(
            ProductImageData imageData, IEnumerable<string> expectedPackages)
        {
            if (imageData.Arch == Arch.Arm64)
            {
                OutputHelper.WriteLine("Skip test until Arm64 Dockerfiles install packages instead of tarballs");
                return;
            }

            foreach (string expectedPackage in expectedPackages)
            {
                bool installed = GetInstalledRpmPackages(imageData).Any(pkg => pkg.StartsWith(expectedPackage));
                Assert.True(installed, $"Package '{expectedPackage}' is not installed.");
            }
        }

        public static IEnumerable<EnvironmentVariableInfo> GetCommonEnvironmentVariables()
        {
            yield return new EnvironmentVariableInfo("DOTNET_RUNNING_IN_CONTAINER", "true");
        }
    }
}
