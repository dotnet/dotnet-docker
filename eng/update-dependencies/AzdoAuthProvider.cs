// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace Dotnet.Docker;

public class AzdoAuthProvider
{
    /// <summary>
    /// This scope provides access to Azure DevOps Services REST API.
    /// </summary>
    /// <remarks>
    /// See https://learn.microsoft.com/rest/api/azure/devops/tokens/?view=azure-devops-rest-7.1&tabs=powershell#personal-access-tokens-pats
    /// </remarks
    private const string Scope = "499b84ac-1321-427f-aa17-267ca6975798/.default";

    private readonly ILogger<AzdoAuthProvider> _logger;
    private readonly Lazy<string> _accessToken;

    public AzdoAuthProvider(ILogger<AzdoAuthProvider> logger)
    {
        _logger = logger;
        _accessToken = new(GetAccessTokenInternal);
    }

    /// <summary>
    /// Gets an Azure DevOps REST API access token.
    /// </summary>
    public string AccessToken => _accessToken.Value;

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

    private string GetAccessTokenInternal()
    {
        var accessToken = Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            return accessToken;
        }

        _logger.LogInformation("Environment variable SYSTEM_ACCESSTOKEN was not set."
            + " Did you forget to explicitly pass it in to your pipeline step?"
            + " See https://learn.microsoft.com/azure/devops/pipelines/build/variables#systemaccesstoken");

        var credential = new AzureDeveloperCliCredential();
        var requestContext = new Azure.Core.TokenRequestContext([Scope]);
        accessToken = credential.GetToken(requestContext).Token;
        return accessToken;
    }
}
