// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.ProductConstructionService.Client.Models;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public interface IBarBuildUpdater : IUpdater
{
    Task<DependencyUpdate> ResolveFromBarBuildAsync(Build build, CancellationToken cancellationToken);
}
