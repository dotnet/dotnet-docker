// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromLatestVersionOptions : CreatePullRequestOptions
{
    /// <summary>
    /// Use the dependency's internal (non-public) location instead of its public one.
    /// </summary>
    public bool Internal { get; init; }

    /// <summary>
    /// Only update images for this .NET major.minor version, such as "9.0". When null, the updater
    /// chooses which .NET versions to update.
    /// </summary>
    public string? DotnetVersion { get; init; }

    private static readonly Option<bool> s_internal = new("--internal")
    {
        Description = "Use the internal (non-public) location for the version",
    };

    private static readonly Option<string?> s_dotnetVersion = new("--dotnet-version")
    {
        Description = "Only update images for this .NET major.minor version (for example, 9.0)",
    };

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Options.Add(s_internal);
        command.Options.Add(s_dotnetVersion);
    }

    public static new FromLatestVersionOptions Bind(ParseResult result) =>
        Bind(result, new FromLatestVersionOptions
        {
            Internal = result.GetValue(s_internal),
            DotnetVersion = result.GetValue(s_dotnetVersion),
        });
}
