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
    public class AspnetCompositeImageTests : CommonRuntimeImageTests
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

            ImageScenarioVerifier verifier = new ImageScenarioVerifier(
                                                imageData: imageData,
                                                dockerHelper: DockerHelper,
                                                outputHelper: OutputHelper,
                                                isWeb: true);
            await verifier.Execute();
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>
            {
                RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper)
            };

            EnvironmentVariableInfo compositeVersionVariableInfo = GetAspnetCompositeVersionVariableInfo(
                    imageData,
                    DockerHelper);

            if (compositeVersionVariableInfo != null)
            {
                variables.Add(compositeVersionVariableInfo);
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
