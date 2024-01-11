// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "runtime-deps")]
    public class RuntimeDepsImageTests : CommonRuntimeImageTests
    {
        public RuntimeDepsImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Runtime_Deps;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageRepo.Runtime_Deps);

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAotAppScenario(ProductImageData imageData)
        {
            if (!imageData.ImageVariant.HasFlag(DotNetImageVariant.AOT))
            {
                OutputHelper.WriteLine("Test is only relevant to AOT images.");
                return;
            }

            if (imageData.Arch == Arch.Arm)
            {
                OutputHelper.WriteLine("Skipping test due to https://github.com/dotnet/docker-tools/issues/1177. "
                        + "ImageBuilder is unable to queue arm32 AOT images together with the arm64 AOT SDKs. "
                        + "Re-enable once fixed.");
                return;
            }

            ImageScenarioVerifier verifier = new(imageData, DockerHelper, OutputHelper, isWeb: true);
            await verifier.Execute();
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            base.VerifyCommonEnvironmentVariables(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPackageInstallation(ProductImageData imageData)
        {
            if (!imageData.OS.Contains("cbl-mariner") || imageData.IsDistroless || imageData.Version.Major > 6)
            {
                return;
            }

            VerifyExpectedInstalledRpmPackages(
                imageData,
                GetExpectedRpmPackagesInstalled(imageData));
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyShellNotInstalledForDistroless(ProductImageData imageData)
        {
            base.VerifyCommonShellNotInstalledForDistroless(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyNoSasToken(ProductImageData imageData)
        {
            base.VerifyCommonNoSasToken(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDefaultUser(ProductImageData imageData)
        {
            VerifyCommonDefaultUser(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDistrolessOSReleaseInfo(ProductImageData imageData)
        {
            if (!imageData.IsDistroless)
            {
                OutputHelper.WriteLine("This test is only relevant to distroless images.");
                return;
            }
            Assert.NotEmpty(GetOSReleaseInfo(imageData, ImageRepo, DockerHelper));
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledPackages(ProductImageData imageData)
        {
            VerifyInstalledPackagesBase(imageData, ImageRepo, DockerHelper, OutputHelper);
        }

        /// <summary>
        /// Verifies that the packages installed are correct and scannable by security tools.
        /// </summary>
        internal static void VerifyInstalledPackagesBase(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper,
            ITestOutputHelper outputHelper)
        {
            IEnumerable<string> expectedPackages = GetExpectedPackages(imageData, imageRepo);
            IEnumerable<string> actualPackages = GetInstalledPackages(imageData, imageRepo, dockerHelper);

            outputHelper.WriteLine($"Expected Packages: [ {string.Join(", ", expectedPackages)} ]");

            // Verify we only include strictly necessary packages in distroless images
            if (imageData.IsDistroless)
            {
                outputHelper.WriteLine($"Actual Packages: [ {string.Join(", ", actualPackages)} ]");
                Assert.Equal(expectedPackages, actualPackages);
                return;
            }

            // Verify we have all of the .NET dependencies on non-distroless images
            IEnumerable<string> missingPackages = expectedPackages.Except(actualPackages);
            if (missingPackages.Any())
            {
                outputHelper.WriteLine($"Missing packages: [ {string.Join(", ", missingPackages)} ]");
            }
            Assert.Empty(missingPackages);
        }

        private static IEnumerable<string> GetInstalledPackages(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper)
        {
            JsonNode output = GetSyftOutput("package-info", imageData, imageRepo, dockerHelper);
            return ((JsonArray)output["artifacts"])
                .Select(artifact => artifact["name"]?.ToString());
        }

        private static string GetOSReleaseInfo(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper)
        {
            JsonNode output = GetSyftOutput("os-release-info", imageData, imageRepo, dockerHelper);
            JsonObject distro = (JsonObject)output["distro"];
            return (string)distro["version"];
        }

        private static JsonNode GetSyftOutput(
            string syftContainerName,
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper)
        {
            const string SyftImage = "anchore/syft:v0.97.1";
            dockerHelper.Pull(SyftImage);

            string imageToInspect = imageData.GetImage(imageRepo, dockerHelper);

            string outputContainerFilePath = "/artifacts/output.json";
            string tempDir = null;
            string outputContents = null;

            string[] args = [
                "packages",
                $"docker:{imageToInspect}",
                $"-o json={outputContainerFilePath}",
                // Ignore the dotnet folder, or else syft will report all the packages in the .NET Runtime. We only care
                // about the packages from the linux distro for this test.
                "--exclude /usr/share/dotnet"
            ];

            try
            {
                dockerHelper.Run(
                    SyftImage,
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

        private static IEnumerable<string> GetExpectedPackages(ProductImageData imageData, DotNetImageRepo imageRepo)
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

        internal static IEnumerable<string> GetExtraPackages(ProductImageData imageData) => imageData switch
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

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
                {
                    $"dotnet-runtime-deps-{imageData.VersionString}"
                };
    }
}
