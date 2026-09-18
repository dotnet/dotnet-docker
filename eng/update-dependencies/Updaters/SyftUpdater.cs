// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Octokit;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class SyftUpdater(IReleasesClient releases) : IGitHubReleaseUpdater
{
    public const string Owner = "anchore";
    public const string Repo = "syft";
    public const string VariableName = "syft|version";

    public static string Name => "syft";
    public static string VersionSourceName => Name;

    public async Task UpdateFromGitHubReleaseAsync(
        ManifestVariables variables,
        CancellationToken cancellationToken)
    {
        Release release = await releases.GetLatest(Owner, Repo).WaitAsync(cancellationToken);

        if (variables.ShouldUpdateLiteral(VariableName))
        {
            variables.SetValue(VariableName, release.TagName);
        }
    }
}
