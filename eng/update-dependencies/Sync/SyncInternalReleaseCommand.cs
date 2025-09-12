// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Sync;

public sealed class SyncInternalReleaseCommand(
    ILocalGitRepoFactory localGitRepoFactory,
    ILogger<SyncInternalReleaseCommand> logger) : BaseCommand<SyncInternalReleaseOptions>
{
    private readonly ILocalGitRepoFactory _localGitRepoFactory = localGitRepoFactory;
    private readonly ILogger<SyncInternalReleaseCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(SyncInternalReleaseOptions options)
    {
        var repo = _localGitRepoFactory.Create(new NativePath("."));

        var currentBranch = await repo.GetCheckedOutBranchAsync();
        ValidateCurrentBranch(currentBranch, options.SourceBranch);

        _logger.LogInformation(
            "Syncing internal branch for source branch {Branch}", currentBranch);

        var sourceCommit = await repo.GetGitCommitAsync();
        _logger.LogDebug("Source branch {Branch} at commit {Commit}", currentBranch, sourceCommit);

        var internalBranch = $"internal/{currentBranch}";
        var internalCommit = await repo.TryGetShaForBranchAsync(internalBranch);

        if (internalCommit is null)
        {
            await CreateInternalBranchAsync(repo, internalBranch, sourceCommit);
            _logger.LogInformation(
                "Created internal branch {InternalBranch} at {Commit}",
                internalBranch, sourceCommit);
            return 0;
        }

        if (string.Equals(internalCommit, sourceCommit, StringComparison.Ordinal))
        {
            _logger.LogInformation("Internal branch already up to date. Nothing to do.");
            return 0;
        }

        // Determine if fast-forward is possible
        bool canFastForward = await repo.IsAncestorCommit(internalCommit, sourceCommit);
        if (!canFastForward)
        {
            throw new InvalidOperationException("Internal branch has diverged: "
                + $"{internalBranch}@{internalCommit} is not an ancestor of {currentBranch}@{sourceCommit}.");
        }

        await repo.UpdateRefAsync($"refs/heads/{internalBranch}", sourceCommit);
        _logger.LogInformation(
            "Fast-forwarded {InternalBranch} to {NewCommit} (was {OldCommit})",
            internalBranch, sourceCommit, internalCommit);
        return 0;
    }

    private static async Task CreateInternalBranchAsync(ILocalGitRepo repo, string branch, string commit)
    {
        await repo.CreateBranchAsync(branch, overwriteExistingBranch: false);
        var headSha = await repo.GetShaForRefAsync($"refs/heads/{branch}");
        if (!string.Equals(headSha, commit, StringComparison.Ordinal))
        {
            await repo.ExecuteGitCommand("update-ref", $"refs/heads/{branch}", commit);
        }
    }

    private static void ValidateCurrentBranch(string? currentBranch, string? sourceBranch)
    {
        if (string.IsNullOrWhiteSpace(sourceBranch))
        {
            throw new ArgumentException("argument 'source-branch' is required.");
        }

        if (string.IsNullOrWhiteSpace(currentBranch))
        {
            throw new IncorrectBranchException("Cannot determine checked out branch.");
        }

        if (currentBranch == "HEAD")
        {
            throw new DetachedHeadException("Cannot sync while HEAD is detached.");
        }

        if (currentBranch.StartsWith("internal/", StringComparison.Ordinal))
        {
            throw new IncorrectBranchException("Cannot sync while checked out to an internal/* branch.");
        }

        if (currentBranch != sourceBranch)
        {
            throw new IncorrectBranchException($"Specified source branch '{sourceBranch}'"
                + $" does not match the currently checked out branch '{currentBranch}'.");
        }
    }
}
