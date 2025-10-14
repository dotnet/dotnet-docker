// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

/// <summary>
/// Updates a single variable in the manifest.versions.json file.
/// </summary>
/// <param name="manifestVersionsFilePath">
/// Path to the manifest.versions.json file to updated.
/// </param>
/// <param name="dependencyInfo">
/// Information about what variable to update and what the new value should be.
/// </param>
internal sealed class VariableUpdater(
    string manifestVersionsFilePath,
    VariableUpdateInfo dependencyInfo
) : VariableUpdaterBase(manifestVersionsFilePath, dependencyInfo.SimpleName)
{
    private readonly VariableUpdateInfo _dependencyInfo = dependencyInfo;

    protected override string TryGetDesiredValue(
        IEnumerable<IDependencyInfo> dependencyInfos,
        out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = [_dependencyInfo];
        return _dependencyInfo.SimpleVersion;
    }
}
