// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.RegularExpressions;

namespace Dotnet.Docker;

internal partial record SemanticVersion
{
    private readonly Match _match;

    public SemanticVersion(string versionString)
    {
        VersionString = versionString;
        _match = SemanticVersionRegex.Match(VersionString);

        Major = int.Parse(_match.Groups["major"].Value);
        Minor = int.Parse(_match.Groups["minor"].Value);
        Patch = int.Parse(_match.Groups["patch"].Value);
        PreRelease = _match.Groups["prerelease"].Value;
        BuildMetadata = _match.Groups["buildmetadata"].Value;
    }

    protected string VersionString { get; }

    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }

    /// <summary>
    /// Pre-release build info for the version. May be an empty string. Does
    /// not include the leading hyphen.
    /// </summary>
    public string PreRelease { get; }

    /// <summary>
    /// Build metadata for the version. May be an empty string if the version
    /// has no bulid metadata. Does not include the leading plus sign.
    /// </summary>
    public string BuildMetadata { get; }

    /// <summary>
    /// Implicitly converts a string to a <see cref="SemanticVersion"/>.
    /// </summary>
    public static implicit operator SemanticVersion(string versionString) => new(versionString);

    /// <inheritdoc/>
    public override string ToString() => VersionString;

    // Semantic version 2.0.0 regex from https://semver.org/
    [GeneratedRegex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$")]
    private static partial Regex SemanticVersionRegex { get; }
}
