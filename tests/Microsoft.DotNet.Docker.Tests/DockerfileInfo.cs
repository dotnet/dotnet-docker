// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests;

/// <summary>
/// Represents information about a specific Dockerfile's location.
/// </summary>
/// <param name="Repo">
/// The repository directory where the Dockerfile is located.
/// </param>
/// <param name="MajorMinor">
/// The version directory where the Dockerfile is located. Empty string if the
/// Dockerfile is not contained in a version directory.
/// </param>
/// <param name="Os">
/// The operating system directory where the Dockerfile is located. Empty
/// string if the Dockerfile is not contained in an OS-specific directory.
/// </param>
/// <param name="Architecture">
/// The architecture directory where the Dockerfile is located.
/// </param>
public partial record DockerfileInfo(string Repo, string MajorMinor, string Os, string Architecture)
{
    [GeneratedRegex(
        @"^
            src
            (?<repo>/[\w-]+)
            # Optional version segment
            (?<major_minor>/\d+\.\d+)?
            # Optional OS segment with negative lookahead to exclude arch
            (?<os>/(?!amd64$|arm64v8$|arm32v7$)[^/]+)?
            # Required arch segment
            (?<architecture>/(?:amd64|arm64v8|arm32v7))
        $",
        RegexOptions.IgnorePatternWhitespace
    )]
    private static partial Regex DockerfileRegex { get; }

    public static DockerfileInfo Create(string dockerfilePath)
    {
        Match match = DockerfileRegex.Match(dockerfilePath);
        if (!match.Success)
        {
            throw new Exception($"Failed to parse dockerfile: {dockerfilePath}");
        }

        return new DockerfileInfo(
            Repo: match.Groups["repo"].Value.TrimStart('/'),
            MajorMinor: match.Groups["major_minor"].Value.TrimStart('/'),
            Os: match.Groups["os"].Value.TrimStart('/'),
            Architecture: match.Groups["architecture"].Value.TrimStart('/'));
    }
}
