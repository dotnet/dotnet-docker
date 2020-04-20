// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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

        protected override DotNetImageType ImageType => DotNetImageType.Aspnet;

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAppScenario(ProductImageData imageData)
        {
            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, DockerHelper, OutputHelper, isWeb: true);
            await verifier.Execute();
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();

            EnvironmentVariableInfo aspnetVersionVariableInfo = GetAspnetVersionVariableInfo(imageData, DockerHelper);
            if (aspnetVersionVariableInfo != null)
            {
                variables.Add(aspnetVersionVariableInfo);
            }

            if (imageData.Version.Major >= 5)
            {
                variables.Add(RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper));
            }

            base.VerifyCommonEnvironmentVariables(imageData, variables);
        }

        public static EnvironmentVariableInfo GetAspnetVersionVariableInfo(ProductImageData imageData, DockerHelper dockerHelper)
        {
            string versionEnvName = null;
            if (imageData.Version.Major == 2 && DockerHelper.IsLinuxContainerModeEnabled)
            {
                versionEnvName = "ASPNETCORE_VERSION";
            }
            else if (imageData.Version.Major >= 5)
            {
                versionEnvName = "ASPNET_VERSION";
            }

            if (versionEnvName != null)
            {
                string version = imageData.GetProductVersion(DotNetImageType.Aspnet, dockerHelper);
                return new EnvironmentVariableInfo(versionEnvName, version);
            }

            return null;
        }
    }
}
