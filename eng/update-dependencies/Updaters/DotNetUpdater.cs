// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies.Model.Release;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class DotNetUpdater(IBasicBarClient barClient, ILogger<DotNetUpdater> logger)
    : IBarBuildUpdater, IBarChannelUpdater, IStagingPipelineUpdater
{
    public const string Key = "dotnet";
    public const string PublicRepository = "https://github.com/dotnet/dotnet";
    public const string InternalRepository = "https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet";

    public async Task UpdateFromBarBuildAsync(
        ManifestVariables variables,
        string repoRoot,
        Build build,
        CancellationToken cancellationToken)
    {
        ValidateRepository(build.GitHubRepository ?? build.AzureDevOpsRepository);
        logger.LogInformation("Updating .NET from BAR build {BuildId} at {Commit}", build.Id, build.Commit);

        IEnumerable<Asset> assets = await barClient.GetAssetsAsync(buildId: build.Id).WaitAsync(cancellationToken);
        var versions = ProductVersions.FromVmrBuildAssets(assets);
        string dockerfileVersion = VersionHelper.ResolveMajorMinorVersion(versions.Sdk.Version).ToString(2);
        var productVersions = new Dictionary<string, string?>
        {
            ["dotnet"] = versions.Runtime.Version,
            ["runtime"] = versions.Runtime.Version,
            ["aspnet"] = versions.AspNetCore.Version,
            ["aspnet-composite"] = versions.AspNetCore.Version,
            ["sdk"] = versions.Sdk.Version,
        };

        ApplyProductVersions(variables, repoRoot, dockerfileVersion, productVersions, "");
    }

    public async Task UpdateFromBarChannelAsync(
        ManifestVariables variables,
        string repoRoot,
        int channelId,
        CancellationToken cancellationToken)
    {
        Build build = await barClient.GetLatestBuildAsync(PublicRepository, channelId).WaitAsync(cancellationToken);
        await UpdateFromBarBuildAsync(variables, repoRoot, build, cancellationToken);
    }

    public Task UpdateFromStagingPipelineAsync(
        ManifestVariables variables,
        string repoRoot,
        ReleaseConfig release,
        string internalBaseUrl,
        CancellationToken cancellationToken)
    {
        bool isInternal = !string.IsNullOrEmpty(internalBaseUrl);
        string dockerfileVersion = VersionHelper.ResolveMajorMinorVersion(release.RuntimeBuild).ToString(2);
        var productVersions = new Dictionary<string, string?>();

        // SDK-only releases are almost always one-off updates/bug
        // fixes on top of an existing release of the Runtime and
        // ASP.NET Core.
        //
        // If the release config tells us that this is an
        // SDK-only release, we can assume that we have already
        // released the runtime/aspnet versions that it's based on, and
        // therefore we shouldn't update them unnecessarily.
        if (!isInternal || !release.SdkOnly)
        {
            productVersions["dotnet"] = VersionHelper.ResolveProductVersion(release.RuntimeBuild);
            productVersions["runtime"] = isInternal ? release.RuntimeBuild : release.Runtime;
            productVersions["aspnet"] = isInternal ? release.AspBuild : release.Asp;
            productVersions["aspnet-composite"] = productVersions["aspnet"];
        }
        productVersions["sdk"] = VersionHelper.GetHighestSdkVersion(isInternal ? release.SdkBuilds : release.Sdks);

        logger.LogInformation("Resolved .NET product versions: {Versions}",
            string.Join(", ", productVersions.Select(pair => $"{pair.Key}: {pair.Value}")));
        ApplyProductVersions(variables, repoRoot, dockerfileVersion, productVersions, internalBaseUrl);
        return Task.CompletedTask;
    }

    private void ApplyProductVersions(
        ManifestVariables variables,
        string repoRoot,
        string dockerfileVersion,
        IReadOnlyDictionary<string, string?> productVersions,
        string internalBaseUrl)
    {
        NuGetConfigUpdater.Update(variables, repoRoot, dockerfileVersion, productVersions["sdk"], !string.IsNullOrEmpty(internalBaseUrl));
        BaseUrlUpdater.Update(variables, dockerfileVersion, internalBaseUrl, productVersions.Count == 1);

        foreach (var (product, version) in productVersions)
        {
            string productVersion = "";
            if (version is not null)
            {
                var parsedVersion = SemanticVersion.Parse(version);
                productVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}.{parsedVersion.Patch}";
                if (parsedVersion.IsPrerelease && parsedVersion.ReleaseLabels.First() is not ("servicing" or "rtm"))
                {
                    productVersion += $"-{string.Join(".", parsedVersion.ReleaseLabels.Take(2))}";
                }
            }

            UpdateVersion(variables, $"{product}|{dockerfileVersion}|build-version", version ?? "");
            UpdateVersion(variables, $"{product}|{dockerfileVersion}|product-version", productVersion);
        }
    }

    private void UpdateVersion(ManifestVariables variables, string name, string version)
    {
        if (variables.Contains(name) && !SemanticVersion.TryParse(variables.GetRawValue(name), out _))
        {
            logger.LogInformation("Leaving manifest variable '{Variable}' unchanged.", name);
            return;
        }

        variables.SetValue(name, version);
    }

    private static void ValidateRepository(string repository)
    {
        if (repository is not (PublicRepository or InternalRepository))
        {
            throw new InvalidOperationException($"Unsupported .NET build repository '{repository}'.");
        }
    }
}
