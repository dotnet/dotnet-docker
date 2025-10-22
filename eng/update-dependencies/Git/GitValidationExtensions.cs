// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker.Git;

internal static class GitValidationExtensions
{
    public static async Task EnsureBranchExistsAsync(this IRemoteGitRepoHelper remote, string branchName)
    {
        var branch = await remote.RemoteBranchExistsAsync(branchName);
        if (!branch)
        {
            throw new InvalidOperationException(
                $"The branch '{branchName}' does not exist on the remote {remote}.");
        }
    }
}
