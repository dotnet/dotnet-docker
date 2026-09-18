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
    public static Command Create(UpdaterRegistration registration, Func<IServiceProvider> getServices)
    {
        var command = new Command("build-id", "Update from a specific BAR build");
        FromBuildOptions.AddTo(command);

        command.SetAction(async (result, cancellationToken) =>
        {
            FromBuildOptions options = FromBuildOptions.Bind(result);
            IServiceProvider services = getServices();
            var barClient = services.GetRequiredService<IBasicBarClient>();
            var updater = (IBarBuildUpdater)services.GetRequiredService(registration.Type);
            var runner = services.GetRequiredService<DependencyUpdateRunner>();
            Build build = await barClient.GetBuildAsync(options.Id).WaitAsync(cancellationToken);

            await runner.RunAsync(
                options,
                registration.VersionSourceName,
                (variables, repoRoot, token) => updater.UpdateFromBarBuildAsync(variables, repoRoot, build, token),
                cancellationToken);
        });

        return command;
    }
}
