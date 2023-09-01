// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

public static class GitHubHelper
{
    private const string BaseGitApiUrl = "https://api.github.com";

    private static HttpClient s_httpClient = new();

    static GitHubHelper()
    {
        s_httpClient.BaseAddress = new Uri(BaseGitApiUrl);
        s_httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Dotnet.Docker.UpdateDependencies", "1.0"));
    }

    public static async Task<string> GetLatestReleaseTagAsync(string owner, string repo)
    {
        JObject releaseResponse = await GetLatestReleaseAsync(owner, repo);
        return releaseResponse.GetRequiredToken<JValue>("tag_name").ToString();
    }

    public static async Task<JObject> GetLatestReleaseAsync(string owner, string repo)
    {
        string request = $"/repos/{owner}/{repo}/releases/latest";
        JObject response = await Get(request);
        return response;
    }

    private static async Task<JObject> Get(string request) {
        string responseString = await s_httpClient.GetStringAsync(request);
        JObject response = (JObject)(JsonConvert.DeserializeObject(responseString) ??
            throw new InvalidOperationException($"Unable to deserialize response from '{s_httpClient.BaseAddress}{request}' as JSON."));
        return response;
    }
}
#nullable disable
