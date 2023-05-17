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
            await base.VerifyAspnetAppScenario(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            EnvironmentVariableInfo compositeVersionVariableInfo = AspnetImageTests.GetAspnetVersionVariableInfo(
                                                                       imageData,
                                                                       DockerHelper,
                                                                       isComposite: true);

            base.VerifyAspnetEnvironmentVariables(imageData, compositeVersionVariableInfo);
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
    }
}
