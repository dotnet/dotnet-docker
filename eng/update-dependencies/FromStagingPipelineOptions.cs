// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

internal record FromStagingPipelineOptions : CreatePullRequestOptions, IOptions
{
    public const string StagingStorageAccountOption = "--staging-storage-account";
    public const string InternalOption = "--internal";

    /// <summary>
    /// The staging pipeline run ID to use as a source for the update.
    /// </summary>
    public required int StagingPipelineRunId { get; init; }

    /// <summary>
    /// Whether or not to use the internal versions of the staged build.
    /// </summary>
    public bool Internal { get; init; } = false;

    /// <summary>
    /// This Azure Storage Account will be used as a source for the update.
    /// This should be one of two storage accounts: dotnetstagetest or dotnetstage.
    /// </summary>
    public string? StagingStorageAccount { get; init; }

    public static new List<Argument> Arguments { get; } =
    [
        new Argument<int>("staging-pipeline-run-id")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The staging pipeline run ID to use as a source for the update"
        },
        ..CreatePullRequestOptions.Arguments,
    ];

    public static new List<Option> Options { get; } =
    [
        new Option<string>(StagingStorageAccountOption)
        {
            Description = "The Azure Storage Account to use as a source for the update."
                + " This should be one of two storage accounts: dotnetstagetest or dotnetstage."
                + " For example: https://dotnetstagetest.blob.core.windows.net/"
        },
        new Option<bool>(InternalOption)
        {
            Description = "Whether or not to use the internal versions of the staged build. When not using an internal"
                + " build, Dockerfiles will be updated as if the staged build has already been released. When using an"
                + " internal build, Dockerfiles will be updated with internal download links and will only be buildable"
                + " by using an internal Azure DevOps access token."
        },
        ..CreatePullRequestOptions.Options,
    ];
}
