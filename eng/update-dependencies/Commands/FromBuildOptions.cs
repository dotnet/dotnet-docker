// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromBuildOptions : CreatePullRequestOptions
{
    public required int Id { get; init; }

    private static readonly PositiveIdArgument s_id = new("The BAR build ID to use as the update source");

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Arguments.Add(s_id);
    }

    public static new FromBuildOptions Bind(ParseResult result) =>
        Bind(result, new FromBuildOptions { Id = result.GetValue(s_id) });
}
