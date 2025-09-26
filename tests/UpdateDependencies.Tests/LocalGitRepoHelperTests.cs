// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Git;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class LocalGitRepoHelperTests
{
    [Fact]
    public async Task IsBranchAncestor()
    {
        const string AncestorBranch = "ancestor";
        const string DescendantBranch = "descendant";

        // example.com is safe for use in testing - it is reserved per RFC2606
        var testIdentity = (Name: "Automation", Email: "test@example.com");

        var repo = await CreateLocalRepoHelperAsync(
            async localRepo =>
            {
                await localRepo.ExecuteGitCommand("init");

                // Create initial commit on main branch
                var initialFilePath = Path.Join(localRepo.Path, "initialFile.txt");
                File.WriteAllText(initialFilePath, "");
                await localRepo.StageAsync([initialFilePath]);
                await localRepo.CommitAsync("Initial commit", allowEmpty: false, author: testIdentity);

                // Create ancestor branch
                await localRepo.CreateBranchAsync(AncestorBranch);

                // Create descendant branch
                await localRepo.CreateBranchAsync(DescendantBranch);
                var newFilePath = Path.Join(localRepo.Path, "newFile.txt");
                File.WriteAllText(newFilePath, "");
                await localRepo.StageAsync([newFilePath]);
                await localRepo.CommitAsync("New commit", allowEmpty: false, author: testIdentity);
            });

        // Validate that repo helper correctly identifies ancestor/descendant relationship
        var isAncestorAsync = await repo.IsAncestorAsync(AncestorBranch, DescendantBranch);
        isAncestorAsync.ShouldBeTrue();
    }

    private static async Task<ILocalGitRepoHelper> CreateLocalRepoHelperAsync(
        Func<ILocalGitRepo, Task> setupLocalAsync)
    {
        var tempRepo = new TempRepo();
        await setupLocalAsync(tempRepo.LocalGitRepo);

        var localRepoHelper = new LocalGitRepoHelper(
            localPath: tempRepo.LocalPath,
            localGitRepo: tempRepo.LocalGitRepo,
            logger: Mock.Of<ILogger<LocalGitRepoHelper>>());

        return localRepoHelper;
    }
}
