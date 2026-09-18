// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
using Octokit;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class ChiselUpdater(IReleasesClient releases, HttpClient httpClient) : IGitHubReleaseUpdater
{
    private const string Owner = "canonical";
    private const string Repo = "chisel";

    public static string Name => "chisel";
    public static string VersionSourceName => "chisel";

    private static readonly string[] s_supportedArchitectures = ["amd64", "arm", "arm64"];
    private static string GetChiselManifestVariable(string product, string arch, string type)
    {
        // Workaround for ambiguous method call, will be fixed with https://github.com/dotnet/csharplang/issues/8374
        return string.Join('|', new string[] { product, "latest", ToManifestArch(arch), type });
    }

    private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;

    public async Task<DependencyUpdate> ResolveFromGitHubReleaseAsync(CancellationToken cancellationToken)
    {
        Release release = await releases.GetLatest(Owner, Repo).WaitAsync(cancellationToken);

        return new DependencyUpdate(
            Scope: "",
            Description: $"Update Chisel to {release.TagName}",
            ApplyAsync: (variables, _, token) => ApplyAsync(variables, release, token));
    }

    private async Task ApplyAsync(
        ManifestVariables variables,
        Release release,
        CancellationToken cancellationToken)
    {
        foreach (string arch in s_supportedArchitectures)
        {
            string urlVariable = GetChiselManifestVariable(Name, arch, "url");
            string shaVariable = GetChiselManifestVariable(Name, arch, "sha384");
            bool updateUrl = variables.ShouldUpdateLiteral(urlVariable);
            bool updateSha = variables.ShouldUpdateLiteral(shaVariable);
            if (!updateUrl && !updateSha)
            {
                continue;
            }

            Regex assetRegex = GetAssetRegex(arch);
            ReleaseAsset asset = release.Assets.FirstOrDefault(asset => assetRegex.IsMatch(asset.Name))
                ?? throw new InvalidOperationException($"Could not find Chisel release asset matching regex {assetRegex}.");

            if (updateUrl)
            {
                variables.SetValue(urlVariable, asset.BrowserDownloadUrl);
            }

            if (updateSha)
            {
                string checksumUrl = $"{asset.BrowserDownloadUrl}.sha384";
                string content = await httpClient.GetStringAsync(checksumUrl, cancellationToken);

                // Each checksum file contains "<sha384>  <archive name>".
                string sha = content.Split("  ")[0].ToLowerInvariant();
                variables.SetValue(shaVariable, sha);
            }
        }
    }
}
