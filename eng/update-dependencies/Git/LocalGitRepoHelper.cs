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

public sealed class LocalGitRepoHelper(
    string localPath,
    ILocalGitRepo localGitRepo,
    ILogger<LocalGitRepoHelper> logger
) : ILocalGitRepoHelper, IDisposable
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
    public Task CheckoutBranchAsync(string branchName)
    {
        var branchRef = BranchToRef(branchName);
        _logger.LogInformation("Checking out ref {BranchRef}", branchRef);
        return _localGitRepo.CheckoutAsync(branchRef);
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
    public async Task StageAsync(params IEnumerable<string> paths)
    {
        await _localGitRepo.StageAsync(paths);
    }

    /// <inheritdoc />
    public async Task<string> CommitAsync(string message, (string Name, string Email) author)
    {
        await _localGitRepo.CommitAsync(message, allowEmpty: false, author);
        var commitSha = await _localGitRepo.GetShaForRefAsync("HEAD");
        return commitSha;
    }
    
    /// <inheritdoc />
    public async Task<bool> IsBranchAncestorAsync(string ancestorBranch, string descendantBranch)
    {
        var ancestorCommit = await GetLocalBranchShaAsync(ancestorBranch);
        var descendantCommit = await GetLocalBranchShaAsync(descendantBranch);

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

    public void Dispose()
    {
        _logger.LogInformation("Cleaning up repo in {LocalPath}", LocalPath);

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

    private static string BranchToRef(string branch) => $"refs/heads/{branch}";
}
