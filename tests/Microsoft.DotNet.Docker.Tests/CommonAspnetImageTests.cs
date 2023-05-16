// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public abstract class CommonAspnetImageTests : CommonRuntimeImageTests
{
    protected CommonAspnetImageTests(ITestOutputHelper outputHelper)
        : base(outputHelper) {}

    public abstract void VerifyInsecureFiles(ProductImageData imageData);
    public abstract void VerifyShellNotInstalledForDistroless(ProductImageData imageData);
    public abstract void VerifyNoSasToken(ProductImageData imageData);
    public abstract void VerifyDefaultUser(ProductImageData imageData);

    protected async Task VerifyAspnetAppScenario(ProductImageData imageData)
    {
        ImageScenarioVerifier verifier = new ImageScenarioVerifier(
                                            imageData: imageData,
                                            dockerHelper: DockerHelper,
                                            outputHelper: OutputHelper,
                                            isWeb: true);
        await verifier.Execute();
    }

#nullable enable
    protected void VerifyAspnetEnvironmentVariables(
            ProductImageData imageData,
            EnvironmentVariableInfo? aspnetVersionVariableInfo)
    {
        List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>
        {
            RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper)
        };

        if (aspnetVersionVariableInfo != null)
        {
            variables.Add(aspnetVersionVariableInfo);
        }

        base.VerifyCommonEnvironmentVariables(imageData, variables);
    }
#nullable disable

    protected void VerifyAspnetPackageInstallation(ProductImageData imageData,
                                                   string[] expectedRpmPackagesInstalled)
    {
        base.VerifyExpectedInstalledRpmPackages(
            imageData, expectedRpmPackagesInstalled
                       .Concat(RuntimeImageTests.GetExpectedRpmPackagesInstalled(imageData)));
    }
}
