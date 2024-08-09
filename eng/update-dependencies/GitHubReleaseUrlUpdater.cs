using System;
using System.Linq;
using System.Text.RegularExpressions;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates to the latest download URL when runtime dependencies are being updated.
/// </summary>
internal class GitHubReleaseUrlUpdater(string repoRoot, string variableName, Release release, string DependencyInfoToUse, Regex assetRegex)
    : GitHubReleaseVersionUpdater(repoRoot, variableName, release, DependencyInfoToUse)
{
    private readonly string _variableName = variableName;
    private readonly Regex _assetRegex = assetRegex;

    protected override string? GetValue() => GetReleaseAsset().BrowserDownloadUrl;

    protected ReleaseAsset GetReleaseAsset()
    {
        return Release.Assets.FirstOrDefault(asset => _assetRegex.IsMatch(asset.Name))
            ?? throw new Exception($"Could not find release asset for {_variableName} matching regex {_assetRegex}");
    }
}
