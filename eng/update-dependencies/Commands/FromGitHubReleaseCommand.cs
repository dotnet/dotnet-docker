// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromGitHubReleaseCommand
{
    public static void Configure<TUpdater>(Command command, IServiceProvider services)
        where TUpdater : class, IUpdater
    {
        CreatePullRequestOptions.AddTo(command);

        command.SetAction(async (result, cancellationToken) =>
        {
            CreatePullRequestOptions options = CreatePullRequestOptions.Bind(result);
            var updater = (IGitHubReleaseUpdater)services.GetRequiredService<TUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();
            DependencyUpdate update = await updater.ResolveFromGitHubReleaseAsync(cancellationToken);

            await runner.RunAsync(options, TUpdater.VersionSourceName, update, cancellationToken);
        });
    }
}
