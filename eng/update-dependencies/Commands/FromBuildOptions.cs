// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromBuildOptions : CreatePullRequestOptions, IOptions
{
    public required int Id { get; init; }

    public static List<Argument> Arguments { get; } =
    [
        new Argument<int>("id")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The BAR build ID to use as a source for the update (see https://aka.ms/bar)"
        },
    ];

    public static new List<Option> Options { get; } =
    [
        ..CreatePullRequestOptions.Options,
    ];
}
