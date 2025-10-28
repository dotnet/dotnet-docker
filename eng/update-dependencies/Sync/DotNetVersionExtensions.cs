// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.Shared;
using NuGet.Versioning;

namespace Dotnet.Docker.Sync;

/// <summary>
/// <see cref="DotNetVersion"/> Extensions that are only used for release branch synchronization.
/// </summary>
internal static class DotNetVersionExtensions
{
    /// <summary>
    /// Converts a <see cref="DotNetVersion"/> to a new <see cref="DotNetVersion"/>
    /// with only the major and minor version parts.
    /// </summary>
    public static DotNetVersion ToMajorMinorVersion(this DotNetVersion version) =>
        new DotNetVersion(new SemanticVersion(version.Major, version.Minor, 0));
}
