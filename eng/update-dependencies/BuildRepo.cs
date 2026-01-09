// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// Represents supported source repositories for builds from the .NET build
/// asset registry.
/// </summary>
internal enum BuildRepo
{
    Vmr,
    Aspire,
    // When adding new repos, also update "BuildExtensions.GetBuildRepo()"
}
