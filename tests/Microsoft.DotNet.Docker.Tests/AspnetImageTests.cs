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
    public class AspnetImageTests
    {
        private readonly DockerHelper _dockerHelper;
        private readonly ITestOutputHelper _outputHelper;

        public AspnetImageTests(ITestOutputHelper outputHelper)
        {
            _dockerHelper = new DockerHelper(outputHelper);
            _outputHelper = outputHelper;
        }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetImageData()
                .Select(imageData => new object[] { imageData });
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAspNetImage_AppScenario(ImageData imageData)
        {
            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, _dockerHelper, _outputHelper, isWeb: true);
            await verifier.Execute();
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyAspNetImage_EnvironmentVariables(ImageData imageData)
        {
            EnvironmentVariableInfo.VerifyCommonRuntimeEnvironmentVariables(DotNetImageType.Aspnet, imageData, _dockerHelper);
        }
    }
}
