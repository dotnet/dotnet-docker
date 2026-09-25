// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromVersionOptions : CreatePullRequestOptions
{
    public required string Version { get; init; }

    /// <summary>
    /// Use the dependency's internal (non-public) location instead of its public one.
    /// </summary>
    public bool Internal { get; init; }

    /// <summary>
    /// Use this exact location instead of the default public or internal one.
    /// </summary>
    public string? BaseUrl { get; init; }

    /// <summary>
    /// Only update images for this .NET major.minor version, such as "9.0". When null, the updater
    /// chooses which .NET versions to update.
    /// </summary>
    public string? DotnetVersion { get; init; }

    private static readonly Argument<string> s_version = new("version")
    {
        Description = "The product version to use as the update source",
    };

    private static readonly Option<bool> s_internal = new("--internal")
    {
        Description = "Use the internal (non-public) location for the version",
    };

    private static readonly Option<string?> s_baseUrl = new("--base-url")
    {
        Description = "Use this exact location for the version instead of the default public or internal one",
    };

    private static readonly Option<string?> s_dotnetVersion = new("--dotnet-version")
    {
        Description = "Only update images for this .NET major.minor version (for example, 9.0)",
    };

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Arguments.Add(s_version);
        command.Options.Add(s_internal);
        command.Options.Add(s_baseUrl);
        command.Options.Add(s_dotnetVersion);
    }

    public static new FromVersionOptions Bind(ParseResult result) =>
        Bind(result, new FromVersionOptions
        {
            Version = result.GetRequiredValue(s_version),
            Internal = result.GetValue(s_internal),
            BaseUrl = result.GetValue(s_baseUrl),
            DotnetVersion = result.GetValue(s_dotnetVersion),
        });
}
