// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal abstract partial class GitHubReleaseUpdaterBase(
    string manifestVersionsFilePath,
    string toolName,
    string variableName,
    string owner,
    string repo)
    : VariableUpdaterBase(
        manifestVersionsFilePath,
        variableName)
{
    protected string ToolName { get; } = toolName;

    protected string Owner { get; } = owner;

    protected string Repo { get; } = repo;

    protected abstract string? GetValue(GitHubReleaseInfo dependencyInfo);

    protected override string? TryGetDesiredValue(
        IEnumerable<IDependencyInfo> dependencyInfos,
        out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = [];

        string currentVersion = ManifestHelper.TryGetVariableValue(VariableName, ManifestVariables.Value);
        if (string.IsNullOrEmpty(currentVersion))
        {
            return "";
        }
        else if (ManifestHelper.IsManifestVariable(currentVersion))
        {
            // Don't overwrite versions that are set to other variables
            return currentVersion;
        }

        GitHubReleaseInfo? usedDependencyInfo =
            dependencyInfos
                .OfType<GitHubReleaseInfo>()
                .FirstOrDefault(info => info.SimpleName == ToolName);

        if (usedDependencyInfo is null)
        {
            return currentVersion;
        }

        usedDependencyInfos = [ usedDependencyInfo ];
        return GetValue(usedDependencyInfo);
    }
}
