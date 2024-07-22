// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

public static class GitHubHelper
{
    public static Task<Release> GetLatestRelease(string owner, string repo)
    {
        IReleasesClient releasesClient = GetReleasesClient();
        return releasesClient.GetLatest(owner, repo);
    }

    private static IReleasesClient GetReleasesClient() 
    {
        GitHubClient client = new(productInformation: GetProductHeaderValue());
        return client.Repository.Release;
    }

    private static Octokit.ProductHeaderValue GetProductHeaderValue()
    {
        const string AppName = "dotnet-docker-update-dependencies";
        return new Octokit.ProductHeaderValue(AppName);
    }
}
