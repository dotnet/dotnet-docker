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

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will scan a Dockerfile for the .NET Core artifacts that are installed.
    /// The updater will then retrieve and update the checksum sha used to validate the downloaded artifacts.
    /// </summary>
    public class DockerfileShaUpdater : FileRegexUpdater
    {
        private const string ChecksumsHostName = "dotnetclichecksums.blob.core.windows.net";
        private const string ShaVariableGroupName = "shaVariable";
        private const string ShaValueGroupName = "shaValue";

        private static readonly Dictionary<string, string> s_shaCache = new Dictionary<string, string>();
        private static readonly Dictionary<string, Dictionary<string, string>> s_releaseChecksumCache =
             new Dictionary<string, Dictionary<string, string>>();
        private static readonly Dictionary<string, string> s_urls = new Dictionary<string, string> {
            {"powershell", "https://pwshtool.blob.core.windows.net/tool/$VERSION/PowerShell.$OS.$ARCH.$VERSION.nupkg"},
            {"monitor", "https://dotnetcli.azureedge.net/dotnet/diagnostics/monitor5.0/dotnet-monitor.$VERSION.nupkg"},
            {"runtime", "https://dotnetcli.azureedge.net/dotnet/Runtime/$VERSION/dotnet-runtime-$VERSION-$OS-$ARCH.$ARCHIVE_EXT"},
            {"aspnet", "https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$VERSION/aspnetcore-runtime-$VERSION-$OS-$ARCH.$ARCHIVE_EXT"},
            {"sdk", "https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/dotnet-sdk-$VERSION-$OS-$ARCH.$ARCHIVE_EXT"},
            {"lzma", "https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/nuGetPackagesArchive.lzma"}
        };

        private string _productName;
        private string _dockerfileVersion;
        private string _buildVersion;
        private string _arch;
        private string _os;
        private Options _options;
        private string _versions;

        public DockerfileShaUpdater() : base()
        {
        }

        public static IEnumerable<IDependencyUpdater> CreateUpdaters(
            string productName, string dockerfileVersion, string repoRoot, Options options)
        {
            string versionsPath = System.IO.Path.Combine(repoRoot, Program.VersionsFilename);
            string versions = File.ReadAllText(versionsPath);

            // The format of the sha variable name is '<productName>|<dockerfileVersion>|<os>|<arch>|sha'.
            // The 'os' and 'arch' segments are optional.
            string shaVariablePattern =
                $"\"(?<{ShaVariableGroupName}>{Regex.Escape(productName)}\\|{Regex.Escape(dockerfileVersion)}.*\\|sha)\":";
            Regex shaVariableRegex = new Regex(shaVariablePattern);

            return shaVariableRegex.Matches(versions)
                .Select(match => match.Groups[ShaVariableGroupName].Value)
                .Select(variable =>
                {
                    Trace.TraceInformation($"Updating {variable}");

                    string[] parts = variable.Split('|');
                    DockerfileShaUpdater updater = new DockerfileShaUpdater()
                    {
                        _productName = productName,
                        _buildVersion = VersionUpdater.GetBuildVersion(productName, dockerfileVersion, versions),
                        _dockerfileVersion = dockerfileVersion,
                        _os = parts.Length >= 4 ? parts[2] : string.Empty,
                        _arch = parts.Length >= 5 ? parts[3] : string.Empty,
                        _options = options,
                        _versions = versions,
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

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyBuildInfos, out IEnumerable<IDependencyInfo> usedBuildInfos)
        {
            IDependencyInfo productInfo = dependencyBuildInfos.First(info => info.SimpleName == _productName);

            usedBuildInfos = new IDependencyInfo[] { productInfo };

            string downloadUrl = s_urls[_productName]
                .Replace("$ARCHIVE_EXT", _os.Contains("win") ? "zip" : "tar.gz")
                .Replace("$VERSION", _buildVersion)
                .Replace("$OS", _os)
                .Replace("$ARCH", _arch)
                .Replace("..", ".");
            return GetArtifactShaAsync(downloadUrl).Result;
        }

        private async Task<string> GetArtifactShaAsync(string downloadUrl)
        {
            if (!s_shaCache.TryGetValue(downloadUrl, out string sha))
            {
                sha = await GetDotNetCliChecksumsShaAsync(downloadUrl)
                    ?? await GetDotNetReleaseChecksumsShaAsync(downloadUrl)
                    ?? await ComputeChecksumShaAsync(downloadUrl);

                if (sha != null)
                {
                    sha = sha.ToLowerInvariant();
                    s_shaCache.Add(downloadUrl, sha);
                    Trace.TraceInformation($"Retrieved sha '{sha}' for '{downloadUrl}'.");
                }
                else
                {
                    Trace.TraceError($"Unable to retrieve sha for '{downloadUrl}'.");
                }
            }

            return sha;
        }

        private async Task<string> ComputeChecksumShaAsync(string downloadUrl)
        {
            if (!_options.ComputeChecksums)
            {
                return null;
            }

            string sha = null;

            Trace.TraceInformation($"Downloading '{downloadUrl}'.");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(downloadUrl))
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

        private async Task<string> GetDotNetCliChecksumsShaAsync(string productDownloadUrl)
        {
            string sha = null;
            string shaExt = _productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ? ".sha" : ".sha512";

            UriBuilder uriBuilder = new UriBuilder(productDownloadUrl)
            {
                Host = ChecksumsHostName
            };
            string shaUrl = uriBuilder.ToString() + shaExt;

            Trace.TraceInformation($"Downloading '{shaUrl}'.");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(shaUrl))
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

        private async Task<string> GetDotNetReleaseChecksumsShaAsync(
            string productDownloadUrl)
        {
            string buildVersion = _buildVersion;
            // The release checksum file contains content for all products in the release (runtime, sdk, etc.)
            // and is referenced by the runtime version.
            if (_productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ||
                _productName.Contains("aspnet", StringComparison.OrdinalIgnoreCase))
            {
                buildVersion = VersionUpdater.GetBuildVersion("runtime", _dockerfileVersion, _versions);
            }

            IDictionary<string, string> checksumEntries = await GetDotnetReleaseChecksums(buildVersion);

            string installerFileName = productDownloadUrl.Substring(productDownloadUrl.LastIndexOf('/') + 1);

            if (!checksumEntries.TryGetValue(installerFileName, out string sha))
            {
                Trace.TraceInformation($"Failed to find `{installerFileName}` sha");
            }

            return sha;
        }

        private static async Task<IDictionary<string, string>> GetDotnetReleaseChecksums(string version)
        {
            string uri = $"https://dotnetcli.blob.core.windows.net/dotnet/checksums/{version}-sha.txt";
            if (s_releaseChecksumCache.TryGetValue(uri, out Dictionary<string, string> checksumEntries))
            {
                return checksumEntries;
            }

            checksumEntries = new Dictionary<string, string>();
            s_releaseChecksumCache.Add(uri, checksumEntries);

            Trace.TraceInformation($"Downloading '{uri}'.");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uri))
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
    }
}
