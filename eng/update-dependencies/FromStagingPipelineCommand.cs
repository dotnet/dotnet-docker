// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class FromStagingPipelineCommand(
    ILogger<FromStagingPipelineCommand> logger)
    : BaseCommand<FromStagingPipelineOptions>
{
    private readonly ILogger<FromStagingPipelineCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(FromStagingPipelineOptions options)
    {
        _logger.LogInformation(
            "Processing staging pipeline run with ID {options.StagingPipelineRunId}",
            options.StagingPipelineRunId);

        return 0; // Return success for now
    }
}
