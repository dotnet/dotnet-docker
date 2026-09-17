// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record MonitorOptions : CreatePullRequestOptions, IOptions
{
    public string? Version { get; init; }
    public int? PipelineRunId { get; init; }

    public static List<Argument> Arguments { get; } =
    [
        new Argument<string?>("version")
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = "The Monitor version to use instead of --pipeline-run-id",
        },
    ];

    public static new List<Option> Options { get; } =
    [
        new Option<int?>("--pipeline-run-id")
        {
            Description = "The Azure DevOps pipeline run ID to read the Monitor version from."
                + " Defaults to organization https://dev.azure.com/dnceng and project internal."
                + " Use --azdo-organization and --azdo-project to override.",
        },
        ..CreatePullRequestOptions.Options,
    ];
}
