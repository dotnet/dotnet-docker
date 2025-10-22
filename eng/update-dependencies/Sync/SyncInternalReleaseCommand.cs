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
internal sealed class SyncInternalReleaseCommand(
    IGitRepoHelperFactory gitRepoHelperFactory,
    ICommand<FromStagingPipelineOptions> updateFromStagingPipeline,
    IInternalVersionsService internalVersionsService,
    ILogger<SyncInternalReleaseCommand> logger
) : BaseCommand<SyncInternalReleaseOptions>
{
    private readonly IGitRepoHelperFactory _gitRepoHelperFactory = gitRepoHelperFactory;
    private readonly ICommand<FromStagingPipelineOptions> _updateFromStagingPipeline = updateFromStagingPipeline;
    private readonly IInternalVersionsService _internalVersionsService = internalVersionsService;
    private readonly ILogger<SyncInternalReleaseCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(SyncInternalReleaseOptions options)
    {
        var remoteUrl = options.GetAzdoRepoUrl();

        // Do not allow syncing starting from an internal branch.
        if (options.SourceBranch.StartsWith("internal/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidBranchException(
                $"The source branch '{options.SourceBranch}' cannot be an internal branch.");
        }

        using var repo = await _gitRepoHelperFactory.CreateAndCloneAsync(remoteUrl);

        // Verify that the source branch exists on the remote.
        var sourceBranchExists = await repo.Remote.RemoteBranchExistsAsync(options.SourceBranch);
        if (!sourceBranchExists)
        {
            throw new InvalidOperationException(
                $"The source branch '{options.SourceBranch}' does not exist on the remote repository.");
        }

        // If the target branch doesn't exist, then create it based on the source branch.
        var targetBranchExists = await repo.Remote.RemoteBranchExistsAsync(options.TargetBranch);
        if (!targetBranchExists)
        {
            await repo.Remote.CreateRemoteBranchAsync(
                newBranch: options.TargetBranch,
                baseBranch: options.SourceBranch);
            return 0;
        }

        var sourceSha = await repo.Remote.GetRemoteBranchShaAsync(options.SourceBranch)
            ?? throw new Exception("Failed to get source branch SHA even though branch exists on remote.");
        var targetSha = await repo.Remote.GetRemoteBranchShaAsync(options.TargetBranch)
            ?? throw new Exception("Failed to get target branch SHA even though branch exists on remote.");

        // If both branches are at the same commit, then there is nothing to do.
        if (sourceSha == targetSha)
        {
            _logger.LogInformation(
                "The source branch '{SourceBranch}' and target branch '{TargetBranch}' are already in sync.",
                options.SourceBranch, options.TargetBranch);
            return 0;
        }

        // Determine if the target branch is an ancestor of the source branch.
        // If so, then we can submit a fast-forward/merge pull request.
        await repo.Remote.FetchAsync();

        // We haven't pulled either branch into our working tree yet, so we
        // reference them with the usual "refs/heads/..." prefix. Assume that
        // both branches exist on the "origin" remote.
        var targetIsAncestorOfSource = await repo.Local.IsAncestorAsync(
            ancestorRef: $"origin/{options.TargetBranch}",
            descendantRef: $"origin/{options.SourceBranch}");

        if (targetIsAncestorOfSource)
        {
            _logger.LogInformation(
                "Branch {TargetBranch} is an ancestor of {SourceBranch} - creating fast-forward PR",
                options.TargetBranch, options.SourceBranch);

            // "ff" here is an abbreviation for "fast-forward" - just want to keep branch names short
            var fastForwardPrBranch = options.CreatePrBranchName("ff");
            await repo.Remote.CreateRemoteBranchAsync(
                newBranch: fastForwardPrBranch,
                baseBranch: options.SourceBranch);

            await repo.Remote.CreatePullRequestAsync(new(
                Title: $"Fast-forward {options.TargetBranch} to {options.SourceBranch}",
                Body: "",
                HeadBranch: fastForwardPrBranch,
                BaseBranch: options.TargetBranch
            ));

            return 0;
        }

        _logger.LogInformation(
            "Branch {TargetBranch} is not an ancestor of {SourceBranch}",
            options.TargetBranch, options.SourceBranch);

        // If we reach this point, then we know that:
        // 1. Both branches exist on the remote.
        // 2. The branches are at different commits.
        // 3. The branches have diverged.
        //
        // Now we need to:
        // 1. Read the internal-versions.txt file from the target branch to determine the
        //    staging pipeline run IDs that were used for each internal .NET version.
        // 2. Reset the target branch to the state of the source branch.
        // 3. Re-apply internal .NET version updates.

        // At this point, we need to verify that we have everything we need to
        // access internal builds, commit changes, and submit a pull request.
        ArgumentException.ThrowIfNullOrWhiteSpace(options.StagingStorageAccount);
        var commitAuthor = options.GetCommitterIdentity();

        // Checkout both branches locally so that we can work with them directly.
        await repo.CheckoutRemoteBranchAsync(options.SourceBranch);
        await repo.CheckoutRemoteBranchAsync(options.TargetBranch);

        var internalBuilds = _internalVersionsService.GetInternalStagingBuilds(repo.Local.LocalPath);

        // Reset the target branch to match the source branch.
        var prBranchName = options.CreatePrBranchName(name: "sync");
        await repo.Local.CreateAndCheckoutLocalBranchAsync(prBranchName);
        await repo.Local.RestoreAsync(source: sourceSha);
        await repo.Local.StageAsync(".");
        await repo.Local.CommitAsync(
            message: $"Reset {options.TargetBranch} to match {options.SourceBranch} commit {sourceSha}",
            author: commitAuthor);

        // Re-apply internal .NET version updates for each recorded staging pipeline run ID.
        foreach (var (dockerfileVersion, stagingPipelineRunId) in internalBuilds.Versions)
        {
            await ApplyInternalBuildAsync(
                options: options,
                localRepo: repo.Local,
                stagingPipelineRunId: stagingPipelineRunId,
                stagingStorageAccount: options.StagingStorageAccount,
                committerIdentity: commitAuthor);
        }

        // Finally, submit a pull request with all of the changes.
        await repo.PushLocalBranchAsync(prBranchName);
        await repo.Remote.CreatePullRequestAsync(new(
            Title: $"Sync {options.TargetBranch} with {options.SourceBranch}",
            Body: "",
            HeadBranch: prBranchName,
            BaseBranch: options.TargetBranch
        ));

        return 0;
    }

    /// <summary>
    /// Apply an internal build by invoking the FromStagingPipelineCommand.
    /// </summary>
    /// <param name="stagingPipelineRunId">
    /// ID of the Azure DevOps pipeline run to get build information from.
    /// </param>
    /// <param name="committerIdentity">
    /// The identity to use when committing changes.
    /// </param>
    private async Task ApplyInternalBuildAsync(
        SyncInternalReleaseOptions options,
        ILocalGitRepoHelper localRepo,
        int stagingPipelineRunId,
        string stagingStorageAccount,
        (string Name, string Email) committerIdentity)
    {
        _logger.LogInformation(
            "Ignore any git-related logging output below, because git "
            + "operations are being managed by a different command.");

        var fromStagingPipelineOptions = new FromStagingPipelineOptions
        {
            RepoRoot = localRepo.LocalPath,
            Internal = true,
            StagingPipelineRunId = stagingPipelineRunId,
            StagingStorageAccount = stagingStorageAccount,
            AzdoOrganization = options.AzdoOrganization,
            AzdoProject = options.AzdoProject,
            AzdoRepo = options.AzdoRepo,
        };

        var exitCode = await _updateFromStagingPipeline.ExecuteAsync(fromStagingPipelineOptions);
        if (exitCode != 0)
        {
            throw new InvalidOperationException(
                $"Failed to apply internal build {stagingPipelineRunId}. Command exited with code {exitCode}.");
        }

        _logger.LogInformation("Finished applying internal build {BuildNumber}", stagingPipelineRunId);

        await localRepo.StageAsync(".");
        await localRepo.CommitAsync($"Update dependencies from build {stagingPipelineRunId}", committerIdentity);
    }
}
