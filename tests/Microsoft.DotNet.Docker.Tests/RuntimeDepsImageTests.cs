// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.VisualBasic;
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
                return;
            }

            Assert.NotEmpty(GetOSReleaseInfo(imageData));
        }

        /// <summary>
        /// Verifies that the packages installed in distroless images are scannable by security tools.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDistrolessPackages(ProductImageData imageData)
        {
            if (!imageData.IsDistroless)
            {
                return;
            }

            IEnumerable<string> basePackages = GetExpectedPackages(imageData);
            IEnumerable<string> extraPackages = GetExpectedExtraPackages(imageData);

            IEnumerable<string> expectedPackages = imageData.ImageVariant switch
            {
                DotNetImageVariant.Extra => basePackages.Concat(extraPackages).OrderBy(s => s),
                _ => basePackages
            };

            IEnumerable<string> actualPackages = GetInstalledPackages(imageData);

            OutputHelper.WriteLine($"Expected Packages: [ {string.Join(", ", expectedPackages)} ]");
            OutputHelper.WriteLine($"Actual Packages: [ {string.Join(", ", actualPackages)} ]");

            Assert.Equal(expectedPackages, actualPackages);
        }

        private IEnumerable<string> GetInstalledPackages(ProductImageData imageData)
        {
            JsonNode output = GetSyftOutput("package-info", imageData);
            return ((JsonArray)output["artifacts"])
                .Select(artifact => artifact["name"]?.ToString());
        }

        private string GetOSReleaseInfo(ProductImageData imageData)
        {
            JsonNode output = GetSyftOutput("os-release-info", imageData);
            JsonObject distro = (JsonObject)output["distro"];
            return (string)distro["version"];
        }

        private JsonNode GetSyftOutput(string name, ProductImageData imageData)
        {
            const string SyftImage = "anchore/syft:v0.87.1";
            DockerHelper.Pull(SyftImage);

            string imageName = imageData.GetImage(ImageRepo, DockerHelper);
            string output = DockerHelper.Run(
                SyftImage, name, $"packages docker:{imageName} -o json", useMountedDockerSocket: true);

            return JsonNode.Parse(output)
                    ?? throw new JsonException($"Unable to parse the output as JSON:{Environment.NewLine}{output}");
        }

        internal static IEnumerable<string> GetExpectedExtraPackages(ProductImageData imageData) => imageData switch
            {
                { IsDistroless: true, OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "icu",
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
                _ => new string[0]
            };

        internal static IEnumerable<string> GetExpectedPackages(ProductImageData imageData) => imageData switch
            {
                { OS: string os, Version: ImageVersion version }
                        when (version.Major == 6 || version.Major == 7)
                             && os.Contains(OS.Mariner) => new[]
                    {
                        "distroless-packages-minimal",
                        "e2fsprogs-libs",
                        "filesystem",
                        "glibc",
                        "krb5",
                        "libgcc",
                        "libstdc++",
                        "mariner-release",
                        "openssl",
                        "openssl-libs",
                        "prebuilt-ca-certificates",
                        "tzdata",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "distroless-packages-minimal",
                        "filesystem",
                        "glibc",
                        "libgcc",
                        "libstdc++",
                        "mariner-release",
                        "openssl-libs",
                        "prebuilt-ca-certificates",
                        "tzdata", // tzdata is included by default on Distroless Mariner base image
                        "zlib"
                    },
                { OS: OS.JammyChiseled } => new[]
                    {
                        "base-files",
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libssl3",
                        "libstdc++6",
                        "zlib1g"
                    },
                _ => throw new NotImplementedException()
            };

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
                {
                    $"dotnet-runtime-deps-{imageData.VersionString}"
                };
    }
}
