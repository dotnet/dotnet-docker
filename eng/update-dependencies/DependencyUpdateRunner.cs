// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.GitAutomation;
using Microsoft.DotNet.GitAutomation.AzureDevOps;
using Microsoft.DotNet.GitAutomation.GitHub;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public sealed class DependencyUpdateRunner(
    UpdateDependenciesConfiguration configuration,
    ILoggerFactory loggerFactory)
{
    public async Task RunAsync(
        CreatePullRequestOptions options,
        string versionSourceName,
        DependencyUpdate update,
        CancellationToken cancellationToken)
    {
        if (options.UpdateOnly)
        {
            await ApplyAsync(options.RepoRoot, update.ApplyAsync, cancellationToken);
            return;
        }

        string title = $"[{options.TargetBranch}] {update.Description}";

        // The scope keeps updates that can be open at the same time, such as different
        // .NET versions, on separate branches.
        string branchSuffix = $"UpdateDependencies-{options.TargetBranch}-From-{versionSourceName}";
        if (update.Scope.Length > 0)
        {
            branchSuffix += $"-{update.Scope}";
        }

        branchSuffix = branchSuffix.Replace('/', '-');
        string branchName = $"{options.TargetBranch}-{branchSuffix}";

        var definition = new PullRequestDefinition(
            Key: branchName,
            Title: title,
            Body: string.Empty,
            TargetBranch: options.TargetBranch,
            ApplyChanges: (git, token) => ApplyAsync(git.WorkspaceDirectory, update.ApplyAsync, token));

        var manager = new PullRequestManager(loggerFactory);
        PullRequestResult result = await manager.CreateOrUpdateAsync(
            definition,
            CreatePullRequestEndpoint(configuration),
            cancellationToken: cancellationToken);

        Trace.TraceInformation($"Pull request: {result.Action} {result.Url}");
    }

    public static async Task ApplyAsync(
        string repoRoot,
        ApplyUpdateAsync applyUpdate,
        CancellationToken cancellationToken)
    {
        repoRoot = Path.GetFullPath(repoRoot);
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        var variables = ManifestVariables.FromFile(manifestPath);
        string originalContent = variables.Content;

        await applyUpdate(variables, repoRoot, cancellationToken);

        if (variables.Content != originalContent)
        {
            await File.WriteAllTextAsync(manifestPath, variables.Content, cancellationToken);
        }

        // Generators can change files even when the manifest is unchanged.
        await ScriptRunner.GenerateDockerfilesAsync(repoRoot, cancellationToken);
        await ScriptRunner.GenerateReadmesAsync(repoRoot, cancellationToken);
    }

    private static IPullRequestEndpoint CreatePullRequestEndpoint(
        UpdateDependenciesConfiguration configuration) =>
        configuration.PullRequestDestination switch
        {
            GitRemote.GitHub => CreateGitHubPullRequestEndpoint(configuration),
            GitRemote.AzureDevOps => CreateAzureDevOpsPullRequestEndpoint(configuration),
            _ => throw new InvalidOperationException(
                $"Unsupported PR destination: {configuration.PullRequestDestination}"),
        };

    private static GitHubPullRequestEndpoint CreateGitHubPullRequestEndpoint(
        UpdateDependenciesConfiguration configuration)
    {
        var github = configuration.GitHub;
        var identity = new AutomationIdentity(configuration.User, configuration.Email);
        var repo = new GitHubRepo(github.Owner, github.Repository);
        IGitHubAccessProvider accessProvider;

        if (!string.IsNullOrEmpty(github.AppClientId)
            && !string.IsNullOrEmpty(github.AppKeyUri))
        {
            var credential = new AzureCliCredential();
            var cryptographyClient = new CryptographyClient(new Uri(github.AppKeyUri), credential);
            accessProvider = new GitHubAppAccessProvider(github.AppClientId, cryptographyClient);
        }
        else
        {
            accessProvider = new StaticGitHubAccessProvider(github.Token, identity);
        }

        return new GitHubPullRequestEndpoint(accessProvider, repo);
    }

    private static AzureDevOpsPullRequestEndpoint CreateAzureDevOpsPullRequestEndpoint(
        UpdateDependenciesConfiguration configuration)
    {
        var azureDevOps = configuration.AzureDevOps;
        string organization = new Uri(azureDevOps.Organization).Segments.Last().TrimEnd('/');
        var client = new AzureDevOpsClient(
            organization,
            azureDevOps.Project,
            azureDevOps.Token);
        var identity = new AutomationIdentity(configuration.User, configuration.Email);

        return new AzureDevOpsPullRequestEndpoint(
            client,
            azureDevOps.Repository,
            azureDevOps.Token,
            identity);
    }
}
