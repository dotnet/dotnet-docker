// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

public record CreatePullRequestOptions
{
    public string RepoRoot { get; set; } = Directory.GetCurrentDirectory();
    public string SourceBranch { get; set; } = "";
    public string TargetBranch { get; set; } = "nightly";
    public bool UpdateOnly { get; set; }

    private static readonly Option<string> s_repoRoot = new("--repo-root")
    {
        Description = "The root of the dotnet-docker repo to run against (defaults to current working directory)",
        DefaultValueFactory = _ => Directory.GetCurrentDirectory(),
    };
    private static readonly Option<bool> s_updateOnly = new("--update-only")
    {
        Description = "Apply updates locally without creating a pull request",
    };
    private static readonly Option<string> s_sourceBranch = new("--source-branch")
    {
        Description = "Branch to pull updates from",
        DefaultValueFactory = _ => "",
    };
    private static readonly Option<string> s_targetBranch = new("--target-branch")
    {
        Description = "Pull request will be submitted targeting this branch",
        DefaultValueFactory = _ => "nightly",
    };

    public static void AddTo(Command command)
    {
        command.Options.Add(s_repoRoot);
        command.Options.Add(s_updateOnly);
        command.Options.Add(s_sourceBranch);
        command.Options.Add(s_targetBranch);
    }

    public static CreatePullRequestOptions Bind(ParseResult result) =>
        Bind(result, new CreatePullRequestOptions());

    protected static TOptions Bind<TOptions>(ParseResult result, TOptions options)
        where TOptions : CreatePullRequestOptions
    {
        options.RepoRoot = result.GetRequiredValue(s_repoRoot);
        options.UpdateOnly = result.GetValue(s_updateOnly);
        options.SourceBranch = result.GetRequiredValue(s_sourceBranch);
        options.TargetBranch = result.GetRequiredValue(s_targetBranch);
        return options;
    }

    public string CreatePullRequestBranchName(string name, string buildId = "")
    {
        var buildIdSuffix = string.IsNullOrWhiteSpace(buildId) ? string.Empty : $"-{buildId}";
        var sanitizedTargetBranch = TargetBranch.Replace('/', '-');
        return $"{sanitizedTargetBranch}/{name}{buildIdSuffix}";
    }
}
