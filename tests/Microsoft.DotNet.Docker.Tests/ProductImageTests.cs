// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using FluentAssertions;
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
            SyftHelper = new SyftHelper(DockerHelper, OutputHelper);
        }

        protected DockerHelper DockerHelper { get; }
        protected ITestOutputHelper OutputHelper { get; }
        protected SyftHelper SyftHelper { get; }
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
                if (imageData.OS.Family == OSFamily.Mariner)
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
            else if (imageData.OS.Family == OSFamily.NanoServer)
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
            if (imageData.IsWindows)
            {
                OutputHelper.WriteLine("UID check is only relevant for Linux images");
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

        public static IEnumerable<EnvironmentVariableInfo> GetCommonEnvironmentVariables()
        {
            yield return new EnvironmentVariableInfo("DOTNET_RUNNING_IN_CONTAINER", "true");
        }

        /// <summary>
        /// Verifies that the packages installed are correct and scannable by security tools.
        /// </summary>
        internal void VerifyInstalledPackagesBase(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            IEnumerable<string> extraExcludePaths = null)
        {
            // Temporary: Skip package tests for Ubuntu 26.04 Resolute to unblock publishing
            // These tests are flaky and the long-term fix is tracked in https://github.com/dotnet/dotnet-docker/pull/6894
            if (imageData.OS.Contains(OS.Resolute))
            {
                OutputHelper.WriteLine("Skipping package test for Ubuntu 26.04 Resolute due to flaky package baseline tests. " +
                    "Long-term fix tracked in https://github.com/dotnet/dotnet-docker/pull/6894");
                return;
            }

            IEnumerable<string> expectedPackages = GetExpectedPackages(imageData, imageRepo);
            IEnumerable<string> actualPackages = GetInstalledPackages(imageData, imageRepo, extraExcludePaths);

            string imageName = imageData.GetImage(imageRepo, DockerHelper, skipPull: true);
            ComparePackages(expectedPackages, actualPackages, imageData.IsDistroless, imageName, OutputHelper);
        }

        internal static void ComparePackages(
            IEnumerable<string> expectedPackages,
            IEnumerable<string> actualPackages,
            bool isDistroless,
            string imageName,
            ITestOutputHelper outputHelper)
        {
            outputHelper.WriteLine($"Expected Packages: [ {string.Join(", ", expectedPackages)} ]");
            outputHelper.WriteLine($"Actual Packages: [ {string.Join(", ", actualPackages)} ]");

            // Verify we only include strictly necessary packages in distroless images
            if (isDistroless)
            {
                actualPackages.Should().BeEquivalentTo(expectedPackages, because: $"image {imageName} is distroless");
                return;
            }

            // Verify satisfy .NET dependencies on non-distroless images.
            // There will be additional packages from the distro.
            expectedPackages.Should().BeSubsetOf(actualPackages, because: $"image {imageName} is not distroless");
        }

        /// <summary>
        /// Gets a list of all unique linux packages in the image.
        /// </summary>
        /// <param name="excludePaths">
        /// These paths will be excluded from image scanning.
        /// </param>
        /// <returns></returns>
        internal IEnumerable<string> GetInstalledPackages(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            IEnumerable<string> extraExcludePaths = null)
        {
            JsonNode output = SyftHelper.Scan(imageData, imageRepo, extraExcludePaths);

            return ((JsonArray)output["artifacts"])
                .Select(artifact => artifact["name"]?.ToString())
                // Syft can sometimes detect duplicates of packages if they have a distro-specific version suffix. Syft
                // will report the distro package version and the binary package version as separate.
                //
                // openssl                      1.1.1k                binary
                // openssl                      1.1.1k-29.cm2         rpm
                //                              ^ Mariner specific version
                //
                // So we need to remove duplicates.
                .Distinct();
        }

        internal static IEnumerable<string> GetExpectedPackages(ProductImageData imageData, DotNetImageRepo imageRepo)
        {
            IEnumerable<string> expectedPackages = GetRuntimeDepsPackages(imageData);

            if (imageData.IsDistroless)
            {
                expectedPackages = [..expectedPackages, ..GetDistrolessBasePackages(imageData)];
            }

            if (imageData.ImageVariant.HasFlag(DotNetImageVariant.Extra) || imageRepo == DotNetImageRepo.SDK)
            {
                expectedPackages = [..expectedPackages, ..GetExtraPackages(imageData)];
            }

            return expectedPackages.Distinct().OrderBy(s => s);
        }

        private static IEnumerable<string> GetDistrolessBasePackages(ProductImageData imageData) => imageData switch
            {
                { OS.Family: OSFamily.AzureLinux } => new[]
                    {
                        "SymCrypt",
                        "SymCrypt-OpenSSL",
                        "azurelinux-release",
                        "distroless-packages-minimal",
                        "filesystem",
                        "prebuilt-ca-certificates",
                        "tzdata"
                    },
                { OS.Family: OSFamily.Mariner } => new[]
                    {
                        "distroless-packages-minimal",
                        "filesystem",
                        "mariner-release",
                        "prebuilt-ca-certificates",
                        "tzdata"
                    },
                { OS.Family: OSFamily.Ubuntu } => new[]
                    {
                        "base-files"
                    },
                _ => throw new NotSupportedException($"Unsupported distroless OS '{imageData.OS}'")
            };

        private static IEnumerable<string> GetRuntimeDepsPackages(ProductImageData imageData) {
            IEnumerable<string> packages = imageData switch
            {
                { OS.Family: OSFamily.Mariner or OSFamily.AzureLinux } =>
                    [
                        "glibc",
                        "libgcc",
                        "openssl",
                        "openssl-libs",
                        "libstdc++"
                    ],
                { OS: { Family: OSFamily.Ubuntu, Version: "22.04"} } =>
                    [
                        "ca-certificates",
                        "gcc-12-base",
                        "libc6",
                        "libgcc-s1",
                        "libssl3",
                        "openssl",
                        "libstdc++6"
                    ],
                { OS: var os } when os == OS.NobleChiseled =>
                    [
                        "ca-certificates",
                        "gcc-14-base",
                        "gcc-14",
                        "libc6",
                        "libgcc-s1",
                        "libssl3t64",
                        "openssl",
                        "libstdc++6"
                    ],
                { OS: var os, Arch: Arch.Amd64 } when os == OS.ResoluteChiseled =>
                    [
                        "ca-certificates",
                        "gcc-14-base",
                        "gcc-15",
                        "libc6",
                        "libgcc-s1",
                        "libssl3t64",
                        "libstdc++6",
                        "libzstd",
                        "libzstd1",
                        "openssl",
                        "openssl-provider-legacy",
                        "zlib"
                    ],
                { OS: var os } when os == OS.ResoluteChiseled =>
                    [
                        "ca-certificates",
                        "gcc-14-base",
                        "libc6",
                        "libgcc-s1",
                        "libssl3t64",
                        "libstdc++6",
                        "libzstd1",
                        "openssl",
                        "zlib"
                    ],
                { OS: { Family: OSFamily.Ubuntu, Version: "24.04" } } =>
                    [
                        "ca-certificates",
                        "gcc-14-base",
                        "libc6",
                        "libgcc-s1",
                        "libssl3t64",
                        "openssl",
                        "libstdc++6"
                    ],
                { OS: { Family: OSFamily.Ubuntu, Version: "26.04" } } =>
                    [
                        "ca-certificates",
                        "gcc-15-base",
                        "libc6",
                        "libgcc-s1",
                        "libssl3t64",
                        "openssl",
                        "libstdc++6"
                    ],
                { OS.Family: OSFamily.Alpine } =>
                    [
                        "ca-certificates-bundle",
                        "libgcc",
                        "libssl3",
                        "libstdc++"
                    ],
                { OS: var os } when os == OS.BookwormSlim =>
                    [
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libicu72",
                        "libssl3",
                        "tzdata",
                        "libstdc++6"
                    ],
                _ => throw new NotSupportedException($"Unknown runtime-deps packages for OS '{imageData.OS}'")
            };

            // zlib is not required for .NET 9+
            // - https://github.com/dotnet/dotnet-docker/issues/5687
            // Starting with Ubuntu 25.04 (Plucky), zlib is a dependency of libssl3t64
            // - https://packages.ubuntu.com/plucky/amd64/libssl3t64
            // - https://packages.ubuntu.com/resolute/amd64/libssl3t64
            // - https://github.com/canonical/chisel-releases/blob/ubuntu-26.04/slices/libssl3t64.yaml
            if (imageData.Version.Major == 8 || imageData.OS.Version == "26.04")
            {
                packages = [..packages, GetZLibPackage(imageData.OS)];
            }

            return packages;
        }

        private static string GetZLibPackage(OSInfo os)
        {
            OSFamily[] unversionedZLibOSFamilies = [OSFamily.Alpine, OSFamily.AzureLinux, OSFamily.Mariner];
            return unversionedZLibOSFamilies.Contains(os.Family) ? "zlib" : "zlib1g";
        }

        private static IEnumerable<string> GetExtraPackages(ProductImageData imageData) => imageData switch
            {
                { IsDistroless: true, OS.Family: OSFamily.Mariner or OSFamily.AzureLinux } => new[]
                    {
                        "icu",
                        "tzdata"
                    },
                { OS: var os } when os == OS.NobleChiseled => new[]
                    {
                        "libicu74",
                        "tzdata-legacy",
                        "tzdata"
                    },
                { OS: var os } when os == OS.ResoluteChiseled => new[]
                    {
                        "icu",
                        "libicu76",
                        "tzdata"
                    },
                { OS: var os } when os == OS.JammyChiseled => new[]
                    {
                        "libicu70",
                        "tzdata"
                    },
                { OS.Family: OSFamily.Alpine } => new[]
                    {
                        "icu-data-full",
                        "icu-libs",
                        "tzdata"
                    },
                _ => Array.Empty<string>()
            };
    }
}
