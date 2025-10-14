// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal static class RocksToolboxUpdater
{
    public const string ToolName = Repo;

    private const string Owner = "canonical";

    private const string Repo = "rocks-toolbox";

    public static IDependencyUpdater GetUpdater(string manifestVersionsFilePath) =>
        new GitHubReleaseVersionUpdater(
            manifestVersionsFilePath: manifestVersionsFilePath,
            toolName: ToolName,
            variableName: $"{ToolName}|latest|version",
            owner: Owner,
            repo: Repo);

    public static async Task<GitHubReleaseInfo> GetBuildInfoAsync() =>
        new GitHubReleaseInfo(
            SimpleName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));
}
