// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the MinGit SHA checksum in the manifest.versions.json file.
/// </summary>
internal class MinGitShaUpdater : FileRegexUpdater
{
    private const string ShaGroupName = "Sha";
    private readonly JObject _latestMinGitRelease;

    public MinGitShaUpdater(string repoRoot, JObject latestMinGitRelease)
    {
        Path = System.IO.Path.Combine(repoRoot, UpdateDependencies.VersionsFilename);
        VersionGroupName = ShaGroupName;
        Regex = ManifestHelper.GetManifestVariableRegex("mingit|x64|sha", @$"(?<{ShaGroupName}>\S*)");
        _latestMinGitRelease = latestMinGitRelease;
    }

    protected override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();

        JObject asset = MinGitUrlUpdater.GetMinGitAsset(_latestMinGitRelease);

        // The SHA for the MinGit zip file is contained in the body description of the MinGit release as a table listing.

        string name = asset.GetRequiredToken<JValue>("name").ToString();
        string body = _latestMinGitRelease.GetRequiredToken<JValue>("body").ToString();

        const string ShaGroupName = "sha";
        Regex shaRegex = new(@$"{Regex.Escape(name)}\s\|\s(?<{ShaGroupName}>[0-9|a-f]+)");
        string sha = shaRegex.Match(body).Groups[ShaGroupName].Value;
        return sha;
    }

}
#nullable disable
