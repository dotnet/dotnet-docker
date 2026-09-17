// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal sealed class AspireCommand(
    DependencyUpdateRunner runner,
    IBasicBarClient barClient,
    IServiceProvider services,
    ILogger<AspireCommand> logger)
        : BaseCommand<AspireOptions>
{
    public override Task<int> ExecuteAsync(AspireOptions options) => ExecuteAsync(options, CancellationToken.None);

    public async Task<int> ExecuteAsync(AspireOptions options, CancellationToken cancellationToken)
    {
        Build build;
        switch (options)
        {
            case { FromBuildId: int buildId, FromChannel: null } when buildId > 0:
                logger.LogInformation("Getting Aspire BAR build with ID {BuildId}", buildId);
                build = await barClient.GetBuildAsync(buildId).WaitAsync(cancellationToken);
                break;
            case { FromBuildId: null, FromChannel: int channel } when channel > 0:
                logger.LogInformation("Getting latest Aspire build from channel {Channel}", channel);
                build = await barClient.GetLatestBuildAsync(AspireUpdater.PublicRepository, channel).WaitAsync(cancellationToken);
                break;
            default:
                logger.LogError("Specify either --from-build-id or --from-channel with a positive ID, but not both.");
                return 1;
        }

        if (!AspireUpdater.IsAspireBuild(build))
        {
            string repository = build.GitHubRepository ?? build.AzureDevOpsRepository;
            logger.LogError("BAR build {BuildId} is not an Aspire build: {Repository}", build.Id, repository);
            return 1;
        }

        options = options with
        {
            VersionSourceName = string.IsNullOrEmpty(options.VersionSourceName)
                ? "microsoft/aspire"
                : options.VersionSourceName,
        };

        var updater = services.GetUpdater<IBarBuildUpdater>("aspire");
        await runner.RunAsync(
            options,
            (variables, repoRoot, token) => updater.UpdateFromBarBuildAsync(variables, repoRoot, build, token),
            cancellationToken);

        if (options.UpdateOnly)
        {
            logger.LogInformation("Local updates completed without publishing.");
        }

        return 0;
    }
}
