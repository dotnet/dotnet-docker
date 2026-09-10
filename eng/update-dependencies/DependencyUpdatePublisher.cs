// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.DotNet.GitAutomation;
using Microsoft.DotNet.GitAutomation.AzureDevOps;
using Microsoft.DotNet.GitAutomation.GitHub;

namespace Dotnet.Docker;

internal sealed class DependencyUpdatePublisher(SpecificCommandOptions options)
{
    public async Task PublishAsync(
        Func<IGitContext, CancellationToken, Task> applyUpdates,
        CancellationToken cancellationToken = default)
    {
        using var httpClient = new HttpClient();
        IPullRequestEndpoint endpoint = CreatePullRequestEndpoint(httpClient);

        string title = $"[{options.TargetBranch}] Update dependencies from {options.VersionSourceName}";

        // Keep the existing GitHub branch name so scheduled runs find their open PRs.
        string branchSuffix = $"UpdateDependencies-{options.TargetBranch}-From-{options.VersionSourceName}";
        branchSuffix = branchSuffix.Replace('/', '-');
        string branchName = $"{options.TargetBranch}-{branchSuffix}";

        var definition = new PullRequestDefinition(
            Key: branchName,
            Title: title,
            Body: string.Empty,
            TargetBranch: options.TargetBranch,
            ApplyChanges: applyUpdates);

        var manager = new PullRequestManager();
        PullRequestResult result = await manager.CreateOrUpdateAsync(
            definition,
            endpoint,
            updateStrategy: PullRequestUpdateStrategy.Append,
            onForeignCommits: ForeignCommitPolicy.Proceed,
            cancellationToken: cancellationToken);

        Trace.TraceInformation($"Pull request: {result.Action} {result.Url}");
    }

    private IPullRequestEndpoint CreatePullRequestEndpoint(HttpClient httpClient)
    {
        var identity = new AutomationIdentity(options.User, options.Email);
        bool useAzureDevOps =
            !string.IsNullOrEmpty(options.AzdoOrganization)
            && !string.IsNullOrEmpty(options.AzdoProject)
            && !string.IsNullOrEmpty(options.AzdoRepo);

        if (useAzureDevOps)
        {
            // The CLI accepts an organization URL, rather than the library's organization name.
            string project = Uri.EscapeDataString(options.AzdoProject);
            httpClient.BaseAddress = new Uri($"{options.AzdoOrganization}/{project}/_apis/git/repositories/");

            byte[] credentials = Encoding.UTF8.GetBytes($":{options.Password}");
            string encodedCredentials = Convert.ToBase64String(credentials);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);

            // PublishAsync owns the HTTP client for the lifetime of the endpoint.
            var client = new AzureDevOpsClient(httpClient);
            return new AzureDevOpsPullRequestEndpoint(
                client,
                options.AzdoRepo,
                options.Password,
                identity);
        }

        var accessProvider = new StaticGitHubAccessProvider(options.Password, identity);
        var upstream = new GitHubRepo(options.GitHubUpstreamOwner, options.GitHubProject);
        var fork = new GitHubRepo(options.User, options.GitHubProject);
        return new GitHubPullRequestEndpoint(accessProvider, upstream, fork);
    }
}
