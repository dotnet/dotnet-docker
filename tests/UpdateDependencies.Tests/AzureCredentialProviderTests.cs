// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Identity;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.Extensions.Configuration;

namespace UpdateDependencies.Tests;

public sealed class AzureCredentialProviderTests
{
    private const string ServiceConnections = """
        [
            {
                "name": "staging",
                "tenantId": "00000000-0000-0000-0000-000000000001",
                "clientId": "00000000-0000-0000-0000-000000000002",
                "id": "00000000-0000-0000-0000-000000000003"
            }
        ]
        """;

    [Fact]
    public void ConfiguredServiceConnection_UsesAzurePipelinesCredential()
    {
        var provider = CreateProvider(ServiceConnections, systemAccessToken: "token", inPipeline: true);

        provider.GetCredential("staging").ShouldBeOfType<AzurePipelinesCredential>();
    }

    [Fact]
    public void UnknownServiceConnection_Throws()
    {
        var provider = CreateProvider(ServiceConnections, systemAccessToken: "token", inPipeline: true);

        Should.Throw<KeyNotFoundException>(() => provider.GetCredential("other"))
            .Message.ShouldContain("staging");
    }

    [Fact]
    public void MissingSystemAccessToken_Throws()
    {
        var provider = CreateProvider(ServiceConnections, systemAccessToken: null, inPipeline: true);

        Should.Throw<InvalidOperationException>(() => provider.GetCredential("staging"));
    }

    [Fact]
    public void NoServiceConnectionsInPipeline_Throws()
    {
        var provider = CreateProvider(serviceConnections: null, systemAccessToken: "token", inPipeline: true);

        Should.Throw<KeyNotFoundException>(() => provider.GetCredential("staging"))
            .Message.ShouldContain("<none>");
    }

    [Fact]
    public void Locally_UsesDeveloperCredential()
    {
        var provider = CreateProvider(ServiceConnections, systemAccessToken: null, inPipeline: false);

        provider.GetCredential("staging").ShouldBeOfType<AzureCliCredential>();
    }

    private static AzureCredentialProvider CreateProvider(
        string? serviceConnections,
        string? systemAccessToken,
        bool inPipeline)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AZURE_SERVICE_CONNECTIONS"] = serviceConnections,
            })
            .Build();

        var environmentService = Mock.Of<IEnvironmentService>(env =>
            env.IsRunningInAzurePipelines() == inPipeline && env.GetSystemAccessToken() == systemAccessToken);

        return new AzureCredentialProvider(configuration, environmentService);
    }
}
