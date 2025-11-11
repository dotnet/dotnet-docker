// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;

namespace Dotnet.Docker.Git;

internal interface IRemoteGitRepoHelper
{
    /// <summary>
    /// The URL of the remote repository.
    /// </summary>
    string RemoteUrl { get; }

    /// <summary>
    /// Checks if a branch exists on the remote repository.
    /// </summary>
    Task<bool> RemoteBranchExistsAsync(string branchName);

    /// <summary>
    /// Create a new branch in the remote repository. The <see cref="newBranch"/>
    /// will be based off of the <see cref="baseBranch"/>.
    /// </summary>
    /// <param name="newBranch">
    /// This branch will be created on the remote repository.
    /// </param>
    /// <param name="baseBranch">
    /// The base branch must already exist on the remote repository.
    /// </param>
    Task CreateRemoteBranchAsync(string newBranch, string baseBranch);

    /// <summary>
    /// Create a pull request in the remote repository.
    /// </summary>
    /// <param name="request">
    /// Information about the pull request to create. The <see cref="PullRequestCreationInfo.HeadBranch"/> and
    /// <see cref="PullRequestCreationInfo.BaseBranch"/> must both already exist on the remote.
    /// </param>
    /// <returns>
    /// Pull request API URL.
    /// </returns>
    Task<string> CreatePullRequestAsync(PullRequestCreationInfo request);

    /// <summary>
    /// Gets information about a pull request from its URL.
    /// </summary>
    /// <param name="pullRequestUrl">The URL of the pull request.</param>
    /// <returns>Pull request information.</returns>
    Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl);

    /// <summary>
    /// Gets the commit SHA of a remote branch.
    /// </summary>
    /// <param name="branch">The name of the remote branch.</param>
    /// <returns>
    /// The commit SHA, or null if the branch doesn't exist on the remote.
    /// </returns>
    Task<string?> GetRemoteBranchShaAsync(string branch);

    /// <summary>
    /// Fetches updates from the remote repository.
    /// </summary>
    Task FetchAsync();
}
