// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will scan a Dockerfile for the .NET artifacts that are installed.
    /// The updater will then retrieve and update the checksum sha used to validate the downloaded artifacts.
    /// </summary>
    public class DockerfileShaUpdater : FileRegexUpdater
    {
        private const string ReleaseDotnetBaseUrl = $"https://dotnetcli.blob.core.windows.net/dotnet";
        private const string ReleaseDotnetBaseCdnUrl = $"https://dotnetcli.azureedge.net/dotnet";

        private const string ShaVariableGroupName = "shaVariable";
        private const string ShaValueGroupName = "shaValue";
        private const string NetStandard21TargetingPack = "netstandard-targeting-pack-2.1.0";

        private static readonly Dictionary<string, string> s_shaCache = new();
        private static readonly Dictionary<string, Dictionary<string, string>> s_releaseChecksumCache = new();
        private static readonly HttpClient s_httpClient = new();

        private readonly string _productName;
        private readonly Version _dockerfileVersion;
        private readonly string? _buildVersion;
        private readonly string _arch;
        private readonly string _os;
        private readonly Options _options;
        private readonly string _versions;
        private readonly Dictionary<string, string[]> _urls;
        private readonly Lazy<JObject> _manifestVariables;

        public DockerfileShaUpdater(
            string productName, string dockerfileVersion, string? buildVersion, string arch, string os, string versions, Options options)
        {
            _productName = productName;
            _dockerfileVersion = new Version(dockerfileVersion);
            _buildVersion = buildVersion;
            _arch = arch;
            _os = os;
            _versions = versions;
            _options = options;

            // Maps a product name to a set of one or more candidate URLs referencing the associated artifact. The order of the URLs
            // should be in priority order with each subsequent URL being the fallback. This is primarily intended to support targeting
            // pack RPMs because they only ship once for a given major/minor release and never again for servicing releases. However, during
            // preview releases, they ship with each build. By making use of fallback URLs it allows support for either scenario, first checking
            // for the build-specific location and then falling back to the overall major/minor release location.
            _urls = new()
            {
                { "powershell", new string[] { "https://pwshtool.blob.core.windows.net/tool/$VERSION_DIR/PowerShell.$OS.$ARCH.$VERSION_FILE.nupkg" } },

                { "monitor", new string[] { $"$DOTNET_BASE_URL/diagnostics/monitor/$VERSION_DIR/dotnet-monitor-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" } },

                { "runtime", new string[] { $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-runtime-$VERSION_FILE$OPTIONAL_OS-{GetRuntimeSdkArchFormat()}.$ARCHIVE_EXT" } },
                { "runtime-host", new string[] { $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-host-$VERSION_FILE-{GetRpmArchFormat()}.$ARCHIVE_EXT" } },
                { "runtime-hostfxr", new string[] { $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-hostfxr-$VERSION_FILE-{GetRpmArchFormat()}.$ARCHIVE_EXT" } },
                {
                    "runtime-targeting-pack",
                    new string[]
                    {
                        $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-targeting-pack-$VERSION_FILE-{GetRpmArchFormat()}.$ARCHIVE_EXT",
                        $"$DOTNET_BASE_URL/Runtime/$DF_VERSION.0/dotnet-targeting-pack-$DF_VERSION.0-{GetRpmArchFormat()}.$ARCHIVE_EXT"
                    }
                },
                { "runtime-apphost-pack", new string[] { $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-apphost-pack-$VERSION_FILE-{GetRpmArchFormat()}.$ARCHIVE_EXT" } },
                { NetStandard21TargetingPack, new string[] { $"{ReleaseDotnetBaseUrl}/Runtime/3.1.0/netstandard-targeting-pack-2.1.0-$ARCH.$ARCHIVE_EXT" } },
                { "runtime-deps-cm.1", new string[] { $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-runtime-deps-$VERSION_FILE-cm.1-{GetRpmArchFormat()}.$ARCHIVE_EXT" } },
                { "runtime-deps-cm.2", new string[] { $"$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-runtime-deps-$VERSION_FILE-cm.2-{GetRpmArchFormat()}.$ARCHIVE_EXT" } },

                { "aspnet", new string[] { $"$DOTNET_BASE_URL/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-runtime-$VERSION_FILE$OPTIONAL_OS-{GetRuntimeSdkArchFormat()}.$ARCHIVE_EXT" } },
                {
                    "aspnet-runtime-targeting-pack",
                    new string[]
                    {
                        $"$DOTNET_BASE_URL/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-targeting-pack-$VERSION_FILE{GetAspnetTargetingPackArchFormat()}.$ARCHIVE_EXT",
                        $"$DOTNET_BASE_URL/aspnetcore/Runtime/$DF_VERSION.0/aspnetcore-targeting-pack-$DF_VERSION.0{GetAspnetTargetingPackArchFormat()}.$ARCHIVE_EXT"
                    }
                },

                { "sdk", new string[] { $"$DOTNET_BASE_URL/Sdk/$VERSION_DIR/dotnet-sdk-$VERSION_FILE$OPTIONAL_OS-{GetRuntimeSdkArchFormat()}.$ARCHIVE_EXT" } },
                { "lzma", new string[] { $"$DOTNET_BASE_URL/Sdk/$VERSION_DIR/nuGetPackagesArchive.lzma" } }
            };

            _manifestVariables = new Lazy<JObject>(
                () =>
                {
                    const string VariablesProperty = "variables";
                    JToken? variables = ManifestHelper.LoadManifest(UpdateDependencies.VersionsFilename)[VariablesProperty];
                    if (variables is null)
                    {
                        throw new InvalidOperationException($"'{VariablesProperty}' property missing in '{UpdateDependencies.VersionsFilename}'");
                    }
                    return (JObject)variables;
                });
        }

        private string GetRpmArchFormat() => _arch == "arm64" ? "aarch64" : "$ARCH";

        private string GetAspnetTargetingPackArchFormat() => $"-{GetRpmArchFormat()}";

        private string GetRuntimeSdkArchFormat() => _os.Contains("rpm") ? GetRpmArchFormat() : "$ARCH";

        public static IEnumerable<IDependencyUpdater> CreateUpdaters(
            string productName, string dockerfileVersion, string repoRoot, Options options)
        {
            string versionsPath = System.IO.Path.Combine(repoRoot, UpdateDependencies.VersionsFilename);
            string versions = File.ReadAllText(versionsPath);

            // The format of the sha variable name is '<productName>|<dockerfileVersion>|<os>|<arch>|sha'.
            // The 'os' and 'arch' segments are optional.
            string shaVariablePattern;
            if (productName == NetStandard21TargetingPack)
            {
                // NetStandard targeting pack is not associated with a specific Dockerfile version
                shaVariablePattern = $"\"(?<{ShaVariableGroupName}>{Regex.Escape(productName)}.*\\|sha)\":";
            }
            else
            {
                shaVariablePattern = $"\"(?<{ShaVariableGroupName}>{Regex.Escape(productName)}\\|{Regex.Escape(dockerfileVersion)}.*\\|sha)\":";
            }

            Regex shaVariableRegex = new(shaVariablePattern);

            return shaVariableRegex.Matches(versions)
                .Select(match => match.Groups[ShaVariableGroupName].Value)
                .Select(variable =>
                {
                    Trace.TraceInformation($"Updating {variable}");

                    string[] parts = variable.Split('|');
                    DockerfileShaUpdater updater = new(
                        productName,
                        dockerfileVersion,
                        GetBuildVersion(productName, dockerfileVersion, versions, options),
                        GetArch(parts),
                        GetOs(parts),
                        versions,
                        options)
                    {
                        Path = versionsPath,
                        VersionGroupName = ShaValueGroupName
                    };

                    string archPattern = updater._arch == string.Empty ? string.Empty : "|" + updater._arch;
                    string osPattern = updater._os == string.Empty ? string.Empty : "|" + updater._os;
                    updater.Regex = new Regex(
                        $"{shaVariablePattern.Replace(".*", Regex.Escape(osPattern + archPattern))} \"(?<{ShaValueGroupName}>.*)\"");

                    return updater;
                })
                .ToArray();
        }

        protected override string? TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyBuildInfos, out IEnumerable<IDependencyInfo> usedBuildInfos)
        {
            IDependencyInfo productInfo = dependencyBuildInfos.First(info => info.SimpleName == _productName);

            usedBuildInfos = new IDependencyInfo[] { productInfo };

            string? versionDir = _buildVersion;
            string? versionFile = UpdateDependencies.ResolveProductVersion(_buildVersion, _options);

            string archiveExt;
            if (_os.Contains("win"))
            {
                archiveExt = "zip";
            }
            else if (_os.Contains("rpm"))
            {
                archiveExt = "rpm";
            }
            else
            {
                archiveExt = "tar.gz";
            }

            string optionalOs = _os.Contains("rpm") ? string.Empty : $"-{_os}";

            // Each product name has one or more candidate URLs from which to retrieve the artifact. Multiple candidate URLs
            // should be listed in priority order. Each subsequent URL listed is treated as a fallback.
            string[] candidateUrls = _urls[_productName];

            for (int candidateUrlIndex = 0; candidateUrlIndex < candidateUrls.Length; candidateUrlIndex++)
            {
                string baseUrl = ManifestHelper.GetBaseUrl(_manifestVariables.Value, _options);
                string downloadUrl = candidateUrls[candidateUrlIndex]
                    .Replace("$DOTNET_BASE_URL", baseUrl)
                    .Replace("$ARCHIVE_EXT", archiveExt)
                    .Replace("$VERSION_DIR", versionDir)
                    .Replace("$VERSION_FILE", versionFile)
                    .Replace("$OS", _os)
                    .Replace("$OPTIONAL_OS", optionalOs)
                    .Replace("$ARCH", _arch)
                    .Replace("$DF_VERSION", _options.DockerfileVersion)
                    .Replace("..", ".");

                bool isLastUrlToCheck =
                    candidateUrlIndex == candidateUrls.Length - 1;

                string? result = GetArtifactShaAsync(downloadUrl, errorOnNotFound: isLastUrlToCheck).Result;
                if (result is not null)
                {
                    return result;
                }
            }

            return null;
        }

        private static string GetOs(string[] variableParts)
        {
            if (variableParts.Length == 4 && !Version.TryParse(variableParts[1], out _))
            {
                // Handles the case of "netstandard-targeting-pack-2.1.0|linux-rpm|x64|sha".
                return variableParts[1];
            }
            else if (variableParts.Length >= 4)
            {
                return variableParts[2];
            }

            return string.Empty;
        }

        private static string GetArch(string[] variableParts)
        {
            if (variableParts.Length == 4 && !Version.TryParse(variableParts[1], out _))
            {
                // Handles the case of "netstandard-targeting-pack-2.1.0|linux-rpm|x64|sha".
                return variableParts[2];
            }
            else if (variableParts.Length >= 5)
            {
                return variableParts[3];
            }

            return string.Empty;
        }

        private async Task<string?> GetArtifactShaAsync(string downloadUrl, bool errorOnNotFound)
        {
            if (!s_shaCache.TryGetValue(downloadUrl, out string? sha))
            {
                sha = await GetDotNetReleaseChecksumsShaFromRuntimeVersionAsync(downloadUrl)
                    ?? await GetDotNetReleaseChecksumsShaFromBuildVersionAsync(downloadUrl)
                    ?? await GetDotNetReleaseChecksumsShaFromPreviewVersionAsync(downloadUrl)
                    ?? await GetDotNetBinaryStorageChecksumsShaAsync(downloadUrl)
                    ?? await ComputeChecksumShaAsync(downloadUrl);

                if (sha != null)
                {
                    sha = sha.ToLowerInvariant();
                    s_shaCache.Add(downloadUrl, sha);
                    Trace.TraceInformation($"Retrieved sha '{sha}' for '{downloadUrl}'.");
                }
                else
                {
                    string notFoundMsg = $"Unable to retrieve sha for '{downloadUrl}'.";
                    if (errorOnNotFound)
                    {
                        Trace.TraceError(notFoundMsg);
                    }
                    else
                    {
                        Trace.TraceWarning(notFoundMsg);
                    }
                }
            }

            return sha;
        }

        private async Task<string?> ComputeChecksumShaAsync(string downloadUrl)
        {
            if (!_options.ComputeChecksums)
            {
                return null;
            }

            string? sha = null;

            Trace.TraceInformation($"Downloading '{downloadUrl}'.");
            using (HttpResponseMessage response = await s_httpClient.GetAsync(ApplySasQueryStringIfNecessary(downloadUrl, _options.BinarySasQueryString)))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (Stream httpStream = await response.Content.ReadAsStreamAsync())
                    using (SHA512 hash = SHA512.Create())
                    {
                        byte[] hashedInputBytes = hash.ComputeHash(httpStream);

                        StringBuilder stringBuilder = new StringBuilder(128);
                        foreach (byte b in hashedInputBytes)
                        {
                            stringBuilder.Append(b.ToString("X2"));
                        }
                        sha = stringBuilder.ToString();
                    }
                }
                else
                {
                    Trace.TraceInformation($"Failed to download {downloadUrl}.");
                }
            }

            return sha;
        }

        private static bool IsInternalUrl(string url)
        {
            return url.Contains("internal");
        }

        private static string ApplySasQueryStringIfNecessary(string url, string sasQueryString)
        {
            if (IsInternalUrl(url))
            {
                return url + sasQueryString;
            }

            return url;
        }

        private async Task<string?> GetDotNetBinaryStorageChecksumsShaAsync(string productDownloadUrl)
        {
            string? sha = null;
            string shaExt = _productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ? ".sha" : ".sha512";

            string shaUrl = productDownloadUrl
                .Replace("/dotnetcli", "/dotnetclichecksums")
                .Replace("/internal/", "/internal-checksums/")
                .Replace("/public/", "/public-checksums/")
                .Replace("azureedge.net", "blob.core.windows.net")
                + shaExt;

            Trace.TraceInformation($"Downloading '{shaUrl}'.");
            using (HttpResponseMessage response = await s_httpClient.GetAsync(ApplySasQueryStringIfNecessary(shaUrl, _options.ChecksumSasQueryString)))
            {
                if (response.IsSuccessStatusCode)
                {
                    sha = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Trace.TraceInformation($"Failed to find dotnet binary storage account sha");
                }
            }

            return sha;
        }

        private Task<string?> GetDotNetReleaseChecksumsShaFromRuntimeVersionAsync(string productDownloadUrl) =>
            GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, GetRuntimeVersion());

        private string? GetRuntimeVersion()
        {
            string? version = _buildVersion;
            // The release checksum file contains content for all products in the release (runtime, sdk, etc.)
            // and is referenced by the runtime version.
            if (_productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ||
                _productName.Contains("aspnet", StringComparison.OrdinalIgnoreCase))
            {
                version = GetBuildVersion("runtime", _dockerfileVersion.ToString(), _versions, _options);
            }

            return version;
        }

        private Task<string?> GetDotNetReleaseChecksumsShaFromBuildVersionAsync(string productDownloadUrl) =>
            GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, _buildVersion);

        private Task<string?> GetDotNetReleaseChecksumsShaFromPreviewVersionAsync(string productDownloadUrl)
        {
            string? runtimeVersion = GetRuntimeVersion();
            if (runtimeVersion is not null && TryParsePreviewVersion(runtimeVersion, out string? previewVersion))
            {
                return GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, previewVersion);
            }

            return Task.FromResult<string?>(null);
        }

        private async Task<string?> GetDotNetReleaseChecksumsShaAsync(
            string productDownloadUrl, string? version)
        {
            // Only use the release checksums file for base URLs that target the release blob storage. This is because the
            // release checksums file contains checksums for the official release files which will be signed. The same
            // corresponding build in the daily build location, for example, will not be signed due. So when we're targeting
            // the daily build location, we wouldn't use the release checksums file and instead use the other means of
            // retrieving the checksums.
            string baseUrl = ManifestHelper.GetBaseUrl(_manifestVariables.Value, _options);
            if (baseUrl != ReleaseDotnetBaseUrl && baseUrl != ReleaseDotnetBaseCdnUrl)
            {
                return null;
            }

            IDictionary<string, string> checksumEntries = await GetDotnetReleaseChecksums(version);

            string installerFileName = productDownloadUrl.Substring(productDownloadUrl.LastIndexOf('/') + 1);

            if (!checksumEntries.TryGetValue(installerFileName, out string? sha))
            {
                Trace.TraceInformation($"Failed to find `{installerFileName}` sha");
            }

            return sha;
        }

        private static bool TryParsePreviewVersion(string version, out string? previewVersion)
        {
            // Example format: 6.0.0-preview.5.21301.5
            // This method returns the 6.0.0-preview.5 segment of that value.

            const string PreviewGroup = "Preview";
            string PreviewVersionRegex = @$"(?<{PreviewGroup}>\d+\.\d+\.\d+-[\w-]+\.\d+)\.[\d\.]+";
            Match match = Regex.Match(version, PreviewVersionRegex);
            if (match.Success)
            {
                previewVersion = match.Groups[PreviewGroup].Value;
                return true;
            }
            else
            {
                previewVersion = null;
                return false;
            }
        }

        private async Task<IDictionary<string, string>> GetDotnetReleaseChecksums(string? version)
        {
            string uri = $"{ReleaseDotnetBaseUrl}/checksums/{version}-sha.txt";
            if (s_releaseChecksumCache.TryGetValue(uri, out Dictionary<string, string>? checksumEntries))
            {
                return checksumEntries;
            }

            checksumEntries = new Dictionary<string, string>();
            s_releaseChecksumCache.Add(uri, checksumEntries);

            Trace.TraceInformation($"Downloading '{uri}'.");
            using (HttpResponseMessage response = await s_httpClient.GetAsync(ApplySasQueryStringIfNecessary(uri, _options.BinarySasQueryString)))
            {
                if (response.IsSuccessStatusCode)
                {
                    string checksums = await response.Content.ReadAsStringAsync();
                    string[] checksumLines = checksums.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    if (!checksumLines[0].StartsWith("Hash"))
                    {
                        Trace.TraceError($"Checksum file is not in the expected format: {uri}");
                    }

                    for (int i = 1; i < checksumLines.Length; i++)
                    {
                        string[] parts = checksumLines[i].Split(" ");
                        if (parts.Length != 2)
                        {
                            Trace.TraceError($"Checksum file is not in the expected format: {uri}");
                        }

                        string fileName = parts[1];
                        string checksum = parts[0];

                        checksumEntries.Add(fileName, checksum);
                        Trace.TraceInformation($"Parsed checksum '{checksum}' for '{fileName}'");
                    }
                }
                else
                {
                    Trace.TraceInformation($"Failed to find dotnet release checksums");
                }
            }

            return checksumEntries;
        }

        private static string? GetBuildVersion(string productName, string dockerfileVersion, string variables, Options options)
        {
            string? buildVersion;
            if (options.ProductVersions.TryGetValue(productName, out string? version))
            {
                buildVersion = version;
            }
            else
            {
                buildVersion = VersionUpdater.GetBuildVersion(productName, dockerfileVersion, variables);
            }

            return buildVersion;
        }
    }
}
#nullable disable
