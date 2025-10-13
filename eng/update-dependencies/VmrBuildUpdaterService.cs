// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class VmrBuildUpdaterService(
    IBuildAssetService buildAssetService,
    IBasicBarClient barClient,
    ILogger<VmrBuildUpdaterService> logger
) : IBuildUpdaterService
{
    private readonly IBuildAssetService _buildAssetService = buildAssetService;
    private readonly IBasicBarClient _barClient = barClient;
    private readonly ILogger<VmrBuildUpdaterService> _logger = logger;

    /// <summary>
    /// Updates product versions according to a specific BAR build of the VMR. This will update the
    /// manifest.versions.json file, generate Dockerfiles and Readmes from the templates, and if
    /// credentials are provided, submit a pull request.
    /// </summary>
    /// <param name="build">
    /// A build of the VMR repo (dotnet/dotnet)
    /// </param>
    /// <param name="pullRequestOptions">
    /// Options for creating a pull request. If credentials are provided, a pull request will be created.
    /// </param>
    /// <returns>Exit code (0 for success)</returns>
    public async Task<int> UpdateFrom(Build build, CreatePullRequestOptions pullRequestOptions)
    {
        _logger.LogInformation("Updating to build {build.Id} with commit {options.Repo}@{build.Commit}",
            build.Id, build.AzureDevOpsRepository ?? build.GitHubRepository, build.Commit);

        if (build.GetBuildRepo() != BuildRepo.Vmr)
        {
            throw new InvalidOperationException(
                $"Build {build.Id} is from unsupported repository {build.AzureDevOpsRepository ?? build.GitHubRepository}."
            );
        }

        IEnumerable<Asset> assets = await _barClient.GetAssetsAsync(buildId: build.Id);

        Asset productCommitsAsset = assets.FirstOrDefault(a => ProductCommits.SdkAssetRegex.IsMatch(a.Name))
            ?? throw new InvalidOperationException($"Could not find product version commit in assets.");

        string productCommitsJson = await _buildAssetService.GetAssetTextContentsAsync(productCommitsAsset);
        ProductCommits productCommits = ProductCommits.FromJson(productCommitsJson);

        Version dockerfileVersion = VersionHelper.ResolveMajorMinorVersion(productCommits.Sdk.Version);

        // Run old update-dependencies command using the resolved versions
        var updateDependencies = new SpecificCommand();
        var updateDependenciesOptions = new SpecificCommandOptions()
        {
            DockerfileVersion = dockerfileVersion.ToString(),
            ProductVersions = new Dictionary<string, string?>()
            {
                // "dotnet" version is also required. It sets the "dotnet|*|product-version"
                // variable which is used for runtime-deps, runtime, and aspnet tags.
                { "dotnet", productCommits.Runtime.Version },
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
}
