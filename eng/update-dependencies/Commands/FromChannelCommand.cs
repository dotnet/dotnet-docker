// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal class FromChannelCommand(
    DependencyUpdateRunner runner,
    IServiceProvider serviceProvider,
    ILogger<FromChannelCommand> logger)
    : BaseCommand<FromChannelOptions>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<FromChannelCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromChannelOptions options)
    {
        _logger.LogInformation("Getting latest build for {options.Repo} from channel {options.Channel}",
            options.Repo, options.Channel);

        var updater = _serviceProvider.GetUpdater<IBarChannelUpdater>(BuildExtensions.GetUpdaterKey(options.Repo));
        await runner.RunAsync(options,
            (variables, repoRoot, token) => updater.UpdateFromBarChannelAsync(variables, repoRoot, options.Repo, options.Channel, token),
            CancellationToken.None);
        return 0;
    }
}
