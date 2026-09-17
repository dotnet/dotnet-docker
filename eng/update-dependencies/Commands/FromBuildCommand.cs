// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal class FromBuildCommand(
    DependencyUpdateRunner runner,
    IBasicBarClient barClient,
    ILogger<FromBuildCommand> logger,
    IServiceProvider serviceProvider
) : BaseCommand<FromBuildOptions>
{
    private readonly IBasicBarClient _barClient = barClient;
    private readonly ILogger<FromBuildCommand> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override async Task<int> ExecuteAsync(FromBuildOptions options)
    {
        _logger.LogInformation("Getting BAR build with ID {options.Id}", options.Id);
        Build build = await _barClient.GetBuildAsync(options.Id);

        var updater = _serviceProvider.GetUpdater<IBarBuildUpdater>(build.GetUpdaterKey());
        await runner.RunAsync(options,
            (variables, repoRoot, token) => updater.UpdateFromBarBuildAsync(variables, repoRoot, build, token),
            CancellationToken.None);
        return 0;
    }
}
