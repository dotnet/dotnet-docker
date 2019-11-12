// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.DotNet.Docker.Tests.ImageVersion;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageTests
    {
        private static readonly ImageData[] s_linuxTestData =
        {
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Alpine39,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Alpine39,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Disco,        Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Alpine39,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.Disco,        Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Disco,        Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Alpine39,     Arch = Arch.Arm64,    SdkOS = OS.Buster },
            new ImageData { Version = V3_0, OS = OS.Alpine310,    Arch = Arch.Arm64,    SdkOS = OS.Buster },
            new ImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ImageData { Version = V3_1, OS = OS.Alpine310,    Arch = Arch.Arm64,    SdkOS = OS.Buster },
        };
        private static readonly ImageData[] s_windowsTestData =
        {
            new ImageData { Version = V2_1, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ImageData { Version = V3_1, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
        };

        private readonly DockerHelper _dockerHelper;
        private readonly ITestOutputHelper _outputHelper;

        public ImageTests(ITestOutputHelper outputHelper)
        {
            _dockerHelper = new DockerHelper(outputHelper);
            _outputHelper = outputHelper;
        }

        public static IEnumerable<object[]> GetImageData()
        {
            string archFilterPattern = GetFilterRegexPattern("IMAGE_ARCH_FILTER");
            string osFilterPattern = GetFilterRegexPattern("IMAGE_OS_FILTER");
            string versionFilterPattern = GetFilterRegexPattern("IMAGE_VERSION_FILTER");

            // Filter out test data that does not match the active architecture, os and version filters.
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxTestData : s_windowsTestData)
                .Where(imageData => archFilterPattern == null
                    || Regex.IsMatch(Enum.GetName(typeof(Arch), imageData.Arch), archFilterPattern, RegexOptions.IgnoreCase))
                .Where(imageData => osFilterPattern == null
                    || Regex.IsMatch(imageData.OS, osFilterPattern, RegexOptions.IgnoreCase))
                .Where(imageData => versionFilterPattern == null
                    || Regex.IsMatch(imageData.VersionString, versionFilterPattern, RegexOptions.IgnoreCase))
                .Select(imageData => new object[] { imageData });
        }

        private static string GetFilterRegexPattern(string filterEnvName)
        {
            string filter = Environment.GetEnvironmentVariable(filterEnvName);
            return filter != null ? $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$" : null;
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySdkImage_EnvironmentVariables(ImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(GetCommonEnvironmentVariables());

            string aspnetUrlsValue = imageData.Version.Major < 3 ? "http://+:80" : string.Empty;
            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", aspnetUrlsValue));
            variables.Add(new EnvironmentVariableInfo("DOTNET_USE_POLLING_FILE_WATCHER", "true"));
            variables.Add(new EnvironmentVariableInfo("NUGET_XMLDOC_MODE", "skip"));

            if (imageData.Version.Major >= 3
                && (DockerHelper.IsLinuxContainerModeEnabled || imageData.Version >= new Version("3.1")))
            {
                variables.Add(new EnvironmentVariableInfo("POWERSHELL_DISTRIBUTION_CHANNEL", allowAnyValue: true));
            }

            if (imageData.SdkOS.StartsWith(Tests.OS.AlpinePrefix))
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "false"));
                variables.Add(new EnvironmentVariableInfo("LC_ALL", "en_US.UTF-8"));
                variables.Add(new EnvironmentVariableInfo("LANG", "en_US.UTF-8"));
            }

            EnvironmentVariableInfo.Validate(variables, DotNetImageType.SDK, imageData, _dockerHelper);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySdkImage_PackageCache(ImageData imageData)
        {
            string verifyCacheCommand = null;
            if (imageData.Version.Major == 2)
            {
                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    verifyCacheCommand = "test -d /usr/share/dotnet/sdk/NuGetFallbackFolder";
                }
                else
                {
                    verifyCacheCommand = "CMD /S /C PUSHD \"C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder\"";
                }
            }
            else
            {
                _outputHelper.WriteLine(".NET Core SDK images >= 3.0 don't include a package cache.");
            }

            if (verifyCacheCommand != null)
            {
                // Simple check to verify the NuGet package cache was created
                _dockerHelper.Run(
                    image: imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                    command: verifyCacheCommand,
                    name: imageData.GetIdentifier("PackageCache"));
            }
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySDKImage_PowerShellScenario_DefaultUser(ImageData imageData)
        {
            VerifySDKImage_PowerShellScenario_Execute(imageData, null);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySDKImage_PowerShellScenario_NonDefaultUser(ImageData imageData)
        {
            if (imageData.Version < V3_1)
            {
                _outputHelper.WriteLine("PowerShell for non-default user does not exist in pre-3.1 images, skip testing");
                return;
            }

            var optRunArgs = "-u 12345:12345"; // Linux containers test as non-root user
            if (imageData.OS.Contains("nanoserver", StringComparison.OrdinalIgnoreCase))
            {
                // windows containers test as Admin, default execution is as ContainerUser
                optRunArgs = "-u ContainerAdministrator ";
            }

            VerifySDKImage_PowerShellScenario_Execute(imageData, optRunArgs);
        }

        private void VerifySDKImage_PowerShellScenario_Execute(ImageData imageData, string optionalArgs)
        {
            if (imageData.Version.Major < 3)
            {
                _outputHelper.WriteLine("PowerShell does not exist in pre-3.0 images, skip testing");
                return;
            }

            // A basic test which executes an arbitrary command to validate PS is functional
            string output = _dockerHelper.Run(
                image: imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                name: imageData.GetIdentifier($"pwsh"),
                optionalRunArgs: optionalArgs,
                command: $"pwsh -c (Get-Childitem env:DOTNET_RUNNING_IN_CONTAINER).Value"
            );

            Assert.Equal(output, bool.TrueString, ignoreCase: true);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyRuntimeDepsImage_EnvironmentVariables(ImageData imageData)
        {
            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                VerifyCommonRuntimeEnvironmentVariables(DotNetImageType.Runtime_Deps, imageData);
            }
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyRuntimeImage_AppScenario(ImageData imageData)
        {
            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, _dockerHelper, _outputHelper);
            await verifier.Execute();
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyRuntimeImage_EnvironmentVariables(ImageData imageData)
        {
            VerifyCommonRuntimeEnvironmentVariables(DotNetImageType.Runtime, imageData);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAspNetImage_AppScenario(ImageData imageData)
        {
            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, _dockerHelper, _outputHelper, isWeb: true);
            await verifier.Execute();
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyAspNetImage_EnvironmentVariables(ImageData imageData)
        {
            VerifyCommonRuntimeEnvironmentVariables(DotNetImageType.Aspnet, imageData);
        }

        private IEnumerable<EnvironmentVariableInfo> GetCommonEnvironmentVariables()
        {
            yield return new EnvironmentVariableInfo("DOTNET_RUNNING_IN_CONTAINER", "true");
        }

        private void VerifyCommonRuntimeEnvironmentVariables(DotNetImageType imageType, ImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(GetCommonEnvironmentVariables());
            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", "http://+:80"));

            if (imageData.OS.StartsWith(Tests.OS.AlpinePrefix))
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "true"));
            }

            EnvironmentVariableInfo.Validate(variables, imageType, imageData, _dockerHelper);
        }
    }
}
