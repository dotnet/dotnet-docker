// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromChannelCommand
{
    public static Command Create<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("channel", "Update from the latest build in a BAR channel");
        FromChannelOptions.AddTo(command);

        command.SetAction((result, cancellationToken) =>
        {
            FromChannelOptions options = FromChannelOptions.Bind(result);
            var updater = (IBarChannelUpdater)services.GetRequiredService<TUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();

            return runner.RunAsync(
                options,
                TUpdater.VersionSourceName,
                (variables, repoRoot, token) => updater.UpdateFromBarChannelAsync(variables, repoRoot, options.Channel, token),
                cancellationToken);
        });

        return command;
    }
}
