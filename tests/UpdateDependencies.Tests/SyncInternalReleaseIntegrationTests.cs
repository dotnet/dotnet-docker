// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;
using Shouldly;

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
                await InitBranchAsync(repo, MainBranch);
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
                await InitBranchAsync(repo, StartingBranch);
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
                await InitBranchAsync(repo, SourceBranch);
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
                await InitBranchAsync(repo, ReleaseBranch);
                await repo.CreateBranchAsync(InternalReleaseBranch);
                await repo.CheckoutAsync(ReleaseBranch);
            }
        );

        var repo = tempRepo.Repo;
        var releaseCommitBefore = await repo.TryGetShaForBranchAsync(ReleaseBranch);
        var internalCommitBefore = await repo.TryGetShaForBranchAsync(InternalReleaseBranch);
        Assert.Equal(releaseCommitBefore, internalCommitBefore);

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
                await InitBranchAsync(repo, MainBranch);
                await repo.CreateBranchAsync(ReleaseBranch);
                await repo.CreateBranchAsync(InternalReleaseBranch);

                // Add a new commit to release branch
                await repo.CheckoutAsync(ReleaseBranch);
                await CreateFileWithCommitAsync(repo, "new-from-release");
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
                await InitBranchAsync(repo, ReleaseBranch);
                await repo.CreateBranchAsync(InternalReleaseBranch);

                // Then create a different new commit on each branch
                await repo.CheckoutAsync(InternalReleaseBranch);
                await CreateFileWithCommitAsync(repo, "new-internal");
                await repo.CheckoutAsync(ReleaseBranch);
                await CreateFileWithCommitAsync(repo, "new-release");
            }
        );

        var options = new SyncInternalReleaseOptions { SourceBranch = ReleaseBranch };
        var command = new SyncInternalReleaseCommand(
            tempRepo.CreateLocalGitRepoFactory(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(options));
    }

    /// <summary>
    /// Create a branch with an initial commit and ensure it is checked out.
    /// </summary>
    private static async Task InitBranchAsync(ILocalGitRepo repo, string name = MainBranch)
    {
        await repo.RunGitCommandAsync(["init"]);
        await repo.CreateBranchAsync(name);
        await CreateInitialCommitAsync(repo);
        await repo.CheckoutAsync(name);
    }

    /// <summary>
    /// Creates an initial commit in the specified repo.
    /// </summary>
    /// <remarks>
    /// A lot of git commands don't work properly unless there's at least one commit in the repo.
    /// </remarks>
    private static async Task CreateInitialCommitAsync(ILocalGitRepo repo)
    {
        var filePath = Path.Join(repo.Path, "file.txt");
        await File.WriteAllTextAsync(filePath, "Hello world");
        await repo.StageAsync([filePath]);
        await repo.CommitAsync("Initial commit", allowEmpty: false);
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

    /// <summary>
    /// Creates a new file with the specified content, stages it, and commits it to the repository.
    /// </summary>
    /// <param name="fileName">If not specified, a random file name will be generated.</param>
    /// <param name="fileContent"></param>
    /// <returns></returns>
    private static async Task CreateFileWithCommitAsync(
        ILocalGitRepo repo,
        string? fileName = null,
        string fileContent = "")
    {
        fileName ??= Path.GetRandomFileName();
        var commitMessage = $"Add {fileName}";
        var filePath = Path.Join(repo.Path, fileName);

        await File.WriteAllTextAsync(filePath, fileContent);
        await repo.StageAsync([filePath]);
        await repo.CommitAsync(commitMessage, allowEmpty: false);
    }
}
