// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dotnet.Docker;

/// <summary>
/// Updates a variable to the latest GitHub Release tag
/// </summary>
internal class GitHubReleaseVersionUpdater(
    string manifestVersionsFilePath,
    string toolName,
    string variableName,
    string owner,
    string repo)
    : GitHubReleaseUpdaterBase(
        manifestVersionsFilePath,
        toolName,
        variableName,
        owner,
        repo)
{
    protected override string? GetValue(GitHubReleaseInfo dependencyInfo) => dependencyInfo.Release.TagName;
}
