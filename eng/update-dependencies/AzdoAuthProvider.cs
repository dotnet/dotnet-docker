// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common.AzureDevOpsTokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public interface IAzdoAuthProvider
{
    /// <summary>
    /// Gets an Azure DevOps REST API access token.
    /// </summary>
    string AccessToken { get; }

    /// <summary>
    /// Gets a connection to Azure DevOps Services.
    /// </summary>
    VssConnection GetVssConnection(string azdoOrg);
}

public class AzdoAuthProvider(IAzureDevOpsTokenProvider tokenProvider) : IAzdoAuthProvider
{
    /// <summary>
    /// Gets an Azure DevOps REST API access token.
    /// </summary>
    public string AccessToken => tokenProvider.GetTokenForAccount("default");

    /// <summary>
    /// Gets a connection to Azure DevOps Services.
    /// </summary>
    /// <param name="azdoOrg">
    /// The URI of the Azure DevOps organization or collection. For example,
    /// https://dev.azure.com/fabrikamfiber/. You can get this from the Azure
    /// Pipeline variable $(System.CollectionUri).
    /// </param>
    /// <returns>
    /// A <see cref="VssConnection"/> that can be used to connect to Azure
    /// DevOps Services.
    /// </returns>
    public VssConnection GetVssConnection(string azdoOrg)
    {
        var baseUrl = new Uri(azdoOrg);
        var credential = new VssBasicCredential(userName: string.Empty, password: AccessToken);
        var connection = new VssConnection(baseUrl, credential);
        return connection;
    }
}

internal static class AzdoAuthProviderExtensions
{
    public static IServiceCollection AddAzdoAuthProvider(this IServiceCollection services) =>
        services.AddSingleton<IAzdoAuthProvider, AzdoAuthProvider>();
}
