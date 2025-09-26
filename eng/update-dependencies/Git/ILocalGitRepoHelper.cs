// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dotnet.Docker.Git;

public interface ILocalGitRepoHelper
{
    /// <summary>
    /// The local path where the repository is cloned.
    /// </summary>
    string LocalPath { get; }

    /// <summary>
    /// Commits all staged changes to the current branch.
    /// </summary>
    /// <param name="message">The commit message.</param>
    /// <param name="author">The author information (name and email).</param>
    /// <returns>The SHA of the created commit.</returns>
    Task<string> CommitAsync(string message, (string Name, string Email) author);

    /// <summary>
    /// Check out a local branch.
    /// </summary>
    /// <param name="branchName">A branch that exists locally.</param>
    Task CheckoutBranchAsync(string branchName);

    /// <summary>
    /// Create a new local branch. This does not push the branch to the remote.
    /// The branch will be based off of the currently checked out branch.
    /// </summary>
    Task CreateAndCheckoutLocalBranchAsync(string branchName);

    /// <summary>
    /// Gets the name of the currently checked out branch.
    /// </summary>
    /// <returns>The name of the current branch.</returns>
    Task<string> GetCurrentBranchAsync();

    /// <summary>
    /// Get the commit SHA of a branch that exists locally.
    /// </summary>
    /// <returns>
    /// The commit SHA, or null if the branch doesn't exist locally.
    /// </returns>
    Task<string?> GetShaForRefAsync(string gitRef);

    /// <summary>
    /// Adds local files to the index/staging area
    /// </summary>
    /// <param name="paths">The file paths to stage.</param>
    Task StageAsync(params IEnumerable<string> paths);

    /// <summary>
    /// Determines whether <paramref name="ancestorRef"/> is an ancestor of
    /// <paramref name="descendantRef"/> in the local repository.
    /// </summary>
    /// <param name="ancestorRef">Ref that exists locally.</param>
    /// <param name="descendantRef">Ref that exists locally.</param>
    /// <returns>
    /// True if <paramref name="ancestorRef"/>'s current commit is in the
    /// history of <paramref name="descendantRef"/>.
    /// </returns>
    Task<bool> IsAncestorAsync(string ancestorRef, string descendantRef);

    /// <summary>
    /// List all remotes for the local repository.
    /// </summary>
    Task<IEnumerable<GitRemoteInfo>> ListAllRemotesAsync();
}
