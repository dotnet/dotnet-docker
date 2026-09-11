// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Octokit;

namespace Dotnet.Docker;

internal static class RocksToolboxUpdater
{
    public const string ToolName = Repo;

    private const string Owner = "canonical";

    private const string Repo = "rocks-toolbox";

    public static void Update(ManifestVariables variables, Release release)
    {
        string variableName = $"{ToolName}|latest|version";
        if (Tools.ShouldUpdateVariable(variables, variableName))
        {
            VariableUpdater.Update(variables, variableName, release.TagName);
        }
    }

    public static async Task<GitHubReleaseInfo> GetReleaseAsync() =>
        new GitHubReleaseInfo(
            ToolName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));
}
