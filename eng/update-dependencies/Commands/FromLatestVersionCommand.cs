// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromLatestVersionCommand
{
    public static Command CreateCliCommand<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("latest", "Update to the latest available version");
        FromLatestVersionOptions.AddTo(command);

        command.SetAction(async (result, cancellationToken) =>
        {
            FromLatestVersionOptions options = FromLatestVersionOptions.Bind(result);
            var updater = (ILatestVersionUpdater)services.GetRequiredService<TUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();

            ManifestVariables variables = await runner.GetTargetManifestAsync(options, cancellationToken);

            IReadOnlyList<DependencyUpdate> updates = await updater.ResolveLatestAsync(
                variables,
                options.Internal,
                options.DotnetVersion,
                cancellationToken);

            foreach (DependencyUpdate update in updates)
            {
                await runner.RunAsync(options, TUpdater.VersionSourceName, update, cancellationToken);
            }
        });

        return command;
    }
}
