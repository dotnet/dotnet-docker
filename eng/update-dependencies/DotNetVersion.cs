// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.RegularExpressions;

namespace Dotnet.Docker;

internal partial class DotNetVersion
{
    private readonly string _versionString;
    private readonly Lazy<Match> _match;

    public DotNetVersion(string versionString)
    {
        _versionString = versionString;
        _match = new(() => SemanticVersionRegex.Match(_versionString));
    }

    // Allow a string to be implicitly converted to DotNetVersion
    public static implicit operator DotNetVersion(string versionString) => new(versionString);

    // Allow implicit conversion to string
    public override string ToString() => _versionString;

    /// <summary>
    /// .NET major version (e.g. "8", "9", or "10")
    /// </summary>
    public string Major => _match.Value.Groups["major"].Value;

    /// <summary>
    /// .NET minor version (usually "0")
    /// </summary>
    public string Minor => _match.Value.Groups["minor"].Value;

    /// <summary>
    /// .NET patch version. For runtime versions, this is number typically
    /// starts at 0, and for SDK versions, it typically starts at 100, 200,
    /// 300, or 400.
    /// </summary>
    public string Patch => _match.Value.Groups["patch"].Value;

    /// <summary>
    /// Refers to the "prerelease" portion of the version string in semantic
    /// versioning terms. For stable release .NET versions, this is typically
    /// empty. For preview .NET versions, it is usually a suffix like
    /// "preview.6.12345.678-shipping". For internal builds of stable verisons,
    /// it can be a suffix like "servicing.12345.6".
    /// </summary>
    /// <remarks>
    /// Does not include the leading hyphen.
    /// </remarks>
    public string Prerelease => _match.Value.Groups["prerelease"].Value;

    /// <summary>
    /// Whether the .NET version is a public preview version.
    /// </summary>
    public bool IsPublicPreview =>
        // Assume all "preview" versions are public, non-security releases.
        // Assume that all "rc" and "servicing" versions are internal security releases.
        Prerelease.StartsWith("preview", StringComparison.OrdinalIgnoreCase);

    // Semantic version 2.0.0 regex from https://semver.org/
    [GeneratedRegex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$")]
    private static partial Regex SemanticVersionRegex { get; }
}
