// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dotnet.Docker;

/// <summary>
/// Updates to the latest download URL when runtime dependencies are being updated.
/// </summary>
internal class GitHubReleaseUrlUpdater(
    string manifestVersionsFilePath,
    string toolName,
    string variableName,
    string owner,
    string repo,
    Regex assetRegex)
    : GitHubReleaseUpdaterBase(
        manifestVersionsFilePath,
        toolName,
        variableName,
        owner,
        repo)
{
    private readonly string _variableName = variableName;

    private readonly Regex _assetRegex = assetRegex;

    protected override string? GetValue(GitHubReleaseInfo dependencyInfo) =>
        dependencyInfo.Release.Assets.FirstOrDefault(asset => _assetRegex.IsMatch(asset.Name))?.BrowserDownloadUrl
            ?? throw new Exception(
                $"Could not find release asset for {_variableName} matching regex {_assetRegex}");
}
