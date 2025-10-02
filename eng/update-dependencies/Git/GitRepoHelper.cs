// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Git;

/// <remarks>
/// Use <see cref="IGitRepoHelperFactory"/> to instantiate this.
/// </remarks>
internal sealed class GitRepoHelper(
    string remoteRepoUrl,
    ILocalGitRepoHelper localGitRepoHelper,
    IRemoteGitRepoHelper remoteGitRepoHelper,
    ILocalLibGit2Client libGit2Client,
    ILogger<GitRepoHelper> logger
) : IGitRepoHelper
{
    /// <summary>
    /// Use this client for "push" operations only, all other local git
    /// operations should use <see cref="Local"/>.
    /// </summary>
    private readonly ILocalLibGit2Client _libGit2Client = libGit2Client;

    private readonly ILogger<GitRepoHelper> _logger = logger;

    private readonly string _repoUri = remoteRepoUrl;

    private bool _disposed;

    /// <inheritdoc />
    public ILocalGitRepoHelper Local { get; } = localGitRepoHelper;

    /// <inheritdoc />
    public IRemoteGitRepoHelper Remote { get; } = remoteGitRepoHelper;

    /// <inheritdoc />
    public async Task CheckoutRemoteBranchAsync(string branchName)
    {
        var currentBranch = await Local.GetCurrentBranchAsync();
        if (currentBranch == branchName)
        {
            _logger.LogInformation("Already on branch {BranchName}", branchName);
            return;
        }

        var branchExists = await Remote.RemoteBranchExistsAsync(branchName);
        if (!branchExists)
        {
            throw new InvalidBranchException($"Branch '{branchName}' does not exist on remote.");
        }

        await Local.CheckoutRefAsync($"origin/{branchName}");
    }

    /// <inheritdoc />
    public async Task PushLocalBranchAsync(string branchName)
    {
        // Get all remotes and find the one that matches the target repo.
        var remotes = await Local.ListAllRemotesAsync();
        var targetRemote = remotes.First(remote => remote.Url == _repoUri);

        _logger.LogInformation("Pushing branch {BranchName} to remote {RemoteUrl}", branchName, targetRemote.Url);
        await _libGit2Client.Push(Local.LocalPath, branchName, targetRemote.Url);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            Directory.Delete(Local.LocalPath, recursive: true);
        }
        catch (DirectoryNotFoundException)
        {
            // Directory is already gone
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (Exception e)
        {
            _logger.LogWarning(
                "Failed to delete temporary directory {LocalPath}: {ExceptionMessage}",
                Local.LocalPath, e.Message);
        }

        _disposed = true;
    }
}
