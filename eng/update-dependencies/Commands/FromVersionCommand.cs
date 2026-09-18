// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromVersionCommand
{
    public static Command Create<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("version", "Update to a specific version");
        FromVersionOptions.AddTo(command);

        command.SetAction((result, cancellationToken) =>
        {
            FromVersionOptions options = FromVersionOptions.Bind(result);
            var updater = (IVersionUpdater)ActivatorUtilities.GetServiceOrCreateInstance<TUpdater>(services);
            var runner = services.GetRequiredService<DependencyUpdateRunner>();

            return runner.RunAsync(
                options,
                TUpdater.VersionSourceName,
                (variables, _, token) => updater.UpdateFromVersionAsync(variables, options.Version, token),
                cancellationToken);
        });

        return command;
    }
}
