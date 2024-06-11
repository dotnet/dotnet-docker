// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "aspnet")]
    public class AspnetImageTests : CommonRuntimeImageTests
    {
        public AspnetImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Aspnet;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageRepo.Aspnet);

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAppScenario(ProductImageData imageData)
        {
            if (imageData.IsArm && imageData.OS == OS.Jammy)
            {
                OutputHelper.WriteLine(
                    "Skipping test due to https://github.com/dotnet/runtime/issues/66310. Re-enable when fixed.");
                return;
            }

            string[] unsupportedWindowsVersions = [ OS.ServerCoreLtsc2019, OS.NanoServer1809 ];
            if (imageData.Version.Major == 9 && unsupportedWindowsVersions.Contains(imageData.OS))
            {
                OutputHelper.WriteLine(
                    "Skipping test due to https://github.com/dotnet/msbuild/issues/9662. Re-enable when fixed.");
                return;
            }

            using ProjectTemplateTestScenario scenario = imageData.ImageVariant.HasFlag(DotNetImageVariant.Composite)
                ? new WebScenarioComposite(imageData, DockerHelper, OutputHelper)
                : new WebScenario(imageData, DockerHelper, OutputHelper);
            await scenario.ExecuteAsync();
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new();

            // Skip runtime version check due to https://github.com/dotnet/dotnet-docker/issues/4834.
            // Re-enable when fixed.
            if (imageData.ImageVariant != DotNetImageVariant.Composite)
            {
                variables.Add(RuntimeImageTests.GetRuntimeVersionVariableInfo(ImageRepo, imageData, DockerHelper));
            }

            EnvironmentVariableInfo aspnetVersionVariableInfo = GetAspnetVersionVariableInfo(ImageRepo, imageData, DockerHelper);
            if (aspnetVersionVariableInfo != null)
            {
                variables.Add(aspnetVersionVariableInfo);
            }

            base.VerifyCommonEnvironmentVariables(imageData, variables);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPackageInstallation(ProductImageData imageData)
        {
            VerifyExpectedInstalledRpmPackages(
                    imageData,
                    GetExpectedRpmPackagesInstalled(imageData)
                        .Concat(RuntimeImageTests.GetExpectedRpmPackagesInstalled(imageData)));
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledPackages(ProductImageData imageData)
        {
            ProductImageTests.VerifyInstalledPackagesBase(imageData, ImageRepo, DockerHelper, OutputHelper);
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

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyNoSasToken(ProductImageData imageData)
        {
            base.VerifyCommonNoSasToken(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDefaultUser(ProductImageData imageData)
        {
            VerifyCommonDefaultUser(imageData);
        }

        public static EnvironmentVariableInfo GetAspnetVersionVariableInfo(
            DotNetImageRepo imageRepo, ProductImageData imageData, DockerHelper dockerHelper)
        {
            string version = imageData.GetProductVersion(imageRepo, DotNetImageRepo.Aspnet, dockerHelper);

            return new EnvironmentVariableInfo("ASPNET_VERSION", version)
            {
                IsProductVersion = true
            };
        }

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
                {
                    $"aspnetcore-runtime-{imageData.VersionString}"
                };
    }
}
