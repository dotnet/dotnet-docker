// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies.Model.Release;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public interface IStagingPipelineUpdater : IUpdater
{
    Task UpdateFromStagingPipelineAsync(
        ManifestVariables variables,
        string repoRoot,
        ReleaseConfig release,
        string internalBaseUrl,
        CancellationToken cancellationToken);
}
