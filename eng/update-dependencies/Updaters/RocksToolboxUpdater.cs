// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Octokit;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class RocksToolboxUpdater(IReleasesClient releases) : IGitHubReleaseUpdater
{
    private const string Owner = "canonical";
    private const string Repo = "rocks-toolbox";

    public static string Name => "rocks-toolbox";
    public static string VersionSourceName => Name;

    public async Task<DependencyUpdate> ResolveFromGitHubReleaseAsync(CancellationToken cancellationToken)
    {
        Release release = await releases.GetLatest(Owner, Repo).WaitAsync(cancellationToken);
        string variableName = $"{Name}|latest|version";

        return new DependencyUpdate(
            Scope: "",
            Description: $"Update {Name} to {release.TagName}",
            ApplyAsync: (variables, _, _) =>
            {
                if (variables.ShouldUpdateLiteral(variableName))
                {
                    variables.SetValue(variableName, release.TagName);
                }

                return Task.CompletedTask;
            });
    }
}
