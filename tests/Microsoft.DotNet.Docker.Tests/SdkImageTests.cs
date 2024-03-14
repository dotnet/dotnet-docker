﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private static readonly Dictionary<string, IEnumerable<SdkContentFileInfo>> s_sdkContentsCache =
            new Dictionary<string, IEnumerable<SdkContentFileInfo>>();

        public SdkImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageRepo ImageRepo => DotNetImageRepo.SDK;

        private bool IsPowerShellSupported(ProductImageData imageData, out string reason)
        {
            if (imageData.OS.Contains("alpine"))
            {
                if (imageData.IsArm)
                {
                    reason = "PowerShell does not support Arm-based Alpine, skip testing (https://github.com/PowerShell/PowerShell/issues/14667, https://github.com/PowerShell/PowerShell/issues/12937)";
                    return false;
                }
                else if (imageData.Version.Major == 6 && imageData.OS.Contains("3.19"))
                {
                    reason = "Powershell does not support Alpine 3.19 yet, skip testing (https://github.com/PowerShell/PowerShell/issues/20945)";
                    return false;
                }
            }

            reason = null;
            return true;
        }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetImageData(DotNetImageRepo.SDK)
                .Where(imageData => !imageData.IsDistroless)
                // Filter the image data down to the distinct SDK OSes
                .Distinct(new SdkImageDataEqualityComparer())
                .Select(imageData => new object[] { imageData });
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public async void VerifyBlazorWasmScenario(ProductImageData imageData)
        {
            // Disable test since `dotnet workload install` does not work with an empty NuGet config.
            return;

            bool isAlpine = imageData.OS.StartsWith(OS.Alpine);

            // Microsoft.NETCore.App.Runtime.Mono.linux-musl-arm* package does not exist
            if (isAlpine && imageData.IsArm)
            {
                return;
            }

            // `wasm-tools` workload does not work on .NET 6 with CBL Mariner 2.0.
            // Re-enable when issue is resolved: https://github.com/dotnet/aspnetcore/issues/53469
            if (imageData.OS.Contains(OS.Mariner) && imageData.Version.Major == 6)
            {
                return;
            }

            // `wasm-tools` workload does not work on ARM
            // `wasm-tools` is also not supported on Alpine for .NET < 9 due to https://github.com/dotnet/sdk/issues/32327
            int[] unsupportedVersionsForAlpine = [6, 7, 8];
            bool isSupportedVersionForAlpine = !unsupportedVersionsForAlpine.Contains(imageData.Version.Major);
            bool useWasmTools = !imageData.IsArm && (!isAlpine || isSupportedVersionForAlpine);

            using BlazorWasmScenario testScenario = new(imageData, DockerHelper, OutputHelper, useWasmTools);
            await testScenario.ExecuteAsync();
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyNoSasToken(ProductImageData imageData)
        {
            base.VerifyCommonNoSasToken(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            string imageName = imageData.GetImage(ImageRepo, DockerHelper);
            string version = imageData.GetProductVersion(ImageRepo, ImageRepo, DockerHelper);

            List<EnvironmentVariableInfo> variables = new()
            {
                new EnvironmentVariableInfo("DOTNET_GENERATE_ASPNET_CERTIFICATE", "false"),
                new EnvironmentVariableInfo("DOTNET_USE_POLLING_FILE_WATCHER", "true"),
                new EnvironmentVariableInfo("NUGET_XMLDOC_MODE", "skip"),
                new EnvironmentVariableInfo("POWERSHELL_DISTRIBUTION_CHANNEL", allowAnyValue: true),
                new EnvironmentVariableInfo("DOTNET_SDK_VERSION", version)
                {
                    IsProductVersion = true
                },
                AspnetImageTests.GetAspnetVersionVariableInfo(ImageRepo, imageData, DockerHelper),
                RuntimeImageTests.GetRuntimeVersionVariableInfo(ImageRepo, imageData, DockerHelper),
                new EnvironmentVariableInfo("DOTNET_NOLOGO", "true")
            };
            variables.AddRange(GetCommonEnvironmentVariables());

            if (imageData.SdkOS.StartsWith(OS.Alpine))
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "false"));
            }

            EnvironmentVariableInfo.Validate(variables, imageName, imageData, DockerHelper);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPowerShellScenario_DefaultUser(ProductImageData imageData)
        {
            PowerShellScenario_Execute(imageData, null);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPowerShellScenario_NonDefaultUser(ProductImageData imageData)
        {
            string optRunArgs = "-u 12345:12345"; // Linux containers test as non-root user
            if (!DockerHelper.IsLinuxContainerModeEnabled)
            {
                // windows containers test as Admin, default execution is as ContainerUser
                optRunArgs = "-u ContainerAdministrator ";
            }

            PowerShellScenario_Execute(imageData, optRunArgs);
        }

        /// <summary>
        /// Verifies that the dotnet folder contents of an SDK container match the contents in the official SDK archive file.
        /// </summary>
        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyDotnetFolderContents(ProductImageData imageData)
        {
            if (!IsPowerShellSupported(imageData, out string powerShellReason))
            {
                OutputHelper.WriteLine(powerShellReason);
                return;
            }

            // Skip test on CBL-Mariner. Since installation is done via RPM package, we just need to verify the package installation
            // was done (handled by VerifyPackageInstallation test). There's no need to check the actual contents of the package.
            if (imageData.OS.Contains("cbl-mariner"))
            {
                return;
            }

            if (!imageData.SdkOS.StartsWith(OS.Alpine) && DockerHelper.IsLinuxContainerModeEnabled)
            {
                return;
            }

            IEnumerable<SdkContentFileInfo> actualDotnetFiles = GetActualSdkContents(imageData);
            IEnumerable<SdkContentFileInfo> expectedDotnetFiles = await GetExpectedSdkContentsAsync(imageData);

            bool hasCountDifference = expectedDotnetFiles.Count() != actualDotnetFiles.Count();

            bool hasFileContentDifference = false;

            int fileCount = expectedDotnetFiles.Count();
            for (int i = 0; i < fileCount; i++)
            {
                if (expectedDotnetFiles.ElementAt(i).CompareTo(actualDotnetFiles.ElementAt(i)) != 0)
                {
                    hasFileContentDifference = true;
                    break;
                }
            }

            if (hasCountDifference || hasFileContentDifference)
            {
                OutputHelper.WriteLine(string.Empty);
                OutputHelper.WriteLine("EXPECTED FILES:");
                foreach (SdkContentFileInfo file in expectedDotnetFiles)
                {
                    OutputHelper.WriteLine($"Path: {file.Path}");
                    OutputHelper.WriteLine($"Checksum: {file.Sha512}");
                }

                OutputHelper.WriteLine(string.Empty);
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

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledRpmPackages(ProductImageData imageData)
        {
            if (!imageData.OS.Contains("cbl-mariner") || imageData.IsDistroless || imageData.Version.Major > 6)
            {
                return;
            }

            VerifyExpectedInstalledRpmPackages(
                imageData,
                new string[]
                {
                    $"dotnet-sdk-{imageData.VersionString}",
                    $"dotnet-targeting-pack-{imageData.VersionString}",
                    $"aspnetcore-targeting-pack-{imageData.VersionString}",
                    $"dotnet-apphost-pack-{imageData.VersionString}",
                    $"netstandard-targeting-pack-2.1"
                }
                .Concat(AspnetImageTests.GetExpectedRpmPackagesInstalled(imageData)));
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledPackages(ProductImageData imageData)
        {
            RuntimeDepsImageTests.VerifyInstalledPackagesBase(imageData, ImageRepo, DockerHelper, OutputHelper);
        }

        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDefaultUser(ProductImageData imageData)
        {
            VerifyCommonDefaultUser(imageData);
        }

        /// <summary>
        /// Verifies that a git command can be executed without failure.
        /// </summary>
        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyGitInstallation(ProductImageData imageData)
        {
            if (!DockerHelper.IsLinuxContainerModeEnabled && imageData.Version.Major == 6)
            {
                OutputHelper.WriteLine("Git is not installed on Windows containers older than .NET 7");
                return;
            }

            DockerHelper.Run(
                image: imageData.GetImage(DotNetImageRepo.SDK, DockerHelper),
                name: imageData.GetIdentifier($"git"),
                command: "git version"
            );
        }

        /// <summary>
        /// Verifies that a tar command can be executed without failure.
        /// </summary>
        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyTarInstallation(ProductImageData imageData)
        {
            // tar should exist in the SDK for both Linux and Windows. The --version option works in either OS
            DockerHelper.Run(
                image: imageData.GetImage(DotNetImageRepo.SDK, DockerHelper),
                name: imageData.GetIdentifier("tar"),
                command: "tar --version"
            );
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
                image: imageData.GetImage(ImageRepo, DockerHelper),
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
                string sha512Hash = BitConverter.ToString(sha512HashBytes).Replace("-", string.Empty);
                yield return new SdkContentFileInfo(
                    file.FullName.Substring(tempFolderContext.Path.Length), sha512Hash);
            }
        }

        private async Task<IEnumerable<SdkContentFileInfo>> GetExpectedSdkContentsAsync(ProductImageData imageData)
        {
            string sdkUrl = GetSdkUrl(imageData);

            if (!s_sdkContentsCache.TryGetValue(sdkUrl, out IEnumerable<SdkContentFileInfo> files))
            {
                string sdkFile = Path.GetTempFileName();

                using HttpClient httpClient = new();
                await httpClient.DownloadFileAsync(new Uri(sdkUrl), sdkFile);

                files = EnumerateArchiveContents(sdkFile)
                    .OrderBy(file => file.Path)
                    .ToArray();

                s_sdkContentsCache.Add(sdkUrl, files);
            }

            return files;
        }

        private string GetSdkUrl(ProductImageData imageData)
        {
            bool isInternal = Config.IsInternal(imageData.VersionString);
            string sdkBuildVersion = Config.GetBuildVersion(ImageRepo, imageData.VersionString);
            string sdkFileVersionLabel = isInternal
                    ? imageData.GetProductVersion(ImageRepo, ImageRepo, DockerHelper)
                    : sdkBuildVersion;

            string osType = DockerHelper.IsLinuxContainerModeEnabled ? "linux" : "win";
            if (imageData.SdkOS.StartsWith(OS.Alpine))
            {
                osType += "-musl";
            }

            string architecture = imageData.Arch switch
            {
                Arch.Amd64 => "x64",
                Arch.Arm => "arm",
                Arch.Arm64 => "arm64",
                _ => throw new InvalidOperationException($"Unexpected architecture type: '{imageData.Arch}'"),
            };

            string fileType = DockerHelper.IsLinuxContainerModeEnabled ? "tar.gz" : "zip";
            string baseUrl = Config.GetBaseUrl(imageData.VersionString);
            string url = $"{baseUrl}/Sdk/{sdkBuildVersion}/dotnet-sdk-{sdkFileVersionLabel}-{osType}-{architecture}.{fileType}";
            if (isInternal)
            {
                url += Config.SasQueryString;
            }

            return url;
        }

        private void PowerShellScenario_Execute(ProductImageData imageData, string optionalArgs)
        {
            if (!IsPowerShellSupported(imageData, out string powershellReason))
            {
                OutputHelper.WriteLine(powershellReason);
                return;
            }

            // A basic test which executes an arbitrary command to validate PS is functional
            string output = DockerHelper.Run(
                image: imageData.GetImage(DotNetImageRepo.SDK, DockerHelper),
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
                    .Replace("/usr/share/dotnet", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Replace("c:/program files/dotnet", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .TrimStart('/')
                    .ToLower();
            }
        }
    }
}
