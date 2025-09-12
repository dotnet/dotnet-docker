// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

internal static class SyncInternalReleaseScenarios
{
    public static SyncScenario NonReleaseBranch => new(
        LocalState: new RepoState(
            CurrentBranch: "main",
            CurrentCommit: Fake.Sha1(1)
        )
    );
}

public sealed class SyncInternalReleaseTests
{
    [Fact]
    public async Task WrongBranch()
    {
        var scenario = SyncInternalReleaseScenarios.NonReleaseBranch;
        var command = scenario.GetCommand();

        var options = new SyncInternalReleaseOptions()
        {
            SourceBranch = "release/foo"
        };

        await Assert.ThrowsAsync<WrongBranchException>(() => command.ExecuteAsync(options));
    }
}

internal static class SyncScenarioExtensions
{
    public static SyncInternalReleaseCommand GetCommand(this SyncScenario scenario)
    {
        var repo = new Mock<ILocalGitRepo>();
        repo.Setup(r => r.GetCheckedOutBranchAsync())
            .Returns(Task.FromResult(scenario.LocalState.CurrentBranch));
        repo.Setup(r => r.GetGitCommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(scenario.LocalState.CurrentCommit));

        var repoFactory = new Mock<ILocalGitRepoFactory>();
        repoFactory.Setup(f => f.Create(It.IsAny<NativePath>())).Returns(repo.Object);

        return new SyncInternalReleaseCommand(
            repoFactory.Object,
            Mock.Of<ILogger<SyncInternalReleaseCommand>>()
        );
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
