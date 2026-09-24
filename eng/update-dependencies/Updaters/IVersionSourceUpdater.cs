// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

/// <summary>
/// An <see cref="IVersionUpdater"/> whose versions can come from more than one location.
/// </summary>
public interface IVersionSourceUpdater : IVersionUpdater
{
    Task<DependencyUpdate> ResolveFromVersionAsync(
        string version,
        VersionSource source,
        CancellationToken cancellationToken);

    Task<DependencyUpdate> IVersionUpdater.ResolveFromVersionAsync(
        string version,
        CancellationToken cancellationToken) =>
        ResolveFromVersionAsync(version, new VersionSource(), cancellationToken);
}
