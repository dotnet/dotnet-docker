// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromVersionCommand
{
    public static Command CreateCliCommand<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("version", "Update to a specific version");
        FromVersionOptions.AddTo(command);
        if (typeof(IVersionSourceUpdater).IsAssignableFrom(typeof(TUpdater)))
        {
            VersionSourceOptions.AddTo(command, includeBaseUrl: true);
        }

        command.SetAction(async (result, cancellationToken) =>
        {
            FromVersionOptions options = FromVersionOptions.Bind(result);
            TUpdater updater = services.GetRequiredService<TUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();
            DependencyUpdate update = updater is IVersionSourceUpdater sourceUpdater
                ? await sourceUpdater.ResolveFromVersionAsync(
                    options.Version, VersionSourceOptions.Bind(result), cancellationToken)
                : await ((IVersionUpdater)updater).ResolveFromVersionAsync(options.Version, cancellationToken);

            await runner.RunAsync(options, TUpdater.VersionSourceName, update, cancellationToken);
        });

        return command;
    }
}
