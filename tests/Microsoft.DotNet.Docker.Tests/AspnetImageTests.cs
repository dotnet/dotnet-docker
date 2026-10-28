// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.DotNet.Docker.Tests.TestScenarios;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "aspnet")]
public class AspnetImageTests(ITestOutputHelper outputHelper) : CommonRuntimeImageTests(outputHelper)
{
    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Aspnet;

    public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageRepo.Aspnet);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyFxDependentAppScenario(ProductImageData imageData)
    {
        if (Config.IsNightlyRepo && imageData.ImageVariant.HasFlag(DotNetImageVariant.Composite))
        {
            OutputHelper.WriteLine("Skip test due to https://github.com/dotnet/dotnet-docker/issues/4834");
            return;
        }

        WebScenario scenario = new WebScenario.FxDependent(imageData, DockerHelper, OutputHelper);
        await scenario.ExecuteAsync();
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyGlobalizationScenario(ProductImageData imageData)
    {
        if (Config.IsNightlyRepo && imageData.ImageVariant.HasFlag(DotNetImageVariant.Composite))
        {
            OutputHelper.WriteLine("Skip test due to https://github.com/dotnet/dotnet-docker/issues/4834");
            return;
        }

        await VerifyGlobalizationScenarioBase(imageData);
    }

    [WindowsImageTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyNLSScenario(ProductImageData imageData) =>
        await VerifyNlsScenarioBase(imageData);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyEnvironmentVariables(ProductImageData imageData)
    {
        List<EnvironmentVariableInfo> variables = [];

        EnvironmentVariableInfo runtimeVersionInfo =
            RuntimeImageTests.GetRuntimeVersionVariableInfo(ImageRepo, imageData, DockerHelper);

        // Skip runtime version check due to https://github.com/dotnet/dotnet-docker/issues/4834.
        // Re-enable when fixed.
        if (imageData.ImageVariant.HasFlag(DotNetImageVariant.Composite))
        {
            runtimeVersionInfo = runtimeVersionInfo with { AllowAnyValue = true };
        }

        variables.Add(runtimeVersionInfo);

        EnvironmentVariableInfo aspnetVersionVariableInfo =
            GetAspnetVersionVariableInfo(ImageRepo, imageData, DockerHelper);
        if (aspnetVersionVariableInfo != null)
        {
            variables.Add(aspnetVersionVariableInfo);
        }

        VerifyCommonEnvironmentVariables(imageData, variables);
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInstalledPackages(ProductImageData imageData) => VerifyInstalledPackagesBase(imageData, ImageRepo);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInsecureFiles(ProductImageData imageData) => VerifyCommonInsecureFiles(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyShellNotInstalledForDistroless(ProductImageData imageData) => VerifyCommonShellNotInstalledForDistroless(imageData);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyNoSasToken(ProductImageData imageData) => VerifyCommonNoSasToken(imageData);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyDefaultUser(ProductImageData imageData) => VerifyCommonDefaultUser(imageData);

    public static EnvironmentVariableInfo GetAspnetVersionVariableInfo(
        DotNetImageRepo imageRepo, ProductImageData imageData, DockerHelper dockerHelper)
    {
        string version = imageData.GetProductVersion(imageRepo, DotNetImageRepo.Aspnet, dockerHelper);

        return EnvironmentVariableInfo.Require("ASPNET_VERSION", version) with
        {
            IsProductVersion = true
        };
    }
}
