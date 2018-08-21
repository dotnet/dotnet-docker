// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will scan a Dockerfile for the .NET Core artifacts that are installed.
    /// The updater will then retrieve and update the checksum sha used to validate the downloaded artifacts.
    /// </summary>
    public class DockerfileShaUpdater : FileRegexUpdater
    {
        private const string EnvNameGroupName = "envName";
        private const string ValueGroupName = "value";

        private static readonly string s_envVersionPattern =
            $"ENV (?<{EnvNameGroupName}>(DOTNET|ASPNETCORE)_[\\S]*VERSION) (?<{ValueGroupName}>[\\S]*)";
        private static readonly string s_envShaPattern = $"ENV DOTNET_[\\S]*DOWNLOAD_SHA (?<{ValueGroupName}>[\\S]*)";
        private static readonly string s_envDownloadUrlPattern = $"ENV (DOTNET_[\\S]*DOWNLOAD_URL) (?<{ValueGroupName}>[\\S]*)";
        private static readonly string s_inlineUrlPatternFormat =
            $"(?<{ValueGroupName}>https://dotnetcli.blob.core.windows.net/[^;\\s]*{{0}})";
        private static readonly string s_inlineProductUrlPattern = string.Format(s_inlineUrlPatternFormat, string.Empty);
        private static readonly string s_inlineLzmaUrlPattern = string.Format(s_inlineUrlPatternFormat, "lzma");
        private static readonly string s_varShaPatternFormat = $"[ \\$]({{0}})sha512( )*=( )*'(?<{ValueGroupName}>[^'\\s]*)'";
        private static readonly string s_varProductShaPattern = string.Format(s_varShaPatternFormat, "dotnet_|aspnetcore_");
        private static readonly string s_varLzmaShaPattern = string.Format(s_varShaPatternFormat, "lzma_");

        private static readonly Regex s_productDownloadUrlRegex =
            new Regex($"({s_envDownloadUrlPattern})|{s_inlineProductUrlPattern}");
        private static readonly Regex s_lzmaDownloadUrlRegex = new Regex(s_inlineLzmaUrlPattern);
        private static readonly Regex s_productShaRegex = new Regex($"({s_envShaPattern})|({s_varProductShaPattern})");
        private static readonly Regex s_lzmaShaRegex = new Regex(s_varLzmaShaPattern);
        private static readonly Regex s_versionRegex = new Regex(s_envVersionPattern);

        private static readonly Dictionary<string, string> s_shaCache = new Dictionary<string, string>();

        private Regex _downloadUrlRegex;

        private DockerfileShaUpdater(string dockerfilePath, Regex regex, Regex downloadUrlRegex) : base()
        {
            _downloadUrlRegex = downloadUrlRegex;
            Path = dockerfilePath;
            Regex = regex;
            VersionGroupName = ValueGroupName;
        }

        public static DockerfileShaUpdater CreateProductShaUpdater(string dockerfilePath) =>
            new DockerfileShaUpdater(dockerfilePath, s_productShaRegex, s_productDownloadUrlRegex);

        public static DockerfileShaUpdater CreateLzmaShaUpdater(string dockerfilePath) =>
            new DockerfileShaUpdater(dockerfilePath, s_lzmaShaRegex, s_lzmaDownloadUrlRegex);

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyBuildInfos, out IEnumerable<IDependencyInfo> usedBuildInfos)
        {
            usedBuildInfos = Enumerable.Empty<IDependencyInfo>();

            Trace.TraceInformation($"DockerfileShaUpdater is processing '{Path}'.");
            string dockerfile = File.ReadAllText(Path);
            return GetArtifactShaAsync(dockerfile).Result;
        }

        private async Task<string> GetArtifactShaAsync(string dockerfile)
        {
            string sha = null;

            if (TryGetDotNetVersion(dockerfile, out (string Version, string EnvName) versionInfo))
            {
                if (TryGetDotNetDownloadUrl(dockerfile, out string downloadUrl))
                {
                    downloadUrl = SubstituteEnvRef(downloadUrl, versionInfo.EnvName, versionInfo.Version);
                    if (!s_shaCache.TryGetValue(downloadUrl, out sha))
                    {
                        sha = await GetDotNetCliChecksumsShaAsync(downloadUrl, versionInfo.EnvName)
                            ?? await GetDotNetReleaseChecksumsShaAsync(downloadUrl, versionInfo.EnvName, versionInfo.Version)
                            ?? await ComputeChecksumShaAsync(downloadUrl);

                        if (sha != null)
                        {
                            sha = sha.ToLowerInvariant();
                            s_shaCache.Add(downloadUrl, sha);
                            Trace.TraceInformation($"Retrieved sha '{sha}' for '{downloadUrl}'.");
                        }
                    }
                }
                else
                {
                    Trace.TraceInformation($"DockerfileShaUpdater no-op - .NET download url not found.");
                }
            }
            else
            {
                Trace.TraceInformation($"DockerfileShaUpdater no-op - .NET product found.");
            }

            return sha;
        }

        private static async Task<string> ComputeChecksumShaAsync(string downloadUrl)
        {
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

        private static async Task<string> GetDotNetCliChecksumsShaAsync(string productDownloadUrl, string envName)
        {
            string sha = null;
            string shaExt = envName.Contains("SDK") ? ".sha" : ".sha512";
            string shaUrl = productDownloadUrl.Replace("dotnetcli", "dotnetclichecksums") + shaExt;

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

        private static async Task<string> GetDotNetReleaseChecksumsShaAsync(
            string productDownloadUrl, string envName, string productVersion)
        {
            string sha = null;
            string product = envName == "DOTNET_SDK_VERSION" ? "sdk" : "runtime";
            string uri = $"https://dotnetcli.blob.core.windows.net/dotnet/checksums/{productVersion}-{product}-sha.txt";

            Trace.TraceInformation($"Downloading '{uri}'.");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    string checksums = await response.Content.ReadAsStringAsync();
                    string installerFileName = productDownloadUrl.Substring(productDownloadUrl.LastIndexOf('/') + 1);

                    Regex shaRegex = new Regex($"(?<sha>[\\S]+)[\\s]+{Regex.Escape(installerFileName)}");
                    Match shaMatch = shaRegex.Match(checksums);
                    if (shaMatch.Success)
                    {
                        sha = shaMatch.Groups["sha"].Value;
                    }
                    else
                    {
                        Trace.TraceInformation($"Failed to find `{installerFileName}` sha");
                    }
                }
                else
                {
                    Trace.TraceInformation($"Failed to find dotnet release checksums");
                }
            }

            return sha;
        }

        private bool TryGetDotNetDownloadUrl(string dockerfile, out string downloadUrl)
        {
            Match match = _downloadUrlRegex.Match(dockerfile);
            downloadUrl = match.Success ? match.Groups[ValueGroupName].Value : null;
            return match.Success;
        }

        private static bool TryGetDotNetVersion(string dockerfile, out (string Version, string EnvName) versionInfo)
        {
            Match match = s_versionRegex.Match(dockerfile);
            versionInfo = match.Success ? (match.Groups[ValueGroupName].Value, match.Groups[EnvNameGroupName].Value) : (null, null);
            return match.Success;
        }

        private static string SubstituteEnvRef(string value, string envName, string envValue)
        {
            return value
                .Replace($"${envName}", envValue)       // *nix ENV var reference format
                .Replace($"$Env:{envName}", envValue);  // Windows ENV var reference format
        }
    }
}
