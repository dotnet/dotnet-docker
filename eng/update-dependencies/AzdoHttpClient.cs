// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Docker;

internal class AzdoHttpClient
{
    private readonly IAzdoAuthProvider _azdoAuthProvider;
    private readonly HttpClient _httpClient;

    public AzdoHttpClient(IAzdoAuthProvider azdoAuthProvider, HttpClient httpClient)
    {
        _azdoAuthProvider = azdoAuthProvider;
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
                Encoding.ASCII.GetBytes($":{_azdoAuthProvider.AccessToken}")
            )
        );
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken ct = default) =>
        await _httpClient.GetAsync(requestUri, ct);
}

internal static class AzdoHttpClientExtensions
{
    public static IServiceCollection AddAzdoHttpClient(this IServiceCollection services)
    {
        // Add dependencies
        services.AddHttpClient();
        services.AddAzdoAuthProvider();

        // Add self
        services.AddHttpClient<AzdoHttpClient>();
        return services;
    }
}
