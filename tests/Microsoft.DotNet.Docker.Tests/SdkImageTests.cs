// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
                    // PowerShell needs support for Arm-based Alpine (https://github.com/PowerShell/PowerShell/issues/14667, https://github.com/PowerShell/PowerShell/issues/12937)
                    reason = "PowerShell does not have Alpine arm images, skip testing";
                    return false;
                }
                else if (imageData.Version.Major == 6 && imageData.OS.Contains("3.18"))
                {
                    // PowerShell does not support Alpine 3.18 yet (https://github.com/PowerShell/PowerShell/issues/19703)
                    reason = "Powershell does not support Alpine 3.18 yet, skip testing";
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
            // Skip test due to https://github.com/dotnet/dotnet-docker/issues/4841
            // Re-enable for release in main branch.
            if (imageData.IsWindows)
            {
                return;
            }

            // Get container SDK contents
            IEnumerable<SdkContentFileInfo> sdkContents = GetImageSdkContents(imageData);

            //print out the contents of the SDK
            foreach (SdkContentFileInfo fi in sdkContents)
            {
                OutputHelper.WriteLine(fi.Path);
            }

            // Get official MSFT SDK contents
            // IEnumerable<SdkContentFileInfo> msftSdkContents = await GetMsftSdkContentsAsync(imageData);

            // Write lines of each to files

            // Run a git diff of the files
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

        private IEnumerable<SdkContentFileInfo> GetImageSdkContents(ProductImageData imageData)
        {
            string dotnetPath = DockerHelper.IsLinuxContainerModeEnabled
                ? "/usr/share/dotnet"
                : "Program Files\\dotnet";

            string imageName = imageData.GetImage(ImageRepo, DockerHelper);
            string imageFsArchivePath = Path.Combine(DockerHelper.TestArtifactsDir, "image.tar");

            string imageFsArchive = DockerHelper.Export(imageName, imageFsArchivePath);
            OutputHelper.WriteLine($"Exported {imageFsArchivePath}");

            return EnumerateArchiveContents(imageFsArchivePath, dotnetPath).Select(tarEntry => new SdkContentFileInfo(tarEntry));
        }

        # nullable enable
        private static IEnumerable<TarEntry> EnumerateArchiveContents(string archivePath, string pathToEnumerate = "/")
        {
            using FileStream fileStream = File.OpenRead(archivePath);
            using TarReader reader = new TarReader(fileStream);

            IEnumerable<TarEntry> tarEntries = new List<TarEntry>();
            TarEntry? currentEntry;
            while ((currentEntry = reader.GetNextEntry()) != null)
            {
                tarEntries = tarEntries.Append(currentEntry);
            }

            tarEntries = tarEntries
                .Where(tarEntry => tarEntry.EntryType != TarEntryType.Directory)
                .Where(tarEntry => $"/{tarEntry.Name}".StartsWith(pathToEnumerate));

            return tarEntries;
        }
        #nullable disable

        private async Task<IEnumerable<string>> GetMsftSdkContentsAsync(ProductImageData imageData)
        {
            string sdkUrl = GetSdkUrl(imageData);

            if (!s_sdkContentsCache.TryGetValue(sdkUrl, out IEnumerable<SdkContentFileInfo> files))
            {
                string sdkFile = Path.GetTempFileName();

                using HttpClient httpClient = new();
                await httpClient.DownloadFileAsync(new Uri(sdkUrl), sdkFile);

                // files = EnumerateArchiveContents(sdkFile)
                //     .OrderBy(file => file.Path)
                //     .ToArray();

                // s_sdkContentsCache.Add(sdkUrl, files);
            }

            return Enumerable.Empty<string>();
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

        private class SdkContentFileInfo : IComparable<SdkContentFileInfo>, IEquatable<SdkContentFileInfo>
        {
            private static readonly SHA512 Sha512Algorithm = SHA512.Create();

            private readonly TarEntry _tarEntry;

            public SdkContentFileInfo(TarEntry tarEntry)
            {
                _tarEntry = tarEntry;
                Path = NormalizePath(_tarEntry.Name);
                // Sha512 = Sha512Algorithm.ComputeHash(_tarEntry.DataStream).ToString();
                Sha512 = string.Empty;
            }

            public string Path { get; init; }
            public string Sha512 { get; init; }

            public int CompareTo([AllowNull] SdkContentFileInfo other)
            {
                return (Path + Sha512).CompareTo(other.Path + other.Sha512);
            }

            public bool Equals(SdkContentFileInfo other) => other.Sha512 == Sha512;

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
