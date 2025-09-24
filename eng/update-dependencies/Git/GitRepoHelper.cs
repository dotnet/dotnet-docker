// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Git;

public interface IGitRepoHelper : IDisposable
{
    /// <summary>
    /// The local path where the repository is cloned.
    /// </summary>
    string LocalPath { get; }

    /// <summary>
    /// Checks if a branch exists on the remote repository.
    /// </summary>
    Task<bool> RemoteBranchExistsAsync(string branchName);

    /// <summary>
    /// Checks out a remote branch locally.
    /// </summary>
    Task CheckoutRemoteBranchAsync(string branchName);

    /// <summary>
    /// Commits all staged changes to the current branch.
    /// </summary>
    /// <param name="message">The commit message.</param>
    /// <param name="author">The author information (name and email).</param>
    /// <returns>The SHA of the created commit.</returns>
    Task<string> CommitAsync(string message, (string Name, string Email) author);

    /// <summary>
    /// Create a new local branch. This does not push the branch to the remote.
    /// The branch will be based off of the currently checked out branch.
    /// </summary>
    Task CreateAndCheckoutLocalBranchAsync(string branchName);

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
    /// Gets the name of the currently checked out branch.
    /// </summary>
    /// <returns>The name of the current branch.</returns>
    Task<string> GetCurrentBranchAsync();

    /// <summary>
    /// Gets information about a pull request from its URL.
    /// </summary>
    /// <param name="pullRequestUrl">The URL of the pull request.</param>
    /// <returns>Pull request information.</returns>
    Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl);

    /// <summary>
    /// Get the commit SHA of a branch that exists locally.
    /// </summary>
    /// <returns>
    /// The commit SHA, or null if the branch doesn't exist locally.
    /// </returns>
    Task<string?> GetLocalBranchShaAsync(string branch);

    /// <summary>
    /// Gets the commit SHA of a remote branch.
    /// </summary>
    /// <param name="branch">The name of the remote branch.</param>
    /// <returns>
    /// The commit SHA, or null if the branch doesn't exist on the remote.
    /// </returns>
    Task<string?> GetRemoteBranchShaAsync(string branch);

    /// <summary>
    /// Push a local branch to the specified remote.
    /// </summary>
    /// <param name="branchName">A local branch that already exists.</param>
    Task PushLocalBranchAsync(string branchName);

    /// <summary>
    /// Adds local files to the index/staging area
    /// </summary>
    /// <param name="paths">The file paths to stage.</param>
    Task StageAsync(params IEnumerable<string> paths);
}

/// <remarks>
/// Use <see cref="IGitRepoHelperFactory"/> to instantiate this.
/// </remarks>
public sealed class GitRepoHelper(
    string remoteRepoUrl,
    string localPath,
    ILocalGitRepo localGitRepo,
    IRemoteGitRepo remoteGitRepo,
    ILocalLibGit2Client libGit2Client,
    ILogger<GitRepoHelper> logger
) : IGitRepoHelper
{
    // Process-based git client - use where possible
    private readonly ILocalGitRepo _localGitRepo = localGitRepo;

    // Remote git client - use for pushing, creating pull requests, etc.
    private readonly IRemoteGitRepo _remoteGitRepo = remoteGitRepo;

    // LibGit2Sharp-based git client - use only where _localGitRepo cannot be used, e.g for "push" operations
    private readonly ILocalLibGit2Client _libGit2Client = libGit2Client;

    private readonly ILogger<GitRepoHelper> _logger = logger;

    // Keep track of changes made to the local repository that have not been pushed to the remote.
    private readonly List<UnsyncedChange> _unsyncedChanges = [];

    private readonly string _repoUri = remoteRepoUrl;

    /// <inheritdoc />
    public string LocalPath { get; } = localPath;

    /// <inheritdoc />
    public Task<string> GetCurrentBranchAsync() => _localGitRepo.GetCheckedOutBranchAsync();

    /// <inheritdoc />
    public async Task CreateAndCheckoutLocalBranchAsync(string branchName)
    {
        var branchExists = await _remoteGitRepo.DoesBranchExistAsync(_repoUri, branchName);
        if (branchExists)
        {
            _logger.LogWarning(
                "Branch {BranchName} already exists on remote, creating local branch anyways",
                branchName);
        }

        _logger.LogInformation("Creating branch {BranchName} locally", branchName);
        await _localGitRepo.CreateBranchAsync(branchName, overwriteExistingBranch: false);

        _unsyncedChanges.Add(new UnsyncedChange(ChangeType.NewBranch, branchName));
    }

    /// <inheritdoc />
    public Task CreateRemoteBranchAsync(string newBranch, string baseBranch)
    {
        _logger.LogInformation(
            "Creating new remote branch {BranchName} based on {BaseBranch} on remote",
            newBranch, baseBranch);

        return _remoteGitRepo.CreateBranchAsync(_repoUri, newBranch, baseBranch);
    }

    /// <inheritdoc />
    public async Task CheckoutRemoteBranchAsync(string branchName)
    {
        var currentBranch = await _localGitRepo.GetCheckedOutBranchAsync();
        if (currentBranch == branchName)
        {
            _logger.LogInformation("Already on branch {BranchName}", branchName);
            return;
        }

        var branchExists = await _remoteGitRepo.DoesBranchExistAsync(_repoUri, branchName);
        if (!branchExists)
        {
            throw new InvalidBranchException($"Branch '{branchName}' does not exist on remote.");
        }

        await _localGitRepo.CheckoutAsync(branchName);
    }

    /// <inheritdoc />
    public async Task<string?> GetLocalBranchShaAsync(string branch)
    {
        try
        {
            var result = await _localGitRepo.GetShaForRefAsync($"refs/heads/{branch}");
            return result;
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc />
    public Task<string?> GetRemoteBranchShaAsync(string branch) =>
        _remoteGitRepo.GetLastCommitShaAsync(_repoUri, branch);

    /// <inheritdoc />
    public async Task StageAsync(params IEnumerable<string> paths)
    {
        await _localGitRepo.StageAsync(paths);
        _unsyncedChanges.Add(new UnsyncedChange(ChangeType.StagedFiles, string.Join(", ", paths)));
    }

    /// <inheritdoc />
    public async Task<string> CommitAsync(string message, (string Name, string Email) author)
    {
        await _localGitRepo.CommitAsync(message, allowEmpty: false, author);
        var commitSha = await _localGitRepo.GetShaForRefAsync("HEAD");

        // All staged changes were committed, so they can be removed from the change list
        _unsyncedChanges.RemoveAll(change => change.Type == ChangeType.StagedFiles);
        _unsyncedChanges.Add(new UnsyncedChange(ChangeType.Commit, commitSha));

        return commitSha;
    }

    /// <inheritdoc />
    public async Task PushLocalBranchAsync(string branchName)
    {
        // Get all remotes and find the one that matches the target repo.
        var remotes = await ListAllRemotesAsync();
        var targetRemote = remotes.First(remote => remote.Url == _repoUri);

        _logger.LogInformation("Pushing branch {BranchName} to remote {RemoteUrl}", branchName, targetRemote.Url);
        await _libGit2Client.Push(LocalPath, branchName, targetRemote.Url);
    }

    /// <inheritdoc />
    public Task<string> CreatePullRequestAsync(PullRequestCreationInfo request)
    {
        var darcPullRequest = new PullRequest
        {
            Title = request.Title,
            Description = request.Body,
            BaseBranch = request.BaseBranch,
            HeadBranch = request.HeadBranch,
        };

        return _remoteGitRepo.CreatePullRequestAsync(_repoUri, darcPullRequest);
    }

    /// <inheritdoc />
    public Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl) =>
        _remoteGitRepo.GetPullRequestAsync(pullRequestUrl);

    /// <inheritdoc />
    public Task<bool> RemoteBranchExistsAsync(string branchName) =>
        _remoteGitRepo.DoesBranchExistAsync(_repoUri, branchName);

    public void Dispose()
    {
        _logger.LogInformation("Cleaning up repo in {LocalPath}", LocalPath);

        if (_unsyncedChanges.Count > 0)
        {
            _logger.LogWarning(
                "There are unsynced changes in the repository for {RepoUri}: {UnsyncedChanges}",
                _repoUri,
                string.Join(", ", _unsyncedChanges.Select(change => change.ToString()))
            );
        }

        try
        {
            if (Directory.Exists(LocalPath))
            {
                Directory.Delete(LocalPath, recursive: true);
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(
                "Failed to delete temporary directory {LocalPath}: {ExceptionMessage}",
                LocalPath, e.Message);
        }
    }

    private async Task<IEnumerable<GitRemoteInfo>> ListAllRemotesAsync()
    {
        var remotesOutput = await _localGitRepo.ExecuteGitCommand("remote", "-v");

        /* Example output of `git remote -v`:
        origin  https://github.com/dotnet/runtime (fetch)
        origin  https://github.com/dotnet/runtime (push)
        */

        return remotesOutput.GetOutputLines()
            .Select(line =>
                line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(parts => new GitRemoteInfo(parts[0], parts[1]))
            // There are typically two entries per remote (fetch and push); we only want one
            .Distinct();
    }

    private sealed record UnsyncedChange(ChangeType Type, string Description);

    private enum ChangeType
    {
        NewBranch,
        StagedFiles,
        Commit,
    }
}

/// <summary>
/// Represents information about a Git remote repository.
/// </summary>
/// <param name="Name">The name of the remote (e.g., "origin").</param>
/// <param name="Url">The URL of the remote repository.</param>
public record GitRemoteInfo(string Name, string Url);

/// <summary>
/// Contains information needed to create a pull request.
/// </summary>
/// <param name="Title">The title of the pull request.</param>
/// <param name="Body">The description/body of the pull request.</param>
/// <param name="BaseBranch">The target branch that changes will be merged into.</param>
/// <param name="HeadBranch">The source branch containing the changes.</param>
public record PullRequestCreationInfo(string Title, string Body, string BaseBranch, string HeadBranch);

/// <summary>
/// Represents an existing pull request.
/// </summary>
public record ExistingPullRequest();
