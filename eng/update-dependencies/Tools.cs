// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

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

    public static async Task<GitHubReleaseInfo> GetReleaseAsync(string tool) =>
        tool switch
        {
            MinGitUpdater.ToolName => await MinGitUpdater.GetReleaseAsync(),
            SyftUpdater.ToolName => await SyftUpdater.GetReleaseAsync(),
            ChiselUpdater.ToolName => await ChiselUpdater.GetReleaseAsync(),
            RocksToolboxUpdater.ToolName => await RocksToolboxUpdater.GetReleaseAsync(),
            _ => throw new ArgumentException($"Unknown tool {tool}", nameof(tool)),
        };

    public static async Task UpdateAsync(
        ManifestVariables variables,
        GitHubReleaseInfo release,
        CancellationToken cancellationToken = default)
    {
        switch (release.ToolName)
        {
            case MinGitUpdater.ToolName:
                MinGitUpdater.Update(variables, release.Release);
                break;
            case SyftUpdater.ToolName:
                SyftUpdater.Update(variables, release.Release);
                break;
            case ChiselUpdater.ToolName:
                await ChiselUpdater.UpdateAsync(variables, release.Release, cancellationToken);
                break;
            case RocksToolboxUpdater.ToolName:
                RocksToolboxUpdater.Update(variables, release.Release);
                break;
            default:
                throw new ArgumentException($"Unknown tool {release.ToolName}", nameof(release));
        }
    }

    /// <summary>
    /// Tool release updates preserve empty values and aliases, and only edit keys present on this branch.
    /// Check before resolving assets so disabled variables do not require downloads.
    /// </summary>
    public static bool ShouldUpdateVariable(ManifestVariables variables, string variableName)
    {
        if (!variables.Contains(variableName))
        {
            Trace.TraceInformation($"Skipping absent manifest variable '{variableName}'.");
            return false;
        }

        string currentValue = variables.GetRawValue(variableName);
        if (currentValue.Length == 0 || ManifestHelper.IsManifestVariable(currentValue))
        {
            Trace.TraceInformation($"Leaving manifest variable '{variableName}' unchanged.");
            return false;
        }

        return true;
    }
}
