// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

#nullable enable
namespace Dotnet.Docker;

internal static class MinGitUpdater
{
    public static async Task<IEnumerable<IDependencyUpdater>> GetMinGitUpdatersAsync(string repoRoot)
    {
        Release minGitRelease = await GitHubHelper.GetLatestRelease("git-for-windows", "git");

        return [
            new MinGitUrlUpdater(repoRoot, minGitRelease),
            new MinGitShaUpdater(repoRoot, minGitRelease),
        ];
    }
}
