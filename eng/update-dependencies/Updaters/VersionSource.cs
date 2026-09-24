// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

/// <summary>
/// Where to find a version, for dependencies that are published to more than one location.
/// </summary>
/// <param name="Internal">
/// Use the dependency's internal (non-public) location instead of its public one.
/// </param>
/// <param name="BaseUrl">
/// Use this exact location instead of the default public or internal one.
/// </param>
/// <param name="DotnetVersion">
/// Only update images for this .NET major.minor version, such as "9.0". When null, the updater
/// chooses which .NET versions to update.
/// </param>
public sealed record VersionSource(
    bool Internal = false,
    string? BaseUrl = null,
    string? DotnetVersion = null);
