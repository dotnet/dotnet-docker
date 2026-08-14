// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "blazor-gateway")]
public class BlazorGatewayImageTests(ITestOutputHelper outputHelper) : CommonRuntimeImageTests(outputHelper)
{
    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Blazor_Gateway;

    public static IEnumerable<object[]> GetImageData() =>
        TestData.GetBlazorGatewayImageData()
            .Select(imageData => new object[] { imageData });

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyEnvironmentVariables(ProductImageData imageData)
    {
        // The image relies on the base ASP.NET Core image's non-root port 8080 convention
        // (via the inherited ASPNETCORE_HTTP_PORTS=8080 variable) and must not set
        // ASPNETCORE_URLS, which would take precedence over ASPNETCORE_HTTP_PORTS.
        IEnumerable<EnvironmentVariableInfo> customVariables =
        [
            EnvironmentVariableInfo.Forbid("ASPNETCORE_URLS"),
        ];

        VerifyCommonEnvironmentVariables(imageData, customVariables);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyBasicScenario(ProductImageData imageData)
    {
        BlazorGatewayBasicScenario testScenario = new(imageData, DockerHelper, OutputHelper);
        await testScenario.ExecuteAsync();
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInsecureFiles(ProductImageData imageData) => VerifyCommonInsecureFiles(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyShellNotInstalledForDistroless(ProductImageData imageData)
        => VerifyCommonShellNotInstalledForDistroless(imageData);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyNoSasToken(ProductImageData imageData) => VerifyCommonNoSasToken(imageData);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyDefaultUser(ProductImageData imageData) => VerifyCommonDefaultUser(imageData);
}
