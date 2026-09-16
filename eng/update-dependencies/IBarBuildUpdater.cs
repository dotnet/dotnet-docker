// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.ProductConstructionService.Client.Models;

namespace Dotnet.Docker;

public interface IBarBuildUpdater : IUpdater
{
    Task UpdateFromBarBuildAsync(
        ManifestVariables variables,
        string repoRoot,
        Build build,
        CancellationToken cancellationToken);
}
