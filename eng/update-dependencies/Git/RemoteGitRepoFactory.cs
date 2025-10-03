// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Docker.Git;

/// <summary>
/// Factory for creating <see cref="IRemoteGitRepo"/> instances based on repository URLs.
/// </summary>
internal interface IRemoteGitRepoFactory
{
    /// <summary>
    /// Creates a remote Git repository client appropriate for the given repository URL.
    /// </summary>
    /// <param name="repoUrl">The URL of the remote repository.</param>
    /// <returns>A configured <see cref="IRemoteGitRepo"/> instance.</returns>
    IRemoteGitRepo CreateRemoteGitRepo(string repoUrl);
}

/// <inheritdoc />
internal sealed class RemoteGitRepoFactory(IServiceProvider serviceProvider) : IRemoteGitRepoFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <inheritdoc />
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

    /// <summary>
    /// Determines the type of Git remote (GitHub, Azure DevOps, etc.) from a repository URL.
    /// </summary>
    /// <param name="repoUrl">The repository URL to parse.</param>
    /// <returns>The type of Git remote, or <see cref="GitRemote.None"/> if not supported.</returns>
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
