// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

/// <summary>
/// Command line options that bind to a <see cref="VersionSource"/>.
/// </summary>
internal static class VersionSourceOptions
{
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

    public static void AddTo(Command command, bool includeBaseUrl)
    {
        command.Options.Add(s_internal);
        if (includeBaseUrl)
        {
            command.Options.Add(s_baseUrl);
        }

        command.Options.Add(s_dotnetVersion);
    }

    public static VersionSource Bind(ParseResult result) =>
        new(
            Internal: result.GetValue(s_internal),
            BaseUrl: result.GetValue(s_baseUrl),
            DotnetVersion: result.GetValue(s_dotnetVersion));
}
