// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public interface ILatestVersionUpdater : IUpdater
{
    /// <summary>
    /// Finds the newest available versions after the ones currently in the manifest.
    /// </summary>
    /// <returns>One update for each group of images that can be updated. Empty when everything is up to date.</returns>
    Task<IReadOnlyList<DependencyUpdate>> ResolveLatestAsync(
        ManifestVariables variables,
        bool isInternal,
        string? dotnetVersion,
        CancellationToken cancellationToken);
}
