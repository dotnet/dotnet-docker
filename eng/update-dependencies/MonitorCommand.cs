// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Dotnet.Docker;

internal sealed class MonitorCommand(
    IPipelineArtifactProvider pipelineArtifactProvider,
    ICommand<SpecificCommandOptions> specificCommand,
    ILogger<MonitorCommand> logger)
        : BaseCommand<MonitorOptions>
{
    public override async Task<int> ExecuteAsync(MonitorOptions options)
    {
        if ((options.Version is null) == (options.PipelineRunId is null))
        {
            logger.LogError("Specify either a Monitor version or --pipeline-run-id, but not both.");
            return 1;
        }

        if (options.PipelineRunId is <= 0)
        {
            logger.LogError("--pipeline-run-id must be a positive Azure DevOps pipeline run ID.");
            return 1;
        }

        string? version = options.Version;
        if (options.PipelineRunId is int pipelineRunId)
        {
            version = await GetVersionFromPipelineAsync(options, pipelineRunId);
        }

        version = version?.Trim();
        if (string.IsNullOrEmpty(version) || !SemanticVersion.TryParse(version, out var parsedVersion))
        {
            logger.LogError("Invalid .NET Monitor version: '{Version}'.", version);
            return 1;
        }

        logger.LogInformation("Updating .NET Monitor to {Version}", version);

        string dockerfileVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}";
        var variables = ManifestVariables.FromFile(options.GetManifestVersionsFilePath());
        string branch = variables.GetValue("branch");

        bool stableBranding = parsedVersion.ReleaseLabels.FirstOrDefault() is "servicing" or "rtm";

        ReleaseState releaseState = branch == "main"
            ? ReleaseState.Release
            : ReleaseState.Prerelease;

        var specificOptions = SpecificCommandOptions.FromPullRequestOptions(options) with
        {
            RepoRoot = options.RepoRoot,
            DockerfileVersion = dockerfileVersion,
            VersionSourceName = $"dotnet/dotnet-monitor/{dockerfileVersion}",
            StableBranding = stableBranding,
            ReleaseState = releaseState,
            ProductVersions = new Dictionary<string, string?>
            {
                { "monitor", version },
                { "monitor-base", version },
                { "monitor-ext-azureblobstorage", version },
                { "monitor-ext-s3storage", version },
            },
        };

        return await specificCommand.ExecuteAsync(specificOptions);
    }

    private Task<string> GetVersionFromPipelineAsync(MonitorOptions options, int pipelineRunId)
    {
        string organization = string.IsNullOrEmpty(options.AzdoOrganization)
            ? "https://dev.azure.com/dnceng"
            : options.AzdoOrganization;

        string project = string.IsNullOrEmpty(options.AzdoProject)
            ? "internal"
            : options.AzdoProject;

        var versionFile = new PipelineArtifactFile("Build_Info", "dotnet-monitor.nupkg.buildversion");

        return pipelineArtifactProvider.GetArtifactTextContentAsync(
            organization,
            project,
            pipelineRunId,
            versionFile);
    }
}
