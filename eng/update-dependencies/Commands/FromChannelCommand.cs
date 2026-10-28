// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromChannelCommand
{
    public static Command CreateCliCommand<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        Command command = new Command("channel", "Update from the latest build in a BAR channel");
        FromChannelOptions.AddTo(command);

        command.SetAction(async (result, cancellationToken) =>
        {
            FromChannelOptions options = FromChannelOptions.Bind(result);
            IBarChannelUpdater updater = (IBarChannelUpdater)services.GetRequiredService<TUpdater>();
            DependencyUpdateRunner runner = services.GetRequiredService<DependencyUpdateRunner>();
            DependencyUpdate update = await updater.ResolveFromBarChannelAsync(options.Channel, cancellationToken);

            await runner.RunAsync(options, TUpdater.VersionSourceName, update, cancellationToken);
        });

        return command;
    }
}
