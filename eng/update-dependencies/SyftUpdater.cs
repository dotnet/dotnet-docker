// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

namespace Dotnet.Docker;

#nullable enable
internal static class SyftUpdater
{
    public static async Task<IDependencyUpdater> GetSyftUpdaterAsync(string repoRoot)
    {
        Release syftRelease = await GitHubHelper.GetLatestRelease("anchore", "syft");
        return new GitHubReleaseVersionUpdater(repoRoot, "syft|tag", syftRelease, "dotnet");
    }
}
