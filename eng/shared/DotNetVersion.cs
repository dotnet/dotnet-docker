// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.Shared;

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
public sealed partial class DotNetVersion(SemanticVersion version) : SemanticVersion(version)
{
    /// <summary>
    /// Parse a string into a <see cref="DotNetVersion"/>. This is not a
    /// "strict" parse. Major-only versions are allowed. Unspecified minor and
    /// patch versions default to 0.
    /// </summary>
    public static new DotNetVersion Parse(string versionString)
    {
        // SemanticVersion only handles full, strict semantic versions.
        // If we can parse it as a full semantic version, use that. It resolves
        // full build versions correctly including prerelease suffixes (e.g. '-preview.7')
        if (SemanticVersion.TryParse(versionString, out var fullSemanticVersion))
        {
            return new DotNetVersion(fullSemanticVersion);
        }

        // We also need to allow major-only and major.minor versions, so fall
        // back to simple parsing when that happens.
        // A valid major.minor.patch version would have been handled above so
        // we don't need to account for that in our parsing.
        // Don't use System.Version.Parse here, because it defaults unmatched
        // minor/patch versions to -1, whereas we want them to default to 0.
        var matchGroups = MajorMinorVersionRegex.Match(versionString).Groups;
        int major = int.Parse(matchGroups["major"].Value);
        int minor = int.TryParse(matchGroups["minor"].Value.TrimStart('.'), out int minorValue) ? minorValue : 0;
        return new DotNetVersion(new SemanticVersion(major, minor, 0));
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="DotNetVersion"/>.
    /// </summary>
    public static implicit operator DotNetVersion(string versionString)
    {
        SemanticVersion version = Parse(versionString);
        return new(version);
    }

    /// <summary>
    /// Formats the version to a string with the specified number of parts.
    /// </summary>
    /// <param name="parts">
    /// Number of parts to include. If greater than 3, the full version is
    /// returned, whether or not there are any additional parts.
    /// </param>
    public string ToString(int parts) => parts switch
    {
        < 1 => throw new ArgumentOutOfRangeException(nameof(parts), "Cannot format less than 1 version part"),
        1 => $"{Major}",
        2 => $"{Major}.{Minor}",
        3 => $"{Major}.{Minor}.{Patch}",
        > 3 => ToString(),
    };

    /// <summary>
    /// Whether the .NET version is a public preview version.
    /// </summary>
    public bool IsPublicPreview =>
        // Assume all "preview" versions are public, non-security releases.
        // Assume that all "rc" and "servicing" versions are internal security releases.
        Release.StartsWith("preview", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Determines if the version is considered a GA version.
    /// </summary>
    /// <remarks>
    /// RTM versions are also accepted as GA versions.
    /// </remarks>
    public bool IsGA => ReleaseLabels.Contains("rtm") || !IsPrerelease;

    [GeneratedRegex(@"^(?<major>\d+)(?<minor>\.\d+)?")]
    private static partial Regex MajorMinorVersionRegex { get; }
}
