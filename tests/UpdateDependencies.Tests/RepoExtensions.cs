// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;

namespace UpdateDependencies.Tests;

public static class RepoExtensions
{
    /// <summary>
    /// Create a repo with an initial branch and commit and ensure it is checked out.
    /// The repo will be initialized in the <see cref="repo"/>'s path.
    /// </summary>
    public static async Task InitAsync(this ILocalGitRepo repo, string branchName)
    {
        await repo.RunGitCommandAsync(["init"]);
        await repo.CreateBranchAsync(branchName);
        await repo.CreateInitialCommitAsync();
        await repo.CheckoutAsync(branchName);
    }

    /// <summary>
    /// Creates an initial commit in the specified repo.
    /// </summary>
    /// <remarks>
    /// A lot of git commands don't work properly unless there's at least one commit in the repo.
    /// </remarks>
    public static async Task CreateInitialCommitAsync(this ILocalGitRepo repo)
    {
        var filePath = Path.Join(repo.Path, "file.txt");
        await File.WriteAllTextAsync(filePath, "Hello world");
        await repo.StageAsync([filePath]);
        await repo.CommitAsync("Initial commit", allowEmpty: false);
    }

    /// <summary>
    /// Creates a new file with the specified content, stages it, and commits it to the repository.
    /// </summary>
    /// <param name="fileName">If not specified, a random file name will be generated.</param>
    public static async Task CreateFileWithCommitAsync(
        this ILocalGitRepo repo,
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
