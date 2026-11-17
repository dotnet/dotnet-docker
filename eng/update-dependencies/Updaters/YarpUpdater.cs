// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class YarpUpdater(IBasicBarClient barClient, ILogger<YarpUpdater> logger)
    : IBarBuildUpdater, IBarChannelUpdater
{
    public const string Repository = "https://github.com/dotnet/yarp";

    // The Dockerfile downloads these archives, so a build must publish all of them.
    private static readonly string[] RequiredArchives =
    [
        "reverse-proxy-linux-x64.tar.gz",
        "reverse-proxy-linux-arm64.tar.gz",
    ];

    public static string Name => "yarp";
    public static string VersionSourceName => "dotnet/yarp";

    public async Task<DependencyUpdate> ResolveFromBarChannelAsync(int channelId, CancellationToken cancellationToken)
    {
        Build build = await barClient.GetLatestBuildAsync(Repository, channelId).WaitAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Could not find a YARP build in BAR channel {channelId}.");

        logger.LogInformation("Using latest YARP build {BuildId}", build.Id);
        return await ResolveFromBarBuildAsync(build, cancellationToken);
    }

    public Task<DependencyUpdate> ResolveFromBarBuildAsync(Build build, CancellationToken cancellationToken)
    {
        string repository = build.GitHubRepository ?? build.AzureDevOpsRepository;
        if (repository != Repository)
        {
            throw new ArgumentException($"BAR build {build.Id} is not a YARP build.", nameof(build));
        }

        string[] versions = RequiredArchives
            .Select(archive => build.Assets.FirstOrDefault(asset => asset.Name.EndsWith($"/{archive}", StringComparison.Ordinal))?.Version
                ?? throw new InvalidOperationException($"Could not find asset '{archive}' in YARP build {build.Id}."))
            .Distinct()
            .ToArray();

        if (versions.Length != 1)
        {
            throw new InvalidOperationException(
                $"YARP build {build.Id} has archives with different versions: {string.Join(", ", versions)}");
        }

        logger.LogInformation("Found YARP build version: {Version}", versions[0]);
        return Task.FromResult(ResolveFromVersion(versions[0]));
    }

    public static DependencyUpdate ResolveFromVersion(string buildVersion)
    {
        SemanticVersion parsedVersion = SemanticVersion.Parse(buildVersion);

        return new DependencyUpdate(
            Description: $"Update YARP to {buildVersion}",
            ApplyAsync: (variables, _, _) =>
            {
                Apply(variables, buildVersion, parsedVersion);
                return Task.CompletedTask;
            });
    }

    private static void Apply(ManifestVariables variables, string buildVersion, SemanticVersion parsedVersion)
    {
        string prefix = $"yarp|{parsedVersion.Major}|";
        string majorMinor = $"{parsedVersion.Major}.{parsedVersion.Minor}";

        // Dockerfiles and test data are organized by major.minor version, so a new one must be added by hand.
        if (!variables.Contains($"{prefix}build-version")
            || !variables.GetValue($"{prefix}product-version").StartsWith($"{majorMinor}.", StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"YARP {majorMinor} is not in the manifest. Add YARP {majorMinor} images before updating to {buildVersion}.");
        }

        string productVersion = VersionHelper.ResolveArcadeProductVersion(parsedVersion);
        string tagSuffix = productVersion.Contains('-') ? "-preview" : "";

        variables.SetValue($"{prefix}build-version", buildVersion);
        variables.SetValue($"{prefix}product-version", productVersion);
        variables.SetValue($"{prefix}minor-tag", $"{majorMinor}{tagSuffix}");
        variables.SetValue($"{prefix}major-tag", $"{parsedVersion.Major}{tagSuffix}");
    }
}
