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
namespace Microsoft.DotNet.Docker.Tests
{
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
                .Where(imageData => !imageData.IsDistroless)
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
            string command = "(Invoke-WebRequest -Uri 'https://raw.githubusercontent.com/PowerShell/PowerShell/master/tools/metadata.json').Content";
            string output = PowerShellScenario_Execute(imageData, command, string.Empty);

            Assert.Contains("\"StableReleaseTag\":", output);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPowerShellScenario_PSDistributionEnvVar(ProductImageData imageData)
        {
            string command = "$env:POWERSHELL_DISTRIBUTION_CHANNEL";
            string output = PowerShellScenario_Execute(imageData, command, string.Empty);

            Assert.StartsWith("PSDocker-", output, StringComparison.OrdinalIgnoreCase);
        }

        private string PowerShellScenario_Execute(ProductImageData imageData, string command, string? optionalArgs = null)
        {
            string image = imageData.GetImage(DotNetImageRepo.SDK, DockerHelper);

            if (!imageData.SupportsPowerShell)
            {
                OutputHelper.WriteLine($"PowerShell is not supproted on {image}");
                return string.Empty;
            }

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
}
