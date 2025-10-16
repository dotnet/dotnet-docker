// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.ProductConstructionService.Client.Models;

namespace Dotnet.Docker;

/// <summary>
/// Extensions for .NET build asset registry <see cref="Build"/>s.
/// </summary>
internal static class BuildExtensions
{
    /// <summary>
    /// Given a <see cref="Build"/>, maps its source repository (either GitHub
    /// or Azure Devops) to a supported <see cref="BuildRepo"/> enum value.
    /// </summary>
    public static BuildRepo GetBuildRepo(this Build build)
    {
        string repo = build.GitHubRepository ?? build.AzureDevOpsRepository;
        return repo switch
        {
            "https://github.com/dotnet/dotnet" or "https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet" => BuildRepo.Vmr,
            "https://github.com/dotnet/aspire" or "https://dev.azure.com/dnceng/internal/_git/dotnet-aspire" => BuildRepo.Aspire,
            _ => throw new InvalidOperationException($"Build {build.Id} was from unsupported repository '{repo}'"),
        };
    }
}
