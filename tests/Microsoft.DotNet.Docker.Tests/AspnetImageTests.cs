// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            base.VerifyCommonEnvironmentVariables(imageData);
        }
    }
}
