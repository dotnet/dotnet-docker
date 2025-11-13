// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "aspire-dashboard")]
public class AspireDashboardImageTests(ITestOutputHelper outputHelper) : CommonRuntimeImageTests(outputHelper)
{
    private const string AppPath = "/app";

    private const int DashboardWebPort = 18888;

    private const int DashboardOtlpPort = 18889;

    private const int DashboardOtlpHttpPort = 18890;

    private const int DashboardMcpPort = 18891;

    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Aspire_Dashboard;

    public static IEnumerable<object[]> GetImageData() =>
        TestData.GetAspireDashboardImageData()
            .Select(imageData => new object[] { imageData });

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyDashboardEndpoint(ProductImageData imageData)
    {
        AspireDashboardBasicScenario testScenario = new(DashboardWebPort, imageData, DockerHelper, OutputHelper);
        await testScenario.ExecuteAsync();
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyEnvironmentVariables(ProductImageData imageData)
    {
        string baseUrl = "http://+";

        IEnumerable<EnvironmentVariableInfo> expectedVariables =
        [
            // Unset ASPNETCORE_HTTP_PORTS from base image
            new EnvironmentVariableInfo("ASPNETCORE_HTTP_PORTS", string.Empty),
            new EnvironmentVariableInfo("ASPNETCORE_URLS", $"{baseUrl}:{DashboardWebPort}"),
            new EnvironmentVariableInfo("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL", $"{baseUrl}:{DashboardOtlpPort}"),
            new EnvironmentVariableInfo("DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL", $"{baseUrl}:{DashboardOtlpHttpPort}"),
            new EnvironmentVariableInfo("DOTNET_DASHBOARD_MCP_ENDPOINT_URL", $"{baseUrl}:{DashboardMcpPort}"),
        ];

        string imageTag = imageData.GetImage(ImageRepo, DockerHelper);
        EnvironmentVariableInfo.Validate(expectedVariables, imageTag, imageData, DockerHelper);
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInstalledPackages(ProductImageData imageData)
    {
        ProductImageData expectedPackagesImageData = imageData;

        // Special case for Aspire Dashboard 9.0 images:
        // Aspire Dashboard 9.0 is based on .NET 8 since Azure Linux 3.0 does not yet have FedRAMP certification.
        // Remove workaround once https://github.com/dotnet/dotnet-docker/issues/5375 is fixed.
        if (imageData.VersionFamily == ImageVersion.V9_0)
        {
            expectedPackagesImageData = imageData with
            {
                Version = ImageVersion.V8_0
            };
        }

        // Aspire Dashboard image is based on an "extra" image, but doesn't have the "extra" qualifier itself, so we
        // need to make sure we compare the correct lists of packages.
        IEnumerable<string> expectedPackages =
            GetExpectedPackages(expectedPackagesImageData with { ImageVariant = DotNetImageVariant.Extra }, ImageRepo);
        IEnumerable<string> actualPackages =
            GetInstalledPackages(imageData, ImageRepo, [ AppPath ]);

        string imageName = imageData.GetImage(ImageRepo, DockerHelper, skipPull: true);
        ComparePackages(expectedPackages, actualPackages, imageData.IsDistroless, imageName, OutputHelper);
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
