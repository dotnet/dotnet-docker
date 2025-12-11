// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class FromBuildCommand(
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

        // To add support for new repos to the from-build command:
        // - Implement a new IBuildUpdaterService
        // - Add a new BuildRepo enum value
        // - Update the BuildRepo.GetBuildRepo() extension method to return the new enum value
        // - Register a new keyed singleton in Program.cs BuildRepo key
        BuildRepo buildRepo = build.GetBuildRepo();
        var buildUpdater = _serviceProvider.GetKeyedService<IBuildUpdaterService>(buildRepo);
        if (buildUpdater is null)
        {
            _logger.LogError("No updater service registered for build repo: {buildRepo}", buildRepo);
            return 1;
        }

        return await buildUpdater.UpdateFrom(build, options);
    }
}
