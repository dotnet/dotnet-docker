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
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Alpine39,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Disco,        Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Alpine39,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.Disco,        Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Disco,        Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Alpine39,     Arch = Arch.Arm64,    SdkOS = OS.Buster },
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
            new ImageData { Version = V2_1, OS = OS.NanoServer1803, Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1803, Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1803, Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.NanoServer1803, Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ImageData { Version = V3_1, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ImageData { Version = V3_1, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
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
        public void VerifySDKImage_PowerShellScenario(ImageData imageData)
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
                command: $"pwsh -c (Get-Childitem env:DOTNET_RUNNING_IN_CONTAINER).Value"
            );

            Assert.Equal(output, bool.TrueString, ignoreCase: true);
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
        public async Task VerifyAspNetCoreRuntimeImage_AppScenario(ImageData imageData)
        {
            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, _dockerHelper, _outputHelper, isWeb: true);
            await verifier.Execute();
        }
    }
}
