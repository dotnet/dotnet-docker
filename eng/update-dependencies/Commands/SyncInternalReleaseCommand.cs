// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

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
    UpdateDependenciesConfiguration configuration,
    IGitRepoHelperFactory gitRepoHelperFactory,
    ICommand<FromStagingPipelineOptions> updateFromStagingPipeline,
    IInternalVersionsService internalVersionsService,
    IEnvironmentService environmentService,
    ILogger<SyncInternalReleaseCommand> logger
) : ICommand<SyncInternalReleaseOptions>
{
    private readonly IGitRepoHelperFactory _gitRepoHelperFactory = gitRepoHelperFactory;
    private readonly ICommand<FromStagingPipelineOptions> _updateFromStagingPipeline = updateFromStagingPipeline;
    private readonly IInternalVersionsService _internalVersionsService = internalVersionsService;
    private readonly IEnvironmentService _environmentService = environmentService;
    private readonly ILogger<SyncInternalReleaseCommand> _logger = logger;

    public static Command CreateCliCommand(IServiceProvider services)
    {
        Command command = new Command("sync-internal-release", "Sync release/* branch to internal/release/* branch");
        SyncInternalReleaseOptions.AddTo(command);
        command.SetAction((result, _) =>
        {
            SyncInternalReleaseOptions options = SyncInternalReleaseOptions.Bind(result);
            return services.GetRequiredService<SyncInternalReleaseCommand>().ExecuteAsync(options);
        });
        return command;
    }

    public async Task<int> ExecuteAsync(SyncInternalReleaseOptions options)
    {
        string remoteUrl = configuration.AzureDevOps.GetRepoUrl();

        // Do not allow syncing starting from an internal branch.
        if (options.SourceBranch.StartsWith("internal/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidBranchException(
                $"The source branch '{options.SourceBranch}' cannot be an internal branch.");
        }

        // Get git identity if available (required for commits, optional for read-only operations)
        (string, string)? gitIdentity = !string.IsNullOrWhiteSpace(configuration.User) && !string.IsNullOrWhiteSpace(configuration.Email)
            ? configuration.GetCommitterIdentity()
            : null;

        using IGitRepoHelper repo = await _gitRepoHelperFactory.CreateAndCloneAsync(
            remoteUrl,
            gitIdentity: gitIdentity);

        // Verify that the source branch exists on the remote.
        bool sourceBranchExists = await repo.Remote.RemoteBranchExistsAsync(options.SourceBranch);
        if (!sourceBranchExists)
        {
            throw new InvalidOperationException(
                $"The source branch '{options.SourceBranch}' does not exist on the remote repository.");
        }

        // If the target branch doesn't exist, then create it based on the source branch.
        bool targetBranchExists = await repo.Remote.RemoteBranchExistsAsync(options.TargetBranch);
        if (!targetBranchExists)
        {
            await repo.Remote.CreateRemoteBranchAsync(
                newBranch: options.TargetBranch,
                baseBranch: options.SourceBranch);
            return 0;
        }

        string sourceSha = await repo.Remote.GetRemoteBranchShaAsync(options.SourceBranch)
            ?? throw new InvalidOperationException("Failed to get source branch SHA even though branch exists on remote.");
        string targetSha = await repo.Remote.GetRemoteBranchShaAsync(options.TargetBranch)
            ?? throw new InvalidOperationException("Failed to get target branch SHA even though branch exists on remote.");

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
        bool targetIsAncestorOfSource = await repo.Local.IsAncestorAsync(
            ancestorRef: $"origin/{options.TargetBranch}",
            descendantRef: $"origin/{options.SourceBranch}");

        string buildId = _environmentService.GetBuildId() ?? "";

        if (targetIsAncestorOfSource)
        {
            _logger.LogInformation(
                "Branch {TargetBranch} is an ancestor of {SourceBranch} - creating fast-forward PR",
                options.TargetBranch, options.SourceBranch);

            // "ff" here is an abbreviation for "fast-forward" - just want to keep branch names short
            string fastForwardPrBranch = options.CreatePullRequestBranchName(
                "ff",
                buildId);
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
        (string Name, string Email) commitAuthor = configuration.GetCommitterIdentity();

        // Checkout both branches locally so that we can work with them directly.
        await repo.CheckoutRemoteBranchAsync(options.SourceBranch);
        await repo.CheckoutRemoteBranchAsync(options.TargetBranch);

        InternalStageContainers internalBuilds = _internalVersionsService.GetInternalStagingBuilds(repo.Local.LocalPath);

        // Reset the target branch to match the source branch.
        string prBranchName = options.CreatePullRequestBranchName(
            name: "sync",
            buildId: buildId);
        await repo.Local.CreateAndCheckoutLocalBranchAsync(prBranchName);
        await repo.Local.RestoreAsync(source: sourceSha);
        await repo.Local.StageAsync(".");
        await repo.Local.CommitAsync(
            message: $"Reset {options.TargetBranch} to match {options.SourceBranch} commit {sourceSha}",
            author: commitAuthor);

        // Re-apply internal .NET version updates for each recorded stage container.
        foreach ((Shared.DotNetVersion? dockerfileVersion, string? stageContainer) in internalBuilds.Versions)
        {
            await ApplyInternalBuildAsync(
                localRepo: repo.Local,
                stageContainer: stageContainer,
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
    /// <param name="localRepo">The local repository to update.</param>
    /// <param name="stageContainer">
    /// Stage container name (e.g., "stage-1234567") to get build information from.
    /// </param>
    /// <param name="stagingStorageAccount">The storage account containing the staged build.</param>
    /// <param name="committerIdentity">
    /// The identity to use when committing changes.
    /// </param>
    private async Task ApplyInternalBuildAsync(
        ILocalGitRepoHelper localRepo,
        string stageContainer,
        string stagingStorageAccount,
        (string Name, string Email) committerIdentity)
    {
        _logger.LogInformation(
            "Ignore any git-related logging output below, because git "
            + "operations are being managed by a different command.");

        FromStagingPipelineOptions fromStagingPipelineOptions = new FromStagingPipelineOptions
        {
            RepoRoot = localRepo.LocalPath,
            Internal = true,
            StageContainers = stageContainer,
            StagingStorageAccount = stagingStorageAccount,
        };

        int exitCode = await _updateFromStagingPipeline.ExecuteAsync(fromStagingPipelineOptions);
        if (exitCode != 0)
        {
            throw new InvalidOperationException(
                $"Failed to apply internal build {stageContainer}. Command exited with code {exitCode}.");
        }

        _logger.LogInformation("Finished applying internal build {StageContainer}", stageContainer);

        await localRepo.StageAsync(".");
        await localRepo.CommitAsync($"Update dependencies from {stageContainer}", committerIdentity);
    }
}
