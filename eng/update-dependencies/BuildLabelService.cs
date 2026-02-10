// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dotnet.Docker;

/// <summary>
/// Service for adding build tags to Azure Pipelines runs.
/// </summary>
public interface IBuildLabelService
{
    /// <summary>
    /// Adds one or more build tags to the current Azure Pipelines run.
    /// </summary>
    /// <remarks>
    /// When not running in Azure Pipelines, the logging directives are harmlessly echoed.
    /// </remarks>
    void AddBuildTags(params IEnumerable<string> tags);
}

/// <inheritdoc />
internal sealed class BuildLabelService(TextWriter output) : IBuildLabelService
{
    // Azure Pipelines logging command format:
    // https://learn.microsoft.com/azure/devops/pipelines/scripts/logging-commands

    /// <inheritdoc />
    public void AddBuildTags(params IEnumerable<string> tags)
    {
        foreach (var tag in tags)
        {
            output.WriteLine($"##vso[build.addbuildtag]{tag}");
        }
    }
}

internal static class BuildLabelServiceExtensions
{
    public static IServiceCollection AddBuildLabelService(this IServiceCollection services)
    {
        services.TryAddSingleton<IBuildLabelService>(new BuildLabelService(Console.Out));
        return services;
    }
}
