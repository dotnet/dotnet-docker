// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class AspireBuildUpdaterService(
    ILogger<AspireBuildUpdaterService> logger,
    IBuildAssetService buildAssetService,
    IBasicBarClient barClient
) : IBuildUpdaterService
{
    private readonly ILogger<AspireBuildUpdaterService> _logger = logger;
    private readonly IBuildAssetService _buildAssetService = buildAssetService;
    private readonly IBasicBarClient _barClient = barClient;

    /// <summary>
    /// Given a BAR (https://aka.ms/bar) build of Aspire, updates the Aspire Dashboard build version.
    /// </summary>
    public async Task<int> UpdateFrom(Build build, CreatePullRequestOptions pullRequestOptions)
    {
        if (build.GetBuildRepo() != BuildRepo.Aspire)
        {
            throw new InvalidOperationException(
                $"AspireBuildUpdaterService cannot process builds from repo: {build.GitHubRepository}");
        }

        var buildAssets = await _barClient.GetAssetsAsync(buildId: build.Id);
        var dashboardAsset = build.Assets.FirstOrDefault(asset => asset.Name.Contains("aspire-dashboard-linux-x64"));
        if (dashboardAsset is null)
        {
            throw new InvalidOperationException($"Could not find aspire-dashboard-linux-x64 asset in build {build.Id}");
        }

        _logger.LogInformation("Aspire build version: {version}", dashboardAsset.Version);
        throw new NotImplementedException();
    }
}
