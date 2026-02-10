// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.TeamFoundation.Build.WebApi;

namespace Dotnet.Docker;

internal interface IPipelinesService
{
    /// <summary>
    /// Gets a build artifact from an Azure DevOps pipeline run.
    /// </summary>
    Task<BuildArtifact> GetArtifactAsync(string azdoOrg, string azdoProject, int buildId, string artifactName);

    /// <summary>
    /// Gets the build tags from an Azure DevOps pipeline run.
    /// </summary>
    Task<IReadOnlyList<string>> GetBuildTagsAsync(string azdoOrg, string azdoProject, int buildId);
}

/// <inheritdoc/>
internal sealed class PipelinesService(IAzdoAuthProvider azdoAuthProvider) : IPipelinesService
{
    private readonly IAzdoAuthProvider _azdoAuthProvider = azdoAuthProvider;

    /// <inheritdoc/>
    public async Task<BuildArtifact> GetArtifactAsync(string azdoOrg, string azdoProject, int buildId, string artifactName)
    {
        var client = GetBuildHttpClient(azdoOrg);
        return await client.GetArtifactAsync(azdoProject, buildId, artifactName);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<string>> GetBuildTagsAsync(string azdoOrg, string azdoProject, int buildId)
    {
        var client = GetBuildHttpClient(azdoOrg);
        return await client.GetBuildTagsAsync(azdoProject, buildId);
    }

    private BuildHttpClient GetBuildHttpClient(string azdoOrg)
    {
        var connection = _azdoAuthProvider.GetVssConnection(azdoOrg);
        return connection.GetClient<BuildHttpClient>();
    }
}

internal static class PipelinesServiceExtensions
{
    public static IServiceCollection AddPipelinesService(this IServiceCollection services)
    {
        services.AddAzdoAuthProvider();
        services.AddSingleton<IPipelinesService, PipelinesService>();
        return services;
    }
}
