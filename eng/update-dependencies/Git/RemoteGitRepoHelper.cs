// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Git;

internal sealed class RemoteGitRepoHelper(
    string remoteRepoUrl,
    IRemoteGitRepo remoteGitRepo,
    ILocalGitRepo localGitRepo,
    ILogger<RemoteGitRepoHelper> logger
) : IRemoteGitRepoHelper
{
    private readonly IRemoteGitRepo _remoteGitRepo = remoteGitRepo;
    private readonly ILocalGitRepo _localGitRepo = localGitRepo;
    private readonly ILogger<RemoteGitRepoHelper> _logger = logger;
    private readonly string _repoUri = remoteRepoUrl;

    /// <inheritdoc />
    public Task CreateRemoteBranchAsync(string newBranch, string baseBranch)
    {
        _logger.LogInformation(
            "Creating new remote branch {BranchName} based on {BaseBranch} on remote",
            newBranch, baseBranch);
        return _remoteGitRepo.CreateBranchAsync(_repoUri, newBranch, baseBranch);
    }

    /// <inheritdoc />
    public Task<string?> GetRemoteBranchShaAsync(string branch) =>
        _remoteGitRepo.GetLastCommitShaAsync(_repoUri, branch);

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

        _logger.LogInformation(
            "Creating pull request '{PrTitle}' from branch {PrHeadBranch} to {PrBaseBranch}",
            darcPullRequest.Title, darcPullRequest.HeadBranch, darcPullRequest.BaseBranch);

        return _remoteGitRepo.CreatePullRequestAsync(_repoUri, darcPullRequest);
    }

    /// <inheritdoc />
    public Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl) =>
        _remoteGitRepo.GetPullRequestAsync(pullRequestUrl);

    /// <inheritdoc />
    public Task<bool> RemoteBranchExistsAsync(string branchName) =>
        _remoteGitRepo.DoesBranchExistAsync(_repoUri, branchName);

    /// <inheritdoc />
    public Task FetchAsync() => _localGitRepo.FetchAllAsync([_repoUri]);
}
