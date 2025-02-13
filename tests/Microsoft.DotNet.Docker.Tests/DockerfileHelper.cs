// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests;

#nullable enable
public static partial class DockerfileHelper
{
    [GeneratedRegex("[A-Fa-f0-9]{128}")]
    public static partial Regex Sha512Regex { get; }

    [GeneratedRegex("[A-Fa-f0-9]{96}")]
    public static partial Regex Sha386Regex { get; }

    [GeneratedRegex("[A-Fa-f0-9]{64}")]
    public static partial Regex Sha256Regex { get; }

    // Match versions like `1.2.3`, `1.2.3.4`, `1.2.3-foo.45678.9`, and `1.2.3-preview.4.56789.0`
    [GeneratedRegex(@"\d+\.\d+\.\d+(\.d+)?(-\w+(\.\d+){2,})?")]
    public static partial Regex VersionRegex { get; }

    [GeneratedRegex(@"v\d+\.\d+\.\d+\.windows\.\d+")]
    public static partial Regex MinGitVersionRegex { get; }

    [GeneratedRegex(@"alpine\d+\.\d+")]
    public static partial Regex AlpineVersionRegex { get; }

    public static string[] GetAllDockerfilesInDirectory(string directory) =>
        Directory.GetFiles(
            path: directory,
            searchPattern: "Dockerfile",
            new EnumerationOptions { RecurseSubdirectories = true });
}
