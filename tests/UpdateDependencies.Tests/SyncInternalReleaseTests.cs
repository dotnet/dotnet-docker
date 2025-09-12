// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

internal static class SyncInternalReleaseScenarios
{
    public static SyncScenario MainBranch => new(
        LocalState: new RepoState(
            CurrentBranch: "main",
            CurrentCommit: Fake.Sha1(1)
        )
    );

    public static SyncScenario InternalBranch => new(
        LocalState: new RepoState(
            CurrentBranch: "internal/main",
            CurrentCommit: Fake.Sha1(1)
        )
    );
}

public sealed class SyncInternalReleaseTests
{
    [Fact]
    public async Task SourceBranchMismatch()
    {
        var scenario = SyncInternalReleaseScenarios.MainBranch;
        var harness = scenario.CreateHarness();

        var options = new SyncInternalReleaseOptions { SourceBranch = "feature/other" };
        await Assert.ThrowsAsync<IncorrectBranchException>(() => harness.Command.ExecuteAsync(options));
    }

    [Fact]
    public async Task RejectInternalBranchAsSource()
    {
        var scenario = SyncInternalReleaseScenarios.InternalBranch;
        var harness = scenario.CreateHarness();

        var options = new SyncInternalReleaseOptions { SourceBranch = "internal/main" };
        await Assert.ThrowsAsync<IncorrectBranchException>(() => harness.Command.ExecuteAsync(options));
    }

    [Fact]
    public async Task CreateInternalBranch()
    {
        var scenario = SyncInternalReleaseScenarios.MainBranch;
        var harness = scenario.CreateHarness();

        // internal branch missing
        harness.RepoMock.Setup(r => r.GetShaForRefAsync("refs/heads/internal/main"))
            .ThrowsAsync(new InvalidOperationException("ref not found"));

        // When creating branch, repo.CreateBranchAsync should be called then update-ref if needed.
        harness.RepoMock.Setup(r => r.CreateBranchAsync("internal/main", false))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // After creation, simulate branch currently at a different sha to trigger update-ref.
        harness.RepoMock.SetupSequence(r => r.GetShaForRefAsync("refs/heads/internal/main"))
            .ThrowsAsync(new InvalidOperationException("ref not found")) // existence check pre-create
            .ReturnsAsync(Fake.Sha1(99)); // post-create head mismatch

        harness.RepoMock.Setup(r => r.ExecuteGitCommand(It.Is<string[]>(a => a.Length > 0 && a[0] == "update-ref")))
            .ReturnsAsync(new ProcessExecutionResult() { ExitCode = 0 });

        var options = new SyncInternalReleaseOptions { SourceBranch = "main" };
        var exit = await harness.Command.ExecuteAsync(options);

        Assert.Equal(0, exit);
        harness.RepoMock.Verify();
    }

    [Fact]
    public async Task AlreadyUpToDate()
    {
        var scenario = SyncInternalReleaseScenarios.MainBranch;
        var harness = scenario.CreateHarness();

        harness.RepoMock.Setup(r => r.GetShaForRefAsync("refs/heads/internal/main"))
            .ReturnsAsync(scenario.LocalState.CurrentCommit);

        var options = new SyncInternalReleaseOptions { SourceBranch = "main" };
        var exit = await harness.Command.ExecuteAsync(options);
        Assert.Equal(0, exit);
        harness.RepoMock.Verify(r => r.ExecuteGitCommand(It.IsAny<string[]>()), Times.Never);
    }

    [Fact]
    public async Task FastForwardInternalBranch()
    {
        var baseScenario = SyncInternalReleaseScenarios.MainBranch;
        var scenario = new SyncScenario(
            LocalState: baseScenario.LocalState with { CurrentCommit = Fake.Sha1(2) }
        );
        var harness = scenario.CreateHarness();

        var oldCommit = Fake.Sha1(1);
        harness.RepoMock.Setup(r => r.GetShaForRefAsync("refs/heads/internal/main"))
            .ReturnsAsync(oldCommit);

        harness.RepoMock.Setup(r => r.IsAncestorCommit(oldCommit, Fake.Sha1(2))).ReturnsAsync(true);

        harness.RepoMock.Setup(r => r.ExecuteGitCommand(It.Is<string[]>(a => a.Length > 0 && a[0] == "update-ref")))
            .ReturnsAsync(new ProcessExecutionResult() { ExitCode = 0 })
            .Verifiable();

        var options = new SyncInternalReleaseOptions { SourceBranch = "main" };
        var exit = await harness.Command.ExecuteAsync(options);
        Assert.Equal(0, exit);
        harness.RepoMock.Verify();
    }

    [Fact]
    public async Task DivergenceThrows()
    {
        var baseScenario = SyncInternalReleaseScenarios.MainBranch;
        var scenario = new SyncScenario(
            LocalState: baseScenario.LocalState with { CurrentCommit = Fake.Sha1(3) }
        );
        var harness = scenario.CreateHarness();

        var internalCommit = Fake.Sha1(2);
        harness.RepoMock.Setup(r => r.GetShaForRefAsync("refs/heads/internal/main"))
            .ReturnsAsync(internalCommit);
        harness.RepoMock.Setup(r => r.IsAncestorCommit(internalCommit, Fake.Sha1(3)))
            .ReturnsAsync(false);

        var options = new SyncInternalReleaseOptions { SourceBranch = "main" };
        await Assert.ThrowsAsync<InvalidOperationException>(() => harness.Command.ExecuteAsync(options));
    }
}

internal sealed class SyncHarness
{
    public required Mock<ILocalGitRepo> RepoMock { get; init; }
    public required SyncInternalReleaseCommand Command { get; init; }
}

internal static class SyncScenarioExtensions
{
    public static SyncHarness CreateHarness(this SyncScenario scenario)
    {
        var repo = new Mock<ILocalGitRepo>();
        repo.Setup(r => r.GetCheckedOutBranchAsync())
            .Returns(Task.FromResult(scenario.LocalState.CurrentBranch));
        repo.Setup(r => r.GetGitCommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(scenario.LocalState.CurrentCommit));

        var repoFactory = new Mock<ILocalGitRepoFactory>();
        repoFactory.Setup(f => f.Create(It.IsAny<NativePath>())).Returns(repo.Object);

        return new SyncHarness
        {
            RepoMock = repo,
            Command = new SyncInternalReleaseCommand(
                repoFactory.Object,
                Mock.Of<ILogger<SyncInternalReleaseCommand>>()
            )
        };
    }
}

internal static class Fake
{
    /// <summary>
    /// Create a SHA-1 looking hash that starts with the ID and pads the
    /// remaining characters with zeros.
    /// </summary>
    /// <param name="id">
    /// The SHA-1 will start with this ID.
    /// </param>
    /// <returns>
    /// 40-character long SHA-1 looking string.
    /// </returns>
    public static string Sha1(int id) => id.ToString().PadRight(40, '0');
}

internal sealed record SyncScenario(
    RepoState LocalState,
    RepoState? RemoteState = null
);

internal record RepoState(
    string CurrentBranch,
    string CurrentCommit
);
