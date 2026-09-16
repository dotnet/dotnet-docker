// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

public interface IGitHubReleaseUpdater : IUpdater
{
    Task UpdateFromGitHubReleaseAsync(
        ManifestVariables variables,
        CancellationToken cancellationToken);
}
