// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Dotnet.Docker.Git;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Sync;

public sealed class SyncInternalReleaseCommand(
    IGitRepoHelperFactory gitRepoHelperFactory,
    ILogger<SyncInternalReleaseCommand> logger
) : BaseCommand<SyncInternalReleaseOptions>
{
    private readonly IGitRepoHelperFactory _gitRepoHelperFactory = gitRepoHelperFactory;
    private readonly ILogger<SyncInternalReleaseCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(SyncInternalReleaseOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.RemoteUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.TargetBranch);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.SourceBranch);

        // Do not allow syncing starting from an internal branch.
        if (options.SourceBranch.StartsWith("internal/", StringComparison.OrdinalIgnoreCase))
        {
            throw new IncorrectBranchException(
                $"The source branch '{options.SourceBranch}' cannot be an internal branch.");
        }

        using var repo = await _gitRepoHelperFactory.CreateAsync(options.RemoteUrl);

        var currentBranch = await repo.GetCurrentBranchAsync();
        _logger.LogInformation("Current branch is {CurrentBranch}", currentBranch);

        // If the target branch doesn't exist, then create it based on the source branch.
        var targetBranchExists = await repo.RemoteBranchExistsAsync(options.TargetBranch);
        if (!targetBranchExists)
        {
            await repo.CreateRemoteBranchAsync(
                newBranch: options.TargetBranch,
                baseBranch: options.SourceBranch);

            return 0;
        }

        return 0;
    }
}
