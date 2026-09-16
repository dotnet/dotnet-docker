// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

public record PipelineBuildReference(string Organization, string Project, int RunId);

public interface IPipelineBuildUpdater : IUpdater
{
    Task UpdateFromPipelineBuildAsync(
        ManifestVariables variables,
        PipelineBuildReference build,
        CancellationToken cancellationToken);
}
