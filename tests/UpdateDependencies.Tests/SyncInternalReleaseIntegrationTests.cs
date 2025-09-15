// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class SyncInternalReleaseIntegrationTests
{
    // Common branch name constants to avoid hardcoding literals in tests.
    private const string MainBranch = "main"; // Use sparingly; prefer release branches.
    private const string ReleaseBranch = "release/1";
    private const string InternalReleaseBranch = $"internal/{ReleaseBranch}";
    private const string ArbitraryInternalBranch = "internal/foo";

    /// <summary>
    /// If the argument passed in <see cref="SyncInternalReleaseOptions.SourceBranch"/> does not match the currently
    /// checked out branch, the command should fail.
    /// </summary>
    [Fact]
    public async Task SourceBranchMismatchFails()
    {
        // Start with main branch, and pass in a different branch name.
        using var tempRepo = await SetupScenarioAsync(
            nameof(SourceBranchMismatchFails),
            async repo =>
            {
                await repo.InitAsync(MainBranch);
            }
        );

        var options = new SyncInternalReleaseOptions { SourceBranch = ReleaseBranch };
        var command = new SyncInternalReleaseCommand(
            localGitRepoFactory: tempRepo.CreateLocalGitRepoFactory(),
            logger: Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        await command.ExecuteAsync(options).ShouldThrowAsync<IncorrectBranchException>();
    }

    /// <summary>
    /// If the currently checked out branch is an internal branch (i.e. "internal/foo"), the command should fail.
    /// </summary>
    [Fact]
    public async Task InternalSourceBranchFails()
    {
        const string StartingBranch = ArbitraryInternalBranch;

        using var tempRepo = await SetupScenarioAsync(
            nameof(InternalSourceBranchFails),
            async repo =>
            {
                await repo.InitAsync(StartingBranch);
                await repo.CheckoutAsync(StartingBranch);
            }
        );

        var options = new SyncInternalReleaseOptions { SourceBranch = StartingBranch };
        var command = new SyncInternalReleaseCommand(
            tempRepo.CreateLocalGitRepoFactory(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        await command.ExecuteAsync(options).ShouldThrowAsync<IncorrectBranchException>();
    }

    /// <summary>
    /// If we start on a release/* branch and the corresponding internal/release/* branch does not exist,
    /// then it should be created with the same commit as the release branch.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateInternalBranch()
    {
        const string SourceBranch = ReleaseBranch;
        const string DestBranch = InternalReleaseBranch;

        using var tempRepo = await SetupScenarioAsync(
            nameof(CreateInternalBranch),
            async repo =>
            {
                await repo.InitAsync(SourceBranch);
            }
        );

        var repo = tempRepo.Repo;
        var sourceCommitBefore = await repo.TryGetShaForBranchAsync(SourceBranch);

        var options = new SyncInternalReleaseOptions { SourceBranch = SourceBranch };
        var command = new SyncInternalReleaseCommand(
            tempRepo.CreateLocalGitRepoFactory(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        var sourceCommitAfter = await repo.TryGetShaForBranchAsync(SourceBranch);
        sourceCommitAfter.ShouldBe(sourceCommitBefore);

        var newBranchCommit = await repo.TryGetShaForBranchAsync(DestBranch);
        newBranchCommit.ShouldBe(sourceCommitAfter);
    }

    /// <summary>
    /// If the internal release branch already matches the release branch, then nothing should happen.
    /// The command should not fail. 
    /// </summary>
    [Fact]
    public async Task AlreadyUpToDate()
    {
        using var tempRepo = await SetupScenarioAsync(
            nameof(AlreadyUpToDate),
            async repo =>
            {
                await repo.InitAsync(ReleaseBranch);
                await repo.CreateBranchAsync(InternalReleaseBranch);
                await repo.CheckoutAsync(ReleaseBranch);
            }
        );

        var repo = tempRepo.Repo;
        var releaseCommitBefore = await repo.TryGetShaForBranchAsync(ReleaseBranch);
        var internalCommitBefore = await repo.TryGetShaForBranchAsync(InternalReleaseBranch);
        internalCommitBefore.ShouldBe(releaseCommitBefore);

        var options = new SyncInternalReleaseOptions { SourceBranch = ReleaseBranch };
        var command = new SyncInternalReleaseCommand(
            tempRepo.CreateLocalGitRepoFactory(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        var internalCommitAfter = await repo.TryGetShaForBranchAsync(InternalReleaseBranch);
        internalCommitAfter.ShouldBe(internalCommitBefore);
    }

    /// <summary>
    /// If the internal release branch already exists, and it is behind the release branch, it should be
    /// fast-forwarded to match the release branch.
    /// </summary>
    [Fact]
    public async Task FastForwardInternalBranch()
    {
        using var tempRepo = await SetupScenarioAsync(
            nameof(FastForwardInternalBranch),
            async repo =>
            {
                // Start with release branch and internal release branch in sync
                await repo.InitAsync(MainBranch);
                await repo.CreateBranchAsync(ReleaseBranch);
                await repo.CreateBranchAsync(InternalReleaseBranch);

                // Add a new commit to release branch
                await repo.CheckoutAsync(ReleaseBranch);
                await repo.CreateFileWithCommitAsync("new-from-release");
            }
        );

        var repo = tempRepo.Repo;

        // Validate assumptions - ensure the two branches don't start in sync
        var releaseCommitBefore = await repo.TryGetShaForBranchAsync(ReleaseBranch);
        var internalCommitBefore = await repo.TryGetShaForBranchAsync(InternalReleaseBranch);
        releaseCommitBefore.ShouldNotBe(internalCommitBefore);

        var options = new SyncInternalReleaseOptions { SourceBranch = ReleaseBranch };
        var command = new SyncInternalReleaseCommand(
            tempRepo.CreateLocalGitRepoFactory(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        // Run command and make sure it didn't fail
        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        // Ensure release branch wasn't moved
        var releaseCommitAfter = await repo.TryGetShaForBranchAsync(ReleaseBranch);
        releaseCommitBefore.ShouldBe(releaseCommitAfter);

        // Ensure that internal branch was fast-forwarded to match the release branch
        var internalCommitAfter = await repo.TryGetShaForBranchAsync(InternalReleaseBranch);
        internalCommitAfter.ShouldNotBeNullOrWhiteSpace();
        internalCommitAfter.ShouldBe(releaseCommitAfter);
    }

    /// <summary>
    /// If the source and destination branches have diverged, then the command should fail.
    /// </summary>
    [Fact]
    public async Task DivergingBranchesFails()
    {
        using var tempRepo = await SetupScenarioAsync(
            nameof(DivergingBranchesFails),
            async repo =>
            {
                // Start with both branches in sync
                await repo.InitAsync(ReleaseBranch);
                await repo.CreateBranchAsync(InternalReleaseBranch);

                // Then create a different new commit on each branch
                await repo.CheckoutAsync(InternalReleaseBranch);
                await repo.CreateFileWithCommitAsync("new-internal");
                await repo.CheckoutAsync(ReleaseBranch);
                await repo.CreateFileWithCommitAsync("new-release");
            }
        );

        var options = new SyncInternalReleaseOptions { SourceBranch = ReleaseBranch };
        var command = new SyncInternalReleaseCommand(
            tempRepo.CreateLocalGitRepoFactory(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        await command.ExecuteAsync(options).ShouldThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Creates a temporary Git repository, applies the specified setup actions, and returns the configured repository
    /// instance.
    /// </summary>
    /// <remarks>
    /// The caller is responsible for disposing the returned <see cref="TempRepo"/> instance when it is no longer
    /// needed. The temporary repository is created in the system's temporary directory and will be deleted when the
    /// TempRepo is disposed.
    /// </remarks>
    /// <param name="name">
    /// The name to use for the temporary repository directory. This value is appended to a randomly generated path.
    /// </param>
    /// <param name="setup">
    /// A delegate that performs setup actions on the newly created local Git repository.
    /// </param>
    /// <returns>
    /// A <see cref="TempRepo"/> instance representing the configured repository.
    /// </returns>
    private static async Task<TempRepo> SetupScenarioAsync(string name, Func<ILocalGitRepo, Task> setup)
    {
        var tempRepoPath = Path.Join(Path.GetTempPath(), Path.GetRandomFileName(), name);
        var tempRepo = new TempRepo(tempRepoPath);
        await setup(tempRepo.Repo);
        return tempRepo;
    }
}
