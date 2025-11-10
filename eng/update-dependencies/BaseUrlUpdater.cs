// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

namespace Dotnet.Docker;

/// <summary>
/// Updates the baseUrl variables in the manifest.versions.json file.
/// </summary>
internal class BaseUrlUpdater : FileRegexUpdater
{
    private const string BaseUrlGroupName = "BaseUrlValue";
    private readonly SpecificCommandOptions _options;
    private readonly ManifestVariables _manifestVariables;
    private readonly string _manifestVariableName;

    /// <summary>
    /// Creates a new <see cref="IDependencyUpdater"/> for updating base URLs.
    /// If the base URL variable cannot be found in the manifest, the updater
    /// won't do anything.
    /// </summary>
    public static IEnumerable<IDependencyUpdater> CreateUpdaters(ManifestVariables manifestVariables, SpecificCommandOptions options)
    {
        if (manifestVariables is null)
        {
            Trace.TraceWarning("BaseUrlUpdater: manifest variables missing - skipping base URL update.");
            return [];
        }

        var upstreamBranch = manifestVariables.GetValue("branch");
        var baseUrlVarNames = ManifestHelper.GetBaseUrlVariableNames(
            dockerfileVersion: options.DockerfileVersion,
            branch: upstreamBranch,
            versionSourceName: options.VersionSourceName,
            sdkOnlyRelease: options.IsSdkOnly);

        IEnumerable<IDependencyUpdater> updaters = baseUrlVarNames
            .SelectMany(variable => CreateUpdater(variable, manifestVariables, options));

        return updaters;
    }

    private static IEnumerable<IDependencyUpdater> CreateUpdater(
        string baseUrlVarName,
        ManifestVariables manifestVariables,
        SpecificCommandOptions options)
    {
        var variableHasValue = manifestVariables.HasValue(baseUrlVarName);

        if (!variableHasValue)
        {
            Trace.TraceWarning($"BaseUrlUpdater: variable '{baseUrlVarName}' not found - skipping base URL update.");
            return [];
        }

        return [new BaseUrlUpdater(options, manifestVariables, baseUrlVarName)];
    }

    private BaseUrlUpdater(
        SpecificCommandOptions options,
        ManifestVariables manifestVariables,
        string manifestVariableName)
    {
        Path = options.GetManifestVersionsFilePath();
        VersionGroupName = BaseUrlGroupName;
        _options = options;
        _manifestVariables = manifestVariables;
        _manifestVariableName = manifestVariableName;

        Regex = ManifestHelper.GetManifestVariableRegex(_manifestVariableName, $"(?<{BaseUrlGroupName}>.+)");
    }

    protected override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();

        string baseUrlVersionVarName = _manifestVariableName;
        string unresolvedBaseUrl = _manifestVariables.Variables[baseUrlVersionVarName]?.ToString() ??
            throw new InvalidOperationException($"Variable with name '{baseUrlVersionVarName}' is missing.");

        if (_options.IsInternal)
        {
            if (string.IsNullOrEmpty(_options.InternalBaseUrl))
            {
                throw new InvalidOperationException("InternalBaseUrl must be set in order to update base url for internal builds");
            }

            unresolvedBaseUrl = _options.InternalBaseUrl;
        }
        else if (_options.ReleaseState.HasValue)
        {
            unresolvedBaseUrl = $"$({ManifestHelper.GetBaseUrlVariableName(_options.ReleaseState.Value, _options.TargetBranch)})";
        }
        else
        {
            // Modifying the URL from internal to public is not suppported because it's not possible to know
            // what common variable it was originally referencing when it was last public.
        }

        return unresolvedBaseUrl;
    }
}
