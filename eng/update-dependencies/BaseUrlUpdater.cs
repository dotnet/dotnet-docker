// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the baseUrl variables in the manifest.versions.json file.
/// </summary>
internal class BaseUrlUpdater : FileRegexUpdater
{
    private const string BaseUrlGroupName = "BaseUrlValue";
    private readonly Options _options;
    private readonly JObject _manifestVariables;

    public BaseUrlUpdater(string repoRoot, Options options)
    {
        Path = System.IO.Path.Combine(repoRoot, UpdateDependencies.VersionsFilename);
        VersionGroupName = BaseUrlGroupName;
        Regex = ManifestHelper.GetManifestVariableRegex(
            ManifestHelper.GetBaseUrlVariableName(options.DockerfileVersion, options.SourceBranch, options.VersionSourceName),
            $"(?<{BaseUrlGroupName}>.+)");
        _options = options;

        _manifestVariables = (JObject)ManifestHelper.LoadManifest(UpdateDependencies.VersionsFilename)["variables"];
    }

    protected override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();

        string baseUrlVersionVarName = ManifestHelper.GetBaseUrlVariableName(_options.DockerfileVersion, _options.SourceBranch, _options.VersionSourceName);
        string unresolvedBaseUrl = _manifestVariables[baseUrlVersionVarName].ToString();

        if (_options.IsInternal)
        {
            unresolvedBaseUrl = unresolvedBaseUrl.Replace("public", "internal");
        }
        else
        {
            unresolvedBaseUrl = unresolvedBaseUrl.Replace("internal", "public");
        }

        return unresolvedBaseUrl;
    }
}
#nullable disable
