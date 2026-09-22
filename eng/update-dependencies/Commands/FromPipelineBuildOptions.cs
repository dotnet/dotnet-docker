// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromPipelineBuildOptions : CreatePullRequestOptions
{
    public required int RunId { get; init; }

    private static readonly PositiveIdArgument s_runId = new("The Azure DevOps pipeline run ID to use as the update source");

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Arguments.Add(s_runId);
    }

    public static new FromPipelineBuildOptions Bind(ParseResult result) =>
        Bind(result, new FromPipelineBuildOptions { RunId = result.GetValue(s_runId) });
}
