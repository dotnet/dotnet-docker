// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using SharpCompress.Common;
using SharpCompress.Readers;
using Xunit;
using Xunit.Abstractions;

#nullable enable
namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "sdk")]
public class PowerShellTests : ProductImageTests
{
    public PowerShellTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.SDK;

    public static IEnumerable<object[]> GetImageData()
    {
        return TestData.GetImageData(DotNetImageRepo.SDK)
            .Where(imageData => imageData.SupportsPowerShell)
            .Select(imageData => new object[] { imageData });
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyPowerShellScenario_DefaultUser(ProductImageData imageData)
    {
        // An arbitrary command to validate PS is functional
        string command = "(Get-Childitem env:DOTNET_RUNNING_IN_CONTAINER).Value";
        string output = PowerShellScenario_Execute(imageData, command, string.Empty);
        Assert.Equal(output, bool.TrueString, ignoreCase: true);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyPowerShellScenario_NonDefaultUser(ProductImageData imageData)
    {
        // An arbitrary command to validate PS is functional
        string command = "(Get-Childitem env:DOTNET_RUNNING_IN_CONTAINER).Value";
        string optRunArgs = "-u 12345:12345"; // Linux containers test as non-root user

        if (!DockerHelper.IsLinuxContainerModeEnabled)
        {
            // windows containers test as Admin, default execution is as ContainerUser
            optRunArgs = "-u ContainerAdministrator ";
        }

        string output = PowerShellScenario_Execute(imageData, command, optRunArgs);
        Assert.Equal(output, bool.TrueString, ignoreCase: true);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyPowerShellScenario_PSVersion(ProductImageData imageData)
    {
        string command = "$PSVersionTable.PSVersion.ToString()";
        string output = PowerShellScenario_Execute(imageData, command, string.Empty);

        Assert.StartsWith("7", output, StringComparison.OrdinalIgnoreCase);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyPowerShellScenario_InvokeWebRequest(ProductImageData imageData)
    {
        string command = "(Invoke-WebRequest -Uri 'https://graph.microsoft.com').StatusCode";
        string output = PowerShellScenario_Execute(imageData, command, string.Empty);

        Assert.Equal("200", output);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyPowerShellScenario_PSDistributionEnvVar(ProductImageData imageData)
    {
        string imageName = imageData.GetImage(ImageRepo, DockerHelper);
        IDictionary<string, string> envVars = DockerHelper.GetEnvironmentVariables(imageName);
        string powershellEnvVar = "POWERSHELL_DISTRIBUTION_CHANNEL";
        Assert.True(envVars.ContainsKey(powershellEnvVar));
        envVars.TryGetValue(powershellEnvVar, out string? envVarValue);
        Assert.NotNull(envVarValue);
        Assert.StartsWith("PSDocker-", envVarValue, StringComparison.OrdinalIgnoreCase);
    }

    private string PowerShellScenario_Execute(ProductImageData imageData, string command, string? optionalArgs = null)
    {
        string image = imageData.GetImage(DotNetImageRepo.SDK, DockerHelper);

        // The test executes a provided command to validate PS functionality
        string output = DockerHelper.Run(
            image: image,
            name: imageData.GetIdentifier($"pwsh"),
            optionalRunArgs: optionalArgs,
            command: $"pwsh -nologo -noprofile -c {command}"
        );

        return output;
    }
}
