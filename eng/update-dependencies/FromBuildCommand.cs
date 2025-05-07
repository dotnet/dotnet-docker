// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class FromBuildCommand(
    IBasicBarClient barClient,
    IBuildUpdaterService buildUpdaterService,
    ILogger<FromBuildCommand> logger)
    : BaseCommand<FromBuildOptions>
{
    private readonly IBasicBarClient _barClient = barClient;
    private readonly IBuildUpdaterService _buildUpdaterService = buildUpdaterService;
    private readonly ILogger<FromBuildCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromBuildOptions options)
    {
        _logger.LogInformation("Getting BAR build with ID {options.Id}", options.Id);
        Build build = await _barClient.GetBuildAsync(options.Id);
        return await _buildUpdaterService.UpdateFrom(build, options);
    }
}
