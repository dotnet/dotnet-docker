// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet.Docker
{
    /// <summary>
    /// Updates pinned PowerShell and Aspire Dashboard artifact checksums.
    /// .NET artifact checksums are downloaded when building the images instead.
    /// </summary>
    internal class DockerfileShaUpdater
    {
        private static readonly Dictionary<string, string> s_shaCache = new();
        private static readonly Dictionary<string, Dictionary<string, string>> s_checksumFileCache = new();
        private static readonly HttpClient s_httpClient = new();

        private static readonly Dictionary<string, string> s_urls = new()
        {
            { "aspire-dashboard", "$BASE_URL/aspire/$VERSION_DIR/aspire-dashboard-$OS-$ARCH.zip" },
            { "powershell", "https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$VERSION_DIR/PowerShell.$OS.$ARCH.$VERSION_FILE.nupkg" },
        };

        private readonly string _productName;
        private readonly SpecificCommandOptions _options;
        private readonly ManifestVariables _variables;

        public DockerfileShaUpdater(
            string productName,
            SpecificCommandOptions options,
            ManifestVariables variables)
        {
            _productName = productName;
            _options = options;
            _variables = variables;

            if (!string.IsNullOrEmpty(_options.InternalAccessToken))
            {
                s_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "",
                        _options.InternalAccessToken))));
            }
        }

        public static IEnumerable<string> SupportedProducts => s_urls.Keys;

        private string? BuildVersion => _options.ProductVersions.TryGetValue(_productName, out string? version)
            ? version
            : VersionUpdater.GetBuildVersion(_productName, _options.DockerfileVersion, _variables);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            // The format of the sha variable name is '<productName>|<dockerfileVersion>|<os>|<arch>|sha'.
            // The 'os' and 'arch' segments are optional.
            string prefix = $"{_productName}|{_options.DockerfileVersion}|";

            string[] variableNames = _variables.Names
                .Where(name => name.StartsWith(prefix, StringComparison.Ordinal))
                .Where(name => name.EndsWith("|sha", StringComparison.Ordinal))
                .ToArray();

            foreach (string variableName in variableNames)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string downloadUrl = GetDownloadUrl(variableName);
                string sha = await GetArtifactShaAsync(downloadUrl, cancellationToken);
                VariableUpdater.Update(_variables, variableName, sha);
            }
        }

        private string GetDownloadUrl(string variableName)
        {
            string[] parts = variableName.Split('|');
            string os = GetOs(parts);
            string arch = GetArch(parts);
            string versionDir = BuildVersion ?? "";
            string versionFile = VersionHelper.ResolveProductVersion(versionDir, _options.StableBranding);
            string url = s_urls[_productName];

            if (url.Contains("$BASE_URL", StringComparison.Ordinal))
            {
                string baseUrl = ManifestHelper.GetBaseUrls(_variables, _options).First();
                url = url.Replace("$BASE_URL", baseUrl);
            }

            return url
                .Replace("$VERSION_DIR", versionDir)
                .Replace("$VERSION_FILE", versionFile)
                .Replace("$OS", os)
                .Replace("$ARCH", arch)
                .Replace("..", ".");
        }

        private static string GetOs(string[] variableParts)
        {
            if (variableParts.Length >= 4)
            {
                return variableParts[2];
            }

            return string.Empty;
        }

        private static string GetArch(string[] variableParts)
        {
            if (variableParts.Length >= 5)
            {
                return variableParts[3];
            }

            return string.Empty;
        }

        private async Task<string> GetArtifactShaAsync(string downloadUrl, CancellationToken cancellationToken)
        {
            if (!s_shaCache.TryGetValue(downloadUrl, out string? sha))
            {
                sha = await GetChecksumShaFromChecksumsFileAsync(downloadUrl, cancellationToken)
                    ?? await ComputeChecksumShaAsync(downloadUrl, cancellationToken);

                if (sha != null)
                {
                    sha = sha.ToLowerInvariant();
                    s_shaCache.Add(downloadUrl, sha);
                    Trace.TraceInformation($"Retrieved sha '{sha}' for '{downloadUrl}'.");
                }
                else
                {
                    throw new InvalidOperationException($"Unable to retrieve sha for '{downloadUrl}'.");
                }
            }

            return sha;
        }

        private Task<string?> ComputeChecksumShaAsync(string downloadUrl, CancellationToken cancellationToken)
        {
            Trace.TraceInformation($"Downloading '{downloadUrl}'.");
            return ChecksumHelper.ComputeChecksumShaAsync(
                s_httpClient, downloadUrl, cancellationToken);
        }

        private async Task<string?> GetChecksumShaFromChecksumsFileAsync(string productDownloadUrl, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_options.ChecksumsFile))
                return null;

            return GetProductChecksum(await GetChecksumsFromChecksumsFile(cancellationToken), productDownloadUrl);
        }

        private static string? GetProductChecksum(IDictionary<string, string> checksumEntries, string productDownloadUrl)
        {
            string installerFileName = productDownloadUrl.Substring(productDownloadUrl.LastIndexOf('/') + 1);

            if (!checksumEntries.TryGetValue(installerFileName, out string? sha))
            {
                Trace.TraceInformation($"Failed to find `{installerFileName}` sha");
            }

            return sha;
        }

        private async Task<IDictionary<string, string>> GetChecksumsFromChecksumsFile(CancellationToken cancellationToken)
        {
            string path = _options.ChecksumsFile;
            if (s_checksumFileCache.TryGetValue(path, out Dictionary<string, string>? checksumEntries))
            {
                return checksumEntries;
            }

            checksumEntries = new Dictionary<string, string>();

            Trace.TraceInformation($"Opening '{path}'.");
            string content = await File.ReadAllTextAsync(path, cancellationToken);

            if (string.IsNullOrEmpty(content))
            {
                // Return empty dictionary since there are no checksums
                s_checksumFileCache.Add(path, checksumEntries);
                return checksumEntries;
            }

            string[] checksumLines = content.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);

            /**
                Sometimes the checksum file starts with the following line:

                # Hash: SHA512

                Other times the first line is the first checksum entry. This
                happens sometimes for preview releases.
            **/
            int firstChecksumEntry = checksumLines[0].Contains("Hash") ? 1 : 0;
            for (int i = firstChecksumEntry; i < checksumLines.Length; i++)
            {
                string[] parts = checksumLines[i].Split(" ");
                if (parts.Length != 2)
                {
                    throw new FormatException($"Checksum file is not in the expected format: {path}");
                }

                string fileName = parts[1];
                string checksum = parts[0];

                checksumEntries.Add(fileName, checksum);
                Trace.TraceInformation($"Parsed checksum '{checksum}' for '{fileName}'");
            }

            s_checksumFileCache.Add(path, checksumEntries);
            return checksumEntries;
        }
    }
}
