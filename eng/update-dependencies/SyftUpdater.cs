// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Octokit;

namespace Dotnet.Docker;

internal static class SyftUpdater
{
    public const string Owner = "anchore";

    public const string Repo = "syft";

    public const string ToolName = Repo;

    public const string VariableName = "syft|version";

    public static async Task<GitHubReleaseInfo> GetReleaseAsync() =>
         new GitHubReleaseInfo(
            ToolName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));

    public static void Update(ManifestVariables variables, Release release)
    {
        if (Tools.ShouldUpdateVariable(variables, VariableName))
        {
            VariableUpdater.Update(variables, VariableName, release.TagName);
        }
    }
}
