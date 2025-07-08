// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotnet.Docker;

internal static class VersionHelper
{
    public static Version ResolveMajorMinorVersion(string versionString)
    {
        string[] versionParts = versionString.Split('.');
        if (versionParts.Length < 2)
        {
            throw new InvalidOperationException($"Could not parse major-minor version from '{versionString}'.");
        }

        return new Version(major: int.Parse(versionParts[0]), minor: int.Parse(versionParts[1]));
    }

    /// <summary>
    /// Given a build version or product version, resolve the product version.
    /// </summary>
    /// <param name="buildVersion">
    /// A version that may already be a simple 3-part version (e.g. "8.0.100"),
    /// or a full build version (e.g. "8.0.100-servicing.12345.6").
    /// </param>
    /// <param name="isStableRelease"
    /// Whether the build version is a stable release. This should be false
    /// for preview releases only. Release candidates are considered stable.
    /// </param>
    /// <returns>
    /// The product version, which doesn't have a suffix like `-servicing*`,
    /// `-rc*` etc.
    /// </returns>
    public static string ResolveProductVersion(string buildVersion, bool isStableRelease = false)
    {
        if (!string.IsNullOrEmpty(buildVersion) && isStableRelease)
        {
            int monikerSeparatorIndex = buildVersion.IndexOf('-');
            if (monikerSeparatorIndex >= 0)
            {
                return buildVersion.Substring(0, monikerSeparatorIndex);
            }
        }

        return buildVersion;
    }

    /// <summary>
    /// Given a list of .NET SDK versions, return the highest version. For example, given the
    /// following versions:
    /// <code>
    /// 8.0.409-servicing.25218.4
    /// 8.0.312-servicing.25218.5
    /// 8.0.116-servicing.25218.9
    /// </code>
    /// The version <c>8.0.409-servicing.25218.4</c> will be returned.
    /// </summary>
    /// <param name="sdkVersions">List of versions</param>
    /// <returns>The highest version</returns>
    public static string GetHighestSdkVersion(IEnumerable<string> sdkVersions) =>
        sdkVersions
            .OrderByDescending(version => new Version(version.Split('-')[0]))
            .First();
}
