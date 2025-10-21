// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

internal static class CreatePullRequestOptionsExtensions
{
    /// <summary>
    /// Gets the path to the manifest.versions.json file based on the
    /// repository root specified in the options.
    /// </summary>
    public static string GetManifestVersionsFilePath(this CreatePullRequestOptions options) =>
        Path.Combine(options.RepoRoot, "manifest.versions.json");

    /// <summary>
    /// Constructs the Azure DevOps repository URL from the options.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if any of the required options are null or whitespace.
    /// </exception>
    public static string GetAzdoRepoUrl(this CreatePullRequestOptions options)
    {
        // Validate that we have all the required pieces to construct the repo URL.
        ArgumentException.ThrowIfNullOrWhiteSpace(options.AzdoOrganization);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.AzdoProject);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.AzdoRepo);

        // AzdoOrganization is a URL like https://dev.azure.com/<org>
        // A valid Azure DevOps repository URL is formatted like https://dev.azure.com/<org>/<project>/_git/<repo>
        return $"{options.AzdoOrganization}/{options.AzdoProject}/_git/{options.AzdoRepo}";
    }

    /// <summary>
    /// Validates that the committer identity is present and returns it.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if any of the required options are null or whitespace.
    /// </exception>
    public static (string Name, string Email) GetCommitterIdentity(this CreatePullRequestOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.User);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Email);
        return (options.User, options.Email);
    }

    /// <summary>
    /// Automatically generates a branch name for the pull request. Uses the target branch, prefix,
    /// and <paramref name="name"/> to create a descriptive branch name.
    /// </summary>
    /// <param name="name">
    /// Should be something short but descriptive, like "sync" or "update-deps-int-{buildNumber}".
    /// </param>
    /// <returns>
    /// A valid branch name that is descriptive but not guaranteed to be unique.
    /// </returns>
    public static string CreatePrBranchName(this CreatePullRequestOptions options, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(options.PrBranchPrefix);
        ArgumentException.ThrowIfNullOrEmpty(options.TargetBranch);

        var buildIdSuffix = AzurePipelinesHelper.IsRunningInAzurePipelines()
            ? $"-{AzurePipelinesHelper.GetBuildId()}"
            : string.Empty;

        var sanitizedTargetBranch = options.TargetBranch.Replace('/', '-');
        var prefix = options.PrBranchPrefix.TrimEnd('/');
        return $"{prefix}/{sanitizedTargetBranch}/{name}{buildIdSuffix}";
    }
}
