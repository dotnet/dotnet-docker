// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

internal abstract class MinGitUpdater : FileRegexUpdater
{
    private readonly Lazy<JObject> _manifestVariables;
    private readonly string _variableName;

    public MinGitUpdater(string repoRoot, JObject latestMinGitRelease, string variableName)
    {
        _variableName = variableName;
        Path = System.IO.Path.Combine(repoRoot, UpdateDependencies.VersionsFilename);
        VersionGroupName = "val";
        Regex = ManifestHelper.GetManifestVariableRegex(variableName, @$"(?<{VersionGroupName}>\S*)");
        LatestMinGitRelease = latestMinGitRelease;

        _manifestVariables = new Lazy<JObject>(
            () =>
            {
                const string VariablesProperty = "variables";
                JToken? variables = ManifestHelper.LoadManifest(UpdateDependencies.VersionsFilename)[VariablesProperty];
                if (variables is null)
                {
                    throw new InvalidOperationException($"'{VariablesProperty}' property missing in '{UpdateDependencies.VersionsFilename}'");
                }
                return (JObject)variables;
            });
    }

    protected JObject LatestMinGitRelease { get; }

    protected sealed override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        IDependencyInfo? sdkDependencyInfo = dependencyInfos.FirstOrDefault(info => info.SimpleName == "sdk");

        // Only update MinGit variables when we're updating the SDK Dockerfiles
        if (sdkDependencyInfo is null)
        {
            usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();
            return ManifestHelper.GetVariableValue(_variableName, _manifestVariables.Value);
        }

        usedDependencyInfos = new[] { sdkDependencyInfo };
        return GetValue();
    }

    protected abstract string GetValue();

}
#nullable disable
