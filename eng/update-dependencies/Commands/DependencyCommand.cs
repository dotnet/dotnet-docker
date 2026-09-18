// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class DependencyCommand
{
    public static Command Create<TUpdater>(Func<IServiceProvider> getServices)
        where TUpdater : class, IUpdater
    {
        var command = new Command(TUpdater.Name, $"Update {TUpdater.Name}");

        if (typeof(IBarBuildUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromBuildCommand.Create<TUpdater>(getServices));

        if (typeof(IBarChannelUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromChannelCommand.Create<TUpdater>(getServices));

        if (typeof(IPipelineBuildUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromPipelineBuildCommand.Create<TUpdater>(getServices));

        if (typeof(IVersionUpdater).IsAssignableFrom(typeof(TUpdater)))
            command.Subcommands.Add(FromVersionCommand.Create<TUpdater>(getServices));

        if (typeof(IGitHubReleaseUpdater).IsAssignableFrom(typeof(TUpdater)))
            FromGitHubReleaseCommand.Configure<TUpdater>(command, getServices);

        if (typeof(TUpdater) == typeof(DotNetUpdater))
            command.Subcommands.Add(FromStagingPipelineCommand.Create(getServices));

        return command;
    }
}
