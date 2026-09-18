// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromPipelineBuildCommand
{
    public static Command Create<TUpdater>(IServiceProvider services) where TUpdater : class, IUpdater
    {
        var command = new Command("pipeline-build", "Update from an Azure DevOps pipeline run");
        FromPipelineBuildOptions.AddTo(command);

        command.SetAction((result, cancellationToken) =>
        {
            FromPipelineBuildOptions options = FromPipelineBuildOptions.Bind(result);
            var updater = (IPipelineBuildUpdater)ActivatorUtilities.GetServiceOrCreateInstance<TUpdater>(services);
            var runner = services.GetRequiredService<DependencyUpdateRunner>();
            var configuration = services.GetRequiredService<UpdateDependenciesConfiguration>();
            var build = new PipelineBuildReference(
                configuration.AzureDevOps.Organization,
                configuration.AzureDevOps.Project,
                options.RunId);

            return runner.RunAsync(
                options,
                TUpdater.VersionSourceName,
                (variables, _, token) => updater.UpdateFromPipelineBuildAsync(variables, build, token),
                cancellationToken);
        });

        return command;
    }
}
