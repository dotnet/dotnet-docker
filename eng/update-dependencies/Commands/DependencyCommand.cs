// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class DependencyCommand
{
    public static Command Create(UpdaterRegistration registration, Func<IServiceProvider> getServices)
    {
        var command = new Command(registration.Name, $"Update {registration.Name}");

        if (typeof(IBarBuildUpdater).IsAssignableFrom(registration.Type))
            command.Subcommands.Add(FromBuildCommand.Create(registration, getServices));

        if (typeof(IBarChannelUpdater).IsAssignableFrom(registration.Type))
            command.Subcommands.Add(FromChannelCommand.Create(registration, getServices));

        if (typeof(IPipelineBuildUpdater).IsAssignableFrom(registration.Type))
            command.Subcommands.Add(FromPipelineBuildCommand.Create(registration, getServices));

        if (typeof(IVersionUpdater).IsAssignableFrom(registration.Type))
            command.Subcommands.Add(FromVersionCommand.Create(registration, getServices));

        if (typeof(IGitHubReleaseUpdater).IsAssignableFrom(registration.Type))
            FromGitHubReleaseCommand.Configure(command, registration, getServices);

        if (registration.Type == typeof(DotNetUpdater))
            command.Subcommands.Add(FromStagingPipelineCommand.Create(getServices));

        return command;
    }
}
