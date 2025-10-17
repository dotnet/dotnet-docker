// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Dotnet.Docker;

public abstract record CreatePullRequestOptions
{
    private string _azdoOrganizationUrl = "";

    /// <summary>
    /// The root of the dotnet-docker repo to run against.
    /// </summary>
    public string RepoRoot { get; init; } = Directory.GetCurrentDirectory();

    public string User { get; init; } = "";
    public string Email { get; init; } = "";
    public string Password { get; init; } = "";
    public string AzdoOrganization
    {
        get => _azdoOrganizationUrl;
        init => _azdoOrganizationUrl = value.TrimEnd('/');
    }
    public string AzdoProject { get; init; } = "";
    public string AzdoRepo { get; init; } = "";
    public string VersionSourceName { get; init; } = "";
    public string SourceBranch { get; init; } = "";
    public string TargetBranch { get; init; } = "nightly";
    public string PrBranchPrefix { get; init; } = "pr";

    // If new properties or options are added, they may need to be added to
    // SpecificCommandOptions.FromPullRequestOptions(...)

    public static List<Option> Options =>
    [
        new Option<string>("--repo-root") { Description = "The root of the dotnet-docker repo to run against (defaults to current working directory)" },
        new Option<string>("--user") { Description = "GitHub or AzDO user used to make PR (if not specified, a PR will not be created)" },
        new Option<string>("--email") { Description = "GitHub or AzDO email used to make PR (if not specified, a PR will not be created)" },
        new Option<string>("--password") { Description = "GitHub or AzDO password used to make PR (if not specified, a PR will not be created)" },
        new Option<string>("--azdo-organization")
        {
            Description = "URL of the AzDO organization (like https://dev.azure.com/<orgname>), with or without a trailing slash."
                + " The Azure Pipelines variable 'System.CollectionUri' provides this value.",
        },
        new Option<string>("--azdo-project") { Description = "Name of the AzDO project" },
        new Option<string>("--azdo-repo") { Description = "Name of the AzDO repo" },
        new Option<string>("--version-source-name") { Description = "The name of the source from which the version information was acquired." },
        new Option<string>("--source-branch") { Description = "If synchronizing multiple branches, the branch to pull updates from" },
        new Option<string>("--target-branch") { Description = "Pull request will be submitted targeting this branch" },
        new Option<string>("--pr-branch-prefix") { Description = "Prefix to use for branches created for pull requests" },
    ];

    public static List<Argument> Arguments => [];
}
