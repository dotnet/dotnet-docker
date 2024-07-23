// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the MinGit version in the manifest.versions.json file.
/// </summary>
internal partial class MinGitUrlUpdater(string repoRoot, Release release, string type = "url")
    : GitHubReleaseUrlUpdater(repoRoot, GetManifestVariableName(type), release, DependencyInfoToUse, MinGitUrlRegex())
{
    private const string DependencyInfoToUse = "sdk";

    protected static string GetManifestVariableName(string type) => "mingit|latest|x64|" + type;

    [GeneratedRegex(@"^MinGit.*64-bit.*\.zip$")]
    protected static partial Regex MinGitUrlRegex();
}
