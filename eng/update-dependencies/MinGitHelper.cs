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

public static class MinGitHelper
{
    public static async Task<JObject> GetLatestMinGitReleaseAsync()
    {
        HttpClient httpClient = new();
        string url = "https://api.github.com/repos/git-for-windows/git/releases/latest";
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Dotnet.Docker.UpdateDependencies", "1.0"));
        string releaseString = await httpClient.GetStringAsync(url);
        JObject release = (JObject)(JsonConvert.DeserializeObject(releaseString) ??
            throw new InvalidOperationException($"Unable to deserialize response from '{url}' as JSON."));
        return release;
    }
}
#nullable disable
