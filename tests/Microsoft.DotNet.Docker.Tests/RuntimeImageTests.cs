// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.DotNet.Docker.Tests.TestScenarios;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "runtime")]
public class RuntimeImageTests(ITestOutputHelper outputHelper) : CommonRuntimeImageTests(outputHelper)
{
    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Runtime;

    public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageRepo.Runtime);

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyFxDependentAppScenario(ProductImageData imageData)
    {
        ConsoleAppScenario testScenario = new ConsoleAppScenario.FxDependent(imageData, DockerHelper, OutputHelper);
        await testScenario.ExecuteAsync();
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyTestProjectScenario(ProductImageData imageData)
    {
        ConsoleAppScenario testScenario = new ConsoleAppScenario.TestProject(imageData, DockerHelper, OutputHelper);
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
        List<EnvironmentVariableInfo> variables =
        [
            GetRuntimeVersionVariableInfo(ImageRepo, imageData, DockerHelper)
        ];

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

    public static EnvironmentVariableInfo GetRuntimeVersionVariableInfo(
        DotNetImageRepo imageRepo, ProductImageData imageData, DockerHelper dockerHelper)
    {
        string version = imageData.GetProductVersion(imageRepo, DotNetImageRepo.Runtime, dockerHelper);
        return EnvironmentVariableInfo.Require("DOTNET_VERSION", version) with
        {
            IsProductVersion = true
        };
    }
}
