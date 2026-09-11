// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
using Octokit;

namespace Dotnet.Docker;

internal static class ChiselUpdater
{
    public const string ToolName = Repo;

    private const string Owner = "canonical";

    private const string Repo = "chisel";

    private static readonly string[] s_supportedArchitectures = ["amd64", "arm", "arm64"];
    private static readonly HttpClient s_httpClient = new();

    public static async Task<GitHubReleaseInfo> GetReleaseAsync() =>
        new GitHubReleaseInfo(
            ToolName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));

    public static string GetChiselManifestVariable(string product, string arch, string type, string dockerfileVersion = "latest")
    {
        // Workaround for ambiguous method call, will be fixed with https://github.com/dotnet/csharplang/issues/8374
        return string.Join('|', new string[] { product, dockerfileVersion, ToManifestArch(arch), type });
    }

    private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;

    public static async Task UpdateAsync(
        ManifestVariables variables,
        Release release,
        CancellationToken cancellationToken = default)
    {
        foreach (string arch in s_supportedArchitectures)
        {
            string urlVariable = GetChiselManifestVariable(ToolName, arch, "url");
            string shaVariable = GetChiselManifestVariable(ToolName, arch, "sha384");
            bool updateUrl = Tools.ShouldUpdateVariable(variables, urlVariable);
            bool updateSha = Tools.ShouldUpdateVariable(variables, shaVariable);
            if (!updateUrl && !updateSha)
            {
                continue;
            }

            Regex assetRegex = GetAssetRegex(arch);
            ReleaseAsset asset = release.Assets.FirstOrDefault(asset => assetRegex.IsMatch(asset.Name))
                ?? throw new InvalidOperationException($"Could not find Chisel release asset matching regex {assetRegex}.");

            if (updateUrl)
            {
                VariableUpdater.Update(variables, urlVariable, asset.BrowserDownloadUrl);
            }

            if (updateSha)
            {
                string checksumUrl = $"{asset.BrowserDownloadUrl}.sha384";
                string content = await s_httpClient.GetStringAsync(checksumUrl, cancellationToken);

                // Each checksum file contains "<sha384>  <archive name>".
                string sha = content.Split("  ")[0].ToLowerInvariant();
                VariableUpdater.Update(variables, shaVariable, sha);
            }
        }
    }
}
