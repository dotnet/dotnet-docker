// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Dotnet.Docker.Git;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Sync;

/// <summary>
/// This command is intended to update the state of an internal/release/* branch with the state of
/// the corresponding release/* branch. It does so by submitting pull requests to update the
/// internal/release/* branch with the latest changes to the release/* branch.
/// </summary>
/// <remarks>
/// To be "in sync" means that the content of both branches is identical, except that the
/// internal/release/* branch may contain .NET versions that are not yet publicly available.
/// "In sync" does not necessarily mean that the commit SHAs of both branches are identical.
/// </remarks>
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

        // Verify that the source branch exists on the remote.
        var sourceBranchExists = await repo.RemoteBranchExistsAsync(options.SourceBranch);
        if (!sourceBranchExists)
        {
            throw new InvalidOperationException(
                $"The source branch '{options.SourceBranch}' does not exist on the remote repository.");
        }

        // If the target branch doesn't exist, then create it based on the source branch.
        var targetBranchExists = await repo.RemoteBranchExistsAsync(options.TargetBranch);
        if (!targetBranchExists)
        {
            await repo.CreateRemoteBranchAsync(
                newBranch: options.TargetBranch,
                baseBranch: options.SourceBranch);
            return 0;
        }

        // If both branches are at the same commit, then there is nothing to do.
        var sourceSha = await repo.GetRemoteBranchShaAsync(options.SourceBranch);
        var targetSha = await repo.GetRemoteBranchShaAsync(options.TargetBranch);
        if (sourceSha == targetSha)
        {
            _logger.LogInformation(
                "The source branch '{SourceBranch}' and target branch '{TargetBranch}' are already in sync.",
                options.SourceBranch, options.TargetBranch);
            return 0;
        }

        throw new NotImplementedException("Scenario is not yet implemented.");
    }
}
