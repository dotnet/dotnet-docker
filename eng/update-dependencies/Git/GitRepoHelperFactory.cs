// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Git;

internal interface IGitRepoHelperFactory
{
    /// <summary>
    /// Clones a Git repository from the <paramref name="repoUri"/> into a local
    /// directory, and returns an <see cref="IGitRepoHelper"/> for interacting
    /// with the repository.
    /// </summary>
    /// <param name="repoUri">
    /// The URI of the Git repository to clone.
    /// </param>
    /// <param name="localCloneDir">
    /// The local directory to clone the repository into. If not provided,
    /// defaults to a temporary directory. The caller is responsible for
    /// managing/cleaning up the temporary directory.
    /// </param>
    Task<IGitRepoHelper> CreateAndCloneAsync(string repoUri, string? localCloneDir = null);

    /// <summary>
    /// Creates an <see cref="IGitRepoHelper"/> that points to an existing
    /// local Git repository.
    /// </summary>
    /// <param name="repoUri">
    /// The repository URI for remote operations.
    /// </param>
    /// <param name="localPath">
    /// The path where the repo is already checked out locally. Defaults to the
    /// current working directory.
    /// </param>
    GitRepoHelper CreateFromLocal(string repoUri, string? localPath = null);
}

internal sealed class GitRepoHelperFactory(
    ILogger<GitRepoHelperFactory> logger,
    IGitRepoCloner gitRepoCloner,
    ILocalGitRepoFactory localGitRepoFactory,
    IRemoteGitRepoFactory remoteFactory,
    IServiceProvider serviceProvider
) : IGitRepoHelperFactory
{
    private readonly ILogger<GitRepoHelperFactory> _logger = logger;
    private readonly IGitRepoCloner _gitRepoCloner = gitRepoCloner;
    private readonly ILocalGitRepoFactory _localGitRepoFactory = localGitRepoFactory;
    private readonly IRemoteGitRepoFactory _remoteFactory = remoteFactory;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <inheritdoc/>
    public async Task<IGitRepoHelper> CreateAndCloneAsync(string repoUri, string? localCloneDir = null)
    {
        localCloneDir ??= Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        await _gitRepoCloner.CloneAsync(
            repoUri: repoUri,
            commit: null,
            targetDirectory: localCloneDir,
            checkoutSubmodules: false,
            gitDirectory: null);
        _logger.LogInformation("Cloned '{RepoUri}' to '{LocalCloneDir}'", repoUri, localCloneDir);

        return CreateFromLocal(repoUri, localCloneDir);
    }

    /// <inheritdoc/>
    public GitRepoHelper CreateFromLocal(string repoUri, string? localPath = null)
    {
        localPath ??= Directory.GetCurrentDirectory();

        var localGitRepo = _localGitRepoFactory.Create(new NativePath(localPath));
        var localGitRepoHelper = new LocalGitRepoHelper(
            localPath: localPath,
            localGitRepo: localGitRepo,
            logger: _serviceProvider.GetRequiredService<ILogger<LocalGitRepoHelper>>());

        var remoteGitRepoHelper = new RemoteGitRepoHelper(
            repoUri,
            _remoteFactory.CreateRemoteGitRepo(repoUri),
            localGitRepo: localGitRepo,
            logger: _serviceProvider.GetRequiredService<ILogger<RemoteGitRepoHelper>>());

        return new GitRepoHelper(
            remoteRepoUrl: repoUri,
            localGitRepoHelper: localGitRepoHelper,
            remoteGitRepoHelper: remoteGitRepoHelper,
            libGit2Client: _serviceProvider.GetRequiredService<ILocalLibGit2Client>(),
            logger: _serviceProvider.GetRequiredService<ILogger<GitRepoHelper>>());
    }
}
