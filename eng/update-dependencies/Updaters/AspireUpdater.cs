// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;
using Octokit;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class AspireUpdater(
    IBasicBarClient barClient,
    IReleasesClient releases,
    HttpClient httpClient,
    ILogger<AspireUpdater> logger)
        : IBarBuildUpdater, IBarChannelUpdater
{
    public const string PublicRepository = "https://github.com/microsoft/aspire";
    public const string InternalRepository = "https://dev.azure.com/dnceng/internal/_git/microsoft-aspire";

    public static string Name => "aspire";
    public static string VersionSourceName => "microsoft/aspire";

    public static bool IsAspireBuild(Build build)
    {
        string repository = build.GitHubRepository ?? build.AzureDevOpsRepository;
        return repository is PublicRepository or InternalRepository;
    }

    public async Task<DependencyUpdate> ResolveFromBarChannelAsync(int channelId, CancellationToken cancellationToken)
    {
        // Aspire channels receive builds from both the public and internal repos (e.g. internal servicing
        // builds), so the latest build may come from either one.
        Build?[] builds = await Task.WhenAll(
            barClient.GetLatestBuildAsync(PublicRepository, channelId),
            barClient.GetLatestBuildAsync(InternalRepository, channelId)).WaitAsync(cancellationToken);

        Build? publicBuild = builds[0];
        Build? internalBuild = builds[1];

        // Internal builds may be unreleased (e.g. embargoed security fixes). Only use one after its
        // version has been publicly released, to avoid disclosing it early in a public PR.
        if (internalBuild is not null
            && (publicBuild is null || internalBuild.DateProduced > publicBuild.DateProduced)
            && !await IsReleasedOnGitHubAsync(internalBuild, cancellationToken))
        {
            internalBuild = null;
        }

        Build[] candidates = new[] { publicBuild, internalBuild }.OfType<Build>().ToArray();
        if (candidates.Length == 0)
        {
            throw new InvalidOperationException($"Could not find a usable Aspire build in BAR channel {channelId}.");
        }

        Build build = candidates.MaxBy(b => b.DateProduced)!;

        logger.LogInformation(
            "Using latest Aspire build {BuildId} from {Repository}",
            build.Id, build.GitHubRepository ?? build.AzureDevOpsRepository);

        return await ResolveFromBarBuildAsync(build, cancellationToken);
    }

    public Task<DependencyUpdate> ResolveFromBarBuildAsync(Build build, CancellationToken cancellationToken)
    {
        if (!IsAspireBuild(build))
        {
            throw new ArgumentException($"BAR build {build.Id} is not an Aspire build.", nameof(build));
        }

        Asset[] dashboardAssets = GetDashboardAssets(build);
        string version = dashboardAssets.First().Version;
        logger.LogInformation("Found Aspire build version: {Version}", version);

        return Task.FromResult(new DependencyUpdate(
            Description: $"Update Aspire Dashboard to {version}",
            ApplyAsync: (variables, _, token) => ApplyAsync(variables, dashboardAssets, version, token)));
    }

    private static Asset[] GetDashboardAssets(Build build)
    {
        Asset[] dashboardAssets = build.Assets.Where(asset =>
            asset.Name.Contains("aspire-dashboard-linux-x64")
            || asset.Name.Contains("aspire-dashboard-linux-arm64"))
            .ToArray();

        if (dashboardAssets.Length == 0)
        {
            throw new InvalidOperationException($"Could not find aspire-dashboard-linux-* assets in build {build.Id}");
        }

        return dashboardAssets;
    }

    private async Task<bool> IsReleasedOnGitHubAsync(Build build, CancellationToken cancellationToken)
    {
        string productVersion = VersionHelper.ResolveProductVersion(
            GetDashboardAssets(build).First().Version, isStableRelease: true);
        string tag = $"v{productVersion}";

        try
        {
            await releases.Get("microsoft", "aspire", tag).WaitAsync(cancellationToken);
            return true;
        }
        catch (NotFoundException)
        {
            logger.LogInformation(
                "Skipping internal Aspire build {BuildId}: no public GitHub release {Tag} exists yet",
                build.Id, tag);
            return false;
        }
    }

    private async Task ApplyAsync(
        ManifestVariables variables,
        IReadOnlyList<Asset> dashboardAssets,
        string version,
        CancellationToken cancellationToken)
    {
        string branch = variables.GetValue("branch");
        string dashboardBaseUrl = variables.GetValue($"aspire-dashboard|base-url|{branch}");

        string[] dashboardChecksums = await Task.WhenAll(dashboardAssets
            .Select(asset => GetChecksumAsync($"{dashboardBaseUrl}/{asset.Name}", cancellationToken)));

        Version majorMinorVersion = VersionHelper.ResolveMajorMinorVersion(version);

        // Aspire Dashboard builds always use "preview 1"; tags use the stable product version.
        string productVersion = VersionHelper.ResolveProductVersion(version, isStableRelease: true);

        variables.SetValue("aspire-dashboard|build-version", version);
        variables.SetValue("aspire-dashboard|product-version", productVersion);
        variables.SetValue("aspire-dashboard|fixed-tag", productVersion);
        variables.SetValue("aspire-dashboard|minor-tag", majorMinorVersion.ToString(2));
        variables.SetValue("aspire-dashboard|major-tag", majorMinorVersion.Major.ToString(CultureInfo.InvariantCulture));

        foreach ((Asset? asset, string? checksum) in dashboardAssets.Zip(dashboardChecksums))
        {
            string arch = asset.Name.Contains("arm64") ? "arm64" : "x64";
            variables.SetValue($"aspire-dashboard|linux|{arch}|sha", checksum);
        }
    }

    private async Task<string> GetChecksumAsync(string url, CancellationToken cancellationToken)
    {
        logger.LogInformation("Downloading Aspire Dashboard archive from {Url}", url);
        return await ChecksumHelper.ComputeChecksumShaAsync(httpClient, url, cancellationToken)
            ?? throw new InvalidOperationException($"Unable to retrieve checksum for '{url}'.");
    }
}
