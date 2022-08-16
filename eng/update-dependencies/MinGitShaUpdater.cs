// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the MinGit SHA checksum in the manifest.versions.json file.
/// </summary>
internal class MinGitShaUpdater : MinGitUpdater
{
    public MinGitShaUpdater(string repoRoot, JObject latestMinGitRelease)
        : base(
            repoRoot,
            latestMinGitRelease,
            "mingit|x64|sha")
    {
    }

    protected override string GetValue()
    {
        JObject asset = MinGitUrlUpdater.GetMinGitAsset(LatestMinGitRelease);

        // The SHA for the MinGit zip file is contained in the body description of the MinGit release as a table listing.

        string name = asset.GetRequiredToken<JValue>("name").ToString();
        string body = LatestMinGitRelease.GetRequiredToken<JValue>("body").ToString();

        const string ShaGroupName = "sha";
        Regex shaRegex = new(@$"{Regex.Escape(name)}\s\|\s(?<{ShaGroupName}>[0-9|a-f]+)");
        string sha = shaRegex.Match(body).Groups[ShaGroupName].Value;
        return sha;
    }

}
#nullable disable
