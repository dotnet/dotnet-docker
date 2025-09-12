// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class SyncInternalReleaseIntegrationTests
{
    [Fact]
    public async Task SourceBranchMismatch()
    {
        using var tempRepo = await GitScenarios.CreateSourceBranchMismatchAsync(nameof(SourceBranchMismatch));
        var factory = tempRepo.CreateLocalGitRepoFactory();

        var command = new SyncInternalReleaseCommand(
            localGitRepoFactory: factory,
            logger: Mock.Of<ILogger<SyncInternalReleaseCommand>>()
        );

        var options = new SyncInternalReleaseOptions
        {
            SourceBranch = "release/1"
        };

        await Assert.ThrowsAsync<IncorrectBranchException>(() => command.ExecuteAsync(options));
    }

    internal static class GitScenarios
    {
        public static Task<TempRepo> CreateSourceBranchMismatchAsync(string name)
        {
            return SetupScenarioAsync(name, async repo =>
                {
                    await repo.RunGitCommandAsync(["init"]);
                    await repo.CreateBranchAsync("main");
                    await CreateInitialCommit(repo);
                }
            );
        }

        private static async Task CreateInitialCommit(ILocalGitRepo repo)
        {
            var filePath = Path.Join(repo.Path, "file.txt");
            await File.WriteAllTextAsync(filePath, "Hello world");
            await repo.StageAsync([filePath]);
            await repo.CommitAsync("Initial commit", allowEmpty: false);
        }

        private static async Task<TempRepo> SetupScenarioAsync(string name, Func<ILocalGitRepo, Task> setup)
        {
            var tempRepoPath = Path.Join(Path.GetTempPath(), Path.GetRandomFileName(), name);
            var tempRepo = new TempRepo(tempRepoPath);
            await setup(tempRepo.Repo);
            return tempRepo;
        }
    }
}
