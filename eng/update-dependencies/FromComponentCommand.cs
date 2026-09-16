// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Command that updates a single component using a registered GitHub release updater.
/// </summary>
public sealed class FromComponentCommand(
    IServiceProvider serviceProvider,
    ILogger<FromComponentCommand> logger
) : BaseCommand<FromComponentOptions>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<FromComponentCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromComponentOptions options)
    {
        _logger.LogInformation(
            "Resolving component '{Component}'",
            options.Component);

        var updater = _serviceProvider.GetUpdater<IGitHubReleaseUpdater>(options.Component);
        options = options with
        {
            VersionSourceName = string.IsNullOrEmpty(options.VersionSourceName) ? options.Component : options.VersionSourceName,
        };

        await DependencyUpdateRunner.RunAsync(
            options,
            (variables, _, token) => updater.UpdateFromGitHubReleaseAsync(variables, token),
            CancellationToken.None);
        return 0;
    }
}
