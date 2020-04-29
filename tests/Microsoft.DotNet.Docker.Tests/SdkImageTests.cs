// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SharpCompress.Common;
using SharpCompress.Readers;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "sdk")]
    public class SdkImageTests : ProductImageTests
    {
        private static readonly Dictionary<string, IEnumerable<SdkContentFileInfo>> sdkContentsCache =
            new Dictionary<string, IEnumerable<SdkContentFileInfo>>();

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

            if (imageData.Version.Major >= 5)
            {
                variables.Add(AspnetImageTests.GetAspnetVersionVariableInfo(imageData, DockerHelper));
                variables.Add(RuntimeImageTests.GetRuntimeVersionVariableInfo(imageData, DockerHelper));
            }

            if (imageData.SdkOS.StartsWith(OS.AlpinePrefix))
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "false"));

                if (imageData.Version.Major < 5)
                {
                    variables.Add(new EnvironmentVariableInfo("LC_ALL", "en_US.UTF-8"));
                    variables.Add(new EnvironmentVariableInfo("LANG", "en_US.UTF-8"));
                }
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

            IEnumerable<SdkContentFileInfo> actualDotnetFiles = GetActualSdkContents(imageData);
            IEnumerable<SdkContentFileInfo> expectedDotnetFiles = await GetExpectedSdkContentsAsync(imageData);

            bool hasCountDifference = expectedDotnetFiles.Count() != actualDotnetFiles.Count();

            bool hasFileContentDifference = false;
            
            // Skip file comparisons for 3.1 until https://github.com/dotnet/sdk/issues/11327 is fixed.
            if (imageData.Version.Major != 3)
            {
                int fileCount = expectedDotnetFiles.Count();
                for (int i = 0; i < fileCount; i++)
                {
                    if (expectedDotnetFiles.ElementAt(i).CompareTo(actualDotnetFiles.ElementAt(i)) != 0)
                    {
                        hasFileContentDifference = true;
                        break;
                    }
                }
            }

            if (hasCountDifference || hasFileContentDifference)
            {
                OutputHelper.WriteLine(String.Empty);
                OutputHelper.WriteLine("EXPECTED FILES:");
                foreach (SdkContentFileInfo file in expectedDotnetFiles)
                {
                    OutputHelper.WriteLine($"Path: {file.Path}");
                    OutputHelper.WriteLine($"Checksum: {file.Sha512}");
                }
                
                OutputHelper.WriteLine(String.Empty);
                OutputHelper.WriteLine("ACTUAL FILES:");
                foreach (SdkContentFileInfo file in actualDotnetFiles)
                {
                    OutputHelper.WriteLine($"Path: {file.Path}");
                    OutputHelper.WriteLine($"Checksum: {file.Sha512}");
                }
            }

            Assert.Equal(expectedDotnetFiles.Count(), actualDotnetFiles.Count());
            Assert.False(hasFileContentDifference, "There are file content differences. Check the log output.");
        }

        private IEnumerable<SdkContentFileInfo> GetActualSdkContents(ProductImageData imageData)
        {
            string dotnetPath;

            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                dotnetPath = "/usr/share/dotnet";
            }
            else
            {
                dotnetPath = "Program Files\\dotnet";
            }

            string powerShellCommand =
                $"Get-ChildItem -File -Force -Recurse '{dotnetPath}' " +
                "| Get-FileHash -Algorithm SHA512 " +
                "| select @{name='Value'; expression={$_.Hash + '  ' +$_.Path}} " +
                "| select -ExpandProperty Value";
            string command = $"pwsh -Command \"{powerShellCommand}\"";

            string containerFileList = DockerHelper.Run(
                image: imageData.GetImage(ImageType, DockerHelper),
                command: command,
                name: imageData.GetIdentifier("DotnetFolder"));

            IEnumerable<SdkContentFileInfo> actualDotnetFiles = containerFileList
                .Replace("\r\n", "\n")
                .Split("\n")
                .Select(output =>
                {
                    string[] outputParts = output.Split("  ");
                    return new SdkContentFileInfo(outputParts[1], outputParts[0]);
                })
                .OrderBy(fileInfo => fileInfo.Path)
                .ToArray();
            return actualDotnetFiles;
        }

        private static IEnumerable<SdkContentFileInfo> EnumerateArchiveContents(string path)
        {
            using FileStream fileStream = File.OpenRead(path);
            using IReader reader = ReaderFactory.Open(fileStream);
            using TempFolderContext tempFolderContext = FileHelper.UseTempFolder();
            reader.WriteAllToDirectory(tempFolderContext.Path, new ExtractionOptions() { ExtractFullPath = true });

            foreach (FileInfo file in new DirectoryInfo(tempFolderContext.Path).EnumerateFiles("*", SearchOption.AllDirectories))
            {
                using SHA512 sha512 = SHA512.Create();
                byte[] sha512HashBytes = sha512.ComputeHash(File.ReadAllBytes(file.FullName));
                string sha512Hash = BitConverter.ToString(sha512HashBytes).Replace("-", String.Empty);
                yield return new SdkContentFileInfo(
                    file.FullName.Substring(tempFolderContext.Path.Length), sha512Hash);
            }
        }

        private async Task<IEnumerable<SdkContentFileInfo>> GetExpectedSdkContentsAsync(ProductImageData imageData)
        {
            string sdkVersion = imageData.GetProductVersion(ImageType, DockerHelper);

            string osType = DockerHelper.IsLinuxContainerModeEnabled ? "linux" : "win";
            if (imageData.SdkOS.StartsWith(OS.AlpinePrefix))
            {
                osType += "-musl";
            }

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

            if (!sdkContentsCache.TryGetValue(sdkUrl, out IEnumerable<SdkContentFileInfo> files))
            {
                string sdkFile = Path.GetTempFileName();

                using WebClient webClient = new WebClient();
                await webClient.DownloadFileTaskAsync(new Uri(sdkUrl), sdkFile);

                files = EnumerateArchiveContents(sdkFile)
                    .OrderBy(file => file.Path)
                    .ToArray();

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

        private class SdkContentFileInfo : IComparable<SdkContentFileInfo>
        {
            public SdkContentFileInfo(string path, string sha512)
            {
                Path = NormalizePath(path);
                Sha512 = sha512.ToLower();
            }

            public string Path { get; }
            public string Sha512 { get; }

            public int CompareTo([AllowNull] SdkContentFileInfo other)
            {
                return (Path + Sha512).CompareTo(other.Path + other.Sha512);
            }

            private static string NormalizePath(string path)
            {
                return path
                    .Replace("\\", "/")
                    .Replace("/usr/share/dotnet", String.Empty, StringComparison.OrdinalIgnoreCase)
                    .Replace("c:/program files/dotnet", String.Empty, StringComparison.OrdinalIgnoreCase)
                    .TrimStart('/')
                    .ToLower();
            }
        }
    }
}
