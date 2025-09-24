// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Git;
using Dotnet.Docker.Sync;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class SyncInternalReleaseTests
{
    private const string MainBranch = "main";
    private const string ReleaseBranch = "release/1";
    private const string InternalReleaseBranch = $"internal/{ReleaseBranch}";
    private const string ArbitraryInternalBranch = "internal/foo";

    private const string AzdoOrg = "test-org";
    private const string AzdoProject = "test-project";
    private const string AzdoRepo = "test-repo";
    private const string RemoteAzdoUrl = $"https://dev.azure.com/{AzdoOrg}/{AzdoProject}/_git/{AzdoRepo}";

    private static readonly SyncInternalReleaseOptions DefaultOptions = new()
    {
        RemoteUrl = RemoteAzdoUrl,
        SourceBranch = ReleaseBranch,
        TargetBranch = InternalReleaseBranch
    };

    /// <summary>
    /// If the currently checked out branch is an internal branch (i.e. "internal/foo"), the command should fail.
    /// </summary>
    [Fact]
    public async Task InternalSourceBranchFails()
    {
        var options = DefaultOptions with { SourceBranch = ArbitraryInternalBranch };

        var command = new SyncInternalReleaseCommand(
            Mock.Of<IGitRepoHelperFactory>(),
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        await Should.ThrowAsync<IncorrectBranchException>(() => command.ExecuteAsync(options));
    }

    /// <summary>
    /// If we start on a release/* branch and the corresponding internal/release/* branch does not exist,
    /// then it should be created with the same commit as the release branch.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateInternalBranch()
    {
        var options = DefaultOptions;

        var repoMock = new Mock<IGitRepoHelper>();
        var repoFactoryMock = new Mock<IGitRepoHelperFactory>();
        repoFactoryMock.Setup(f => f.CreateAsync(options.RemoteUrl)).ReturnsAsync(repoMock.Object);

        // Setup:
        // Target branch does not exist on remote
        repoMock.Setup(r => r.RemoteBranchExistsAsync(options.TargetBranch)).ReturnsAsync(false);
        // Source branch exists on remote
        repoMock.Setup(r => r.RemoteBranchExistsAsync(options.SourceBranch)).ReturnsAsync(true);

        var command = new SyncInternalReleaseCommand(
            repoFactoryMock.Object,
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        // The command should have created the target branch based off of the source branch.
        repoMock.Verify(r => r.CreateRemoteBranchAsync(options.TargetBranch, options.SourceBranch), Times.Once);
    }

    /// <summary>
    /// If the internal release branch already matches the release branch, then nothing should happen.
    /// The command should not fail.
    /// </summary>
    [Fact]
    public async Task AlreadyUpToDate()
    {
        var options = DefaultOptions;

        // Strict mock behavior ensures that no extra calls are made that are
        // not explicitly set up in this test.
        var repoMock = new Mock<IGitRepoHelper>(MockBehavior.Strict);
        var repoFactoryMock = new Mock<IGitRepoHelperFactory>();
        repoFactoryMock.Setup(f => f.CreateAsync(options.RemoteUrl)).ReturnsAsync(repoMock.Object);

        // Setup: Both target and source branches exist on remote.
        repoMock.Setup(r => r.RemoteBranchExistsAsync(options.TargetBranch)).ReturnsAsync(true);
        repoMock.Setup(r => r.RemoteBranchExistsAsync(options.SourceBranch)).ReturnsAsync(true);
        // They point to the same commit.
        const string Sha = "0000000000000000000000000000000000000001";
        repoMock.Setup(r => r.GetRemoteBranchShaAsync(options.TargetBranch)).ReturnsAsync(Sha);
        repoMock.Setup(r => r.GetRemoteBranchShaAsync(options.SourceBranch)).ReturnsAsync(Sha);
        repoMock.Setup(r => r.Dispose());

        var command = new SyncInternalReleaseCommand(
            repoFactoryMock.Object,
            Mock.Of<ILogger<SyncInternalReleaseCommand>>());

        // Command should succeed.
        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);
        // Command should not have done anything else that we didn't expect.
        repoMock.VerifyAll();
    }

    /// <summary>
    /// If the internal release branch already exists, and it is behind the release branch, it should be
    /// fast-forwarded to match the release branch.
    /// </summary>
    // [Fact]
    public async Task FastForwardInternalBranch()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// If the source and destination branches have diverged, then the command should fail.
    /// </summary>
    // [Fact]
    public async Task DivergingBranchesFails()
    {
        throw new NotImplementedException();
    }
}
