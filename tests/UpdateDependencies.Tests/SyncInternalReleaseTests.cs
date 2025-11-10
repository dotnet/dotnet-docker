// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Dotnet.Docker;
using Dotnet.Docker.Git;
using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.Docker.Shared;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class SyncInternalReleaseTests
{
    private const string ReleaseBranch = "release/1";
    private const string InternalReleaseBranch = $"internal/{ReleaseBranch}";
    private const string AzdoOrgName = "test-org";
    private const string AzdoOrgUrl = $"https://dev.azure.com/{AzdoOrgName}";
    private const string AzdoProject = "test-project";
    private const string AzdoRepo = "test-repo";
    private const string RemoteAzdoUrl = $"{AzdoOrgUrl}/{AzdoProject}/_git/{AzdoRepo}";
    private const string LocalRepoPath = "/path/to/local-repo";

    /// <summary>
    /// Default set of command options with some correct values. These can be used in most tests.
    /// The primary thing that changes the behavior of the command is the state of the remote git
    /// repository, which is mocked in the tests.
    /// </summary>
    private static readonly SyncInternalReleaseOptions s_defaultOptions = new()
    {
        AzdoOrganization = AzdoOrgUrl,
        AzdoProject = AzdoProject,
        AzdoRepo = AzdoRepo,
        SourceBranch = ReleaseBranch,
        TargetBranch = InternalReleaseBranch
    };

    /// <summary>
    /// Calling the command with null or whitespace for any of the arguments should fail.
    /// </summary>
    [Fact]
    public async Task WhitespaceArgumentsFails()
    {
        var options = new SyncInternalReleaseOptions
        {
            AzdoOrganization = "   ",
            AzdoProject = "   ",
            AzdoRepo = "   ",
            SourceBranch = "   ",
            TargetBranch = "   "
        };

        var command = CreateCommand();

        await Should.ThrowAsync<ArgumentException>(() => command.ExecuteAsync(options));
    }

    /// <summary>
    /// If the source branch is an internal branch (i.e. "internal/foo"), the command should fail.
    /// Internal branches should always be the target branch of a sync operation, never the source
    /// branch.
    /// </summary>
    [Fact]
    public async Task InternalSourceBranchFails()
    {
        var options = s_defaultOptions with { SourceBranch = "internal/foo" };

        var command = CreateCommand();

        await Should.ThrowAsync<InvalidBranchException>(() => command.ExecuteAsync(options));
    }

    /// <summary>
    /// If we start on a release/* branch and the corresponding internal/release/* branch does not
    /// exist, then it should be created with the same commit as the release branch.
    /// </summary>
    [Fact]
    public async Task CreateInternalBranch()
    {
        var options = s_defaultOptions;

        var repoMock = new Mock<IGitRepoHelper>();
        var repoFactoryMock = new Mock<IGitRepoHelperFactory>();
        repoFactoryMock.Setup(f => f.CreateAndCloneAsync(options.GetAzdoRepoUrl(), null)).ReturnsAsync(repoMock.Object);

        // Setup:
        // Target branch does not exist on remote
        repoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(options.TargetBranch)).ReturnsAsync(false);
        // Source branch exists on remote
        repoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(options.SourceBranch)).ReturnsAsync(true);

        var command = CreateCommand(repoFactory: repoFactoryMock.Object);

        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        // The command should have created the target branch based off of the source branch.
        repoMock.Verify(r => r.Remote.CreateRemoteBranchAsync(options.TargetBranch, options.SourceBranch), Times.Once);
    }

    /// <summary>
    /// If the internal release branch already matches the release branch, then nothing should
    /// happen. The command should not fail.
    /// </summary>
    [Fact]
    public async Task AlreadyUpToDate()
    {
        var options = s_defaultOptions;

        // Strict mock behavior ensures that no extra calls are made that are
        // not explicitly set up in this test.
        var repoMock = new Mock<IGitRepoHelper>(MockBehavior.Strict);
        var repoFactoryMock = new Mock<IGitRepoHelperFactory>();
        repoFactoryMock.Setup(f => f.CreateAndCloneAsync(options.GetAzdoRepoUrl(), null)).ReturnsAsync(repoMock.Object);

        // Setup: Both target and source branches exist on remote.
        repoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(options.TargetBranch)).ReturnsAsync(true);
        repoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(options.SourceBranch)).ReturnsAsync(true);
        // They point to the same commit.
        const string Sha = "0000000000000000000000000000000000000001";
        repoMock.Setup(r => r.Remote.GetRemoteBranchShaAsync(options.TargetBranch)).ReturnsAsync(Sha);
        repoMock.Setup(r => r.Remote.GetRemoteBranchShaAsync(options.SourceBranch)).ReturnsAsync(Sha);
        repoMock.Setup(r => r.Dispose());

        var command = CreateCommand(repoFactory: repoFactoryMock.Object);

        // Command should succeed.
        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);
        // Command should not have done anything else that we didn't expect.
        repoMock.VerifyAll();
    }

    /// <summary>
    /// If the target branch is a direct ancestor of source branch, then submit a pull request
    /// containing the missing commits.
    /// </summary>
    [Fact]
    public async Task FastForward()
    {
        var options = s_defaultOptions;

        var localRepoMock = new Mock<ILocalGitRepoHelper>();
        var remoteRepoMock = new Mock<IRemoteGitRepoHelper>();

        var repoMock = new Mock<IGitRepoHelper>();
        repoMock.Setup(r => r.Local).Returns(localRepoMock.Object);
        repoMock.Setup(r => r.Remote).Returns(remoteRepoMock.Object);

        var repoFactoryMock = new Mock<IGitRepoHelperFactory>();
        repoFactoryMock.Setup(f => f.CreateAndCloneAsync(options.GetAzdoRepoUrl(), null)).ReturnsAsync(repoMock.Object);

        // Setup: Both target and source branches exist on remote.
        repoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(options.TargetBranch)).ReturnsAsync(true);
        repoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(options.SourceBranch)).ReturnsAsync(true);

        // They point to different commits.
        const string OldSha = "0000000000000000000000000000000000000001";
        const string NewSha = "0000000000000000000000000000000000000002";
        remoteRepoMock.Setup(r => r.GetRemoteBranchShaAsync(options.TargetBranch)).ReturnsAsync(OldSha);
        remoteRepoMock.Setup(r => r.GetRemoteBranchShaAsync(options.SourceBranch)).ReturnsAsync(NewSha);

        // The target branch is behind the source branch.
        localRepoMock
            .Setup(r => r.IsAncestorAsync($"origin/{options.TargetBranch}", $"origin/{options.SourceBranch}"))
            .ReturnsAsync(true);

        // Probably going to check the created PR
        remoteRepoMock
            .Setup(r => r.GetPullRequestInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<PullRequest>());

        var command = CreateCommand(repoFactory: repoFactoryMock.Object);

        // Command should succeed.
        var exitCode = await command.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        // Should have created a pull request.
        remoteRepoMock.Verify(r =>
            r.CreatePullRequestAsync(It.Is<PullRequestCreationInfo>(p =>
                p.BaseBranch == options.TargetBranch
                && p.HeadBranch.StartsWith(options.PrBranchPrefix)
                && p.Title.Contains("fast-forward", StringComparison.OrdinalIgnoreCase))
            ),
            Times.Once
        );
    }

    /// <summary>
    /// If the target and source branches have diverged, then a pull request
    /// should be created that resets the target branch to match the source
    /// branch, re-applying any internal .NET version updates that already
    /// existed on the target branch.
    /// </summary>
    [Fact]
    public async Task Sync()
    {
        var options = s_defaultOptions with
        {
            User = "Test User",
            Email = "test@example.com",
            StagingStorageAccount = "dotnetstage"
        };

        // Target and source branches both exist, but at different commits,
        // and the target branch is not an ancestor of the source branch.
        var gitScenario = new GitTestScenario(localRepoPath: LocalRepoPath, remoteRepoUrl: RemoteAzdoUrl)
            .AddRemoteBranch(options.SourceBranch, "0000000000000000000000000000000000000001")
            .AddRemoteBranch(options.TargetBranch, "0000000000000000000000000000000000000002");

        // Actual value of the pull request URL is not important here, just that we allow one to
        // be created and pretend it exists afterwards.
        const string PullRequestUrl = nameof(PullRequestUrl);
        var pullRequestIsCreated = false;
        gitScenario.RepoMock
            .Setup(repo => repo.Remote.CreatePullRequestAsync(It.IsAny<PullRequestCreationInfo>()))
            .Callback(() => pullRequestIsCreated = true)
            .ReturnsAsync(PullRequestUrl);
        gitScenario.RepoMock
            .Setup(repo => repo.Remote.GetPullRequestInfoAsync(PullRequestUrl))
            .ReturnsAsync(() => pullRequestIsCreated
                ? Mock.Of<PullRequest>()
                : throw new Exception($"PR {PullRequestUrl} was not created first"));

        // For this scenario, two internal versions have been checked in to this repo.
        var internalVersions = new Dictionary<DotNetVersion, int>
        {
            { DotNetVersion.Parse("8.0"), 8000000 },
            { DotNetVersion.Parse("10.0"), 1000000 }
        };
        var internalVersionsService = CreateInternalVersionsService(LocalRepoPath, internalVersions);

        var fromStagingPipelineCommandMock = new Mock<ICommand<FromStagingPipelineOptions>>();

        var syncCommand = CreateCommand(
            repoFactory: gitScenario.RepoFactoryMock.Object,
            fromStagingPipelineCommand: fromStagingPipelineCommandMock.Object,
            internalVersionsService: internalVersionsService);

        // Run the command
        var exitCode = await syncCommand.ExecuteAsync(options);
        exitCode.ShouldBe(0);

        // Verify that we re-applied all internal versions by calling the downstream command.
        foreach ((_, int buildId) in internalVersions)
        {
            fromStagingPipelineCommandMock.Verify(command =>
                command.ExecuteAsync(It.Is<FromStagingPipelineOptions>(o =>
                    o.RepoRoot == LocalRepoPath
                    && o.StagingPipelineRunId == buildId
                    && o.StagingStorageAccount == options.StagingStorageAccount
                )),
                Times.Once
            );
        }

        // Verify that we created the expected number of commits.
        // There should be one commit for resetting the target branch to match
        // the source branch, and then one commit for each internal version
        // that was re-applied.
        var numberOfCommits = 1 + internalVersions.Count;
        gitScenario.RepoMock.Verify(
            repo => repo.Local.CommitAsync(
                It.IsAny<string>(),
                It.Is<(string Name, string Email)>(
                    author => author.Name == options.User && author.Email == options.Email
                )
            ),
            Times.Exactly(numberOfCommits)
        );

        // Verify that we pushed the updated target branch.
        gitScenario.RepoMock.Verify(repo => repo.PushLocalBranchAsync(It.IsAny<string>()), Times.Once());

        // Verify that we created exactly one pull request.
        gitScenario.RepoMock.Verify(
            repo => repo.Remote.CreatePullRequestAsync(
                It.Is<PullRequestCreationInfo>(
                    pr => pr.HeadBranch != options.SourceBranch && pr.BaseBranch == options.TargetBranch
                )
            ),
            Times.Once()
        );
    }

    /// <summary>
    /// Helper method to create a <see cref="SyncInternalReleaseCommand"/> instance with optional
    /// mocked dependencies. If dependencies are not provided, default mocks will be used.
    /// </summary>
    private SyncInternalReleaseCommand CreateCommand(
        IGitRepoHelperFactory? repoFactory = null,
        ICommand<FromStagingPipelineOptions>? fromStagingPipelineCommand = null,
        IInternalVersionsService? internalVersionsService = null,
        ILogger<SyncInternalReleaseCommand>? logger = null) =>
            // New parameters should be null by default and initialized with mocks if not specified.
            new(repoFactory ?? Mock.Of<IGitRepoHelperFactory>(),
                fromStagingPipelineCommand ?? Mock.Of<ICommand<FromStagingPipelineOptions>>(),
                internalVersionsService ?? Mock.Of<IInternalVersionsService>(),
                logger ?? Mock.Of<ILogger<SyncInternalReleaseCommand>>());

    /// <summary>
    /// Helper method to create a mock version of <see cref="IInternalVersionsService"/>
    /// </summary>
    private static IInternalVersionsService CreateInternalVersionsService(
        string repoPath, Dictionary<DotNetVersion, int> versions)
    {
        var internalStagingBuilds = new InternalStagingBuilds(versions.ToImmutableDictionary());

        var mock = new Mock<IInternalVersionsService>();
        mock.Setup(s => s.GetInternalStagingBuilds(repoPath))
            .Returns(internalStagingBuilds);

        return mock.Object;
    }

    /// <summary>
    /// Helper class for setting up scenarios on a <see cref="IGitRepoHelper"/> mock.
    /// </summary>
    private class GitTestScenario
    {
        public Mock<IGitRepoHelper> RepoMock { get; } = new();

        /// <summary>
        /// Factory that returns <see cref="RepoMock"/> when
        /// <see cref="IGitRepoHelperFactory.CreateAndCloneAsync"/> is called.
        /// </summary>
        public Mock<IGitRepoHelperFactory> RepoFactoryMock { get; } = new();

        /// <summary>
        /// Create a new scenario for the given local and remote repos.
        /// </summary>
        public GitTestScenario(string localRepoPath, string remoteRepoUrl)
        {
            RepoMock.Setup(r => r.Local.LocalPath).Returns(localRepoPath);
            RepoFactoryMock.Setup(f => f.CreateAndCloneAsync(remoteRepoUrl, null)).ReturnsAsync(RepoMock.Object);
        }

        /// <summary>
        /// Pretend that a branch exists on the remote with the given name and commit SHA.
        /// </summary>
        public GitTestScenario AddRemoteBranch(string branchName, string commitSha)
        {
            RepoMock.Setup(r => r.Remote.RemoteBranchExistsAsync(branchName)).ReturnsAsync(true);
            RepoMock.Setup(r => r.Remote.GetRemoteBranchShaAsync(branchName)).ReturnsAsync(commitSha);
            return this;
        }

        /// <summary>
        /// Declare that <paramref name="ancestorBranch"/> is an ancestor of
        /// <paramref name="descendantBranch"/>.
        /// </summary>
        public GitTestScenario SetAncestor(string descendantBranch, string ancestorBranch)
        {
            RepoMock.Setup(r => r.Local.IsAncestorAsync($"origin/{ancestorBranch}", $"origin/{descendantBranch}"))
                .ReturnsAsync(true);
            return this;
        }
    }
}
