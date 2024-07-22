// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the MinGit SHA checksum in the manifest.versions.json file.
/// </summary>
internal sealed class MinGitShaUpdater(string repoRoot, Release release)
    : MinGitUrlUpdater(repoRoot, release, "sha")
{
    protected override string GetValue()
    {
        ReleaseAsset asset = GetReleaseAsset();

        // The SHA for the MinGit zip file is contained in the body description of the MinGit release as a table listing.
        string name = asset.Name.ToString();
        string body = Release.Body;

        const string ShaGroupName = "sha";
        Regex shaRegex = new(@$"{Regex.Escape(name)}\s\|\s(?<{ShaGroupName}>[0-9|a-f]+)");
        string sha = shaRegex.Match(body).Groups[ShaGroupName].Value;
        return sha;
    }
}
