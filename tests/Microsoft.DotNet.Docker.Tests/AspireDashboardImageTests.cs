// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

    private const int DashboardOTLPPort = 18889;

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
        IEnumerable<EnvironmentVariableInfo> expectedVariables =
        [
            // Unset ASPNETCORE_HTTP_PORTS from base image
            new EnvironmentVariableInfo("ASPNETCORE_HTTP_PORTS", string.Empty),
            // These two URL environment variables should be in the more compact format, i.e. "http://+:18888", but need
            // to have a base URL of 0.0.0.0 due to a bug in the Aspire Dashboard.
            // Change the format when https://github.com/dotnet/dotnet-docker/issues/5190 is closed.
            new EnvironmentVariableInfo("ASPNETCORE_URLS", $"http://0.0.0.0:{DashboardWebPort}"),
            new EnvironmentVariableInfo("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL", $"http://0.0.0.0:{DashboardOTLPPort}"),
        ];

        string imageTag = imageData.GetImage(ImageRepo, DockerHelper);
        EnvironmentVariableInfo.Validate(expectedVariables, imageTag, imageData, DockerHelper);
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInstalledPackages(ProductImageData imageData)
    {
        // Aspire Dashboard image is based on an "extra" image, but doesn't have the "extra" qualifier itself, so we
        // need to make sure we compare the correct lists of packages.
        IEnumerable<string> expectedPackages = GetExpectedPackages(imageData with { ImageVariant = DotNetImageVariant.Extra }, ImageRepo);
        IEnumerable<string> actualPackages = GetInstalledPackages(imageData, ImageRepo, DockerHelper, [ AppPath ]);

        ComparePackages(expectedPackages, actualPackages, imageData.IsDistroless, OutputHelper);
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
