// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal static class CreatePullRequestOptionsExtensions
{
    /// <summary>
    /// Gets the path to the manifest.versions.json file based on the
    /// repository root specified in the options.
    /// </summary>
    public static string GetManifestVersionsFilePath(this CreatePullRequestOptions options) =>
        Path.Combine(options.RepoRoot, "manifest.versions.json");

    /// <summary>
    /// Automatically generates a branch name for the pull request. Uses the target branch, prefix,
    /// and <paramref name="name"/> to create a descriptive branch name.
    /// </summary>
    /// <param name="name">
    /// Should be something short but descriptive, like "sync" or "update-deps-int-{buildNumber}".
    /// </param>
    /// <param name="buildId">
    /// Optional build ID to append as a suffix. When running in Azure Pipelines, callers should
    /// pass the build ID to make the branch name unique across pipeline runs.
    /// </param>
    /// <returns>
    /// A valid branch name that is descriptive but not guaranteed to be unique.
    /// </returns>
    public static string CreatePrBranchName(this CreatePullRequestOptions options, string name, string buildId = "")
    {
        ArgumentException.ThrowIfNullOrEmpty(options.PrBranchPrefix);
        ArgumentException.ThrowIfNullOrEmpty(options.TargetBranch);

        var buildIdSuffix = string.IsNullOrWhiteSpace(buildId) ? string.Empty : $"-{buildId}";

        var sanitizedTargetBranch = options.TargetBranch.Replace('/', '-');
        var prefix = options.PrBranchPrefix.TrimEnd('/');
        return $"{prefix}/{sanitizedTargetBranch}/{name}{buildIdSuffix}";
    }
}
