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
    [GeneratedRegex(@"\d+\.\d+\.\d+(\.\d+)?(-[A-Za-z]+(\.\d+)+)?")]
    public static partial Regex VersionRegex { get; }

    // Match unstable versions that have been partially replaced with variables,
    // like `$aspnetcore_version.25326.107` or `$aspnetcore_version-rtm.25326.107`
    [GeneratedRegex(@"\$[a-zA-Z0-9_]+((-[A-Za-z]+)?(\.\d+)+)")]
    public static partial Regex VersionWithVariableRegex { get; }

    // Match unstable versions that have been partially replaced with variables
    // using string addition, like `$dotnet_sdk_version + '-rtm.25515.111'`
    [GeneratedRegex(@"\$[a-zA-Z0-9_]+\s*\+\s*'(-[A-Za-z]+(\.\d+)+)'")]
    public static partial Regex StringVersionWithVariableRegex { get; }

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
