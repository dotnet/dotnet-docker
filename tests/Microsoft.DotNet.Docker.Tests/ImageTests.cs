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
            new ImageData { Version = V1_0, OS = OS.Jessie,       Arch = Arch.Amd64,    SdkVersion = V1_1 },
            new ImageData { Version = V1_1, OS = OS.Jessie,       Arch = Arch.Amd64,    RuntimeDepsVersion = V1_0 },
            new ImageData { Version = V1_1, OS = OS.Stretch,      Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Alpine37,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.Alpine38,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.Alpine38,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V2_2, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.Alpine39,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.StretchSlim,  Arch = Arch.Arm64 },
            new ImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm64 },
        };
        private static readonly ImageData[] s_windowsTestData =
        {
            new ImageData { Version = V1_0, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64,  SdkVersion = V1_1 },
            new ImageData { Version = V1_1, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1709,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1803,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_1, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServerSac2016,  Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1709,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1803,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ImageData { Version = V2_2, OS = OS.NanoServer1809,     Arch = Arch.Arm },
            new ImageData { Version = V3_0, OS = OS.NanoServer1709,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1803,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ImageData { Version = V3_0, OS = OS.NanoServer1809,     Arch = Arch.Arm },
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
            if (imageData.Version.Major == 1)
            {
                if (!imageData.HasSdk)
                {
                    _outputHelper.WriteLine("No version specific SDK image exists to verify.");
                }
                else if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    verifyCacheCommand = "test -d /root/.nuget/packages";
                }
                else
                {
                    verifyCacheCommand = "CMD /S /C PUSHD \"C:\\Users\\ContainerAdministrator\\.nuget\\packages\"";
                }
            }
            else if (imageData.Version.Major == 2)
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
        public async Task VerifyRuntimeImage_AppScenario(ImageData imageData)
        {
            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, _dockerHelper, _outputHelper);
            await verifier.Execute();
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAspNetCoreRuntimeImage_AppScenario(ImageData imageData)
        {
            if (imageData.Version.Major == 1)
            {
                _outputHelper.WriteLine("1.* ASP.NET Core images reside in https://github.com/aspnet/aspnet-docker, skip testing");
                return;
            }

            ImageScenarioVerifier verifier = new ImageScenarioVerifier(imageData, _dockerHelper, _outputHelper, isWeb: true);
            await verifier.Execute();
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySdkImage_NugetCredentialProviderIsPresent(ImageData imageData)
        {
            const string linuxFilePath = "/usr/share/credentialprovider/plugins/netcore/CredentialProvider.Microsoft/CredentialProvider.Microsoft.dll";
            const string windowsFilePath = "C:\\Users\\Public\\credentialprovider\\plugins\\netcore\\CredentialProvider.Microsoft\\CredentialProvider.Microsoft.dll";
            
            string verifyCredProviderCommand = null;
            if (imageData.Version.Major == 2)
            {
                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    verifyCredProviderCommand = $"sh -c \"test -f $NUGET_PLUGIN_PATHS && test $NUGET_PLUGIN_PATHS = {linuxFilePath}\"";
                }
                else
                {
                    verifyCredProviderCommand = $"CMD /S /C IF NOT EXIST %NUGET_PLUGIN_PATHS% ( exit 1 ) ELSE ( IF %NUGET_PLUGIN_PATHS% NEQ {windowsFilePath} ( exit 1 ) ELSE ( exit 0 ) )";
                }
            }
            else
            {
                _outputHelper.WriteLine("Nuget Credential provider v2 is not supported in 1.* or 3.* images. Skipping test.");
                return;
            }
            if (verifyCredProviderCommand != null)
            {
                // Simple check to verify the NuGet credential provider was installed
                _dockerHelper.Run(
                    image: imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                    command: verifyCredProviderCommand,
                    name: imageData.GetIdentifier("NugetCredentialProviderPresent"));
            }
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySdkImage_NugetCredentialProviderRuns(ImageData imageData)
        {
            string verifyCredProviderRestore = null;
            string addPackageCommand = null;
            if (imageData.Version.Major == 2)
            {
                if (DockerHelper.IsLinuxContainerModeEnabled)
                {
                    addPackageCommand = $"sh -c \"dotnet new console --force --framework netcoreapp{imageData.Version}\"";

                    verifyCredProviderRestore = $"sh -c \"dotnet add package newtonsoft.json --no-restore && dotnet restore --no-cache && ls $HOME/.local/share/NuGet/plugins-cache | grep --quiet CredentialProvider.Microsoft.dll\"";
                }
                else
                {
                    addPackageCommand = $"CMD /S /C dotnet new console --force --framework netcoreapp{imageData.Version}";

                    verifyCredProviderRestore = $"CMD /S /C dotnet add package newtonsoft.json --no-restore && dotnet restore --no-cache && dir %localappdata%\\nuget\\plugins-cache | findstr CredentialProvider.Microsoft.dll";
                }

                if (addPackageCommand != null) {
                    _dockerHelper.Run(
                                image: imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                                command: addPackageCommand,
                                name: imageData.GetIdentifier("NugetCredentialProviderRuns"),
                        workdir: "/test",
                        publishArgs: " -v demo1:/test");
                }

                if (verifyCredProviderRestore != null) {
                    _dockerHelper.Run(
                                image: imageData.GetImage(DotNetImageType.SDK, _dockerHelper),
                                command: verifyCredProviderRestore,
                                name: imageData.GetIdentifier("NugetCredentialProviderRuns"),
                        workdir: "/test",
                        publishArgs: " -v demo1:/test");
                }
            }
            else
            {
                _outputHelper.WriteLine("Nuget Credential provider v2 is not supported in 1.* or 3.* images. Skipping test.");
                return;
            }
        }
    }
}
