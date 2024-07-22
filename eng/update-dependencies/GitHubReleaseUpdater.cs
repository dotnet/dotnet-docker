using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.DotNet.VersionTools.Dependencies;
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

/// <summary>
/// Updates to the latest release tag name when runtime dependencies are being updated.
/// </summary>
internal class GitHubReleaseVersionUpdater(string repoRoot, string variableName, Release release, string dependencyInfoToUse)
    : GitHubReleaseUpdaterBase(repoRoot, variableName, release)
{
    private readonly string _dependencyInfoToUse = dependencyInfoToUse;

    protected override string? GetValue()
    {
        return Release.TagName;
    }

    protected sealed override IDependencyInfo? GetDependencyInfoToUse(IEnumerable<IDependencyInfo> dependencyInfos)
    {
        return dependencyInfos.FirstOrDefault(info => info.SimpleName == _dependencyInfoToUse);
    }
}

internal abstract partial class GitHubReleaseUpdaterBase(string repoRoot, string variableName, Release release)
    : VariableUpdaterBase(repoRoot, variableName)
{
    protected abstract IDependencyInfo? GetDependencyInfoToUse(IEnumerable<IDependencyInfo> dependencyInfos);

    protected abstract string? GetValue();

    protected Release Release { get; } = release;

    protected sealed override string? TryGetDesiredValue(
        IEnumerable<IDependencyInfo> dependencyInfos,
        out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = [];

        string currentVersion = ManifestHelper.TryGetVariableValue(VariableName, ManifestVariables.Value);
        if (string.IsNullOrEmpty(currentVersion))
        {
            return "";
        }

        IDependencyInfo? usedDependencyInfo = GetDependencyInfoToUse(dependencyInfos);
        if (usedDependencyInfo is null)
        {
            return currentVersion;
        }

        // Don't overwrite versions that are set to other variables
        if (AnyVariableRegex().IsMatch(currentVersion))
        {
            return currentVersion;
        }

        usedDependencyInfos = [ usedDependencyInfo ];
        return GetValue();
    }

    [GeneratedRegex(@"^\$\(.*\)$")]
    private static partial Regex AnyVariableRegex();
}
