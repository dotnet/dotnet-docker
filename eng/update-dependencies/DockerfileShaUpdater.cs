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

#nullable enable
namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will scan a Dockerfile for the .NET artifacts that are installed.
    /// The updater will then retrieve and update the checksum sha used to validate the downloaded artifacts.
    /// </summary>
    public class DockerfileShaUpdater : FileRegexUpdater
    {
        private const string ChecksumsHostName = "dotnetclichecksums.blob.core.windows.net";
        private const string ShaVariableGroupName = "shaVariable";
        private const string ShaValueGroupName = "shaValue";
        private const string NetStandard21TargetingPackRpm = "netstandard-targeting-pack-2.1.0-rpm";
        private const string DotnetBaseUrl = "https://dotnetcli.azureedge.net/dotnet";

        private static readonly Dictionary<string, string> s_shaCache = new();
        private static readonly Dictionary<string, Dictionary<string, string>> s_releaseChecksumCache = new();

        // Maps a product name to a set of one or more candidate URLs referencing the associated artifact. The order of the URLs
        // should be in priority order with each subsequent URL being the fallback. This is primarily intended to support targeting
        // pack RPMs because they only ship once for a given major/minor release and never again for servicing releases. However, during
        // preview releases, they ship with each build. By making use of fallback URLs it allows support for either scenario, first checking
        // for the build-specific location and then falling back to the overall major/minor release location.
        private static readonly Dictionary<string, string[]> s_urls = new() {
            {"powershell", new string[] { "https://pwshtool.blob.core.windows.net/tool/$VERSION_DIR/PowerShell.$OS.$ARCH.$VERSION_FILE.nupkg" }},

            {"monitor", new string[] { $"{DotnetBaseUrl}/diagnostics/monitor$CHANNEL_NAME/dotnet-monitor.$VERSION_FILE.nupkg" }},

            {"runtime", new string[] { $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-runtime-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" }},
            {"runtime-rpm", new string[] { $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-runtime-$VERSION_FILE-$ARCH.rpm" }},
            {"runtime-host-rpm", new string[] { $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-host-$VERSION_FILE-$ARCH.rpm" }},
            {"runtime-hostfxr-rpm", new string[] { $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-hostfxr-$VERSION_FILE-$ARCH.rpm" }},
            {"runtime-targeting-pack-rpm", new string[]
                {
                    $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-targeting-pack-$VERSION_FILE-$ARCH.rpm",
                    $"{DotnetBaseUrl}/Runtime/$DF_VERSION.0/dotnet-targeting-pack-$DF_VERSION.0-$ARCH.rpm"
                }
            },
            {"runtime-apphost-pack-rpm", new string[] { $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-apphost-pack-$VERSION_FILE-$ARCH.rpm" }},
            {NetStandard21TargetingPackRpm, new string[] { $"{DotnetBaseUrl}/Runtime/3.1.0/netstandard-targeting-pack-2.1.0-$ARCH.rpm" }},
            {"runtime-deps-cm.1-rpm", new string[] { $"{DotnetBaseUrl}/Runtime/$VERSION_DIR/dotnet-runtime-deps-$VERSION_FILE-cm.1-$ARCH.rpm" }},

            {"aspnet", new string[] { $"{DotnetBaseUrl}/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-runtime-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" }},
            {"aspnet-rpm", new string[] { $"{DotnetBaseUrl}/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-runtime-$VERSION_FILE-$ARCH.rpm" }},
            {"aspnet-runtime-targeting-pack-rpm", new string[]
                {
                    $"{DotnetBaseUrl}/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-targeting-pack-$VERSION_FILE.rpm",
                    $"{DotnetBaseUrl}/aspnetcore/Runtime/$DF_VERSION.0/aspnetcore-targeting-pack-$DF_VERSION.0.rpm"
                }
            },

            {"sdk", new string[] { $"{DotnetBaseUrl}/Sdk/$VERSION_DIR/dotnet-sdk-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" }},
            {"sdk-rpm", new string[] { $"{DotnetBaseUrl}/Sdk/$VERSION_DIR/dotnet-sdk-$VERSION_FILE-$ARCH.rpm" }},
            {"lzma", new string[] { $"{DotnetBaseUrl}/Sdk/$VERSION_DIR/nuGetPackagesArchive.lzma" }}
        };

        private static HttpClient s_httpClient { get; } = new();

        private readonly string _productName;
        private readonly string _dockerfileVersion;
        private readonly string? _buildVersion;
        private readonly string _arch;
        private readonly string _os;
        private readonly Options _options;
        private readonly string _versions;

        public DockerfileShaUpdater(
            string productName, string dockerfileVersion, string? buildVersion, string arch, string os, string versions, Options options)
        {
            _productName = productName;
            _dockerfileVersion = dockerfileVersion;
            _buildVersion = buildVersion;
            _arch = arch;
            _os = os;
            _versions = versions;
            _options = options;
        }

        public static IEnumerable<IDependencyUpdater> CreateUpdaters(
            string productName, string dockerfileVersion, string repoRoot, Options options)
        {
            string versionsPath = System.IO.Path.Combine(repoRoot, Program.VersionsFilename);
            string versions = File.ReadAllText(versionsPath);

            // The format of the sha variable name is '<productName>|<dockerfileVersion>|<os>|<arch>|sha'.
            // The 'os' and 'arch' segments are optional.
            string shaVariablePattern;
            if (productName == NetStandard21TargetingPackRpm)
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
                        parts.Length >= 4 ? parts[2] : string.Empty,
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

            const string RtmSubstring = "-rtm.";

            string? versionDir = _buildVersion;
            string? versionFile = _buildVersion;
            if (versionFile?.Contains(RtmSubstring) == true)
            {
                versionFile = versionFile.Substring(0, versionFile.IndexOf(RtmSubstring));
            }

            // Each product name has one or more candidate URLs from which to retrieve the artifact. Multiple candidate URLs
            // should be listed in priority order. Each subsequent URL listed is treated as a fallback.
            string[] candidateUrls = s_urls[_productName];
            for (int i = 0; i < candidateUrls.Length; i++)
            {
                string downloadUrl = candidateUrls[i]
                    .Replace("$ARCHIVE_EXT", _os.Contains("win") ? "zip" : "tar.gz")
                    .Replace("$VERSION_DIR", versionDir)
                    .Replace("$VERSION_FILE", versionFile)
                    .Replace("$CHANNEL_NAME", _options.ChannelName)
                    .Replace("$OS", _os)
                    .Replace("$ARCH", _arch)
                    .Replace("$DF_VERSION", _options.DockerfileVersion)
                    .Replace("..", ".");
                string? result = GetArtifactShaAsync(downloadUrl, errorOnNotFound: i == candidateUrls.Length - 1).Result;
                if (result is not null)
                {
                    return result;
                }
            }

            return null;
        }

        private static string GetArch(string[] variableParts)
        {
            if (variableParts.Length == 3)
            {
                // Typical 3-part variables look like "lzma|2.1|sha" or "powershell|3.1|build-version" which don't
                // have arch values. In these cases, they are all indicated by having a product version in the
                // second position. But there's another case to consider: "netstandard-targeting-pack-2.1.0-rpm|x64|sha".
                // The arch value in this case is in the second position.
                return Version.TryParse(variableParts[1], out _) ? string.Empty : variableParts[1];
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
                sha = await GetDotNetCliChecksumsShaAsync(downloadUrl)
                    ?? await GetDotNetReleaseChecksumsShaFromRuntimeVersionAsync(downloadUrl)
                    ?? await GetDotNetReleaseChecksumsShaFromBuildVersionAsync(downloadUrl)
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
            using (HttpResponseMessage response = await s_httpClient.GetAsync(downloadUrl))
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

        private async Task<string?> GetDotNetCliChecksumsShaAsync(string productDownloadUrl)
        {
            string? sha = null;
            string shaExt = _productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ? ".sha" : ".sha512";

            UriBuilder uriBuilder = new UriBuilder(productDownloadUrl)
            {
                Host = ChecksumsHostName
            };
            string shaUrl = uriBuilder.ToString() + shaExt;

            Trace.TraceInformation($"Downloading '{shaUrl}'.");
            using (HttpResponseMessage response = await s_httpClient.GetAsync(shaUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    sha = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Trace.TraceInformation($"Failed to find `dotnetclichecksums` sha");
                }
            }

            return sha;
        }

        private Task<string?> GetDotNetReleaseChecksumsShaFromRuntimeVersionAsync(
            string productDownloadUrl)
        {
            string? version = _buildVersion;
            // The release checksum file contains content for all products in the release (runtime, sdk, etc.)
            // and is referenced by the runtime version.
            if (_productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ||
                _productName.Contains("aspnet", StringComparison.OrdinalIgnoreCase))
            {
                version = GetBuildVersion("runtime", _dockerfileVersion, _versions, _options);
            }

            return GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, version);
        }

        private Task<string?> GetDotNetReleaseChecksumsShaFromBuildVersionAsync(string productDownloadUrl) =>
            GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, _buildVersion);

        private async Task<string?> GetDotNetReleaseChecksumsShaAsync(
            string productDownloadUrl, string? version)
        {
            IDictionary<string, string> checksumEntries = await GetDotnetReleaseChecksums(version);

            string installerFileName = productDownloadUrl.Substring(productDownloadUrl.LastIndexOf('/') + 1);

            if (!checksumEntries.TryGetValue(installerFileName, out string? sha))
            {
                Trace.TraceInformation($"Failed to find `{installerFileName}` sha");
            }

            return sha;
        }

        private static async Task<IDictionary<string, string>> GetDotnetReleaseChecksums(string? version)
        {
            string uri = $"https://dotnetcli.blob.core.windows.net/dotnet/checksums/{version}-sha.txt";
            if (s_releaseChecksumCache.TryGetValue(uri, out Dictionary<string, string>? checksumEntries))
            {
                return checksumEntries;
            }

            checksumEntries = new Dictionary<string, string>();
            s_releaseChecksumCache.Add(uri, checksumEntries);

            Trace.TraceInformation($"Downloading '{uri}'.");
            using (HttpResponseMessage response = await s_httpClient.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    string checksums = await response.Content.ReadAsStringAsync();
                    string[] checksumLines = checksums.Replace("\r\n", "\n").Split("\n");
                    if (!checksumLines[0].StartsWith("Hash") || !string.IsNullOrEmpty(checksumLines[1]))
                    {
                        Trace.TraceError($"Checksum file is not in the expected format: {uri}");
                    }

                    for (int i = 2; i < checksumLines.Length - 1; i++)
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
            if (options.ProductVersions.TryGetValue(productName, out string? version))
            {
                return version;
            }

            return VersionUpdater.GetBuildVersion(productName, dockerfileVersion, variables);
        }
    }
}
#nullable disable
