// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
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
    private readonly JObject _manifestVariables;
    private readonly string _manifestVariableName;

    /// <summary>
    /// Creates a new <see cref="IDependencyUpdater"/> for updating base URLs.
    /// If the base URL variable cannot be found in the manifest, the updater
    /// won't do anything.
    /// </summary>
    public static IDependencyUpdater Create(string repoRoot, SpecificCommandOptions options)
    {
        // Load manifest and extract variables once so the constructor doesn't duplicate this logic
        var manifest = ManifestHelper.LoadManifest(SpecificCommand.VersionsFilename);
        var manifestVariables = (JObject?)manifest["variables"];

        if (manifestVariables is null)
        {
            Trace.TraceWarning("BaseUrlUpdater: manifest variables missing - skipping base URL update.");
            return new EmptyDependencyUpdater();
        }

        string baseUrlVarName = ManifestHelper.GetBaseUrlVariableName(
            options.DockerfileVersion,
            options.SourceBranch,
            options.VersionSourceName,
            options.IsSdkOnly);

        if (!manifestVariables.ContainsKey(baseUrlVarName))
        {
            Trace.TraceWarning($"BaseUrlUpdater: variable '{baseUrlVarName}' not found - skipping base URL update.");
            return new EmptyDependencyUpdater();
        }

        return new BaseUrlUpdater(repoRoot, options, manifestVariables, baseUrlVarName);
    }

    private BaseUrlUpdater(
        string repoRoot,
        SpecificCommandOptions options,
        JObject manifestVariables,
        string manifestVariableName)
    {
        Path = System.IO.Path.Combine(repoRoot, SpecificCommand.VersionsFilename);
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
        string unresolvedBaseUrl = _manifestVariables[baseUrlVersionVarName]?.ToString() ??
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
