// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the MinGit version in the manifest.versions.json file.
/// </summary>
internal class MinGitUrlUpdater : FileRegexUpdater
{
    private const string UrlGroupName = "Url";
    private readonly JObject _latestMinGitRelease;

    public MinGitUrlUpdater(string repoRoot, JObject latestMinGitRelease)
    {
        Path = System.IO.Path.Combine(repoRoot, UpdateDependencies.VersionsFilename);
        VersionGroupName = UrlGroupName;
        Regex = ManifestHelper.GetManifestVariableRegex("mingit|x64|url", @$"(?<{UrlGroupName}>\S*)");
        _latestMinGitRelease = latestMinGitRelease;
    }

    protected override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();

        JObject mingitAsset = GetMinGitAsset(_latestMinGitRelease);
        return mingitAsset.GetRequiredToken<JValue>("browser_download_url").ToString();
    }

    internal static JObject GetMinGitAsset(JObject release)
    {
        JArray assets = release.GetRequiredToken<JArray>("assets");

        JObject mingitAsset = (JObject)(assets.FirstOrDefault(asset => IsMinGit64BitAsset((JObject)asset)) ??
            throw new InvalidOperationException("Can't find 64-bit MinGit asset."));
        return mingitAsset;
    }

    private static bool IsMinGit64BitAsset(JObject asset)
    {
        string name = asset.GetRequiredToken<JValue>("name").ToString();
        return name.StartsWith("MinGit", StringComparison.OrdinalIgnoreCase) &&
            name.Contains("64-bit", StringComparison.OrdinalIgnoreCase) &&
            name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
    }
}
#nullable disable
