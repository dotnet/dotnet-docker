// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Provides Azure credentials for Azure DevOps service connections, so that each Azure resource
/// is accessed with its own identity.
/// </summary>
public interface IAzureCredentialProvider
{
    /// <summary>
    /// Gets a credential for the named service connection.
    /// </summary>
    TokenCredential GetCredential(string serviceConnectionName);
}

/// <summary>
/// The information needed to authenticate as an Azure DevOps service connection.
/// </summary>
/// <remarks>
/// Pipelines pass these to the tool as a JSON array in the AZURE_SERVICE_CONNECTIONS environment variable.
/// Keep the pipeline templates in sync with this model.
/// </remarks>
internal sealed record AzureServiceConnection(string Name, string TenantId, string ClientId, string Id);

/// <summary>
/// Names that pipelines use for service connections in AZURE_SERVICE_CONNECTIONS.
/// </summary>
internal static class ServiceConnectionNames
{
    /// <summary>
    /// The staging service connection, which can read internal .NET and PowerShell builds from Azure Storage.
    /// </summary>
    public const string Staging = "staging";
}

/// <summary>
/// Uses <see cref="AzurePipelinesCredential"/> for the service connections configured by the pipeline.
/// Outside of Azure Pipelines, falls back to the developer's Azure CLI login.
/// </summary>
/// <remarks>
/// A pipeline job must reference each service connection in one of its tasks (for example, a no-op
/// AzureCLI@2 task) for the service connection's OIDC token exchange to be allowed.
/// </remarks>
internal sealed class AzureCredentialProvider(IConfiguration configuration, IEnvironmentService environmentService)
    : IAzureCredentialProvider
{
    private const string ServiceConnectionsVariable = "AZURE_SERVICE_CONNECTIONS";

    private readonly Dictionary<string, AzureServiceConnection> _serviceConnections =
        ParseServiceConnections(configuration[ServiceConnectionsVariable]);

    public TokenCredential GetCredential(string serviceConnectionName)
    {
        if (!environmentService.IsRunningInAzurePipelines())
        {
            return new AzureCliCredential();
        }

        if (!_serviceConnections.TryGetValue(serviceConnectionName, out AzureServiceConnection? serviceConnection))
        {
            string configuredNames = _serviceConnections.Count == 0
                ? "<none>"
                : string.Join(", ", _serviceConnections.Keys);

            throw new KeyNotFoundException(
                $"Service connection '{serviceConnectionName}' was not found in {ServiceConnectionsVariable}."
                + $" Configured service connections: {configuredNames}.");
        }

        string systemAccessToken = environmentService.GetSystemAccessToken()
            ?? throw new InvalidOperationException(
                $"SYSTEM_ACCESSTOKEN is required to use service connection '{serviceConnectionName}'.");

        return new AzurePipelinesCredential(
            tenantId: serviceConnection.TenantId,
            clientId: serviceConnection.ClientId,
            serviceConnectionId: serviceConnection.Id,
            systemAccessToken: systemAccessToken);
    }

    private static Dictionary<string, AzureServiceConnection> ParseServiceConnections(string? json)
    {
        AzureServiceConnection[] serviceConnections = string.IsNullOrWhiteSpace(json)
            ? []
            : JsonSerializer.Deserialize<AzureServiceConnection[]>(json, JsonSerializerOptions.Web) ?? [];

        return serviceConnections.ToDictionary(sc => sc.Name, StringComparer.OrdinalIgnoreCase);
    }
}

internal static class AzureCredentialProviderExtensions
{
    public static IServiceCollection AddAzureCredentialProvider(this IServiceCollection services) =>
        services.AddSingleton<IAzureCredentialProvider, AzureCredentialProvider>();
}
