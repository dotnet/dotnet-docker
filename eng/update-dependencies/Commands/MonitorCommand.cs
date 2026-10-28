// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class MonitorCommand
{
    private const string VersionSourceName = "dotnet/dotnet-monitor";

    public static Command CreateCliCommand(IServiceProvider services) =>
        new Command("monitor", "Update monitor")
        {
            CreatePipelineBuildCommand(services),
            CreateVersionCommand(services),
        };

    private static Command CreatePipelineBuildCommand(IServiceProvider services)
    {
        PositiveIdArgument runId = new PositiveIdArgument("The Azure DevOps pipeline run ID to use as the update source");

        Command command = new Command("pipeline-build", "Update from an Azure DevOps pipeline run")
        {
            runId
        };

        CreatePullRequestOptions.AddTo(command);

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            CreatePullRequestOptions options = CreatePullRequestOptions.Bind(parseResult);
            MonitorUpdater updater = services.GetRequiredService<MonitorUpdater>();
            DependencyUpdateRunner runner = services.GetRequiredService<DependencyUpdateRunner>();

            DependencyUpdate update = await updater.ResolveFromPipelineBuildAsync(
                parseResult.GetValue(runId),
                cancellationToken);

            await runner.RunAsync(options, VersionSourceName, update, cancellationToken);
        });

        return command;
    }

    private static Command CreateVersionCommand(IServiceProvider services)
    {
        Argument<string> version = new Argument<string>("version")
        {
            Description = "The product version to use as the update source",
        };

        Command command = new Command("version", "Update to a specific version")
        {
            version
        };

        CreatePullRequestOptions.AddTo(command);

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            CreatePullRequestOptions options = CreatePullRequestOptions.Bind(parseResult);
            MonitorUpdater updater = services.GetRequiredService<MonitorUpdater>();
            DependencyUpdateRunner runner = services.GetRequiredService<DependencyUpdateRunner>();

            DependencyUpdate update = updater.ResolveFromVersion(parseResult.GetRequiredValue(version));

            await runner.RunAsync(options, VersionSourceName, update, cancellationToken);
        });

        return command;
    }
}
