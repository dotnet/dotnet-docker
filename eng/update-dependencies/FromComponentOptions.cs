// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

/// <summary>
/// Options for the from-component command which updates a single component
/// (product or tool) using a registered IDependencyVersionSource.
/// </summary>
internal record FromComponentOptions : CreatePullRequestOptions, IOptions
{
    /// <summary>
    /// The registered component key (DI keyed service) to update.
    /// </summary>
    public required string Component { get; init; }

    /// <summary>
    /// Logical Dockerfile version (major.minor) used to scope template variables and SHAs.
    /// For example: 9.0, 8.0.
    /// </summary>
    public required string DockerfileVersion { get; init; }

    /// <summary>
    /// Optional logical channel for the component (e.g. stable, preview, lts, daily).
    /// This can be used by dependency version sources however they see fit.
    /// </summary>
    public string? Channel { get; init; } = null;

    public static new List<Argument> Arguments { get; } =
    [
        new Argument<string>("dockerfile-version")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The Dockerfile major.minor version the update applies to (e.g. 9.0)"
        },
        new Argument<string>("component")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The component key identifying a registered IDependencyVersionSource"
        },
        ..CreatePullRequestOptions.Arguments,
    ];

    public static new List<Option> Options { get; } =
    [
        new Option<string?>("--channel")
        {
            Description = "Logical version channel for the component"
        },
        ..CreatePullRequestOptions.Options,
    ];
}
