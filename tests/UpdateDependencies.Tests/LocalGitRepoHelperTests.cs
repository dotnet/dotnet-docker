// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class LocalGitRepoHelperTests
{
    [Fact]
    public async Task IsBranchAncestor()
    {
        const string ancestorBranch = "ancestor";
        const string descendantBranch = "descendant";

        // example.com is safe for use in testing - it is reserved per RFC2606
        (string Name, string Email) authorIdentity = (Name: "Test Author", Email: "test@example.com");
        (string name, string email) = ("Test Committer", "test@example.com");

        ILocalGitRepoHelper repo = await CreateLocalRepoHelperAsync(
            async localRepo =>
            {
                await localRepo.ExecuteGitCommand("init");

                // Commit author and committer identity are separate concepts.
                // `author:` argument below sets the author identity for the commit, but we also
                // need to set the committer identity beforehand.
                await localRepo.ExecuteGitCommand("config", "user.name", name);
                await localRepo.ExecuteGitCommand("config", "user.email", email);

                // Create initial commit on main branch
                string initialFilePath = Path.Join(localRepo.Path, "initialFile.txt");
                File.WriteAllText(initialFilePath, "");
                await localRepo.StageAsync([initialFilePath]);
                await localRepo.CommitAsync("Initial commit", allowEmpty: false, author: authorIdentity);

                // Create ancestor branch
                await localRepo.CreateBranchAsync(ancestorBranch);

                // Create descendant branch
                await localRepo.CreateBranchAsync(descendantBranch);
                string newFilePath = Path.Join(localRepo.Path, "newFile.txt");
                File.WriteAllText(newFilePath, "");
                await localRepo.StageAsync([newFilePath]);
                await localRepo.CommitAsync("New commit", allowEmpty: false, author: authorIdentity);
            });

        // Validate that repo helper correctly identifies ancestor/descendant relationship
        bool isAncestorAsync = await repo.IsAncestorAsync(ancestorBranch, descendantBranch);
        isAncestorAsync.ShouldBeTrue();
    }

    private static async Task<ILocalGitRepoHelper> CreateLocalRepoHelperAsync(
        Func<ILocalGitRepo, Task> setupLocalAsync)
    {
        TempRepo tempRepo = new TempRepo();
        await setupLocalAsync(tempRepo.LocalGitRepo);

        LocalGitRepoHelper localRepoHelper = new LocalGitRepoHelper(
            localPath: tempRepo.LocalPath,
            localGitRepo: tempRepo.LocalGitRepo,
            logger: Mock.Of<ILogger<LocalGitRepoHelper>>());

        return localRepoHelper;
    }
}
