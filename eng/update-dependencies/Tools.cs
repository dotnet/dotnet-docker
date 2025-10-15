// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal static class Tools
{
    public static readonly string[] SupportedTools =
    [
        SyftUpdater.ToolName,
        ChiselUpdater.ToolName,
        RocksToolboxUpdater.ToolName,
        MinGitUpdater.ToolName
    ];

    public static async Task<GitHubReleaseInfo> GetToolBuildInfoAsync(string tool) =>
        tool switch
        {
            MinGitUpdater.ToolName => await MinGitUpdater.GetBuildInfoAsync(),
            SyftUpdater.ToolName => await SyftUpdater.GetBuildInfoAsync(),
            ChiselUpdater.ToolName => await ChiselUpdater.GetBuildInfoAsync(),
            RocksToolboxUpdater.ToolName => await RocksToolboxUpdater.GetBuildInfoAsync(),
            _ => throw new ArgumentException($"Unknown tool {tool}", nameof(tool)),
        };

    public static IEnumerable<IDependencyUpdater> GetToolUpdaters(string manifestVersionsFilePath) =>
    [
        ..MinGitUpdater.GetUpdaters(manifestVersionsFilePath),
        ..ChiselUpdater.GetUpdaters(manifestVersionsFilePath),
        RocksToolboxUpdater.GetUpdater(manifestVersionsFilePath),
        SyftUpdater.GetUpdater(manifestVersionsFilePath),
    ];
}
