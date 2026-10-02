// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class PowerShellCommand
{
    private const string VersionSourceName = "PowerShell/PowerShell";

    private static readonly Option<bool> s_internal = new("--internal")
    {
        Description = "Use the internal (non-public) location for the version",
    };

    private static readonly Option<string?> s_dotnetVersion = new("--dotnet-version")
    {
        Description = "Only update images for this .NET major.minor version (for example, 9.0)",
    };

    public static Command CreateCliCommand(IServiceProvider services) =>
        new Command("powershell", "Update powershell")
        {
            CreateVersionCommand(services),
            CreateLatestCommand(services),
        };

    private static Command CreateVersionCommand(IServiceProvider services)
    {
        var version = new Argument<string>("version")
        {
            Description = "The PowerShell version to update to",
        };

        var baseUrl = new Option<string?>("--base-url")
        {
            Description = "Use this exact location for the version instead of the default public or internal one",
        };

        var command = new Command("version", "Update to a specific version")
        {
            version,
            baseUrl,
            s_internal,
            s_dotnetVersion,
        };

        CreatePullRequestOptions.AddTo(command);

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            CreatePullRequestOptions options = CreatePullRequestOptions.Bind(parseResult);
            var updater = services.GetRequiredService<PowerShellUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();

            DependencyUpdate update = updater.ResolveFromVersion(
                parseResult.GetRequiredValue(version),
                parseResult.GetValue(s_internal),
                parseResult.GetValue(baseUrl),
                parseResult.GetValue(s_dotnetVersion));

            await runner.RunAsync(options, VersionSourceName, update, cancellationToken);
        });

        return command;
    }

    private static Command CreateLatestCommand(IServiceProvider services)
    {
        var command = new Command("latest", "Update to the latest available version")
        {
            s_internal,
            s_dotnetVersion,
        };

        CreatePullRequestOptions.AddTo(command);

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            CreatePullRequestOptions options = CreatePullRequestOptions.Bind(parseResult);
            var updater = services.GetRequiredService<PowerShellUpdater>();
            var runner = services.GetRequiredService<DependencyUpdateRunner>();

            ManifestVariables variables = await runner.GetTargetManifestAsync(options, cancellationToken);

            IReadOnlyList<DependencyUpdate> updates = await updater.ResolveLatestAsync(
                variables,
                parseResult.GetValue(s_internal),
                parseResult.GetValue(s_dotnetVersion),
                cancellationToken);

            foreach (DependencyUpdate update in updates)
            {
                await runner.RunAsync(options, VersionSourceName, update, cancellationToken);
            }
        });

        return command;
    }
}
