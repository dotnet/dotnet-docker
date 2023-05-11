// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

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
    public override async Task VerifyAppScenario(ProductImageData imageData)
    {
        if (imageData.OSTag != OS.Alpine317Composite)
        {
            OutputHelper.WriteLine("Skipping because currently only Alpine composite"
                                 + " images are supported.");
            return ;
        }

        ImageScenarioVerifier verifier = new ImageScenarioVerifier(
                                            imageData: imageData,
                                            dockerHelper: DockerHelper,
                                            outputHelper: OutputHelper,
                                            isWeb: true);
        await verifier.Execute();
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public override void VerifyEnvironmentVariables(ProductImageData imageData)
    {
        List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>
        {
            RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper)
        };

        EnvironmentVariableInfo compositeVersionVariableInfo = GetAspnetCompositeVersionVariableInfo(
                imageData,
                DockerHelper);

        if (compositeVersionVariableInfo != null)
        {
            variables.Add(compositeVersionVariableInfo);
        }

        base.VerifyCommonEnvironmentVariables(imageData, variables);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public override void VerifyPackageInstallation(ProductImageData imageData)
    {
        VerifyExpectedInstalledRpmPackages(
                imageData,
                GetExpectedRpmPackagesInstalled(imageData)
                .Concat(RuntimeDepsImageTests.GetExpectedRpmPackagesInstalled(imageData)));
    }

    public static EnvironmentVariableInfo GetAspnetCompositeVersionVariableInfo(
            ProductImageData imageData,
            DockerHelper dockerHelper)
    {
        string version = imageData.GetProductVersion(DotNetImageType.Aspnet_Composite,
                                                     dockerHelper);

        return new EnvironmentVariableInfo("ASPNET_VERSION", version)
        {
            IsProductVersion = true
        };
    }

    internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
        new string[]
        {
            $"aspnetcore-runtime-composite-{imageData.VersionString}"
        };
}

