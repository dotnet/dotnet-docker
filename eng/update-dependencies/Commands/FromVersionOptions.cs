// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromVersionOptions : CreatePullRequestOptions
{
    public required string Version { get; init; }

    private static readonly Argument<string> s_version = new("version")
    {
        Description = "The product version to use as the update source",
    };

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Arguments.Add(s_version);
    }

    public static new FromVersionOptions Bind(ParseResult result) =>
        Bind(result, new FromVersionOptions { Version = result.GetRequiredValue(s_version) });
}
