// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

public abstract record CreatePullRequestOptions
{
    /// <summary>
    /// The root of the dotnet-docker repo to run against.
    /// </summary>
    public string RepoRoot { get; init; } = Directory.GetCurrentDirectory();

    public string User { get; init; } = "";
    public string Email { get; init; } = "";
    public string Password { get; init; } = "";
    public string AzdoOrganization { get; init; } = "";
    public string AzdoProject { get; init; } = "";
    public string AzdoRepo { get; init; } = "";
    public string VersionSourceName { get; init; } = "";
    public string SourceBranch { get; init; } = "";
    public string TargetBranch { get; init; } = "nightly";

    public static List<Option> Options =>
    [
        new Option<string>("--repo-root") { Description = "The root of the dotnet-docker repo to run against (defaults to current working directory)" },
        new Option<string>("--user") { Description = "GitHub or AzDO user used to make PR (if not specified, a PR will not be created)" },
        new Option<string>("--email") { Description = "GitHub or AzDO email used to make PR (if not specified, a PR will not be created)" },
        new Option<string>("--password") { Description = "GitHub or AzDO password used to make PR (if not specified, a PR will not be created)" },
        new Option<string>("--azdo-organization") { Description = "Name of the AzDO organization" },
        new Option<string>("--azdo-project") { Description = "Name of the AzDO project" },
        new Option<string>("--azdo-repo") { Description = "Name of the AzDO repo" },
        new Option<string>("--version-source-name") { Description = "The name of the source from which the version information was acquired." },
        new Option<string>("--source-branch") { Description = "If synchronizing multiple branches, the branch to pull updates from" },
        new Option<string>("--target-branch") { Description = "Pull request will be submitted targeting this branch" },
    ];

    public static List<Argument> Arguments => [];
}
