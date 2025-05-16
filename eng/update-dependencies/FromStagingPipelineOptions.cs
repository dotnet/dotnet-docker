// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

internal class FromStagingPipelineOptions : CreatePullRequestOptions, IOptions
{
    public required string StagingPipelineRunId { get; init; }

    public static new List<Argument> Arguments { get; } =
    [
        new Argument<string>("stagingPipelineRunId")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The staging pipeline run ID to use as a source for the update"
        },
        ..CreatePullRequestOptions.Arguments,
    ];

    public static new List<Option> Options { get; } =
    [
        ..CreatePullRequestOptions.Options,
    ];
}
