using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

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
        if (ManifestHelper.IsManifestVariable(currentVersion))
        {
            return currentVersion;
        }

        usedDependencyInfos = [ usedDependencyInfo ];
        return GetValue();
    }
}
