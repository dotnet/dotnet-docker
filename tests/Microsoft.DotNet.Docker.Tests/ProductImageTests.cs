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
        protected abstract DotNetImageRepo ImageRepo { get; }

        /// <summary>
        /// Verifies that a SAS token isn't accidentally leaked through the Docker history of the image.
        /// </summary>
        protected void VerifyCommonNoSasToken(ProductImageData imageData)
        {
            string imageTag = imageData.GetImage(ImageRepo, DockerHelper);
            string historyContents = DockerHelper.GetHistory(imageTag);
            Match match = Regex.Match(historyContents, @"=\?(sig|\S+&sig)=\S+");
            Assert.False(match.Success, $"A SAS token was detected in the Docker history of '{imageTag}'.");
        }

        protected void VerifyCommonInsecureFiles(ProductImageData imageData)
        {
            if (imageData.OS.Contains("alpine") && imageData.IsArm)
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
                imageTag = DockerHelper.BuildDistrolessHelper(ImageRepo, imageData, rootFsPath);
            }
            else
            {
                imageTag = imageData.GetImage(ImageRepo, DockerHelper);
            }

            string output = DockerHelper.Run(
                image: imageTag,
                name: imageData.GetIdentifier($"InsecureFiles-{ImageRepo}"),
                command: command,
                runAsUser: "root",
                optionalRunArgs: "--entrypoint /bin/sh"
            );

            string rootFsPathWithTrailingSlash = rootFsPath;
            if (!rootFsPathWithTrailingSlash.EndsWith("/"))
            {
                rootFsPathWithTrailingSlash += "/";
            }

            IEnumerable<string> lines = output
                .ReplaceLineEndings()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                // Writable files in the /tmp/.dotnet directory are allowed for global mutexes
                .Where(line => !line.StartsWith($"{rootFsPathWithTrailingSlash}tmp/.dotnet"))
                // Exclude the non-root user's home directory. It will show up as a directory without a user or group
                // because we're not examining the distroless container directly, but rather copying its contents into
                // a container with a shell but doesn't have the non-root user defined.
                .Where(line => !imageData.IsDistroless || line != $"{rootFsPathWithTrailingSlash}home/app");

            Assert.Empty(lines);
        }

        protected void VerifyCommonDefaultUser(ProductImageData imageData)
        {
            string imageTag = imageData.GetImage(ImageRepo, DockerHelper);
            string actualUser = DockerHelper.GetImageUser(imageTag);

            string expectedUser;
            if (imageData.IsDistroless && ImageRepo != DotNetImageRepo.SDK)
            {
                if (imageData.OS.Contains("cbl-mariner"))
                {
                    expectedUser = "app";
                }
                else
                {
                    expectedUser = imageData.NonRootUID.ToString();
                }
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

            VerifyNonRootUID(imageData);
        }

        protected void VerifyNonRootUID(ProductImageData imageData)
        {
            if (((imageData.Version.Major == 6 || imageData.Version.Major == 7) && (!imageData.IsDistroless || imageData.OS.StartsWith(OS.Mariner)))
                || imageData.IsWindows)
            {
                OutputHelper.WriteLine("UID check is only relevant for Linux images running .NET versions >= 8.0 and distroless images besides CBL Mariner.");
                return;
            }

            string imageTag = imageData.GetImage(ImageRepo, DockerHelper);
            string rootPath = "/";

            if (imageData.IsDistroless)
            {
                rootPath = "/rootfs/";
                imageTag = DockerHelper.BuildDistrolessHelper(ImageRepo, imageData, rootPath);
            }

            string command = $"-c \"grep '^app' {rootPath}etc/passwd | cut -d: -f3\"";

            string uidString = DockerHelper.Run(
                image: imageTag,
                command: command,
                name: imageData.GetIdentifier($"VerifyUID-{ImageRepo}"),
                optionalRunArgs: $"--entrypoint /bin/sh"
            );

            int uid = int.Parse(uidString);

            // UIDs below 1000 are reserved for system accounts
            Assert.True(uid >= 1000);
            // Debian has a UID_MAX of 60000
            Assert.True(uid <= 60000);
        }

        private IEnumerable<string> GetInstalledRpmPackages(ProductImageData imageData)
        {
            string rootPath = imageData.IsDistroless ? "/rootfs" : "/";
            // Get list of installed RPM packages
            string command = $"-c \"rpm -qa -r {rootPath} | sort\"";

            string imageTag;
            if (imageData.IsDistroless)
            {
                imageTag = DockerHelper.BuildDistrolessHelper(ImageRepo, imageData, rootPath);
            }
            else
            {
                imageTag = imageData.GetImage(ImageRepo, DockerHelper);
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
