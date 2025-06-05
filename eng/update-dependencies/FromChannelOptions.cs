// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

internal record FromChannelOptions : CreatePullRequestOptions, IOptions
{
    public required int Channel { get; init; }
    public required string Repo { get; init; }

    public static new List<Argument> Arguments { get; } =
    [
        new Argument<int>("channel")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The BAR channel ID to use as a source for the update (see https://aka.ms/bar)"
        },
        new Argument<string>("repo")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The repository to get the latest build from (e.g. https://github.com/dotnet/dotnet)"
        },
        ..CreatePullRequestOptions.Arguments,
    ];

    public static new List<Option> Options { get; } =
    [
        ..CreatePullRequestOptions.Options,
    ];
}
