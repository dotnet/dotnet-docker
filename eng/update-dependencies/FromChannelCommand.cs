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

internal class FromChannelCommand(
    IBuildAssetService buildAssetService,
    IBasicBarClient barClient,
    ILogger<FromChannelCommand> logger)
    : BaseCommand<FromChannelOptions>
{
    private readonly IBuildAssetService _buildAssetService = buildAssetService;
    private readonly IBasicBarClient _barClient = barClient;
    private readonly ILogger<FromChannelCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromChannelOptions options)
    {
        _logger.LogInformation("Getting latest build for {options.Repo} from channel {options.Channel}",
            options.Repo, options.Channel);

        Build latestBuild = await _barClient.GetLatestBuildAsync(options.Repo, options.Channel);
        string? channelName = latestBuild.Channels.FirstOrDefault(c => c.Id == options.Channel)?.Name;

        _logger.LogInformation("Channel {options.Channel} is '{channel.Name}'",
            options.Channel, latestBuild.Channels.FirstOrDefault(c => c.Id == options.Channel)?.Name);
        _logger.LogInformation("Got latest build {latestBuild.Id} with commit {options.Repo}@{latestBuild.Commit}",
            latestBuild.Id, latestBuild.AzureDevOpsRepository ?? latestBuild.GitHubRepository, latestBuild.Commit);

        if (!IsVmrBuild(latestBuild))
        {
            throw new InvalidOperationException(
                "Expected a build of the VMR, but got a build of " +
                $"{latestBuild.AzureDevOpsRepository ?? latestBuild.GitHubRepository} instead.");
        }

        IEnumerable<Asset> assets = await _barClient.GetAssetsAsync(buildId: latestBuild.Id);

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
            User = options.User,
            Email = options.Email,
            Password = options.Password,
            AzdoOrganization = options.AzdoOrganization,
            AzdoProject = options.AzdoProject,
            AzdoRepo = options.AzdoRepo,
            VersionSourceName = options.VersionSourceName,
            SourceBranch = options.SourceBranch,
            TargetBranch = options.TargetBranch,
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
