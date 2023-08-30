// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

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

            if (imageData.IsDistroless)
            {
                outputHelper.WriteLine($"Actual Packages: [ {string.Join(", ", actualPackages)} ]");
                Assert.Equal(expectedPackages, actualPackages);
            }
            else
            {
                IEnumerable<string> missingPackages = expectedPackages.Except(actualPackages);
                if (missingPackages.Count() > 0)
                {
                    outputHelper.WriteLine($"Missing packages: [ {string.Join(", ", missingPackages)} ]");
                }
                Assert.Empty(missingPackages);
            }
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
            string name,
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper)
        {
            const string SyftImage = "anchore/syft:v0.87.1";
            dockerHelper.Pull(SyftImage);

            string imageName = imageData.GetImage(imageRepo, dockerHelper);
            string output = dockerHelper.Run(
                SyftImage, name, $"packages docker:{imageName} -o json",
                useMountedDockerSocket: true);

            return JsonNode.Parse(output)
                    ?? throw new JsonException($"Unable to parse the output as JSON:{Environment.NewLine}{output}");
        }

        private static IEnumerable<string> GetExpectedPackages(ProductImageData imageData, DotNetImageRepo imageRepo)
        {
            IEnumerable<string> expectedPackages = GetRuntimeDepsPackages(imageData);

            if (imageData.IsDistroless)
            {
                expectedPackages = expectedPackages.Concat(GetDistrolessBasePackages(imageData));
            }
            if (imageData.ImageVariant.HasFlag(DotNetImageVariant.Extra)
                || (imageRepo == DotNetImageRepo.SDK && imageData.Version.Major != 6 && imageData.Version.Major != 7))
            {
                expectedPackages = expectedPackages.Concat(GetExtraPackages(imageData));
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

        private static IEnumerable<string> GetRuntimeDepsPackages(ProductImageData imageData) => imageData switch
            {
                { OS: OS.Mariner20Distroless, Version: ImageVersion version }
                        when version.Major == 6 || version.Major == 7 => new[]
                    {
                        "e2fsprogs-libs",
                        "glibc",
                        "krb5",
                        "libgcc",
                        "libstdc++",
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
                        "libstdc++",
                        "openssl-libs",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "glibc",
                        "libgcc",
                        "libstdc++",
                        "openssl-libs",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Jammy) => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libssl3",
                        "libstdc++6",
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
                        "libstdc++6",
                        "zlib1g"
                    },
                { OS: string os } when os.Contains(OS.Alpine) => new[]
                    {
                        "ca-certificates-bundle",
                        "libgcc",
                        "libssl3",
                        "libstdc++",
                        "zlib"
                    },
                { OS: OS.BookwormSlim } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1", 
                        "libicu72",
                        "libssl3",
                        "libstdc++6",
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
                        "libstdc++6",
                        "zlib1g"
                    },
                _ => throw new NotSupportedException()
            };

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
