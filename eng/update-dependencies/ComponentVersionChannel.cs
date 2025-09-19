// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// Logical version channel for an image component (e.g. stable, preview, lts,
/// daily, 8.0, 9.0 - there is no set format). This can be used by
/// <see cref="IDependencyVersionSource"/> implementations however they see fit.
/// </summary>
/// <param name="Name"></param>
internal sealed record ComponentVersionChannel(string Name)
{
    public static ComponentVersionChannel Default = new("default");
}
