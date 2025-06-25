// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Azure.Identity;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace Dotnet.Docker;

public class AzdoAuthProvider
{
    /// <summary>
    /// This scope provides access to Azure DevOps Services REST API.
    /// </summary>
    /// <remarks>
    /// See https://learn.microsoft.com/en-us/rest/api/azure/devops/tokens/?view=azure-devops-rest-7.1&tabs=powershell#personal-access-tokens-pats
    /// </remarks
    private const string Scope = "499b84ac-1321-427f-aa17-267ca6975798/.default";

    private readonly Lazy<string> _accessToken = new(GetAccessTokenInternal);

    /// <summary>
    /// Gets an Azure DevOps REST API access token.
    /// </summary>
    public string AccessToken => _accessToken.Value;

    public VssConnection GetVssConnection(string azdoOrg)
    {
        var baseUrl = new Uri($"https://dev.azure.com/{azdoOrg}");
        var credential = new VssBasicCredential(userName: string.Empty, password: AccessToken);
        var connection = new VssConnection(baseUrl, credential);
        return connection;
    }

    public HttpClient GetAuthenticatedHttpClient()
    {
        // Create an HttpClient that uses the Azure Developer CLI credential for authentication.
        var httpClient = new HttpClient();
        var header = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _accessToken.Value))));
        httpClient.DefaultRequestHeaders.Authorization = header;

        return httpClient;
    }

    private static string GetAccessTokenInternal()
    {
        var accessToken = Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            return accessToken;
        }

        var credential = new AzureDeveloperCliCredential();
        var requestContext = new Azure.Core.TokenRequestContext([Scope]);
        accessToken = credential.GetToken(requestContext).Token;
        return accessToken;
    }
}
