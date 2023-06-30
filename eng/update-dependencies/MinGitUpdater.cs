// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

internal abstract class MinGitUpdater : VariableUpdaterBase
{
    protected JObject LatestMinGitRelease { get; }

    public MinGitUpdater(string repoRoot, JObject latestMinGitRelease, string variableName) : base(repoRoot, variableName)
    {
        LatestMinGitRelease = latestMinGitRelease;
    }

    protected sealed override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        IDependencyInfo? sdkDependencyInfo = dependencyInfos.FirstOrDefault(info => info.SimpleName == "sdk");

        // Only update MinGit variables when we're updating the SDK Dockerfiles
        if (sdkDependencyInfo is null)
        {
            usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();
            return ManifestHelper.GetVariableValue(VariableName, ManifestVariables.Value);
        }

        usedDependencyInfos = new[] { sdkDependencyInfo };
        return GetValue();
    }

    protected abstract string GetValue();

}
#nullable disable
