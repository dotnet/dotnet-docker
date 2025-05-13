// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class FromChannelCommand(
    IBasicBarClient barClient,
    IBuildUpdaterService buildUpdaterService,
    ILogger<FromChannelCommand> logger)
    : BaseCommand<FromChannelOptions>
{
    private readonly IBasicBarClient _barClient = barClient;
    private readonly IBuildUpdaterService _buildUpdaterService = buildUpdaterService;
    private readonly ILogger<FromChannelCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromChannelOptions options)
    {
        _logger.LogInformation("Getting latest build for {options.Repo} from channel {options.Channel}",
            options.Repo, options.Channel);

        Build latestBuild = await _barClient.GetLatestBuildAsync(options.Repo, options.Channel);
        string? channelName = latestBuild.Channels.FirstOrDefault(c => c.Id == options.Channel)?.Name;

        _logger.LogInformation("Channel {options.Channel} is '{channel.Name}'",
            options.Channel, latestBuild.Channels.FirstOrDefault(c => c.Id == options.Channel)?.Name);

        return await _buildUpdaterService.UpdateFrom(latestBuild, options);
    }
}
