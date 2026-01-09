// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

/// <summary>
/// Command that updates a single component using a registered
/// IDependencyVersionSource. This provides an extensibility point so new
/// components can be onboarded by simply registering a keyed singleton of
/// IDependencyVersionSource in Program.cs without modifying the legacy
/// SpecificCommand or adding bespoke commands.
/// </summary>
internal sealed class FromComponentCommand(
    IServiceProvider serviceProvider,
    ILogger<FromComponentCommand> logger
) : BaseCommand<FromComponentOptions>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<FromComponentCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromComponentOptions options)
    {
        _logger.LogInformation(
            "Resolving component '{Component}' with channel '{Channel}'",
            options.Component, options.Channel);

        bool versionSourceExists = _serviceProvider.TryGetRequiredKeyedService(
            serviceKey: options.Component,
            out IDependencyVersionSource? versionSource);

        if (!versionSourceExists || versionSource is null)
        {
            throw new InvalidOperationException($"No version source registered for component '{options.Component}'");
        }

        var channel = options.Channel is not null
            ? new ComponentVersionChannel(options.Channel)
            : ComponentVersionChannel.Default;

        ComponentVersionInfo versionInfo = await versionSource.GetVersionInfoAsync(channel);

        _logger.LogInformation(
            "Resolved version {Version} for component {Component}",
            versionInfo.Version, options.Component);

        // Bridge to legacy implementation via SpecificCommand
        var specific = new SpecificCommand();
        var specificOptions = SpecificCommandOptions.FromPullRequestOptions(options) with
        {
            DockerfileVersion = options.DockerfileVersion,
            ProductVersions = new Dictionary<string, string?>
            {
                { options.Component, versionInfo.Version }
            },
            VersionSourceName = string.IsNullOrEmpty(options.VersionSourceName) ? options.Component : options.VersionSourceName,
        };

        return await specific.ExecuteAsync(specificOptions);
    }
}
