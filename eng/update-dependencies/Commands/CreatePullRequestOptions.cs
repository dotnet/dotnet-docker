// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

public abstract record CreatePullRequestOptions
{
    /// <summary>
    /// The root of the dotnet-docker repo to run against.
    /// </summary>
    public string RepoRoot { get; init; } = Directory.GetCurrentDirectory();

    public string VersionSourceName { get; init; } = "";
    public string SourceBranch { get; init; } = "";
    public string TargetBranch { get; init; } = "nightly";
    public string PrBranchPrefix { get; init; } = "pr";

    public bool UpdateOnly { get; init; }

    public static List<Option> Options =>
    [
        new Option<string>("--repo-root") { Description = "The root of the dotnet-docker repo to run against (defaults to current working directory)" },
        new Option<bool>("--update-only") { Description = "Apply updates locally without creating a pull request" },
        new Option<string>("--version-source-name") { Description = "The name of the source from which the version information was acquired." },
        new Option<string>("--source-branch") { Description = "If synchronizing multiple branches, the branch to pull updates from" },
        new Option<string>("--target-branch") { Description = "Pull request will be submitted targeting this branch" },
        new Option<string>("--pr-branch-prefix") { Description = "Prefix to use for branches created for pull requests" },
    ];

    public static List<Argument> Arguments => [];
}
