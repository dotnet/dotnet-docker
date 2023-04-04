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

        protected override DotNetImageType ImageType => DotNetImageType.Runtime_Deps;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageType.Runtime_Deps);

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

            if (imageData.OS == OS.Mariner10Distroless)
            {
                OutputHelper.WriteLine("Scanning support not implemented for Mariner 1.0.");
                return;
            }

            const string SyftImage = "anchore/syft:v0.75.0";
            DockerHelper.Pull(SyftImage);

            string imageName = imageData.GetImage(ImageType, DockerHelper);
            string output = DockerHelper.Run(
                SyftImage, "distroless-packages", $"packages docker:{imageName} -o json", useMountedDockerSocket: true);

            string[] expectedPackages;
            if (imageData.OS.Contains(OS.Mariner))
            {
                expectedPackages = new[]
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
                };
            }
            else if (imageData.OS == OS.JammyChiseled)
            {
                OutputHelper.WriteLine("Package scanning support not implemented for Chiseled Ubuntu images.");
                expectedPackages = Array.Empty<string>();
            }
            else
            {
                throw new NotImplementedException();
            }

            JsonNode node = JsonNode.Parse(output);
            if (node is null)
            {
                throw new JsonException($"Unable to parse the output as JSON:{Environment.NewLine}{output}");
            }

            string[] actualPackages = ((JsonArray)node["artifacts"])
                .Select(artifact => artifact["name"]?.ToString())
                .ToArray();

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
