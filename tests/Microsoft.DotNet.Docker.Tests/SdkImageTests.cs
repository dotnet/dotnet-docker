// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SdkImageTests
    {
        private readonly DockerHelper _dockerHelper;
        private readonly ITestOutputHelper _outputHelper;

        public SdkImageTests(ITestOutputHelper outputHelper)
        {
            _dockerHelper = new DockerHelper(outputHelper);
            _outputHelper = outputHelper;
        }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetImageData()
                // Filter the image data down to the distinct SDK OSes
                .Distinct(new SdkImageDataEqualityComparer())
                .Select(imageData => new object[] { imageData });
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifySdkImage_EnvironmentVariables(ImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(EnvironmentVariableInfo.GetCommonEnvironmentVariables());

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

        private class SdkImageDataEqualityComparer : IEqualityComparer<ImageData>
        {
            public bool Equals([AllowNull] ImageData x, [AllowNull] ImageData y)
            {
                if (x is null && y is null)
                {
                    return true;
                }

                if (x is null && !(y is null))
                {
                    return false;
                }

                if (!(x is null) && y is null)
                {
                    return false;
                }

                return x.VersionString == y.VersionString &&
                    x.SdkOS == y.SdkOS &&
                    x.Arch == y.Arch;
            }

            public int GetHashCode([DisallowNull] ImageData obj)
            {
                return $"{obj.VersionString}-{obj.SdkOS}-{obj.Arch}".GetHashCode();
            }
        }
    }
}
