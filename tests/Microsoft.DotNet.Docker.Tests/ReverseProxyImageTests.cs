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

[Trait("Category", "yarp")]
public class YarpImageTests(ITestOutputHelper outputHelper) : CommonRuntimeImageTests(outputHelper)
{
    private const string AppPath = "/app";

    private const int YarpWebPort = 5000;


    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Yarp;

    public static IEnumerable<object[]> GetImageData() =>
        TestData.GetYarpImageData()
            .Select(imageData => new object[] { imageData });

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifyBasicScenario(ProductImageData imageData)
    {
        OutputHelper.WriteLine("Skipping VerifyBasicScenario test due to known issues."
            + " Re-enable this test once the following issue is resolved:"
            + " https://github.com/dotnet/dotnet-docker/issues/6639");

        // YarpBasicScenario testScenario = new(YarpWebPort, imageData, DockerHelper, OutputHelper);
        // await testScenario.ExecuteAsync();
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyEnvironmentVariables(ProductImageData imageData)
    {
        IEnumerable<EnvironmentVariableInfo> expectedVariables =
        [
            // Unset ASPNETCORE_HTTP_PORTS from base image
            new EnvironmentVariableInfo("ASPNETCORE_HTTP_PORTS", string.Empty),
            new EnvironmentVariableInfo("ASPNETCORE_URLS", "http://+:5000"),
        ];

        string imageTag = imageData.GetImage(ImageRepo, DockerHelper);
        EnvironmentVariableInfo.Validate(expectedVariables, imageTag, imageData, DockerHelper);
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInsecureFiles(ProductImageData imageData)
    {
        OutputHelper.WriteLine("Skipping VerifyInsecureFiles test due to known issues."
            + " Re-enable this test once the following issue is resolved:"
            + " https://github.com/dotnet/dotnet-docker/issues/6638");

        // VerifyCommonInsecureFiles(imageData);
    }

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
