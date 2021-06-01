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

            string output = DockerHelper.Run(
                    image: imageData.GetImage(ImageType, DockerHelper),
                    name: imageData.GetIdentifier($"InsecureFiles-{ImageType}"),
                    command: command
                );

            Assert.Empty(output);
        }

        private IEnumerable<string> GetInstalledRpmPackages(ProductImageData imageData)
        {
            // Get list of installed RPM packages
            string command = $"bash -c \"rpm -qa | sort\"";

            string installedPackages = DockerHelper.Run(
                image: imageData.GetImage(ImageType, DockerHelper),
                command: command,
                name: imageData.GetIdentifier("PackageInstallation"));

            return installedPackages.Split(Environment.NewLine);
        }

        protected void VerifyExpectedInstalledRpmPackages(
            ProductImageData imageData, IEnumerable<string> expectedPackages)
        {
            foreach (string expectedPackage in expectedPackages)
            {
                // Example package name: dotnet-runtime-6.0-6.0.0-0.1.preview.5.21270.12.x86_64
                string prefix;
                if (expectedPackage.EndsWith(imageData.VersionString))
                {
                    prefix = $"{expectedPackage}-{imageData.VersionString}.";
                }
                else
                {
                    prefix = expectedPackage;
                }

                bool installed = GetInstalledRpmPackages(imageData).Any(pkg => pkg.StartsWith(prefix));
                Assert.True(installed, $"Package '{expectedPackage}' is not installed.");
            }
        }

        public static IEnumerable<EnvironmentVariableInfo> GetCommonEnvironmentVariables()
        {
            yield return new EnvironmentVariableInfo("DOTNET_RUNNING_IN_CONTAINER", "true");
        }
    }
}
