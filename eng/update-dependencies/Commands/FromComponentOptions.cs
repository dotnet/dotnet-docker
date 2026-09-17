// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

/// <summary>
/// Options for updating a component from its latest GitHub release.
/// </summary>
public record FromComponentOptions : CreatePullRequestOptions, IOptions
{
    /// <summary>
    /// The registered component key (DI keyed service) to update.
    /// </summary>
    public required string Component { get; init; }

    public static List<Argument> Arguments { get; } =
    [
        new Argument<string>("component")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The key of a registered GitHub release updater"
        },
    ];

    public static new List<Option> Options { get; } =
    [
        ..CreatePullRequestOptions.Options,
    ];
}
