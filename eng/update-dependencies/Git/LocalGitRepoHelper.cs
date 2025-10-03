// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Git;

internal sealed class LocalGitRepoHelper(
    string localPath,
    ILocalGitRepo localGitRepo,
    ILogger<LocalGitRepoHelper> logger
) : ILocalGitRepoHelper
{
    private readonly ILocalGitRepo _localGitRepo = localGitRepo;
    private readonly ILogger<LocalGitRepoHelper> _logger = logger;

    /// <inheritdoc />
    public string LocalPath { get; } = localPath;

    /// <inheritdoc />
    public Task<string> GetCurrentBranchAsync() => _localGitRepo.GetCheckedOutBranchAsync();

    /// <inheritdoc />
    public async Task CreateAndCheckoutLocalBranchAsync(string branchName)
    {
        _logger.LogInformation("Creating branch {BranchName}", branchName);
        await _localGitRepo.CreateBranchAsync(branchName, overwriteExistingBranch: false);
    }

    /// <inheritdoc />
    public Task CheckoutRefAsync(string gitRef)
    {
        _logger.LogInformation("Checking out ref {GitRef}", gitRef);
        return _localGitRepo.CheckoutAsync(gitRef);
    }

    /// <inheritdoc />
    public async Task<string?> GetShaForRefAsync(string gitRef)
    {
        try
        {
            var result = await _localGitRepo.GetShaForRefAsync(gitRef);
            return result;
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task StageAsync(params IEnumerable<string> paths)
    {
        await _localGitRepo.StageAsync(paths);
    }

    /// <inheritdoc />
    public async Task<string> CommitAsync(string message, (string Name, string Email) author)
    {
        var gitStatusLines = await _localGitRepo.RunGitCommandAsync(["status"]);
        var gitStatus = string.Join(Environment.NewLine, gitStatusLines);
        _logger.LogInformation(
            """
            Git status:
            {gitStatus}
            """,
            gitStatus);

        await _localGitRepo.CommitAsync(message, allowEmpty: false, author);
        var commitSha = await _localGitRepo.GetShaForRefAsync("HEAD");

        _logger.LogInformation(
            "Created commit {commitSha}, message: '{commitMessage}' ({authorName} <{authorEmail}>)",
            commitSha, message, author.Name, author.Email);

        return commitSha;
    }

    /// <inheritdoc />
    public async Task<bool> IsAncestorAsync(string ancestorRef, string descendantRef)
    {
        var ancestorCommit = await GetShaForRefAsync(ancestorRef);
        var descendantCommit = await GetShaForRefAsync(descendantRef);

        if (ancestorCommit is null || descendantCommit is null)
        {
            return false;
        }

        var isAncestor = await _localGitRepo.IsAncestorCommit(ancestorCommit, descendantCommit);
        return isAncestor;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GitRemoteInfo>> ListAllRemotesAsync()
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

    /// <inheritdoc />
    public Task RestoreAsync(string source)
    {
        _logger.LogInformation("Restoring working tree from source {Source}", source);
        return _localGitRepo.ExecuteGitCommand("restore", "--source", source, ".");
    }
}
