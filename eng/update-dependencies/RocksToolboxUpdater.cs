// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Octokit;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public sealed class RocksToolboxUpdater(IReleasesClient releases) : IGitHubReleaseUpdater
{
    public const string ToolName = Repo;

    private const string Owner = "canonical";

    private const string Repo = "rocks-toolbox";

    public async Task UpdateFromGitHubReleaseAsync(
        ManifestVariables variables,
        CancellationToken cancellationToken)
    {
        Release release = await releases.GetLatest(Owner, Repo).WaitAsync(cancellationToken);

        string variableName = $"{ToolName}|latest|version";
        if (variables.ShouldUpdateLiteral(variableName))
        {
            variables.SetValue(variableName, release.TagName);
        }
    }
}
