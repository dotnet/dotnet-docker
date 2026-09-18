// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class FromGitHubReleaseCommand
{
    public static void Configure(
        Command command,
        UpdaterRegistration registration,
        Func<IServiceProvider> getServices)
    {
        CreatePullRequestOptions.AddTo(command);

        command.SetAction((result, cancellationToken) =>
        {
            CreatePullRequestOptions options = CreatePullRequestOptions.Bind(result);
            IServiceProvider services = getServices();
            var updater = (IGitHubReleaseUpdater)services.GetRequiredService(registration.Type);
            var runner = services.GetRequiredService<DependencyUpdateRunner>();

            return runner.RunAsync(
                options,
                registration.VersionSourceName,
                (variables, _, token) => updater.UpdateFromGitHubReleaseAsync(variables, token),
                cancellationToken);
        });
    }
}
