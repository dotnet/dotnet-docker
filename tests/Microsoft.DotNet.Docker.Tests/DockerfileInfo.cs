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
/// <param name="Repo">The repository directory where the Dockerfile is located.</param>
/// <param name="MajorMinor">The version directory where the Dockerfile is located.</param>
/// <param name="Os">The operating system directory where the Dockerfile is located.</param>
/// <param name="Architecture">The architecture directory where the Dockerfile is located.</param>
public partial record DockerfileInfo(string Repo, string MajorMinor, string Os, string Architecture)
{
    [GeneratedRegex(@"src/(?<repo>.+)/(?<major_minor>\d+\.\d+)/(?<os>.+)/(?<architecture>.+)")]
    private static partial Regex DockerfileRegex { get; }

    public static DockerfileInfo Create(string dockerfilePath)
    {
        Match match = DockerfileRegex.Match(dockerfilePath);
        if (!match.Success)
        {
            throw new Exception($"Failed to parse dockerfile: {dockerfilePath}");
        }

        return new DockerfileInfo(
            match.Groups["repo"].Value,
            match.Groups["major_minor"].Value,
            match.Groups["os"].Value,
            match.Groups["architecture"].Value);
    }
}
