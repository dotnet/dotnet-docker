// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.Docker.Tests.TestScenarios;
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

        protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Runtime;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageRepo.Runtime);

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyFxDependentAppScenario(ProductImageData imageData)
        {
            using ConsoleAppScenario testScenario =
                new ConsoleAppScenario.FxDependent(imageData, DockerHelper, OutputHelper);
            await testScenario.ExecuteAsync();
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyTestProjectScenario(ProductImageData imageData)
        {
            using ConsoleAppScenario testScenario =
                new ConsoleAppScenario.TestProject(imageData, DockerHelper, OutputHelper);
            await testScenario.ExecuteAsync();
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyGlobalizationScenario(ProductImageData imageData) =>
            await VerifyGlobalizationScenarioBase(imageData);

        [WindowsImageTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyNLSScenario(ProductImageData imageData) =>
            await VerifyNlsScenarioBase(imageData);

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>
            {
                GetRuntimeVersionVariableInfo(ImageRepo, imageData, DockerHelper)
            };

            base.VerifyCommonEnvironmentVariables(imageData, variables);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledPackages(ProductImageData imageData)
        {
            VerifyInstalledPackagesBase(imageData, ImageRepo);
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

        public static EnvironmentVariableInfo GetRuntimeVersionVariableInfo(
            DotNetImageRepo imageRepo, ProductImageData imageData, DockerHelper dockerHelper)
        {
            string version = imageData.GetProductVersion(imageRepo, DotNetImageRepo.Runtime, dockerHelper);
            return new EnvironmentVariableInfo("DOTNET_VERSION", version)
            {
                IsProductVersion = true
            };
        }
    }
}
