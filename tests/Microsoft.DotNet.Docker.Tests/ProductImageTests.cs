// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
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

        protected void VerifyCommonInsecureFiles(ProductImageData imageData)
        {
            if (imageData.Version < new Version("3.1") ||
                (imageData.OS.Contains("alpine") && imageData.IsArm))
            {
                return;
            }

            string worldWritableDirectoriesWithoutStickyBitCmd = @"find / -xdev -type d \( -perm -0002 -a ! -perm -1000 \)";
            string worldWritableFilesCmd = "find / -xdev -type f -perm -o+w";
            string noUserOrGroupFilesCmd;
            if (imageData.OS.Contains("alpine"))
            {
                // BusyBox in Alpine doesn't support the more convenient -nouser and -nogroup options for the find command
                noUserOrGroupFilesCmd = @"find / -xdev -exec stat -c %U-%n {} \+ | { grep ^UNKNOWN || true; }";
            }
            else
            {
                noUserOrGroupFilesCmd = @"find / -xdev \( -nouser -o -nogroup \)";
            }

            string command = $"/bin/sh -c \"{worldWritableDirectoriesWithoutStickyBitCmd} && {worldWritableFilesCmd} && {noUserOrGroupFilesCmd}\"";

            string imageTag;
            if (imageData.IsDistroless)
            {
                imageTag = DockerHelper.BuildDistrolessHelper(ImageType, imageData, "bash", "findutils");
            }
            else
            {
                imageTag = imageData.GetImage(ImageType, DockerHelper);
            }

            string output = DockerHelper.Run(
                    image: imageTag,
                    name: imageData.GetIdentifier($"InsecureFiles-{ImageType}"),
                    command: command,
                    runAsUser: "root"
                );

            Assert.Empty(output);
        }

        private IEnumerable<string> GetInstalledRpmPackages(ProductImageData imageData)
        {
            // Get list of installed RPM packages
            string command = $"bash -c \"{(imageData.OS == OS.Mariner10Distroless ? "rpm -qa" : "tdnf list installed")} | sort\"";

            string imageTag;
            if (imageData.IsDistroless)
            {
                imageTag = DockerHelper.BuildDistrolessHelper(ImageType, imageData, "bash", imageData.OS == OS.Mariner10Distroless ? "rpm": "tdnf");
            }
            else
            {
                imageTag = imageData.GetImage(ImageType, DockerHelper);
            }

            string installedPackages = DockerHelper.Run(
                image: imageTag,
                command: command,
                name: imageData.GetIdentifier("PackageInstallation"));

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
