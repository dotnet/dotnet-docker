// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Dotnet.Docker;

/// <summary>
/// Represents the product commit versions for the SDK, runtime, and ASP.NET Core.
/// </summary>
/// <example>
/// {
///   "runtime": { "commit": "721dc7a2a59416b21fc49447d264009d708d6000", "version": "10.0.0-preview.4.25223.119" },
///   "aspnetcore": { "commit": "721dc7a2a59416b21fc49447d264009d708d6000", "version": "10.0.0-preview.5.25223.119" },
///   "windowsdesktop": { "commit": "721dc7a2a59416b21fc49447d264009d708d6000", "version": "10.0.0-preview.5.25222.4" },
///   "sdk": { "commit": "721dc7a2a59416b21fc49447d264009d708d6000", "version": "10.0.100-preview.5.25223.119" }
/// }
/// </example>
internal partial record ProductCommits(
    ProductCommit Sdk,
    ProductCommit Runtime,
    ProductCommit AspNetCore)
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [GeneratedRegex("^Sdk/.*/productCommit-linux-x64.json$")]
    public static partial Regex SdkAssetRegex { get; }

    public static ProductCommits FromJson(string json)
    {
        return JsonSerializer.Deserialize<ProductCommits>(json, s_jsonOptions)
            ?? throw new InvalidOperationException(
                $"""
                Could not deserialize product commit versions from content:
                {json}
                """);
    }
};

internal record ProductCommit(string Commit, string Version);
