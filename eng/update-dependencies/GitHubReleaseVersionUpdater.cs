// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates a variable to the latest GitHub Release tag
/// </summary>
internal class GitHubReleaseVersionUpdater(
    string repoRoot,
    string toolName,
    string variableName,
    string owner,
    string repo)
    : GitHubReleaseUpdaterBase(
        repoRoot,
        toolName,
        variableName,
        owner,
        repo)
{
    protected override string? GetValue(GitHubReleaseInfo dependencyInfo) => dependencyInfo.Release.TagName;
}
