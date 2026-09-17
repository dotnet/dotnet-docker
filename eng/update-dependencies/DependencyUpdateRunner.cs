// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.GitAutomation;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public sealed class DependencyUpdateRunner(PullRequestManager manager, IPullRequestEndpoint endpoint)
{
    public async Task RunAsync(
        CreatePullRequestOptions options,
        string versionSourceName,
        Func<ManifestVariables, string, CancellationToken, Task> applyUpdates,
        CancellationToken cancellationToken)
    {
        if (options.UpdateOnly)
        {
            await ApplyAsync(options.RepoRoot, applyUpdates, cancellationToken);
            return;
        }

        string title = $"[{options.TargetBranch}] Update dependencies from {versionSourceName}";

        // Keep the existing GitHub branch name so scheduled runs find their open PRs.
        string branchSuffix = $"UpdateDependencies-{options.TargetBranch}-From-{versionSourceName}";
        branchSuffix = branchSuffix.Replace('/', '-');
        string branchName = $"{options.TargetBranch}-{branchSuffix}";

        var definition = new PullRequestDefinition(
            Key: branchName,
            Title: title,
            Body: string.Empty,
            TargetBranch: options.TargetBranch,
            ApplyChanges: (git, token) => ApplyAsync(git.WorkspaceDirectory, applyUpdates, token));

        PullRequestResult result = await manager.CreateOrUpdateAsync(
            definition,
            endpoint,
            cancellationToken: cancellationToken);

        Trace.TraceInformation($"Pull request: {result.Action} {result.Url}");
    }

    public static async Task ApplyAsync(
        string repoRoot,
        Func<ManifestVariables, string, CancellationToken, Task> applyUpdates,
        CancellationToken cancellationToken)
    {
        repoRoot = Path.GetFullPath(repoRoot);
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        var variables = ManifestVariables.FromFile(manifestPath);
        string originalContent = variables.Content;

        await applyUpdates(variables, repoRoot, cancellationToken);

        if (variables.Content != originalContent)
        {
            await File.WriteAllTextAsync(manifestPath, variables.Content, cancellationToken);
        }

        // Generators can change files even when the manifest is unchanged.
        await ScriptRunner.GenerateDockerfilesAsync(repoRoot, cancellationToken);
        await ScriptRunner.GenerateReadmesAsync(repoRoot, cancellationToken);
    }

}
