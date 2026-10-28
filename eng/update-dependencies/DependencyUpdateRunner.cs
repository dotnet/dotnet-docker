// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
        if (!options.SubmitPullRequest)
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

        PullRequestDefinition definition = new PullRequestDefinition(
            Key: branchName,
            Title: title,
            Body: string.Empty,
            TargetBranch: options.TargetBranch,
            ApplyChanges: (git, token) => ApplyAsync(git.WorkspaceDirectory, update.ApplyAsync, token));

        PullRequestManager manager = new PullRequestManager(loggerFactory);
        PullRequestResult result = await manager.CreateOrUpdateAsync(
            definition,
            CreatePullRequestEndpoint(configuration),
            cancellationToken: cancellationToken);

        Trace.TraceInformation($"Pull request: {result.Action} {result.Url}");
    }

    /// <summary>
    /// Loads the manifest that updates will be applied to: the local repo, or the pull request's target branch.
    /// </summary>
    public async Task<ManifestVariables> GetTargetManifestAsync(
        CreatePullRequestOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.SubmitPullRequest)
        {
            return ManifestVariables.FromFile(GetManifestPath(options.RepoRoot));
        }

        // This clone is only read. PullRequestManager makes its own clone when it applies updates.
        IPullRequestEndpoint endpoint = CreatePullRequestEndpoint(configuration);
        ProcessRunner processRunner = new ProcessRunner(loggerFactory.CreateLogger<ProcessRunner>());

        using GitWorkingCopy workingCopy = new GitWorkingCopy(processRunner, loggerFactory.CreateLogger<GitWorkingCopy>());

        AutomationIdentity identity = await endpoint.GetIdentityAsync(cancellationToken);
        await endpoint.CloneTargetAsync(workingCopy, options.TargetBranch, identity, cancellationToken);

        return ManifestVariables.FromFile(GetManifestPath(workingCopy.WorkspaceDirectory));
    }

    public static async Task ApplyAsync(
        string repoRoot,
        ApplyUpdateAsync applyUpdate,
        CancellationToken cancellationToken)
    {
        repoRoot = Path.GetFullPath(repoRoot);
        string manifestPath = GetManifestPath(repoRoot);
        ManifestVariables variables = ManifestVariables.FromFile(manifestPath);
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
        GitHubConfiguration github = configuration.GitHub;
        AutomationIdentity identity = new AutomationIdentity(configuration.User, configuration.Email);
        GitHubRepo repo = new GitHubRepo(github.Owner, github.Repository);
        IGitHubAccessProvider accessProvider;

        if (!string.IsNullOrEmpty(github.AppClientId)
            && !string.IsNullOrEmpty(github.AppKeyUri))
        {
            AzureCliCredential credential = new AzureCliCredential();
            CryptographyClient cryptographyClient = new CryptographyClient(new Uri(github.AppKeyUri), credential);
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
        AzureDevOpsConfiguration azureDevOps = configuration.AzureDevOps;
        string organization = new Uri(azureDevOps.Organization).Segments.Last().TrimEnd('/');
        AzureDevOpsClient client = new AzureDevOpsClient(
            organization,
            azureDevOps.Project,
            azureDevOps.Token);
        AutomationIdentity identity = new AutomationIdentity(configuration.User, configuration.Email);

        return new AzureDevOpsPullRequestEndpoint(
            client,
            azureDevOps.Repository,
            azureDevOps.Token,
            identity);
    }

    private static string GetManifestPath(string repoRoot) => Path.Combine(repoRoot, "manifest.versions.json");
}
