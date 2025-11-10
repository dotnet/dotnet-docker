// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal static class SyftUpdater
{
    public const string Owner = "anchore";

    public const string Repo = "syft";

    public const string ToolName = Repo;

    public const string VariableName = "syft|version";

    public static async Task<GitHubReleaseInfo> GetBuildInfoAsync() =>
         new GitHubReleaseInfo(
            SimpleName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));

    public static IDependencyUpdater GetUpdater(string manifestVersionsFilePath) =>
        new GitHubReleaseVersionUpdater(manifestVersionsFilePath, ToolName, VariableName, Owner, Repo);
}
