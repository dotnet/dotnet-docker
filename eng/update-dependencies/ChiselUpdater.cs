// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal static class ChiselUpdater
{
    public const string ToolName = Repo;

    private const string Owner = "canonical";

    private const string Repo = "chisel";

    private static readonly string[] s_supportedArchitectures = ["amd64", "arm", "arm64"];

    public static IEnumerable<IDependencyUpdater> GetUpdaters(string manifestVersionsFilePath) =>
        s_supportedArchitectures
            .SelectMany<string, IDependencyUpdater>(arch =>
                [
                    new GitHubReleaseUrlUpdater(
                        manifestVersionsFilePath: manifestVersionsFilePath,
                        toolName: ToolName,
                        variableName: GetChiselManifestVariable(ToolName, arch, "url", "latest"),
                        owner: Owner,
                        repo: Repo,
                        assetRegex: GetAssetRegex(arch)),
                    new ChiselReleaseShaUpdater(
                        manifestVersionsFilePath,
                        arch),
                ]);

    public static async Task<GitHubReleaseInfo> GetBuildInfoAsync() =>
        new GitHubReleaseInfo(
            SimpleName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));

    public static string GetChiselManifestVariable(string product, string arch, string type, string dockerfileVersion = "latest")
    {
        // Workaround for ambiguous method call, will be fixed with https://github.com/dotnet/csharplang/issues/8374
        return string.Join('|', new string[] { product, dockerfileVersion, ToManifestArch(arch), type });
    }

    private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;

    private class ChiselReleaseShaUpdater(
        string manifestVersionsFilePath,
        string arch)
        : GitHubReleaseUrlUpdater(
            manifestVersionsFilePath,
            ChiselUpdater.ToolName,
            GetChiselManifestVariable("chisel", arch, ShaFunction, "latest"),
            ChiselUpdater.Owner,
            ChiselUpdater.Repo,
            GetAssetRegex(arch))
    {
        private const string ShaFunction = "sha384";

        private static readonly HttpClient s_httpClient = new();

        protected override string? GetValue(GitHubReleaseInfo dependencyInfo)
        {
            string? downloadUrl = base.GetValue(dependencyInfo);
            if (downloadUrl is null)
            {
                return null;
            }

            downloadUrl = $"{downloadUrl}.{ShaFunction}";
            return GetChecksumFromUrlAsync(downloadUrl).Result;
        }

        private static async Task<string?> GetChecksumFromUrlAsync(string downloadUrl)
        {
            using HttpResponseMessage response = await s_httpClient.GetAsync(downloadUrl);
            if (!response.IsSuccessStatusCode)
            {
                Trace.TraceInformation($"Failed to download {downloadUrl}.");
                return null;
            }

            // Expected format:
            // abcdef1234567890  chisel_v1.0.0_linux_amd64.tar.gz
            string content = await response.Content.ReadAsStringAsync();
            string sha = content.Split("  ")[0];
            return sha.ToLowerInvariant();
        }
    }
}
