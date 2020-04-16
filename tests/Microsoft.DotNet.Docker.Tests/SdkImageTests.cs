// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Archives.Tar;
using SharpCompress.Readers;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "sdk")]
    public class SdkImageTests : ProductImageTests
    {
        private static readonly Dictionary<string, IEnumerable<string>> sdkContentsCache =
            new Dictionary<string, IEnumerable<string>>();

        public SdkImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageType ImageType => DotNetImageType.SDK;

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetImageData()
                // Filter the image data down to the distinct SDK OSes
                .Distinct(new SdkImageDataEqualityComparer())
                .Select(imageData => new object[] { imageData });
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(GetCommonEnvironmentVariables());

            string aspnetUrlsValue = imageData.Version.Major < 3 ? "http://+:80" : string.Empty;
            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", aspnetUrlsValue));
            variables.Add(new EnvironmentVariableInfo("DOTNET_USE_POLLING_FILE_WATCHER", "true"));
            variables.Add(new EnvironmentVariableInfo("NUGET_XMLDOC_MODE", "skip"));

            if (imageData.Version.Major >= 3)
            {
                variables.Add(new EnvironmentVariableInfo("POWERSHELL_DISTRIBUTION_CHANNEL", allowAnyValue: true));
            }

            if (imageData.Version.Major >= 5 || (imageData.Version.Major == 2 && DockerHelper.IsLinuxContainerModeEnabled))
            {
                string version = imageData.GetProductVersion(ImageType, DockerHelper);
                variables.Add(new EnvironmentVariableInfo("DOTNET_SDK_VERSION", version));
            }

            if (imageData.Version.Major >= 5 ||
                (imageData.Version.Major >= 3 &&
                    (imageData.SdkOS.StartsWith(OS.AlpinePrefix) || !DockerHelper.IsLinuxContainerModeEnabled)))
            {
                variables.Add(AspnetImageTests.GetAspnetVersionVariableInfo(imageData, DockerHelper));
                variables.Add(RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper));
            }

            if (imageData.SdkOS.StartsWith(OS.AlpinePrefix))
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "false"));
                variables.Add(new EnvironmentVariableInfo("LC_ALL", "en_US.UTF-8"));
                variables.Add(new EnvironmentVariableInfo("LANG", "en_US.UTF-8"));
            }

            EnvironmentVariableInfo.Validate(variables, DotNetImageType.SDK, imageData, DockerHelper);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPackageCache(ProductImageData imageData)
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
                OutputHelper.WriteLine(".NET Core SDK images >= 3.0 don't include a package cache.");
            }

            if (verifyCacheCommand != null)
            {
                // Simple check to verify the NuGet package cache was created
                DockerHelper.Run(
                    image: imageData.GetImage(DotNetImageType.SDK, DockerHelper),
                    command: verifyCacheCommand,
                    name: imageData.GetIdentifier("PackageCache"));
            }
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPowerShellScenario_DefaultUser(ProductImageData imageData)
        {
            PowerShellScenario_Execute(imageData, null);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPowerShellScenario_NonDefaultUser(ProductImageData imageData)
        {
            var optRunArgs = "-u 12345:12345"; // Linux containers test as non-root user
            if (imageData.OS.Contains("nanoserver", StringComparison.OrdinalIgnoreCase))
            {
                // windows containers test as Admin, default execution is as ContainerUser
                optRunArgs = "-u ContainerAdministrator ";
            }

            PowerShellScenario_Execute(imageData, optRunArgs);
        }

        /// <summary>
        /// Verifies that the dotnet folder contents of an SDK container match the contents in the official SDK archive file.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyDotnetFolderContents(ProductImageData imageData)
        {
            if (!(imageData.Version.Major >= 5 ||
                (imageData.Version.Major >= 3 &&
                    (imageData.SdkOS.StartsWith(OS.AlpinePrefix) || !DockerHelper.IsLinuxContainerModeEnabled))))
            {
                return;
            }

            string command;
            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                command = "find /usr/share/dotnet -type f";
            }
            else
            {
                command = "cmd /c dir /s /b /A:-D \"Program Files\\dotnet\"";
            }

            string containerFileList = DockerHelper.Run(
                image: imageData.GetImage(ImageType, DockerHelper),
                command: command,
                name: imageData.GetIdentifier("DotnetFolder"));

            IEnumerable<string> actualDotnetFiles = containerFileList.ToLower()
                .Replace("/usr/share/dotnet", String.Empty)
                .Replace("c:\\program files\\dotnet", String.Empty)
                .Replace("\\", "/")
                .Replace("\r\n", "\n")
                .Split("\n")
                .Select(file => file.TrimStart('/'))
                .OrderBy(file => file);

            IEnumerable<string> expectedDotnetFiles = await GetSdkContentsAsync(imageData);

            Assert.Equal(expectedDotnetFiles, actualDotnetFiles);
        }

        private IEnumerable<string> EnumerateArchiveContents(string path)
        {
            using var file = File.OpenRead(path);
            using var reader = ReaderFactory.Open(file);
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    yield return reader.Entry.Key;
                }
            }
        }

        private async Task<IEnumerable<string>> GetSdkContentsAsync(ProductImageData imageData)
        {
            string sdkVersion = imageData.GetProductVersion(ImageType, DockerHelper);

            string osType = DockerHelper.IsLinuxContainerModeEnabled ? "linux" : "win";
            string fileType = DockerHelper.IsLinuxContainerModeEnabled ? "tar.gz" : "zip";

            string architecture = imageData.Arch switch
            {
                Arch.Amd64 => "x64",
                Arch.Arm => "arm",
                Arch.Arm64 => "arm64",
                _ => throw new InvalidOperationException($"Unexpected architecture type: '{imageData.Arch}'"),
            };

            string sdkUrl =
                $"https://dotnetcli.azureedge.net/dotnet/Sdk/{sdkVersion}/dotnet-sdk-{sdkVersion}-{osType}-{architecture}.{fileType}";

            if (!sdkContentsCache.TryGetValue(sdkUrl, out IEnumerable<string> files))
            {
                string sdkFile = Path.GetTempFileName();

                using WebClient webClient = new WebClient();
                await webClient.DownloadFileTaskAsync(new Uri(sdkUrl), sdkFile);

                files = EnumerateArchiveContents(sdkFile)
                    .Select(file => file.TrimStart('.').TrimStart('/').ToLower())
                    .OrderBy(file => file);

                sdkContentsCache.Add(sdkUrl, files);
            }

            return files;
        }

        private void PowerShellScenario_Execute(ProductImageData imageData, string optionalArgs)
        {
            if (imageData.Version.Major < 3)
            {
                OutputHelper.WriteLine("PowerShell does not exist in pre-3.0 images, skip testing");
                return;
            }

            // A basic test which executes an arbitrary command to validate PS is functional
            string output = DockerHelper.Run(
                image: imageData.GetImage(DotNetImageType.SDK, DockerHelper),
                name: imageData.GetIdentifier($"pwsh"),
                optionalRunArgs: optionalArgs,
                command: $"pwsh -c (Get-Childitem env:DOTNET_RUNNING_IN_CONTAINER).Value"
            );

            Assert.Equal(output, bool.TrueString, ignoreCase: true);
        }

        private class SdkImageDataEqualityComparer : IEqualityComparer<ProductImageData>
        {
            public bool Equals([AllowNull] ProductImageData x, [AllowNull] ProductImageData y)
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

            public int GetHashCode([DisallowNull] ProductImageData obj)
            {
                return $"{obj.VersionString}-{obj.SdkOS}-{obj.Arch}".GetHashCode();
            }
        }
    }
}
