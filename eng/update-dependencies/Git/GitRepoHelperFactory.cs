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

public interface IGitRepoHelperFactory
{
    Task<IGitRepoHelper> CreateAsync(string repoUri);
}

public sealed class GitRepoHelperFactory(
    IGitRepoCloner gitRepoCloner,
    ILocalGitRepoFactory localGitRepoFactory,
    IRemoteGitRepoFactory remoteFactory,
    IServiceProvider serviceProvider
) : IGitRepoHelperFactory
{
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

        var localGitRepo = _localGitRepoFactory.Create(new NativePath(localCloneDir));
        var remoteGitRepo = _remoteFactory.CreateRemoteGitRepo(repoUri);

        return new GitRepoHelper(
            remoteRepoUrl: repoUri,
            localPath: localCloneDir,
            localGitRepo: localGitRepo,
            remoteGitRepo: remoteGitRepo,
            libGit2Client: _serviceProvider.GetRequiredService<ILocalLibGit2Client>(),
            logger: _serviceProvider.GetRequiredService<ILogger<GitRepoHelper>>());
    }
}
