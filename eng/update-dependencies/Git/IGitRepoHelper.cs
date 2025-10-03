// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Dotnet.Docker.Git;

/// <summary>
/// Handles operations that require interactions with both local and remote git repos.
/// </summary>
internal interface IGitRepoHelper : IDisposable
{
    /// <summary>
    /// For any git operations that only affect the local repo.
    /// </summary>
    ILocalGitRepoHelper Local { get; }

    /// <summary>
    /// For any git operations that reach out to the internet.
    /// </summary>
    IRemoteGitRepoHelper Remote { get; }

    /// <summary>
    /// Checks out a remote branch locally.
    /// </summary>
    /// <throws cref="InvalidBranchException">
    /// Thrown if the branch does not exist on the remote.
    /// </throws>
    Task CheckoutRemoteBranchAsync(string branchName);

    /// <summary>
    /// Push a local branch to the specified remote.
    /// </summary>
    /// <param name="branchName">A local branch that already exists.</param>
    Task PushLocalBranchAsync(string branchName);

}
