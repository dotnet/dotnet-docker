// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Git;

internal interface IGitRepoHelperFactory
{
    Task<IGitRepoHelper> CreateAsync(string repoUri);
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

    public async Task<IGitRepoHelper> CreateAsync(string repoUri)
    {
        var localCloneDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        await _gitRepoCloner.CloneAsync(
            repoUri: repoUri,
            commit: null,
            targetDirectory: localCloneDir,
            checkoutSubmodules: false,
            gitDirectory: null);
        _logger.LogInformation("Cloned '{RepoUri}' to '{LocalCloneDir}'", repoUri, localCloneDir);

        var localGitRepo = _localGitRepoFactory.Create(new NativePath(localCloneDir));
        var localGitRepoHelper = new LocalGitRepoHelper(
            localPath: localCloneDir,
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
