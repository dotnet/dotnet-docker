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

        /// <summary>
        /// Verifies that the packages installed are correct and scannable by security tools.
        /// </summary>
        internal static void VerifyInstalledPackagesBase(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper,
            ITestOutputHelper outputHelper,
            IEnumerable<string> extraExcludePaths = null)
        {
            IEnumerable<string> expectedPackages = GetExpectedPackages(imageData, imageRepo);
            IEnumerable<string> actualPackages = GetInstalledPackages(imageData, imageRepo, dockerHelper, extraExcludePaths);

            ComparePackages(expectedPackages, actualPackages, imageData.IsDistroless, outputHelper);
        }

        internal static void ComparePackages(
            IEnumerable<string> expectedPackages,
            IEnumerable<string> actualPackages,
            bool isDistroless,
            ITestOutputHelper outputHelper)
        {
            outputHelper.WriteLine($"Expected Packages: [ {string.Join(", ", expectedPackages)} ]");

            // Verify we only include strictly necessary packages in distroless images
            if (isDistroless)
            {
                outputHelper.WriteLine($"Actual Packages: [ {string.Join(", ", actualPackages)} ]");
                Assert.Equal(expectedPackages, actualPackages);
                return;
            }

            // Verify satisfy .NET dependencies on non-distroless images.
            // There will be additional packages from the distro.
            IEnumerable<string> missingPackages = expectedPackages.Except(actualPackages);
            if (missingPackages.Any())
            {
                outputHelper.WriteLine($"Missing packages: [ {string.Join(", ", missingPackages)} ]");
            }

            Assert.Empty(missingPackages);
        }

        internal static IEnumerable<string> GetInstalledPackages(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper,
            IEnumerable<string> extraExcludePaths = null)
        {
            JsonNode output = GetSyftOutput("package-info", imageData, imageRepo, dockerHelper, extraExcludePaths);
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

        protected static JsonNode GetSyftOutput(
            string syftContainerName,
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper,
            IEnumerable<string> extraExcludePaths = null)
        {
            string syftImage = $"{Config.GetVariableValue("syft|repo")}:{Config.GetVariableValue("syft|tag")}";
            dockerHelper.Pull(syftImage);

            string imageToInspect = imageData.GetImage(imageRepo, dockerHelper);

            string outputContainerFilePath = "/artifacts/output.json";
            string tempDir = null;
            string outputContents = null;

            // Ignore the dotnet folder, or else syft will report all the packages in the .NET Runtime. We only care
            // about the packages from the linux distro for this test.
            extraExcludePaths ??= [];
            extraExcludePaths = extraExcludePaths.Append("/usr/share/dotnet");
            IEnumerable<string> excludeArgs = extraExcludePaths.Select(path => $"--exclude {path}");

            string[] args = [
                "scan",
                $"docker:{imageToInspect}",
                $"-o json={outputContainerFilePath}",
                ..excludeArgs
            ];

            try
            {
                dockerHelper.Run(
                    syftImage,
                    syftContainerName,
                    string.Join(' ', args),
                    skipAutoCleanup: true,
                    useMountedDockerSocket: true);

                tempDir = Directory.CreateDirectory(
                    Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())).FullName;

                dockerHelper.Copy($"{syftContainerName}:{outputContainerFilePath}", tempDir);

                string outputLocalFilePath = Path.Join(tempDir, Path.GetFileName(outputContainerFilePath));
                outputContents = File.ReadAllText(outputLocalFilePath);
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                dockerHelper.DeleteContainer(syftContainerName);
            }

            return JsonNode.Parse(outputContents)
                    ?? throw new JsonException($"Unable to parse the output as JSON:{Environment.NewLine}{outputContents}");
        }

        internal static IEnumerable<string> GetExpectedPackages(ProductImageData imageData, DotNetImageRepo imageRepo)
        {
            IEnumerable<string> expectedPackages = imageData.ImageVariant.HasFlag(DotNetImageVariant.AOT)
                ? GetAotDepsPackages(imageData)
                : GetRuntimeDepsPackages(imageData);

            if (imageData.IsDistroless)
            {
                expectedPackages = [..expectedPackages, ..GetDistrolessBasePackages(imageData)];
            }

            if (imageData.ImageVariant.HasFlag(DotNetImageVariant.Extra)
                || (imageRepo == DotNetImageRepo.SDK && imageData.Version.Major != 6 && imageData.Version.Major != 7))
            {
                expectedPackages = [..expectedPackages, ..GetExtraPackages(imageData)];
            }

            return expectedPackages.Distinct().OrderBy(s => s);
        }

        private static IEnumerable<string> GetDistrolessBasePackages(ProductImageData imageData) => imageData switch
            {
                { OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "distroless-packages-minimal",
                        "filesystem",
                        "mariner-release",
                        "prebuilt-ca-certificates",
                        "tzdata"
                    },
                { OS: OS.JammyChiseled } => new[]
                    {
                        "base-files"
                    },
                _ => throw new NotSupportedException()
            };

        private static IEnumerable<string> GetAotDepsPackages(ProductImageData imageData) => imageData switch
            {
                { OS: OS.Mariner20Distroless, Version: ImageVersion version }
                        when version.Major == 6 || version.Major == 7 => new[]
                    {
                        "e2fsprogs-libs",
                        "glibc",
                        "krb5",
                        "libgcc",
                        "openssl",
                        "openssl-libs",
                        "prebuilt-ca-certificates",
                        "zlib"
                    },
                { OS: OS.Mariner20, Version: ImageVersion version }
                        when version.Major == 6 || version.Major == 7 => new[]
                    {
                        "glibc",
                        "icu",
                        "krb5",
                        "libgcc",
                        "openssl-libs",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "glibc",
                        "libgcc",
                        "openssl-libs",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Jammy) => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libssl3",
                        "zlib1g"
                    },
                { OS: OS.Focal } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libgssapi-krb5-2",
                        "libicu66",
                        "libssl1.1",
                        "zlib1g"
                    },
                { OS: string os } when os.Contains(OS.Alpine) => new[]
                    {
                        "ca-certificates-bundle",
                        "libgcc",
                        "libssl3",
                        "zlib"
                    },
                { OS: OS.BookwormSlim } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libicu72",
                        "libssl3",
                        "tzdata",
                        "zlib1g"
                    },
                { OS: OS.BullseyeSlim } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1", // Listed as libgcc1 in the Dockerfile
                        "libgssapi-krb5-2",
                        "libicu67",
                        "libssl1.1",
                        "zlib1g"
                    },
                _ => throw new NotSupportedException()
            };

        private static IEnumerable<string> GetRuntimeDepsPackages(ProductImageData imageData) {
            string libstdcppPkgName = imageData.OS.Contains(OS.Mariner) || imageData.OS.Contains(OS.Alpine)
                ? "libstdc++"
                : "libstdc++6";
            return GetAotDepsPackages(imageData).Append(libstdcppPkgName);
        }

        private static IEnumerable<string> GetExtraPackages(ProductImageData imageData) => imageData switch
            {
                { IsDistroless: true, OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "icu",
                        "tzdata"
                    },
                { OS: OS.JammyChiseled } => new[]
                    {
                        "libicu70",
                        "tzdata"
                    },
                { OS: string os } when os.Contains(OS.Alpine) => new[]
                    {
                        "icu-data-full",
                        "icu-libs",
                        "tzdata"
                    },
                _ => Array.Empty<string>()
            };
    }
}
