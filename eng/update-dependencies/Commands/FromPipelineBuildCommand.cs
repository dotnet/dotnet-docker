// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromPipelineBuildCommand
{
    public static Command CreateCliCommand<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("pipeline-build", "Update from an Azure DevOps pipeline run");
        FromPipelineBuildOptions.AddTo(command);

        command.SetAction(async (result, cancellationToken) =>
        {
            FromPipelineBuildOptions options = FromPipelineBuildOptions.Bind(result);
            var updater = (IPipelineBuildUpdater)services.GetRequiredService<TUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();
            var configuration = services.GetRequiredService<UpdateDependenciesConfiguration>();
            var build = new PipelineBuildReference(
                configuration.AzureDevOps.Organization,
                configuration.AzureDevOps.Project,
                options.RunId);

            DependencyUpdate update = await updater.ResolveFromPipelineBuildAsync(build, cancellationToken);

            await runner.RunAsync(options, TUpdater.VersionSourceName, update, cancellationToken);
        });

        return command;
    }
}
