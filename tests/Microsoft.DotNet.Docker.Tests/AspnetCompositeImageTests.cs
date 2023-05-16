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
    [Trait("Category", "aspnet-composite")]
    public class AspnetCompositeImageTests : CommonAspnetImageTests
    {
        public AspnetCompositeImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageType ImageType => DotNetImageType.Aspnet_Composite;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageType.Aspnet_Composite);

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAppScenario(ProductImageData imageData)
        {
            if (imageData.OSTag != OS.Alpine317Composite)
            {
                OutputHelper.WriteLine("Skipping because currently only Alpine composite"
                                     + " images are supported.");
                return ;
            }

            await base.VerifyAspnetAppScenario(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            EnvironmentVariableInfo compositeVersionVariableInfo = GetAspnetCompositeVersionVariableInfo(
                    imageData,
                    DockerHelper);

            base.VerifyAspnetEnvironmentVariables(imageData, compositeVersionVariableInfo);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPackageInstallation(ProductImageData imageData)
        {
            string[] expectedRpmPackages = GetExpectedRpmPackagesInstalled(imageData);
            base.VerifyAspnetPackageInstallation(imageData, expectedRpmPackages);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public override void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public override void VerifyShellNotInstalledForDistroless(ProductImageData imageData)
        {
            base.VerifyCommonShellNotInstalledForDistroless(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public override void VerifyNoSasToken(ProductImageData imageData)
        {
            base.VerifyCommonNoSasToken(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public override void VerifyDefaultUser(ProductImageData imageData)
        {
            base.VerifyCommonDefaultUser(imageData);
        }

        public static EnvironmentVariableInfo GetAspnetCompositeVersionVariableInfo(
                ProductImageData imageData,
                DockerHelper dockerHelper)
        {
            string version = imageData.GetProductVersion(DotNetImageType.Aspnet_Composite,
                                                         dockerHelper);

            return new EnvironmentVariableInfo("ASPNET_VERSION", version)
            {
                IsProductVersion = true
            };
        }

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
            {
                $"aspnetcore-runtime-composite-{imageData.VersionString}"
            };
    }
}
