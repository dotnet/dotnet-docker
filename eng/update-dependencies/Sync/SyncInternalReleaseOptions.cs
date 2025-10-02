// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker.Sync;

public sealed record SyncInternalReleaseOptions : IOptions
{
    public string RemoteUrl { get; set; } = string.Empty;
    public string SourceBranch { get; set; } = string.Empty;
    public string TargetBranch { get; set; } = string.Empty;
    public string PrBranchPrefix { get; set; } = "pr";
    public string CommitterName { get; set; } = string.Empty;
    public string CommitterEmail { get; set; } = string.Empty;
    public string StagingStorageAccount { get; set; } = string.Empty;

    public static List<Option> Options =>
    [
        new Option<string>("--pr-branch-prefix")
        {
            Description = "Prefix to use for branches created for pull requests",
        },
        new Option<string>("--committer-name")
        {
            Description = "Name used for git commits",
        },
        new Option<string>("--committer-email")
        {
            Description = "Email used for git commits",
        },
        FromStagingPipelineOptions.StagingStorageAccountOption,
    ];

    public static List<Argument> Arguments =>
    [
        new Argument<string>("remote-url")
        {
            Description = "The URL of the remote git repository to sync (e.g., https://dev.azure.com/org/project/_git/repo)"
        },
        new Argument<string>("source-branch")
        {
            Description = "The source branch to sync from (typically a release/* branch)"
        },
        new Argument<string>("target-branch")
        {
            Description = "The target branch to sync to (typically an internal/release/* branch)"
        },
    ];
}
