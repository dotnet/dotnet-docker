// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests;

#nullable enable
public static partial class DockerfileHelper
{
    [GeneratedRegex("[A-Fa-f0-9]{128}")]
    private static partial Regex Sha512Regex { get; }

    [GeneratedRegex("[A-Fa-f0-9]{96}")]
    private static partial Regex Sha386Regex { get; }

    [GeneratedRegex("[A-Fa-f0-9]{64}")]
    private static partial Regex Sha256Regex { get; }

    [GeneratedRegex(@"\d+\.\d+\.\d+(?!\.windows)")]
    private static partial Regex SemanticVersionRegex { get; }

    [GeneratedRegex(@"v\d+\.\d+\.\d+\.windows\.\d+")]
    private static partial Regex MinGitVersionRegex { get; }

    [GeneratedRegex(@"alpine\d+\.\d+")]
    private static partial Regex AlpineVersionRegex { get; }

    private static List<(Regex pattern, string replacement)> ReplacePatterns { get; } =
    [
        (Sha512Regex, "{sha512_placeholder}"),
        (Sha386Regex, "{sha386_placeholder}"),
        (Sha256Regex, "{sha256_placeholder}"),
        (SemanticVersionRegex, "0.0.0"),
        (MinGitVersionRegex, "v0.0.0.windows.0"),
        (AlpineVersionRegex, "alpine3.XX"),
    ];

    public static Func<string, string> DockerfileScrubber { get; } = (content) =>
    {
        foreach ((Regex pattern, string replacement) in ReplacePatterns)
        {
            content = pattern.Replace(content, replacement);
        }
        return content;
    };

    public static string[] GetAllDockerfilesInDirectory(string directory) =>
        Directory.GetFiles(
            path: directory,
            searchPattern: "Dockerfile",
            new EnumerationOptions { RecurseSubdirectories = true });
}
