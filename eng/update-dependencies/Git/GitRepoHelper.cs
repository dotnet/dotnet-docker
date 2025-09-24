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
    string LocalPath { get; }

    Task<bool> RemoteBranchExistsAsync(string branchName);
    Task CheckoutRemoteBranchAsync(string branchName);
    Task<string> CommitAsync(string message, (string Name, string Email) author);
    Task CreateAndCheckoutLocalBranchAsync(string branchName);
    Task CreateRemoteBranchAsync(string newBranch, string baseBranch);
    Task<string> CreatePullRequestAsync(PullRequestCreationInfo request);
    Task<string> GetCurrentBranchAsync();
    Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl);
    Task<string?> GetLocalBranchShaAsync(string branch);
    Task<string?> GetRemoteBranchShaAsync(string branch);
    Task PushLocalBranchAsync(string branchName);
    Task StageAsync(params IEnumerable<string> paths);
    Task UpdateRefAsync(string gitRef, string commit);
}

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

    /// <summary>
    /// Keep track of changes made to the local repository that have not been pushed to the remote.
    /// </summary>
    private readonly List<UnsyncedChange> _unsyncedChanges = [];

    private readonly string _repoUri = remoteRepoUrl;

    public string LocalPath { get; } = localPath;

    public Task<string> GetCurrentBranchAsync() => _localGitRepo.GetCheckedOutBranchAsync();

    /// <summary>
    /// Create a new local branch. This does not push the branch to the remote.
    /// The branch will be based off of the currently checked out branch.
    /// </summary>
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
    public Task CreateRemoteBranchAsync(string newBranch, string baseBranch)
    {
        _logger.LogInformation(
            "Creating new remote branch {BranchName} based on {BaseBranch} on remote",
            newBranch, baseBranch);

        return _remoteGitRepo.CreateBranchAsync(_repoUri, newBranch, baseBranch);
    }

    /// <summary>
    /// Checkout a remote branch locally.
    /// </summary>
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

    /// <summary>
    /// Get the commit SHA of a branch that exists locally.
    /// </summary>
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

    public Task<string?> GetRemoteBranchShaAsync(string branch) =>
        _remoteGitRepo.GetLastCommitShaAsync(_repoUri, branch);

    public async Task StageAsync(params IEnumerable<string> paths)
    {
        await _localGitRepo.StageAsync(paths);
        _unsyncedChanges.Add(new UnsyncedChange(ChangeType.StagedFiles, string.Join(", ", paths)));
    }

    public async Task<string> CommitAsync(string message, (string Name, string Email) author)
    {
        await _localGitRepo.CommitAsync(message, allowEmpty: false, author);
        var commitSha = await _localGitRepo.GetShaForRefAsync("HEAD");

        // All staged changes were committed, so they can be removed from the change list
        _unsyncedChanges.RemoveAll(change => change.Type == ChangeType.StagedFiles);
        _unsyncedChanges.Add(new UnsyncedChange(ChangeType.Commit, commitSha));

        return commitSha;
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

    /// <summary>
    /// Push a local branch to the specified remote.
    /// </summary>
    /// <param name="branchName">
    /// A local branch that already exists.
    /// </param>
    /// <param name="remote">
    /// The local branch will be pushed to this remote.
    /// </param>
    public async Task PushLocalBranchAsync(string branchName)
    {
        // Get all remotes and find the one that matches the target repo.
        var remotes = await ListAllRemotesAsync();
        var targetRemote = remotes.First(remote => remote.Url == _repoUri);

        _logger.LogInformation("Pushing branch {BranchName} to remote {RemoteUrl}", branchName, targetRemote.Url);
        await _libGit2Client.Push(LocalPath, branchName, targetRemote.Url);
    }

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

    public Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl) =>
        _remoteGitRepo.GetPullRequestAsync(pullRequestUrl);

    public Task UpdateRefAsync(string gitRef, string commit) =>
        _localGitRepo.ExecuteGitCommand("update-ref", gitRef, commit);

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

    public Task<bool> RemoteBranchExistsAsync(string branchName) =>
        _remoteGitRepo.DoesBranchExistAsync(_repoUri, branchName);

    private sealed record UnsyncedChange(ChangeType Type, string Description);

    private enum ChangeType
    {
        NewBranch,
        StagedFiles,
        Commit,
    }
}

public record GitRemoteInfo(string Name, string Url);
public record PullRequestCreationInfo(string Title, string Body, string BaseBranch, string HeadBranch);
public record ExistingPullRequest();
