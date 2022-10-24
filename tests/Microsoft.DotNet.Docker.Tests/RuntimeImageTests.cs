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
    [Trait("Category", "runtime")]
    public class RuntimeImageTests : CommonRuntimeImageTests
    {
        public RuntimeImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageType ImageType => DotNetImageType.Runtime;

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

            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, DockerHelper, OutputHelper);
            await verifier.Execute();
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();

            if (imageData.Version.Major >= 5)
            {
                variables.Add(GetRuntimeVersionVariableInfo(imageData, DockerHelper));
            }

            base.VerifyCommonEnvironmentVariables(imageData, variables);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPackageInstallation(ProductImageData imageData)
        {
            if (!imageData.OS.Contains("cbl-mariner") || imageData.IsDistroless || imageData.Version.Major > 6)
            {
                return;
            }

            VerifyExpectedInstalledRpmPackages(
                imageData,
                GetExpectedRpmPackagesInstalled(imageData)
                    .Concat(RuntimeDepsImageTests.GetExpectedRpmPackagesInstalled(imageData)));
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

        public static EnvironmentVariableInfo GetRuntimeVersionVariableInfo(ProductImageData imageData, DockerHelper dockerHelper)
        {
            string version = imageData.GetProductVersion(DotNetImageType.Runtime, dockerHelper);
            return new EnvironmentVariableInfo("DOTNET_VERSION", version)
            {
                IsProductVersion = true
            };
        }

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
                {
                    "dotnet-host",
                    $"dotnet-hostfxr-{imageData.VersionString}",
                    $"dotnet-runtime-{imageData.VersionString}",
                };
    }
}
