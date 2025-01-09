// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

    private static List<(Regex pattern, string replacement)> Patterns { get; } =
    [
        (Sha512Regex, "{sha512_placeholder}"),
        (Sha386Regex, "{sha386_placeholder}"),
        (Sha256Regex, "{sha256_placeholder}"),
        (SemanticVersionRegex, "0.0.0"),
        (MinGitVersionRegex, "v0.0.0.windows.0"),
        (AlpineVersionRegex, "alpine3.XX"),
    ];

    public static async Task ScrubDockerfilesAsync(string path)
    {
        string[] dockerfiles =
            Directory.GetFiles(path, "Dockerfile",
                new EnumerationOptions { RecurseSubdirectories = true });

        IEnumerable<Task> tasks = dockerfiles.Select(dockerfile => ReplaceTextAsync(dockerfile, Patterns));
        await Task.WhenAll(tasks);
    }

    private static async Task ReplaceTextAsync(string filePath, List<(Regex pattern, string replacement)> patterns)
    {
        string content = await File.ReadAllTextAsync(filePath);
        foreach ((Regex pattern, string replacement) in patterns)
        {
            content = pattern.Replace(content, replacement);
        }
        await File.WriteAllTextAsync(filePath, content);
    }
}
