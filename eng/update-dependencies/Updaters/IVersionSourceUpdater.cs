// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

/// <summary>
/// Like <see cref="IVersionUpdater"/>, for dependencies whose versions are published to more
/// than one location.
/// </summary>
public interface IVersionSourceUpdater : IUpdater
{
    Task<DependencyUpdate> ResolveFromVersionAsync(
        string version,
        VersionSource source,
        CancellationToken cancellationToken);
}
