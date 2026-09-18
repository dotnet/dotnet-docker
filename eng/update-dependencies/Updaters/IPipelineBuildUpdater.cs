// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public record PipelineBuildReference(string Organization, string Project, int RunId);

public interface IPipelineBuildUpdater : IUpdater
{
    Task<DependencyUpdate> ResolveFromPipelineBuildAsync(
        // TODO: Reduce to just run ID since org/project are static in Configuration
        PipelineBuildReference build,
        CancellationToken cancellationToken);
}
