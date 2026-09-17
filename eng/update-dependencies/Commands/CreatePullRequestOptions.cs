// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

public abstract record CreatePullRequestOptions
{
    public string RepoRoot { get; init; } = Directory.GetCurrentDirectory();
    public string SourceBranch { get; init; } = "";
    public string TargetBranch { get; init; } = "nightly";
    public bool UpdateOnly { get; init; }

    public static List<Option> Options =>
    [
        new Option<string>("--repo-root") { Description = "The root of the dotnet-docker repo to run against (defaults to current working directory)" },
        new Option<bool>("--update-only") { Description = "Apply updates locally without creating a pull request" },
        new Option<string>("--source-branch") { Description = "Branch to pull updates from" },
        new Option<string>("--target-branch") { Description = "Pull request will be submitted targeting this branch" },
    ];

    public static List<Argument> Arguments => [];

    public string CreatePullRequestBranchName(string name, string buildId = "")
    {
        var buildIdSuffix = string.IsNullOrWhiteSpace(buildId) ? string.Empty : $"-{buildId}";
        var sanitizedTargetBranch = TargetBranch.Replace('/', '-');
        return $"{sanitizedTargetBranch}/{name}{buildIdSuffix}";
    }
}
