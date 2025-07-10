// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dotnet.Docker;

internal class AzdoHttpClient
{
    private readonly AzdoAuthProvider _azdoAuthProvider;
    private readonly HttpClient _httpClient;

    public AzdoHttpClient(AzdoAuthProvider azdoAuthProvider, HttpClient httpClient)
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
