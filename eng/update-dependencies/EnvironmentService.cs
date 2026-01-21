// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.Services.Profile;

namespace Dotnet.Docker;

/// <summary>
/// Provides access to environment variables and runtime context.
/// </summary>
public interface IEnvironmentService
{
    /// <summary>
    /// Gets the Azure DevOps system access token from the SYSTEM_ACCESSTOKEN environment variable.
    /// </summary>
    string? GetSystemAccessToken();

    /// <summary>
    /// Gets the current build ID if running in Azure Pipelines, or null otherwise.
    /// </summary>
    string? GetBuildId();

    /// <summary>
    /// Determines if the code is running in an Azure Pipelines environment.
    /// </summary>
    bool IsRunningInAzurePipelines();
}

/// <inheritdoc />
internal sealed class EnvironmentService : IEnvironmentService
{
    // List of predefined Azure Pipelines variables:
    // https://learn.microsoft.com/azure/devops/pipelines/build/variables#system-variables

    /// <inheritdoc />
    public string? GetSystemAccessToken() =>
        Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");

    /// <inheritdoc />
    public string? GetBuildId() =>
        IsRunningInAzurePipelines()
            ? Environment.GetEnvironmentVariable("BUILD_BUILDID")
            : null;

    /// <inheritdoc />
    public bool IsRunningInAzurePipelines() =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"));
}

internal static class EnvironmentServiceExtensions
{
    public static IServiceCollection AddEnvironmentService(this IServiceCollection services)
    {
        services.TryAddSingleton<IEnvironmentService, EnvironmentService>();
        return services;
    }
}
