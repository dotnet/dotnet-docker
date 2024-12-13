// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates to the latest release tag name when runtime dependencies are being updated.
/// </summary>
internal abstract class GitHubReleaseVersionUpdater(
    string toolName,
    string repoRoot,
    string variableName,
    string owner,
    string repo)
    : GitHubReleaseUpdaterBase(
        toolName,
        repoRoot,
        variableName,
        owner,
        repo)
{
    protected override string? GetValue(GitHubReleaseInfo dependencyInfo) => dependencyInfo.Release.TagName;
}
