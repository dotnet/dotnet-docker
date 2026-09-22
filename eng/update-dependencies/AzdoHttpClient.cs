// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

internal class AzdoHttpClient(IAzdoAuthProvider azdoAuthProvider, HttpClient httpClient)
{
    public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
                Encoding.ASCII.GetBytes($":{azdoAuthProvider.AccessToken}")
            )
        );

        return await httpClient.SendAsync(request, ct);
    }
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
