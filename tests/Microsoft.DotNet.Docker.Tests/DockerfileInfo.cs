// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Text.RegularExpressions;
using Microsoft.DotNet.Docker.Shared;

namespace Microsoft.DotNet.Docker.Tests;

/// <summary>
/// Represents information about a specific Dockerfile's location.
/// </summary>
/// <param name="RepoDir">
/// The repository directory where the Dockerfile is located.
/// </param>
/// <param name="VersionDir">
/// The version directory where the Dockerfile is located. Empty string if the
/// Dockerfile is not contained in a version directory.
/// </param>
/// <param name="OsDir">
/// The operating system directory where the Dockerfile is located. Empty
/// string if the Dockerfile is not contained in an OS-specific directory.
/// </param>
/// <param name="ArchitectureDir">
/// The architecture directory where the Dockerfile is located.
/// </param>
public sealed partial record DockerfileInfo(
    string RepoDir,
    string VersionDir,
    string OsDir,
    string ArchitectureDir,
    Repo Repo,
    Image Image,
    Platform Platform)
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

    public DotNetVersion ProductVersion => DotNetVersion.Parse(ProductVersionString);

    private string ProductVersionString => ManifestHelper.GetResolvedProductVersion(Image);

    public static DockerfileInfo Create(string dockerfilePath, Repo repo, Image image, Platform platform)
    {
        Match match = DockerfileRegex.Match(dockerfilePath);
        if (!match.Success)
        {
            throw new Exception($"Failed to parse dockerfile: {dockerfilePath}");
        }

        return new DockerfileInfo(
            match.Groups["repo"].Value.TrimStart('/'),
            match.Groups["major_minor"].Value.TrimStart('/'),
            match.Groups["os"].Value.TrimStart('/'),
            match.Groups["architecture"].Value.TrimStart('/'),
            repo,
            image,
            platform);
    }
}
