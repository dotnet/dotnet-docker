// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Docker.Git;

public interface IRemoteGitRepoFactory
{
    IRemoteGitRepo CreateRemoteGitRepo(string repoUrl);
}

public sealed class RemoteGitRepoFactory(IServiceProvider serviceProvider) : IRemoteGitRepoFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IRemoteGitRepo CreateRemoteGitRepo(string remoteUrl)
    {
        var gitRemoteKind = ParseGitRemoteFromUrl(remoteUrl);

        return gitRemoteKind switch
        {
            GitRemote.GitHub => _serviceProvider.GetRequiredKeyedService<IRemoteGitRepo>(GitRemote.GitHub),
            GitRemote.AzureDevOps => _serviceProvider.GetRequiredKeyedService<IRemoteGitRepo>(GitRemote.AzureDevOps),
            _ => throw new NotSupportedException($"The git remote for the repository '{remoteUrl}' is not supported."),
        };
    }

    private static GitRemote ParseGitRemoteFromUrl(string repoUrl)
    {
        if (!Uri.TryCreate(repoUrl, UriKind.RelativeOrAbsolute, out Uri? parsedUri))
        {
            return GitRemote.None;
        }

        return parsedUri switch
        {
            { Host: "github.com" } => GitRemote.GitHub,
            { Host: var host } when host is "dev.azure.com"
                || host.EndsWith("visualstudio.com") => GitRemote.AzureDevOps,
            _ => GitRemote.None,
        };
    }
}
