// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.Docker.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal partial class FromStagingPipelineCommand : ICommand<FromStagingPipelineOptions>
{
    /// <summary>
    /// Callback that stages all changes and commits them.
    /// </summary>
    private delegate Task CommitChanges(string commitMessage);

    /// <summary>
    /// Callback that pushes all commits and creates a pull request.
    /// </summary>
    private delegate Task PushAndCreatePullRequest(string prTitle, string prBody);

    private readonly ILogger<FromStagingPipelineCommand> _logger;
    private readonly UpdateDependenciesConfiguration _configuration;
    private readonly IPipelineArtifactProvider _pipelineArtifactProvider;
    private readonly IPipelinesService _pipelinesService;
    private readonly IInternalVersionsService _internalVersionsService;
    private readonly DotNetUpdater _updater;
    private readonly IEnvironmentService _environmentService;
    private readonly IBuildLabelService _buildLabelService;
    private readonly Func<FromStagingPipelineOptions, Task<GitRepoContext>> _createGitRepoContextAsync;

    public FromStagingPipelineCommand(
        UpdateDependenciesConfiguration configuration,
        ILogger<FromStagingPipelineCommand> logger,
        DotNetUpdater updater,
        IPipelineArtifactProvider pipelineArtifactProvider,
        IPipelinesService pipelinesService,
        IInternalVersionsService internalVersionsService,
        IEnvironmentService environmentService,
        IBuildLabelService buildLabelService,
        IGitRepoHelperFactory gitRepoHelperFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _updater = updater;
        _pipelineArtifactProvider = pipelineArtifactProvider;
        _pipelinesService = pipelinesService;
        _internalVersionsService = internalVersionsService;
        _environmentService = environmentService;
        _buildLabelService = buildLabelService;
        _createGitRepoContextAsync = options => GitRepoContext.CreateAsync(_logger, gitRepoHelperFactory, options, _environmentService, configuration);
    }

    public static Command Create(IServiceProvider services)
    {
        var command = new Command("staging-pipeline", "Update .NET from staging pipeline runs");
        FromStagingPipelineOptions.AddTo(command);
        command.SetAction((result, _) =>
        {
            FromStagingPipelineOptions options = FromStagingPipelineOptions.Bind(result);
            return services.GetRequiredService<ICommand<FromStagingPipelineOptions>>().ExecuteAsync(options);
        });
        return command;
    }

    public async Task<int> ExecuteAsync(FromStagingPipelineOptions options)
    {
        var stageContainers = options.GetStageContainerList();

        if (stageContainers.Count == 0)
        {
            _logger.LogError("No stage containers provided.");
            return 1;
        }

        _logger.LogInformation(
            "Updating dependencies based on {Count} stage container(s): {StageContainers}",
            stageContainers.Count,
            string.Join(", ", stageContainers));

        // Delegate all git responsibilities to GitRepoContext. Depending on what options were
        // passed in, we may or may not want to actually perform git operations. GitRepoContext
        // decides what git operations to perform and tells us where to make changes. This keeps
        // all the git-related logic in one place.
        var gitRepoContext = await _createGitRepoContextAsync(options);

        List<string> commitMessages = [];
        List<string> prBodySections = [];

        // Process each stage container, creating a separate commit for each
        foreach (var stageContainer in stageContainers)
        {
            _logger.LogInformation("Processing stage container: {StageContainer}", stageContainer);

            var (commitMessage, prBodySection) = await ProcessStageContainerAsync(
                options,
                stageContainer,
                gitRepoContext);

            // Commit changes for this stage container
            await gitRepoContext.CommitChanges(commitMessage);

            commitMessages.Add(commitMessage);
            prBodySections.Add(prBodySection);
        }

        // Create pull request with all commits
        var prTitle = stageContainers.Count == 1
            ? $"[{options.TargetBranch}] {commitMessages[0]}"
            : $"[{options.TargetBranch}] Update .NET dependencies from {stageContainers.Count} stage containers";

        var prBody = string.Join(Environment.NewLine + Environment.NewLine, prBodySections);
        await gitRepoContext.PushAndCreatePullRequest(prTitle, prBody);

        return 0;
    }

    /// <summary>
    /// Processes a single stage container and applies the updates.
    /// </summary>
    /// <returns>
    /// A tuple containing the commit message and PR body section.
    /// </returns>
    private async Task<(string CommitMessage, string PrBodySection)> ProcessStageContainerAsync(
        FromStagingPipelineOptions options,
        string stageContainer,
        GitRepoContext gitRepoContext)
    {
        var stagingPipelineRunId = StagingPipelineOptionsExtensions.GetStagingPipelineRunId(stageContainer);

        // Log staging pipeline tags for diagnostic purposes
        var stagingPipelineTags = await _pipelinesService.GetBuildTagsAsync(
            _configuration.AzureDevOps.Organization,
            _configuration.AzureDevOps.Project,
            stagingPipelineRunId);
        _logger.LogInformation("Staging pipeline tags: {Tags}", string.Join(", ", stagingPipelineTags));

        string internalBaseUrl = string.Empty;
        if (options.Internal)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(
                options.StagingStorageAccount,
                $"{FromStagingPipelineOptions.StagingStorageAccountOptionName} must be set when using the {FromStagingPipelineOptions.InternalOption} option."
            );

            // Release metadata is stored in metadata/ReleaseManifest.json.
            // Release assets are stored individually under in assets/shipping/assets/[Sdk|Runtime|aspnetcore|...].
            // Full example: https://dotnetstagetest.blob.core.windows.net/stage-2XXXXXX/assets/shipping/assets/Runtime/10.0.0-preview.N.XXXXX.YYY/dotnet-runtime-10.0.0-preview.N.XXXXX.YYY-linux-arm64.tar.gz
            _buildLabelService.AddBuildTags($"Container - {stageContainer}");
            internalBaseUrl = NormalizeStorageAccountUrl(options.StagingStorageAccount)
                + $"/{stageContainer}/assets/shipping/assets";
        }

        var releaseConfig = await _pipelineArtifactProvider.GetReleaseConfigAsync(
            _configuration.AzureDevOps.Organization,
            _configuration.AzureDevOps.Project,
            stagingPipelineRunId);

        string dotnetProductVersion = VersionHelper.ResolveProductVersion(releaseConfig.RuntimeBuild);
        DotNetVersion dotNetVersion = DotNetVersion.Parse(releaseConfig.RuntimeBuild);
        string majorMinorVersionString = dotNetVersion.ToString(2);

        if (options.Internal)
        {
            // Record stage container for this internal version, for later use by sync-internal-release command
            _internalVersionsService.RecordInternalStagingBuild(
                repoRoot: gitRepoContext.LocalRepoPath,
                dotNetVersion: dotNetVersion,
                stageContainer: stageContainer);
        }

        string sdkVersion = VersionHelper.GetHighestSdkVersion(options.Internal ? releaseConfig.SdkBuilds : releaseConfig.Sdks);
        string runtimeVersion = options.Internal ? releaseConfig.RuntimeBuild : releaseConfig.Runtime;
        string? aspnetVersion = options.Internal ? releaseConfig.AspBuild : releaseConfig.Asp;

        // Example build URL: https://dev.azure.com/<org>/<project>/_build/results?buildId=<stagingPipelineRunId>
        var buildUrl = $"{_configuration.AzureDevOps.Organization.TrimEnd('/')}/{_configuration.AzureDevOps.Project}/_build/results?buildId={stagingPipelineRunId}";
        _logger.LogInformation(
            "Applying internal build {StageContainer} ({BuildUrl})",
            stageContainer, buildUrl);

        await DependencyUpdateRunner.ApplyAsync(
            gitRepoContext.LocalRepoPath,
            (variables, repoRoot, token) => _updater.UpdateFromStagingPipelineAsync(
                variables, repoRoot, releaseConfig, internalBaseUrl, token),
            CancellationToken.None);

        var commitMessage = releaseConfig switch
        {
            { SdkOnly: true } => $"Update .NET {majorMinorVersionString} SDK to {sdkVersion}",
            _ => $"Update .NET {majorMinorVersionString} to {sdkVersion} SDK / {runtimeVersion} Runtime",
        };

        List<string> newVersionsList = [];
        if (!options.Internal || !releaseConfig.SdkOnly)
        {
            newVersionsList.Add($"- DOTNET: {dotnetProductVersion}");
            newVersionsList.Add($"- RUNTIME: {runtimeVersion}");
            newVersionsList.Add($"- ASPNET: {aspnetVersion}");
            newVersionsList.Add($"- ASPNET-COMPOSITE: {aspnetVersion}");
        }
        newVersionsList.Add($"- SDK: {sdkVersion}");
        var prBodySection = $"""
            ## .NET {majorMinorVersionString}

            This updates .NET {majorMinorVersionString} to the following versions:

            {string.Join(Environment.NewLine, newVersionsList)}

            These versions are from .NET staging pipeline run [{stageContainer}]({buildUrl}).
            """;

        return (commitMessage, prBodySection);
    }

    /// <summary>
    /// Formats a storage account URL has a specific format:
    /// - Starts with "https://"
    /// - No trailing slash
    /// - Defaults to using blob.core.windows.net as the root domain
    /// </summary>
    private static string NormalizeStorageAccountUrl(string storageAccount)
    {
        if (string.IsNullOrWhiteSpace(storageAccount))
        {
            return storageAccount;
        }

        storageAccount = storageAccount.Trim();

        if (storageAccount.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return storageAccount.TrimEnd('/');
        }

        // If it's just the storage account name, construct the full URL
        return $"https://{storageAccount}.blob.core.windows.net";
    }

    /// <summary>
    /// Holds context about the git repository where changes should be made.
    /// </summary>
    /// <param name="LocalRepoPath">Root of the repo where all changes should be made.</param>
    /// <param name="CommitChanges">Callback that commits changes with the given message.</param>
    /// <param name="PushAndCreatePullRequest">Callback that pushes all commits and creates a pull request.</param>
    private record GitRepoContext(
        string LocalRepoPath,
        CommitChanges CommitChanges,
        PushAndCreatePullRequest PushAndCreatePullRequest)
    {
        /// <summary>
        /// Sets up the remote/local git repository based on <paramref name="options"/>.
        /// Call this before making any changes, then make changes to <see cref="LocalRepoPath"/>
        /// and use <see cref="CommitChanges"/> to commit each change individually,
        /// then use <see cref="PushAndCreatePullRequest"/> to push all commits and create a pull request.
        /// </summary>
        /// <remarks>
        /// If <see cref="FromStagingPipelineOptions.Mode"/> is <see cref="ChangeMode.Local"/>,
        /// no git operations will be performed.
        /// </remarks>
        public static async Task<GitRepoContext> CreateAsync(
            ILogger logger,
            IGitRepoHelperFactory gitRepoFactory,
            FromStagingPipelineOptions options,
            IEnvironmentService environmentService,
            UpdateDependenciesConfiguration configuration)
        {
            CommitChanges commitChanges;
            PushAndCreatePullRequest pushAndCreatePullRequest;
            string localRepoPath;

            if (options.Mode == ChangeMode.Remote)
            {
                var remoteUrl = configuration.AzureDevOps.GetRepoUrl();
                var targetBranch = options.TargetBranch;
                var buildId = environmentService.GetBuildId() ?? "";
                var stageContainerList = options.GetStageContainerList();
                if (stageContainerList.Count == 0)
                {
                    throw new ArgumentException("At least one stage container must be provided.");
                }
                var prBranch = options.CreatePullRequestBranchName(
                    $"update-deps-int-{stageContainerList[0]}",
                    buildId);
                var committer = configuration.GetCommitterIdentity();

                // Clone the repo and configure git identity for commits
                var git = await gitRepoFactory.CreateAndCloneAsync(remoteUrl, gitIdentity: committer);
                // Ensure the branch we want to modify exists, then check it out
                await git.Remote.EnsureBranchExistsAsync(targetBranch);
                // Create a new branch to push changes to and create a PR from
                await git.CheckoutRemoteBranchAsync(targetBranch);
                await git.Local.CreateAndCheckoutLocalBranchAsync(prBranch);

                localRepoPath = git.Local.LocalPath;

                commitChanges = async (commitMessage) =>
                {
                    await git.Local.StageAsync(".");
                    await git.Local.CommitAsync(commitMessage, committer);
                };

                pushAndCreatePullRequest = async (prTitle, prBody) =>
                {
                    await git.PushLocalBranchAsync(prBranch);
                    await git.Remote.CreatePullRequestAsync(new(
                        Title: prTitle,
                        Body: prBody,
                        HeadBranch: prBranch,
                        BaseBranch: targetBranch
                    ));
                };
            }
            else
            {
                logger.LogInformation("No git operations will be performed in {Mode} mode.", options.Mode);
                localRepoPath = options.RepoRoot;

                commitChanges = async (commitMessage) =>
                {
                    logger.LogInformation("Skipping commit in {Mode} mode.", options.Mode);
                    logger.LogInformation("Commit message: {CommitMessage}", commitMessage);
                };

                pushAndCreatePullRequest = async (prTitle, prBody) =>
                {
                    logger.LogInformation("Skipping push and pull request creation in {Mode} mode.", options.Mode);
                    logger.LogInformation("Pull request title: {PullRequestTitle}", prTitle);
                    logger.LogInformation("Pull request body:\n{PullRequestBody}", prBody);
                };
            }

            return new GitRepoContext(localRepoPath, commitChanges, pushAndCreatePullRequest);
        }
    }
}
