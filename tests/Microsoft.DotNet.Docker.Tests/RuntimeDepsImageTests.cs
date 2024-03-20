// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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

            using WebApiAotScenario testScenario = new(imageData, DockerHelper, OutputHelper);
            await testScenario.ExecuteAsync();
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

        private static string GetOSReleaseInfo(
            ProductImageData imageData,
            DotNetImageRepo imageRepo,
            DockerHelper dockerHelper)
        {
            JsonNode output = GetSyftOutput("os-release-info", imageData, imageRepo, dockerHelper);
            JsonObject distro = (JsonObject)output["distro"];
            return (string)distro["version"];
        }


        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledPackages(ProductImageData imageData)
        {
            VerifyInstalledPackagesBase(imageData, ImageRepo, DockerHelper, OutputHelper);
        }

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            [ $"dotnet-runtime-deps-{imageData.VersionString}" ];
    }
}
