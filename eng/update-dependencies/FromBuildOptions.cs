// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

internal record FromBuildOptions : CreatePullRequestOptions, IOptions
{
    public required int Id { get; init; }

    public static new List<Argument> Arguments { get; } =
    [
        new Argument<int>("id")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The BAR build ID to use as a source for the update (see https://aka.ms/bar)"
        },
        ..CreatePullRequestOptions.Arguments,
    ];

    public static new List<Option> Options { get; } =
    [
        ..CreatePullRequestOptions.Options,
    ];
}
