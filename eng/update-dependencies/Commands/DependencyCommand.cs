// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class DependencyCommand
{
    public static Command CreateCliCommand<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command(TUpdater.Name, $"Update {TUpdater.Name}");

        if (typeof(IBarBuildUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromBuildCommand.CreateCliCommand<TUpdater>(services));

        if (typeof(IBarChannelUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromChannelCommand.CreateCliCommand<TUpdater>(services));

        if (typeof(IPipelineBuildUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromPipelineBuildCommand.CreateCliCommand<TUpdater>(services));

        if (typeof(IStagingPipelineUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromStagingPipelineCommand.CreateCliCommand(services));

        if (typeof(IVersionUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromVersionCommand.CreateCliCommand<TUpdater>(services));

        if (typeof(IGitHubReleaseUpdater).IsAssignableFrom(typeof(TUpdater)))
            FromGitHubReleaseCommand.Configure<TUpdater>(command, services);

        return command;
    }
}
