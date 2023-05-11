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
    public class AspnetImageTests : CommonAspnetImageTests
    {
        public AspnetImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageType ImageType => DotNetImageType.Aspnet;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageType.Aspnet);

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public override async Task VerifyAppScenario(ProductImageData imageData)
        {
            if (imageData.IsArm && imageData.OS == OS.Jammy)
            {
                OutputHelper.WriteLine(
                    "Skipping test due to https://github.com/dotnet/runtime/issues/66310. Re-enable when fixed.");
                return;
            }

            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, DockerHelper, OutputHelper, isWeb: true);
            await verifier.Execute();
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public override void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>
            {
                RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper)
            };

            EnvironmentVariableInfo aspnetVersionVariableInfo = GetAspnetVersionVariableInfo(imageData, DockerHelper);
            if (aspnetVersionVariableInfo != null)
            {
                variables.Add(aspnetVersionVariableInfo);
            }

            base.VerifyCommonEnvironmentVariables(imageData, variables);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public override void VerifyPackageInstallation(ProductImageData imageData)
        {
            if (!imageData.OS.Contains("cbl-mariner") || imageData.IsDistroless || imageData.Version.Major > 6)
            {
                return;
            }

            VerifyExpectedInstalledRpmPackages(
                imageData,
                GetExpectedRpmPackagesInstalled(imageData)
                    .Concat(RuntimeImageTests.GetExpectedRpmPackagesInstalled(imageData)));
        }

        public static EnvironmentVariableInfo GetAspnetVersionVariableInfo(ProductImageData imageData, DockerHelper dockerHelper)
        {
            string version = imageData.GetProductVersion(DotNetImageType.Aspnet, dockerHelper);
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
