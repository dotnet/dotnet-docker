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
    public class SdkImageTests : ProductImageTests
    {
        private static readonly Dictionary<string, IEnumerable<SdkContentFileInfo>> s_sdkContentsCache =
            new Dictionary<string, IEnumerable<SdkContentFileInfo>>();

        private static readonly RetryStrategyOptions s_sdkDownloadRetryStrategy =
            new()
            {
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 4,
                Delay = TimeSpan.FromSeconds(3),
            };

        private static readonly ResiliencePipeline s_sdkDownloadPipeline =
            new ResiliencePipelineBuilder()
                .AddRetry(s_sdkDownloadRetryStrategy)
                .Build();

        private static readonly HttpClient s_httpClient = CreateHttpClient();

        public SdkImageTests(ITestOutputHelper outputHelper)
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

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public async void VerifyBlazorWasmScenario(ProductImageData imageData)
        {
            bool useWasmTools = true;

            // `wasm-tools` workload does not work on ARM
            if (imageData.IsArm)
            {
                useWasmTools = false;
            }

            // `wasm-tools` is not supported on Alpine for .NET < 9 due to https://github.com/dotnet/sdk/issues/32327
            if (imageData.OS.StartsWith(OS.Alpine) && imageData.Version.Major == 8)
            {
                useWasmTools = false;
            }

            // Emscripten SDK workload is the wrong version on linux-musl-x64 in .NET 10 Preview 6
            // Remove in nightly branch when https://github.com/dotnet/dotnet/issues/1487 is resolved
            if (imageData.Version.Major == 10)
            {
                useWasmTools = false;
            }

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

            List<EnvironmentVariableInfo> variables =
            [
                new EnvironmentVariableInfo("DOTNET_GENERATE_ASPNET_CERTIFICATE", "false"),
                new EnvironmentVariableInfo("DOTNET_USE_POLLING_FILE_WATCHER", "true"),
                new EnvironmentVariableInfo("NUGET_XMLDOC_MODE", "skip"),
                new EnvironmentVariableInfo("DOTNET_SDK_VERSION", version)
                {
                    IsProductVersion = true
                },
                AspnetImageTests.GetAspnetVersionVariableInfo(ImageRepo, imageData, DockerHelper),
                RuntimeImageTests.GetRuntimeVersionVariableInfo(ImageRepo, imageData, DockerHelper),
                new EnvironmentVariableInfo("DOTNET_NOLOGO", "true"),
                ..GetCommonEnvironmentVariables(),
            ];

            if (imageData.SupportsPowerShell
                || imageData.Version == ImageVersion.V8_0
                || imageData.Version == ImageVersion.V9_0)
            {
                variables.Add(new EnvironmentVariableInfo("POWERSHELL_DISTRIBUTION_CHANNEL", allowAnyValue: true));
            }

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
            PowerShellScenario_Execute(imageData, string.Empty);
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
            if (Config.IsInternal)
            {
                // Skip this test for internal builds, since this test does not
                // yet authenticate to download the internal staged version of
                // the .NET SDK.
                return;
            }

            IEnumerable<SdkContentFileInfo> actualDotnetFiles = GetActualSdkContents(imageData);
            IEnumerable<SdkContentFileInfo> expectedDotnetFiles = await GetExpectedSdkContentsAsync(imageData);

            using TempFileContext actualFilesContext = FileHelper.UseTempFile();
            using TempFileContext expectedFilesContext = FileHelper.UseTempFile();

            File.WriteAllLines(actualFilesContext.Path, actualDotnetFiles.Select(file => $"{file.Path} {file.Sha512}"));
            File.WriteAllLines(expectedFilesContext.Path, expectedDotnetFiles.Select(file => $"{file.Path} {file.Sha512}"));

            bool filesMatch = FileHelper.CompareFiles(expectedFilesContext.Path, actualFilesContext.Path, OutputHelper);

            Assert.True(filesMatch, "Differences found in the dotnet folder contents.");
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInstalledPackages(ProductImageData imageData)
        {
            VerifyInstalledPackagesBase(imageData, ImageRepo);
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
            DockerHelper.Run(
                image: imageData.GetImage(DotNetImageRepo.SDK, DockerHelper),
                name: imageData.GetIdentifier($"git"),
                command: "git version");
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

        /// <summary>
        /// Verifies that dnx is on the PATH and that it is functional.
        /// </summary>
        [DotNetTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDnxInstallation(ProductImageData imageData)
        {
            if (!imageData.SupportsDnx)
            {
                return;
            }

            var dnxCommand = DockerHelper.IsLinuxContainerModeEnabled
                ? "dnx --help"
                : "dnx.cmd --help";

            DockerHelper.Run(
                image: imageData.GetImage(DotNetImageRepo.SDK, DockerHelper),
                name: imageData.GetIdentifier("dnx"),
                command: dnxCommand
            );
        }

        private IEnumerable<SdkContentFileInfo> GetActualSdkContents(ProductImageData imageData)
        {
            string dotnetPath;
            string destinationPath;
            string command;

            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                dotnetPath = "/usr/share/dotnet";
                destinationPath = "/sdk";
                command = $"find {destinationPath} -type f -exec sha512sum {{}} +";
            }
            else
            {
                dotnetPath = "\"Program Files\\dotnet\"";
                destinationPath = "C:\\sdk";
                string powerShellCommand =
                    $"Get-ChildItem -File -Force -Recurse '{destinationPath}' " +
                    "| Get-FileHash -Algorithm SHA512 " +
                    "| select @{name='Value'; expression={$_.Hash + '  ' +$_.Path}} " +
                    "| select -ExpandProperty Value";
                command = $"pwsh -Command \"{powerShellCommand}\"";
            }

            string baseImage = imageData.GetImage(ImageRepo, DockerHelper);
            string tag = imageData.GetIdentifier("SdkContents").ToLower();

            DockerHelper.Build(
                tag: tag,
                dockerfile: Path.Combine(DockerHelper.TestArtifactsDir, "Dockerfile.copy"),
                contextDir: DockerHelper.TestArtifactsDir,
                platform: imageData.Platform,
                buildArgs:
                [
                    $"copy_image={baseImage}",
                    $"base_image={baseImage}",
                    $"copy_origin={dotnetPath}",
                    $"copy_destination={destinationPath}"
                ]);

            string containerFileList = DockerHelper.Run(
                image: tag,
                name: tag,
                command: command,
                silenceOutput: true);

            return containerFileList
                .Split("\n")
                .Select(line =>
                {
                    string[] parts = line.Split("  ").Select(part => part.Trim()).ToArray();
                    return new SdkContentFileInfo(Path.GetRelativePath(destinationPath, parts[1]), parts[0]);
                })
                .OrderBy(fileInfo => fileInfo.Path);
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
            OutputHelper.WriteLine("Downloading SDK archive: " + sdkUrl);

            if (!s_sdkContentsCache.TryGetValue(sdkUrl, out IEnumerable<SdkContentFileInfo>? files))
            {
                string sdkFile = Path.GetTempFileName();

                await s_sdkDownloadPipeline.ExecuteAsync(async cancellationToken =>
                {
                    await s_httpClient.DownloadFileAsync(new Uri(sdkUrl), sdkFile);
                });

                files = EnumerateArchiveContents(sdkFile)
                    .OrderBy(file => file.Path)
                    .ToArray();

                s_sdkContentsCache.Add(sdkUrl, files);
            }

            return files;
        }

        private static string GetSdkVersionFileLabel(string sdkBuildVersion, string dotnetVersion)
        {
            // This should be kept in sync with the template for computing the SDK version file:
            // https://github.com/dotnet/dotnet-docker/blob/4f48d36a98187a6e350d54167ef5b568ccd3882f/eng/dockerfile-templates/sdk/Dockerfile.linux.install-sdk#L22-L31

            bool isStableBranding = !sdkBuildVersion.Contains('-')
                || sdkBuildVersion.Contains("-servicing")
                || sdkBuildVersion.Contains("-rtm");

            string sdkVersionFile = isStableBranding
                ? Config.GetVariableValue($"sdk|{dotnetVersion}|product-version")
                : sdkBuildVersion;

            return sdkVersionFile;
        }

        private string GetSdkUrl(ProductImageData imageData)
        {
            bool isInternal = Config.IsInternal;
            string sdkBuildVersion = Config.GetBuildVersion(ImageRepo, imageData.VersionString);
            string sdkFileVersionLabel = isInternal
                ? imageData.GetProductVersion(ImageRepo, ImageRepo, DockerHelper)
                : GetSdkVersionFileLabel(sdkBuildVersion, imageData.VersionString);

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

            return url;
        }

        private void PowerShellScenario_Execute(ProductImageData imageData, string? optionalArgs = null)
        {
            string image = imageData.GetImage(DotNetImageRepo.SDK, DockerHelper);

            if (!imageData.SupportsPowerShell)
            {
                OutputHelper.WriteLine($"PowerShell is not supproted on {image}");
                return;
            }

            // A basic test which executes an arbitrary command to validate PS is functional
            string output = DockerHelper.Run(
                image: image,
                name: imageData.GetIdentifier($"pwsh"),
                optionalRunArgs: optionalArgs,
                command: $"pwsh -c (Get-Childitem env:DOTNET_RUNNING_IN_CONTAINER).Value"
            );

            Assert.Equal(output, bool.TrueString, ignoreCase: true);
        }

        private record SdkContentFileInfo
        {
            public string Path { get; init; }
            public string Sha512 { get; init; }

            public SdkContentFileInfo(string path, string sha512)
            {
                Path = NormalizePath(path);
                Sha512 = sha512.ToLower();
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

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();

            if (Config.IsInternal)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "",
                        Config.InternalAccessToken))));
            };

            return client;
        }
    }
}
