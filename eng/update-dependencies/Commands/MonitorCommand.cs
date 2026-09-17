// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal sealed class MonitorCommand(
    DependencyUpdateRunner runner,
    UpdateDependenciesConfiguration configuration,
    IServiceProvider services,
    ILogger<MonitorCommand> logger)
        : BaseCommand<MonitorOptions>
{
    public override Task<int> ExecuteAsync(MonitorOptions options) => ExecuteAsync(options, CancellationToken.None);

    public async Task<int> ExecuteAsync(MonitorOptions options, CancellationToken cancellationToken)
    {
        if (!ValidateOptions(options, logger))
        {
            return 1;
        }

        var updater = services.GetUpdater<MonitorUpdater>("monitor");
        string? version = options.Version;
        if (options.PipelineRunId is int pipelineRunId)
        {
            string organization = configuration.AzureDevOps.Organization;
            string project = configuration.AzureDevOps.Project;

            var reference = new PipelineBuildReference(organization, project, pipelineRunId);
            version = await updater.GetVersionFromPipelineAsync(reference, cancellationToken);
        }

        version = version?.Trim();
        if (string.IsNullOrEmpty(version) || !SemanticVersion.TryParse(version, out var parsedVersion))
        {
            logger.LogError("Invalid .NET Monitor version: '{Version}'.", version);
            return 1;
        }

        logger.LogInformation("Updating .NET Monitor to {Version}", version);
        options = options with
        {
            VersionSourceName = $"dotnet/dotnet-monitor/{parsedVersion.Major}.{parsedVersion.Minor}",
        };

        await runner.RunAsync(
            options,
            (variables, _, token) => updater.UpdateFromVersionAsync(variables, version, token),
            cancellationToken);

        if (options.UpdateOnly)
        {
            logger.LogInformation("Local updates completed without publishing.");
        }

        return 0;
    }

    private static bool ValidateOptions(MonitorOptions options, ILogger logger)
    {
        if ((options.Version is null) == (options.PipelineRunId is null))
        {
            logger.LogError("Specify either a Monitor version or --pipeline-run-id, but not both.");
            return false;
        }

        if (options.PipelineRunId is <= 0)
        {
            logger.LogError("--pipeline-run-id must be a positive Azure DevOps pipeline run ID.");
            return false;
        }

        return true;
    }
}
