// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal sealed class AspireCommand(
    IBasicBarClient barClient,
    HttpClient httpClient,
    ILogger<AspireCommand> logger)
        : BaseCommand<AspireOptions>
{
    private const string Repository = "https://github.com/microsoft/aspire";

    public override async Task<int> ExecuteAsync(AspireOptions options)
    {
        Build build;
        switch (options)
        {
            case { FromBuildId: int buildId, FromChannel: null } when buildId > 0:
                logger.LogInformation("Getting Aspire BAR build with ID {BuildId}", buildId);
                build = await barClient.GetBuildAsync(buildId);
                break;
            case { FromBuildId: null, FromChannel: int channel } when channel > 0:
                logger.LogInformation("Getting latest Aspire build from channel {Channel}", channel);
                build = await barClient.GetLatestBuildAsync(Repository, channel);
                break;
            default:
                logger.LogError("Specify either --from-build-id or --from-channel with a positive ID, but not both.");
                return 1;
        }

        string repository = build.GitHubRepository ?? build.AzureDevOpsRepository;
        if (repository is not (Repository or "https://dev.azure.com/dnceng/internal/_git/microsoft-aspire"))
        {
            logger.LogError("BAR build {BuildId} is not an Aspire build: {Repository}", build.Id, repository);
            return 1;
        }

        options = options with
        {
            VersionSourceName = string.IsNullOrEmpty(options.VersionSourceName)
                ? "microsoft/aspire"
                : options.VersionSourceName,
        };

        if (options.UpdateOnly)
        {
            await ApplyUpdatesAsync(Path.GetFullPath(options.RepoRoot), build);
            logger.LogInformation("Local updates completed without publishing.");
        }
        else
        {
            var publisher = new DependencyUpdatePublisher(options);
            await publisher.PublishAsync((gitContext, cancellationToken) =>
                ApplyUpdatesAsync(gitContext.WorkspaceDirectory, build, cancellationToken));
        }

        return 0;
    }

    /// <summary>
    /// Applies Aspire's manifest changes in the supplied workspace without generating files or publishing.
    /// </summary>
    internal async Task ApplyAsync(
        string repoRoot,
        Build build,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        var manifestVariables = ManifestVariables.FromFile(manifestPath);
        string originalContent = manifestVariables.Content;
        var branch = manifestVariables.GetValue("branch");
        var dashboardBaseUrl = manifestVariables.GetValue($"aspire-dashboard|base-url|{branch}");

        var dashboardAssets = build.Assets.Where(asset =>
            asset.Name.Contains("aspire-dashboard-linux-x64")
            || asset.Name.Contains("aspire-dashboard-linux-arm64"))
            .ToArray();

        if (dashboardAssets.Length == 0)
        {
            throw new InvalidOperationException($"Could not find aspire-dashboard-linux-* assets in build {build.Id}");
        }

        logger.LogInformation("Found Aspire build version: {Version}", dashboardAssets.First().Version);

        var dashboardChecksums = await Task.WhenAll(dashboardAssets
            .Select(asset => GetChecksumAsync($"{dashboardBaseUrl}/{asset.Name}", cancellationToken)));

        var dashboardChecksumInfos = dashboardAssets.Zip(dashboardChecksums);

        var version = dashboardAssets.First().Version;
        var majorMinorVersion = VersionHelper.ResolveMajorMinorVersion(version);

        // Known issue: Aspire Dashboard builds are always "preview 1".
        // Passing in isStableRelease here keeps us from setting the product
        // version to the full build version with the "-preview.1" suffix.
        var productVersion = VersionHelper.ResolveProductVersion(version, isStableRelease: true);

        VariableUpdater.Update(manifestVariables, "aspire-dashboard|build-version", version);
        VariableUpdater.Update(manifestVariables, "aspire-dashboard|product-version", productVersion);
        VariableUpdater.Update(manifestVariables, "aspire-dashboard|fixed-tag", productVersion);
        VariableUpdater.Update(manifestVariables, "aspire-dashboard|minor-tag", majorMinorVersion.ToString(2));
        VariableUpdater.Update(manifestVariables, "aspire-dashboard|major-tag", majorMinorVersion.Major.ToString());

        foreach (var (asset, checksum) in dashboardChecksumInfos)
        {
            var arch = asset.Name.Contains("arm64") ? "arm64" : "x64";
            VariableUpdater.Update(manifestVariables, $"aspire-dashboard|linux|{arch}|sha", checksum);
        }

        cancellationToken.ThrowIfCancellationRequested();
        string updatedContent = manifestVariables.Content;
        if (updatedContent != originalContent)
        {
            await File.WriteAllTextAsync(manifestPath, updatedContent, cancellationToken);
        }
    }

    private async Task ApplyUpdatesAsync(
        string repoRoot,
        Build build,
        CancellationToken cancellationToken = default)
    {
        await ApplyAsync(repoRoot, build, cancellationToken);

        // Generators can change files even when the manifest is unchanged.
        await ScriptRunner.GenerateDockerfilesAsync(repoRoot, cancellationToken);
        await ScriptRunner.GenerateReadmesAsync(repoRoot, cancellationToken);
    }

    /// <summary>
    /// Manually compute the checksum of an Aspire Dashboard asset by downloading it from
    /// <paramref name="url"/> and hashing the contents.
    /// </summary>
    /// <remarks>
    /// Remove once https://github.com/dotnet/dotnet-docker/issues/6568 is completed.
    /// </remarks>
    private async Task<string> GetChecksumAsync(string url, CancellationToken cancellationToken)
    {
        logger.LogInformation("Downloading Aspire Dashboard archive from {Url}", url);
        return await ChecksumHelper.ComputeChecksumShaAsync(httpClient, url, cancellationToken)
            ?? throw new InvalidOperationException($"Unable to retrieve checksum for '{url}'.");
    }
}
