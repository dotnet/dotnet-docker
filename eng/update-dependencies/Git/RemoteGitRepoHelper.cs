// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

    /// <inheritdoc />
    public string RemoteUrl { get; } = remoteRepoUrl;

    /// <inheritdoc />
    public Task CreateRemoteBranchAsync(string newBranch, string baseBranch)
    {
        _logger.LogInformation(
            "Creating new remote branch {BranchName} based on {BaseBranch} on remote",
            newBranch, baseBranch);
        return _remoteGitRepo.CreateBranchAsync(RemoteUrl, newBranch, baseBranch);
    }

    /// <inheritdoc />
    public Task<string?> GetRemoteBranchShaAsync(string branch) =>
        _remoteGitRepo.GetLastCommitShaAsync(RemoteUrl, branch);

    /// <inheritdoc />
    public async Task<string> CreatePullRequestAsync(PullRequestCreationInfo request)
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

        var pullRequestApiUrl = await _remoteGitRepo.CreatePullRequestAsync(RemoteUrl, darcPullRequest);
        var pullRequestInfo = await GetPullRequestInfoAsync(pullRequestApiUrl);

        _logger.LogInformation(
            "Created pull request {PullRequestTitle} at {PullRequestUrl}",
            pullRequestInfo.Title, pullRequestApiUrl);

        return pullRequestApiUrl;
    }

    /// <inheritdoc />
    public Task<PullRequest> GetPullRequestInfoAsync(string pullRequestUrl) =>
        _remoteGitRepo.GetPullRequestAsync(pullRequestUrl);

    /// <inheritdoc />
    public Task<bool> RemoteBranchExistsAsync(string branchName) =>
        _remoteGitRepo.DoesBranchExistAsync(RemoteUrl, branchName);

    /// <inheritdoc />
    public Task FetchAsync() => _localGitRepo.FetchAllAsync([RemoteUrl]);
}
