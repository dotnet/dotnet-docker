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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dotnet.Docker
{
    /// <summary>
    /// Updates manifest checksum variables for the selected product's artifacts.
    /// </summary>
    internal class DockerfileShaUpdater
    {
        private const string ReleaseDotnetBaseCdnUrl = $"https://builds.dotnet.microsoft.com/dotnet";

        private static readonly Dictionary<string, string> s_shaCache = new();
        private static readonly Dictionary<string, Dictionary<string, string>> s_releaseChecksumCache = new();
        private static readonly HttpClient s_httpClient = new();

        private readonly string _productName;
        private readonly SpecificCommandOptions _options;
        private readonly ManifestVariables _variables;
        private readonly Dictionary<string, string> _urls;

        public DockerfileShaUpdater(
            string productName,
            SpecificCommandOptions options,
            ManifestVariables variables)
        {
            _productName = productName;
            _options = options;
            _variables = variables;

            // Maps a product name to a set of one or more candidate URLs referencing the associated artifact. The order of the URLs
            // should be in priority order with each subsequent URL being the fallback.
            _urls = new()
            {
                { "aspire-dashboard",               "$DOTNET_BASE_URL/aspire/$VERSION_DIR/aspire-dashboard-$OS-$ARCH.$ARCHIVE_EXT" },
                { "aspnet-composite",               "$DOTNET_BASE_URL/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-runtime-composite-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" },
                { "aspnet",                         "$DOTNET_BASE_URL/aspnetcore/Runtime/$VERSION_DIR/aspnetcore-runtime-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" },
                { "powershell",                     "https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$VERSION_DIR/PowerShell.$OS.$ARCH.$VERSION_FILE.nupkg" },
                { "runtime",                        "$DOTNET_BASE_URL/Runtime/$VERSION_DIR/dotnet-runtime-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" },
                { "sdk",                            "$DOTNET_BASE_URL/Sdk/$VERSION_DIR/dotnet-sdk-$VERSION_FILE-$OS-$ARCH.$ARCHIVE_EXT" },
            };

            if (!string.IsNullOrEmpty(_options.InternalAccessToken))
            {
                s_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "",
                        _options.InternalAccessToken))));
            }
        }

        private string? BuildVersion => GetBuildVersion(_productName, _options.DockerfileVersion, _variables, _options);

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
            string baseUrl = ManifestHelper.GetBaseUrls(_variables, _options).First();
            // Remove Aspire Dashboard case once https://github.com/microsoft/aspire/issues/2035 is fixed.
            string archiveExt = os.Contains("win") || _productName.Contains("aspire-dashboard") ? "zip" : "tar.gz";
            string versionDir = BuildVersion ?? "";
            string versionFile = VersionHelper.ResolveProductVersion(versionDir, _options.StableBranding);

            return _urls[_productName]
                .Replace("$DOTNET_BASE_URL", baseUrl)
                .Replace("$ARCHIVE_EXT", archiveExt)
                .Replace("$VERSION_DIR", versionDir)
                .Replace("$VERSION_FILE", versionFile)
                .Replace("$OS", os)
                .Replace("$ARCH", arch)
                .Replace("$DF_VERSION", _options.DockerfileVersion)
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
                    ?? await GetDotNetReleaseChecksumsShaFromRuntimeVersionAsync(downloadUrl, cancellationToken)
                    ?? await GetDotNetReleaseChecksumsShaFromBuildVersionAsync(downloadUrl, cancellationToken)
                    ?? await GetDotNetReleaseChecksumsShaFromPreviewVersionAsync(downloadUrl, cancellationToken)
                    ?? await GetDotNetBinaryStorageChecksumsShaAsync(downloadUrl, cancellationToken)
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

        private async Task<string?> GetDotNetBinaryStorageChecksumsShaAsync(string productDownloadUrl, CancellationToken cancellationToken)
        {
            string? sha = null;

            string shaUrl = productDownloadUrl
                .Replace("/dotnetcli", "/dotnetclichecksums")
                .Replace("/internal/", "/internal-checksums/")
                .Replace("/public/", "/public-checksums/")
                + ".sha512";

            Trace.TraceInformation($"Downloading '{shaUrl}'.");
            using (HttpResponseMessage response = await s_httpClient.GetAsync(shaUrl, cancellationToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    sha = await response.Content.ReadAsStringAsync(cancellationToken);
                }
                else
                {
                    Trace.TraceInformation($"Failed to find dotnet binary storage account sha");
                }
            }

            return sha;
        }

        private Task<string?> GetDotNetReleaseChecksumsShaFromRuntimeVersionAsync(string productDownloadUrl, CancellationToken cancellationToken) =>
            GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, GetRuntimeVersion(), cancellationToken);

        private string? GetRuntimeVersion()
        {
            string? version = BuildVersion;
            // The release checksum file contains content for all products in the release (runtime, sdk, etc.)
            // and is referenced by the runtime version.
            if (_productName.Contains("sdk", StringComparison.OrdinalIgnoreCase) ||
                _productName.Contains("aspnet", StringComparison.OrdinalIgnoreCase))
            {
                version = GetBuildVersion("runtime", _options.DockerfileVersion, _variables, _options);
            }

            return version;
        }

        private Task<string?> GetDotNetReleaseChecksumsShaFromBuildVersionAsync(string productDownloadUrl, CancellationToken cancellationToken) =>
            GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, BuildVersion, cancellationToken);

        private Task<string?> GetDotNetReleaseChecksumsShaFromPreviewVersionAsync(string productDownloadUrl, CancellationToken cancellationToken)
        {
            string? runtimeVersion = GetRuntimeVersion();
            if (runtimeVersion is not null && TryParsePreviewVersion(runtimeVersion, out string? previewVersion))
            {
                return GetDotNetReleaseChecksumsShaAsync(productDownloadUrl, previewVersion, cancellationToken);
            }

            return Task.FromResult<string?>(null);
        }

        private async Task<string?> GetDotNetReleaseChecksumsShaAsync(
            string productDownloadUrl, string? version, CancellationToken cancellationToken)
        {
            // Only use the release checksums file for base URLs that target the release blob storage. This is because the
            // release checksums file contains checksums for the official release files which will be signed. The same
            // corresponding build in the daily build location, for example, will not be signed due. So when we're targeting
            // the daily build location, we wouldn't use the release checksums file and instead use the other means of
            // retrieving the checksums.
            string? baseUrl = ManifestHelper
                .GetBaseUrls(_variables, _options)
                .Where(url => url == ReleaseDotnetBaseCdnUrl)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return null;
            }

            return GetProductChecksum(await GetDotnetReleaseChecksums(version, cancellationToken), productDownloadUrl);
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

        private async Task<IDictionary<string, string>> GetChecksumsFromChecksumsFile(CancellationToken cancellationToken)
        {
            return await GetChecksums(
                _options.ChecksumsFile,
                () =>
                {
                    Trace.TraceInformation($"Opening '{_options.ChecksumsFile}'.");
                    return File.ReadAllTextAsync(_options.ChecksumsFile, cancellationToken);
                });
        }

        private async Task<IDictionary<string, string>> GetDotnetReleaseChecksums(string? version, CancellationToken cancellationToken)
        {
            string uri = $"{ReleaseDotnetBaseCdnUrl}/checksums/{version}-sha.txt";

            return await GetChecksums(
                uri,
                async () =>
                {
                    Trace.TraceInformation($"Downloading '{uri}'.");
                    using (HttpResponseMessage response = await s_httpClient.GetAsync(uri, cancellationToken))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync(cancellationToken);
                        }
                        else
                        {
                            Trace.TraceInformation($"Failed to find dotnet release checksums");
                            return string.Empty;
                        }
                    }
                });
        }

        private async Task<IDictionary<string, string>> GetChecksums(string sourceUrlOrPath, Func<Task<string>> getContentCallback)
        {
            if (s_releaseChecksumCache.TryGetValue(sourceUrlOrPath, out Dictionary<string, string>? checksumEntries))
            {
                return checksumEntries;
            }

            checksumEntries = new Dictionary<string, string>();

            string content = await getContentCallback();

            if (string.IsNullOrEmpty(content))
            {
                // Return empty dictionary since there are no checksums
                s_releaseChecksumCache.Add(sourceUrlOrPath, checksumEntries);
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
                    throw new FormatException($"Checksum file is not in the expected format: {sourceUrlOrPath}");
                }

                string fileName = parts[1];
                string checksum = parts[0];

                checksumEntries.Add(fileName, checksum);
                Trace.TraceInformation($"Parsed checksum '{checksum}' for '{fileName}'");
            }

            s_releaseChecksumCache.Add(sourceUrlOrPath, checksumEntries);
            return checksumEntries;
        }

        private static string? GetBuildVersion(string productName, string dockerfileVersion, ManifestVariables variables, SpecificCommandOptions options)
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
