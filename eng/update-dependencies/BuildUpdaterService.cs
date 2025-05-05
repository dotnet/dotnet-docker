// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal interface IBuildUpdaterService
{
    Task<int> UpdateFrom(Build build, CreatePullRequestOptions pullRequestOptions);
}

internal class BuildUpdaterService(
    IBuildAssetService buildAssetService,
    IBasicBarClient barClient,
    ILogger<BuildUpdaterService> logger) : IBuildUpdaterService
{
    private readonly IBuildAssetService _buildAssetService = buildAssetService;
    private readonly IBasicBarClient _barClient = barClient;
    private readonly ILogger<BuildUpdaterService> _logger = logger;

    public async Task<int> UpdateFrom(Build build, CreatePullRequestOptions pullRequestOptions)
    {
        _logger.LogInformation("Updating to build {build.Id} with commit {options.Repo}@{build.Commit}",
            build.Id, build.AzureDevOpsRepository ?? build.GitHubRepository, build.Commit);

        if (!IsVmrBuild(build))
        {
            throw new InvalidOperationException(
                "Expected a build of the VMR, but got a build of " +
                $"{build.AzureDevOpsRepository ?? build.GitHubRepository} instead.");
        }

        IEnumerable<Asset> assets = await _barClient.GetAssetsAsync(buildId: build.Id);

        Asset productCommitsAsset = assets.FirstOrDefault(a => ProductCommits.SdkAssetRegex.IsMatch(a.Name))
            ?? throw new InvalidOperationException($"Could not find product version commit in assets.");

        string productCommitsJson = await _buildAssetService.GetAssetTextContentsAsync(productCommitsAsset);
        ProductCommits productCommits = ProductCommits.FromJson(productCommitsJson);

        Version dockerfileVersion = ResolveMajorMinorVersion(productCommits.Sdk.Version);

        // Run old update-dependencies command using the resolved versions
        var updateDependencies = new SpecificCommand();
        var updateDependenciesOptions = new SpecificCommandOptions()
        {
            DockerfileVersion = dockerfileVersion.ToString(),
            ProductVersions = new Dictionary<string, string?>()
            {
                // In the VMR, runtime and aspnetcore versions are coupled
                { "runtime", productCommits.Runtime.Version },
                { "aspnet", productCommits.AspNetCore.Version },
                { "aspnet-composite", productCommits.AspNetCore.Version },
                { "sdk", productCommits.Sdk.Version },
            },

            // Pass through all properties of CreatePullRequestOptions
            User = pullRequestOptions.User,
            Email = pullRequestOptions.Email,
            Password = pullRequestOptions.Password,
            AzdoOrganization = pullRequestOptions.AzdoOrganization,
            AzdoProject = pullRequestOptions.AzdoProject,
            AzdoRepo = pullRequestOptions.AzdoRepo,
            VersionSourceName = pullRequestOptions.VersionSourceName,
            SourceBranch = pullRequestOptions.SourceBranch,
            TargetBranch = pullRequestOptions.TargetBranch,
        };

        return await updateDependencies.ExecuteAsync(updateDependenciesOptions);
    }

    private static Version ResolveMajorMinorVersion(string versionString)
    {
        string[] versionParts = versionString.Split('.');
        if (versionParts.Length < 2)
        {
            throw new InvalidOperationException($"Could not parse major-minor version from '{versionString}'.");
        }

        return new Version(major: int.Parse(versionParts[0]), minor: int.Parse(versionParts[1]));
    }

    private static bool IsVmrBuild(Build build)
    {
        string repo = build.GitHubRepository ?? build.AzureDevOpsRepository;
        return repo == "https://github.com/dotnet/dotnet"
            || repo == "https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet";
    }
}
