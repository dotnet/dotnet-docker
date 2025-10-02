// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Dotnet.Docker.Sync;

public sealed record SyncInternalReleaseOptions : CreatePullRequestOptions, IOptions
{
    public string PrBranchPrefix { get; set; } = "pr";
    public string StagingStorageAccount { get; set; } = string.Empty;

    public static new List<Option> Options =>
    [
        new Option<string>("--pr-branch-prefix")
        {
            Description = "Prefix to use for branches created for pull requests",
        },
        FromStagingPipelineOptions.StagingStorageAccountOption,
        ..CreatePullRequestOptions.Options,
    ];

    public static new List<Argument> Arguments =>
    [
        ..CreatePullRequestOptions.Arguments,
    ];
}
