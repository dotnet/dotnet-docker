using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

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
