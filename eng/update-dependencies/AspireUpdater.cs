// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public sealed class AspireUpdater(
    IBasicBarClient barClient,
    HttpClient httpClient,
    ILogger<AspireUpdater> logger)
        : IBarBuildUpdater, IBarChannelUpdater
{
    public const string PublicRepository = "https://github.com/microsoft/aspire";
    public const string InternalRepository = "https://dev.azure.com/dnceng/internal/_git/microsoft-aspire";

    public static bool IsAspireBuild(Build build)
    {
        string repository = build.GitHubRepository ?? build.AzureDevOpsRepository;
        return repository is PublicRepository or InternalRepository;
    }

    public async Task UpdateFromBarChannelAsync(
        ManifestVariables variables,
        string repoRoot,
        string repository,
        int channelId,
        CancellationToken cancellationToken)
    {
        Build build = await barClient.GetLatestBuildAsync(repository, channelId).WaitAsync(cancellationToken);
        await UpdateFromBarBuildAsync(variables, repoRoot, build, cancellationToken);
    }

    public async Task UpdateFromBarBuildAsync(
        ManifestVariables variables,
        string repoRoot,
        Build build,
        CancellationToken cancellationToken)
    {
        if (!IsAspireBuild(build))
        {
            throw new ArgumentException($"BAR build {build.Id} is not an Aspire build.", nameof(build));
        }

        string branch = variables.GetValue("branch");
        string dashboardBaseUrl = variables.GetValue($"aspire-dashboard|base-url|{branch}");
        var dashboardAssets = build.Assets.Where(asset =>
            asset.Name.Contains("aspire-dashboard-linux-x64")
            || asset.Name.Contains("aspire-dashboard-linux-arm64"))
            .ToArray();

        if (dashboardAssets.Length == 0)
        {
            throw new InvalidOperationException($"Could not find aspire-dashboard-linux-* assets in build {build.Id}");
        }

        string version = dashboardAssets.First().Version;
        logger.LogInformation("Found Aspire build version: {Version}", version);

        var dashboardChecksums = await Task.WhenAll(dashboardAssets
            .Select(asset => GetChecksumAsync($"{dashboardBaseUrl}/{asset.Name}", cancellationToken)));

        var majorMinorVersion = VersionHelper.ResolveMajorMinorVersion(version);

        // Aspire Dashboard builds always use "preview 1"; tags use the stable product version.
        string productVersion = VersionHelper.ResolveProductVersion(version, isStableRelease: true);

        variables.SetValue("aspire-dashboard|build-version", version);
        variables.SetValue("aspire-dashboard|product-version", productVersion);
        variables.SetValue("aspire-dashboard|fixed-tag", productVersion);
        variables.SetValue("aspire-dashboard|minor-tag", majorMinorVersion.ToString(2));
        variables.SetValue("aspire-dashboard|major-tag", majorMinorVersion.Major.ToString());

        foreach (var (asset, checksum) in dashboardAssets.Zip(dashboardChecksums))
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
