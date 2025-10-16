// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class FromChannelCommand(
    IBasicBarClient barClient,
    IServiceProvider serviceProvider,
    ILogger<FromChannelCommand> logger)
    : BaseCommand<FromChannelOptions>
{
    private readonly IBasicBarClient _barClient = barClient;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<FromChannelCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromChannelOptions options)
    {
        _logger.LogInformation("Getting latest build for {options.Repo} from channel {options.Channel}",
            options.Repo, options.Channel);

        Build latestBuild = await _barClient.GetLatestBuildAsync(options.Repo, options.Channel);
        string? channelName = latestBuild.Channels.FirstOrDefault(c => c.Id == options.Channel)?.Name;

        _logger.LogInformation("Channel {options.Channel} is '{channel.Name}'",
            options.Channel, latestBuild.Channels.FirstOrDefault(c => c.Id == options.Channel)?.Name);

        // To add support for new repos to the from-channel command:
        // - Implement a new IBuildUpdaterService
        // - Add a new BuildRepo enum value
        // - Update the BuildRepo.GetBuildRepo() extension method to return the new enum value
        // - Register a new keyed singleton in Program.cs BuildRepo key
        BuildRepo buildRepo = latestBuild.GetBuildRepo();
        var buildUpdater = _serviceProvider.GetKeyedService<IBuildUpdaterService>(buildRepo);
        if (buildUpdater is null)
        {
            _logger.LogError("No updater service registered for build repo: {buildRepo}", buildRepo);
            return 1;
        }

        return await buildUpdater.UpdateFrom(latestBuild, options);
    }
}
