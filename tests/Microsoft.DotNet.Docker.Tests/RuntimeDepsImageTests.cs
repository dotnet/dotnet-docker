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

        /// <summary>
        /// Verifies that the packages installed in distroless images are scannable by security tools.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDistrolessPackages(ProductImageData imageData)
        {
            if (!imageData.IsDistroless)
            {
                OutputHelper.WriteLine("This test is only relevant to distroless images.");
                return;
            }

            const string SyftImage = "anchore/syft:v0.87.1";
            DockerHelper.Pull(SyftImage);

            string imageName = imageData.GetImage(ImageRepo, DockerHelper);
            string output = DockerHelper.Run(
                SyftImage, "distroless-packages", $"packages docker:{imageName} -o json", useMountedDockerSocket: true);

            string[] basePackages = imageData switch
            {
                {  OS: string os, Version: ImageVersion version }
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

            string[] extraPackages = imageData switch
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
                _ => new string[0]
            };

            IEnumerable<string> expectedPackages = imageData switch
            {
                { ImageVariant: DotNetImageVariant.Extra } => basePackages.Concat(extraPackages).OrderBy(s => s),
                _ => basePackages
            };

            JsonNode node = JsonNode.Parse(output);
            if (node is null)
            {
                throw new JsonException($"Unable to parse the output as JSON:{Environment.NewLine}{output}");
            }

            IEnumerable<string> actualPackages = ((JsonArray)node["artifacts"])
                .Select(artifact => artifact["name"]?.ToString());
            
            OutputHelper.WriteLine($"Expected Packages: [ {String.Join(", ", expectedPackages)} ]");
            OutputHelper.WriteLine($"Actual Packages: [ {String.Join(", ", actualPackages)} ]");

            Assert.Equal(expectedPackages, actualPackages);

            // Verify the OS release info is available
            JsonObject distro = (JsonObject)node["distro"];
            Assert.NotEmpty((string)distro["version"]);
        }

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
                {
                    $"dotnet-runtime-deps-{imageData.VersionString}"
                };
    }
}
