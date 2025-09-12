// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;

namespace Dotnet.Docker.Sync;

internal static class DarcLibExtensions
{
    public static Task<string?> TryGetShaForBranchAsync(this ILocalGitRepo repo, string branch) =>
        repo.TryGetShaForRefAsync($"refs/heads/{branch}");

    public static async Task<string?> TryGetShaForRefAsync(this ILocalGitRepo repo, string gitRef)
    {
        try
        {
            string sha = await repo.GetShaForRefAsync(gitRef);
            return string.IsNullOrWhiteSpace(sha) ? null : sha;
        }
        catch
        {
            return null;
        }
    }

    public static Task UpdateRefAsync(this ILocalGitRepo repo, string gitRef, string commit) =>
        repo.ExecuteGitCommand("update-ref", gitRef, commit);
}
