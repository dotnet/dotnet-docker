// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Model.Release;

namespace Dotnet.Docker;

public interface IStagingPipelineUpdater : IUpdater
{
    Task UpdateFromStagingPipelineAsync(
        ManifestVariables variables,
        string repoRoot,
        ReleaseConfig release,
        string internalBaseUrl,
        CancellationToken cancellationToken);
}
