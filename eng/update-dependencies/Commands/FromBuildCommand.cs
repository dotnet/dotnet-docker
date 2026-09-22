// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromBuildCommand
{
    public static Command CreateCliCommand<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("build-id", "Update from a specific BAR build");
        FromBuildOptions.AddTo(command);

        command.SetAction(async (result, cancellationToken) =>
        {
            FromBuildOptions options = FromBuildOptions.Bind(result);
            var barClient = services.GetRequiredService<IBasicBarClient>();
            var updater = (IBarBuildUpdater)services.GetRequiredService<TUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();
            Build build = await barClient.GetBuildAsync(options.Id).WaitAsync(cancellationToken);
            DependencyUpdate update = await updater.ResolveFromBarBuildAsync(build, cancellationToken);

            await runner.RunAsync(options, TUpdater.VersionSourceName, update, cancellationToken);
        });

        return command;
    }
}
