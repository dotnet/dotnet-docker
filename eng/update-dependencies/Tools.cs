// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal static class Tools
{
    public static readonly string[] SupportedTools =
    [
        SyftUpdater.ToolName,
        ..ChiselUpdater.ToolNames,
        MinGitUpdater.ToolName
    ];

    public static async Task<ToolBuildInfo> GetToolBuildInfoAsync(string tool) =>
        tool switch
        {
            _ => throw new ArgumentException($"Unknown tool {tool}", nameof(tool)),
        };
}
