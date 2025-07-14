// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using NuGet.Versioning;

namespace Dotnet.Docker;

/// <summary>
/// Represents the version of a .NET build artifact.
/// </summary>
/// <remarks>
/// Additional notes about .NET versions:
/// - <see cref="SemanticVersion.Patch"/>: For runtime versions, this is number typically starts at
///   0 and counts up with new releases. For SDK versions, this number starts at 100 and can be as
///   high as 400+.
/// - <see cref="SemanticVersion.Release"/>: For stable release .NET versions, this is typically
///   empty. For preview .NET versions, it is usually a suffix like "preview.6.12345.678-shipping".
///   For internal builds of stable versions, it can be a suffix like "servicing.12345.6".
/// </remarks>
internal partial class DotNetVersion : SemanticVersion
{
    public DotNetVersion(SemanticVersion version) : base(version)
    {
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="DotNetVersion"/>.
    /// </summary>
    public static implicit operator DotNetVersion(string versionString)
    {
        SemanticVersion version = SemanticVersion.Parse(versionString);
        return new(version);
    }

    /// <summary>
    /// Whether the .NET version is a public preview version.
    /// </summary>
    public bool IsPublicPreview =>
        // Assume all "preview" versions are public, non-security releases.
        // Assume that all "rc" and "servicing" versions are internal security releases.
        Release.StartsWith("preview", StringComparison.OrdinalIgnoreCase);
}
