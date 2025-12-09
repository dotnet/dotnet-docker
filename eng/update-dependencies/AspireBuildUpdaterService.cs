// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class AspireBuildUpdaterService(
    ILogger<AspireBuildUpdaterService> logger,
    HttpClient httpClient,
    Func<string, IManifestVariables> manifestVariablesFactory
) : IBuildUpdaterService
{
    private readonly ILogger<AspireBuildUpdaterService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;
    private readonly Func<string, IManifestVariables> _createManifestVariables = manifestVariablesFactory;

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

        // Read manifest.versions.json early so that we can fail fast in case we have the wrong file path.
        var manifestVariables = _createManifestVariables(pullRequestOptions.GetManifestVersionsFilePath());
        var branch = manifestVariables.GetValue("branch");
        var dashboardBaseUrl = manifestVariables.GetValue($"aspire-dashboard|base-url|{branch}");

        var dashboardAssets = build.Assets.Where(asset =>
            asset.Name.Contains("aspire-dashboard-linux-x64")
            || asset.Name.Contains("aspire-dashboard-linux-arm64"));

        if (!dashboardAssets.Any())
        {
            throw new InvalidOperationException($"Could not find aspire-dashboard-linux-* assets in build {build.Id}");
        }

        _logger.LogInformation("Found Aspire build version: {version}", dashboardAssets.First().Version);

        IEnumerable<string?> dashboardChecksums = await dashboardAssets
            .Select(asset => GetDashboardUrl(dashboardBaseUrl, asset))
            .ToAsyncEnumerable()
            .SelectAwait(async url => await GetChecksumAsync(url))
            .ToListAsync();

        var dashboardChecksumInfos = dashboardAssets.Zip(dashboardChecksums);

        var manifestVersionsPath = pullRequestOptions.GetManifestVersionsFilePath();

        var version = dashboardAssets.First().Version;
        var majorMinorVersion = VersionHelper.ResolveMajorMinorVersion(version);

        // Known issue: Aspire Dashboard builds are always "preview 1".
        // Passing in isStableRelease here keeps us from setting the product
        // version to the full build version with the "-preview.1" suffix.
        var productVersion = VersionHelper.ResolveProductVersion(version, isStableRelease: true);

        List<VariableUpdateInfo> variableUpdates =
        [
            new VariableUpdateInfo("aspire-dashboard|build-version", version),
            new VariableUpdateInfo("aspire-dashboard|product-version", productVersion),
            new VariableUpdateInfo("aspire-dashboard|fixed-tag", productVersion),
            new VariableUpdateInfo("aspire-dashboard|minor-tag", majorMinorVersion.ToString(2)),
            new VariableUpdateInfo("aspire-dashboard|major-tag", majorMinorVersion.Major.ToString()),
        ];

        variableUpdates.AddRange(dashboardChecksumInfos
            // Filter out null checksums (indicates the checksum was not found above)
            .Where(info => info.Second is not null)
            .Select(info =>
            {
                var (asset, checksum) = info;
                var arch = asset.Name.Contains("arm64") ? "arm64" : "x64";
                // Null-forgiving operator is OK since we filtered out null checksums above.
                return new VariableUpdateInfo($"aspire-dashboard|linux|{arch}|sha", checksum!);
            }));

        var dependencyUpdaters = variableUpdates
            .Select(updateInfo => new VariableUpdater(manifestVersionsPath, updateInfo));

        var updateDependencies = new SpecificCommand();
        updateDependencies.CustomUpdateInfos.AddRange(variableUpdates);
        updateDependencies.CustomUpdaters.AddRange(dependencyUpdaters);

        // Don't pass in any  through options, since we calculated all of the variables
        // and their new versions to update above. Everything is handled through CustomUpdateInfos.
        // Using the SpecificCommand here just allows us to easily create automated pull requests.
        var updateDependenciesOptions = SpecificCommandOptions.FromPullRequestOptions(pullRequestOptions);
        return await updateDependencies.ExecuteAsync(updateDependenciesOptions);
    }

    /// <summary>
    /// Gets the full download URL to an Aspire Dashboard asset given its base
    /// URL and the BAR asset information.
    /// </summary>
    private string GetDashboardUrl(string baseUrl, Asset dashboardAsset) => $"{baseUrl}/{dashboardAsset.Name}";

    /// <summary>
    /// Manually compute the checksum of an Aspire Dashboard asset by downloading it from
    /// <paramref name="url"/> and hashing the contents.
    /// </summary>
    /// <remarks>
    /// Remove once https://github.com/dotnet/dotnet-docker/issues/6568 is completed.
    /// </remarks>
    /// <returns>
    /// Null if the checksum could not be computed for any reason. An error will
    /// be logged to the console if this happens.
    /// </returns>
    private Task<string?> GetChecksumAsync(string url) => ChecksumHelper.ComputeChecksumShaAsync(_httpClient, url);
}
