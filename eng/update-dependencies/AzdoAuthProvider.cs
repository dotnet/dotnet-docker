// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Identity;

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
    /// Gets an Azure DevOps access token. Defaults to SYSTEM_ACCESSTOKEN
    /// environment variable, then falls back to using the Azure Developer CLI
    /// credential to get a PAT.
    /// </summary>
    /// <returns></returns>
    public string AccessToken => _accessToken.Value;

    private static string GetAccessTokenInternal()
    {
        string? accessToken = Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            return accessToken;
        }

        var credential = new AzureDeveloperCliCredential();
        accessToken = credential.GetToken(new Azure.Core.TokenRequestContext(scopes: [Scope])).Token;
        return accessToken;
    }
}
