// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.ProductConstructionService.Client.Models;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Extensions for .NET build asset registry <see cref="Build"/>s.
/// </summary>
internal static class BuildExtensions
{
    /// <summary>
    /// Maps a BAR build's repository to its registered updater.
    /// </summary>
    public static string GetUpdaterKey(this Build build) =>
        GetUpdaterKey(build.GitHubRepository ?? build.AzureDevOpsRepository);

    public static string GetUpdaterKey(string repository) =>
        repository switch
        {
            "https://github.com/dotnet/dotnet" or "https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet" => DotNetUpdater.Key,
            AspireUpdater.PublicRepository or AspireUpdater.InternalRepository => "aspire",
            _ => throw new InvalidOperationException($"No updater registered for build repository '{repository}'."),
        };
}
