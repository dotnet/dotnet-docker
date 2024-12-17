// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Octokit;

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

    private static ProductHeaderValue GetProductHeaderValue()
    {
        const string AppName = "dotnet-docker-update-dependencies";
        return new ProductHeaderValue(AppName);
    }
}
