// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Git;
using Dotnet.Docker.Sync;
using Microsoft.DotNet.Docker.Shared;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal partial class FromStagingPipelineCommand : BaseCommand<FromStagingPipelineOptions>
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
    private readonly IPipelineArtifactProvider _pipelineArtifactProvider;
    private readonly IPipelinesService _pipelinesService;
    private readonly IInternalVersionsService _internalVersionsService;
    private readonly IEnvironmentService _environmentService;
    private readonly IBuildLabelService _buildLabelService;
    private readonly Func<FromStagingPipelineOptions, Task<GitRepoContext>> _createGitRepoContextAsync;

    public FromStagingPipelineCommand(
        ILogger<FromStagingPipelineCommand> logger,
        IPipelineArtifactProvider pipelineArtifactProvider,
        IPipelinesService pipelinesService,
        IInternalVersionsService internalVersionsService,
        IEnvironmentService environmentService,
        IBuildLabelService buildLabelService,
        IGitRepoHelperFactory gitRepoHelperFactory)
    {
        _logger = logger;
        _pipelineArtifactProvider = pipelineArtifactProvider;
        _pipelinesService = pipelinesService;
        _internalVersionsService = internalVersionsService;
        _environmentService = environmentService;
        _buildLabelService = buildLabelService;
        _createGitRepoContextAsync = options => GitRepoContext.CreateAsync(_logger, gitRepoHelperFactory, options, _environmentService);
    }

    public override async Task<int> ExecuteAsync(FromStagingPipelineOptions options)
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

            var (commitMessage, prBodySection, exitCode) = await ProcessStageContainerAsync(
                options,
                stageContainer,
                gitRepoContext);

            if (exitCode != 0)
            {
                return exitCode;
            }

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
    /// A tuple containing the commit message, PR body section, and exit code.
    /// </returns>
    private async Task<(string CommitMessage, string PrBodySection, int ExitCode)> ProcessStageContainerAsync(
        FromStagingPipelineOptions options,
        string stageContainer,
        GitRepoContext gitRepoContext)
    {
        var stagingPipelineRunId = StagingPipelineOptionsExtensions.GetStagingPipelineRunId(stageContainer);

        // Log staging pipeline tags for diagnostic purposes
        var stagingPipelineTags = await _pipelinesService.GetBuildTagsAsync(
            options.AzdoOrganization,
            options.AzdoProject,
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
            options.AzdoOrganization,
            options.AzdoProject,
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

        var productVersions = (options.Internal, releaseConfig.SdkOnly) switch
        {
            // SDK-only internal/staging release
            (true, true) => new Dictionary<string, string?>
            {
                // SDK-only releases are almost always one-off updates/bug
                // fixes on top of an existing release of the Runtime and
                // ASP.NET Core.
                //
                // If the release config tells us that this is an
                // SDK-only release, we can assume that we have already
                // released the runtime/aspnet versions that it's based on, and
                // therefore we shouldn't update them unnecessarily.
                { "sdk", VersionHelper.GetHighestSdkVersion(releaseConfig.SdkBuilds) }
            },
            // Internal/staging release
            (true, false) => new Dictionary<string, string?>
            {
                { "dotnet", dotnetProductVersion },
                { "runtime",  releaseConfig.RuntimeBuild },
                { "aspnet", releaseConfig.AspBuild },
                { "aspnet-composite", releaseConfig.AspBuild },
                { "sdk", VersionHelper.GetHighestSdkVersion(releaseConfig.SdkBuilds) },
            },
            // Public release - whether or not it's an SDK-only release doesn't
            // matter because the product versions will end up being the same.
            (false, _) => new Dictionary<string, string?>
            {
                { "dotnet", dotnetProductVersion },
                { "runtime", releaseConfig.Runtime },
                { "aspnet", releaseConfig.Asp },
                { "aspnet-composite", releaseConfig.Asp },
                { "sdk", VersionHelper.GetHighestSdkVersion(releaseConfig.Sdks) },
            }
        };

        _logger.LogInformation(
            "Resolved product versions: {productVersions}",
            string.Join(", ", productVersions.Select(kv => $"{kv.Key}: {kv.Value}")));

        // Example build URL: https://dev.azure.com/<org>/<project>/_build/results?buildId=<stagingPipelineRunId>
        var buildUrl = $"{options.AzdoOrganization}/{options.AzdoProject}/_build/results?buildId={stagingPipelineRunId}";
        _logger.LogInformation(
            "Applying internal build {StageContainer} ({BuildUrl})",
            stageContainer, buildUrl);

        _logger.LogInformation(
            "Ignore any git-related logging output below, because git "
            + "operations are being managed by a different command.");

        // Run old update-dependencies command using the resolved versions.
        // Do not use the old command to submit a pull request.
        var updateDependencies = new SpecificCommand();
        var updateDependenciesOptions = new SpecificCommandOptions()
        {
            RepoRoot = gitRepoContext.LocalRepoPath,
            DockerfileVersion = majorMinorVersionString,
            ProductVersions = productVersions,
            InternalBaseUrl = internalBaseUrl,
        };
        var exitCode = await updateDependencies.ExecuteAsync(updateDependenciesOptions);
        if (exitCode != 0)
        {
            _logger.LogError(
                "Failed to apply stage container {StageContainer}. "
                + "Command exited with code {ExitCode}.",
                stageContainer, exitCode);
            return (string.Empty, string.Empty, exitCode);
        }

        var commitMessage = releaseConfig switch
        {
            { SdkOnly: true } => $"Update .NET {majorMinorVersionString} SDK to {productVersions["sdk"]}",
            _ => $"Update .NET {majorMinorVersionString} to {productVersions["sdk"]} SDK / {productVersions["runtime"]} Runtime",
        };

        var newVersionsList = productVersions.Select(kvp => $"- {kvp.Key.ToUpper()}: {kvp.Value}");
        var prBodySection = $"""
            ## .NET {majorMinorVersionString}

            This updates .NET {majorMinorVersionString} to the following versions:

            {string.Join(Environment.NewLine, newVersionsList)}

            These versions are from .NET staging pipeline run [{stageContainer}]({buildUrl}).
            """;

        return (commitMessage, prBodySection, 0);
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
            IEnvironmentService environmentService)
        {
            CommitChanges commitChanges;
            PushAndCreatePullRequest pushAndCreatePullRequest;
            string localRepoPath;

            if (options.Mode == ChangeMode.Remote)
            {
                var remoteUrl = options.GetAzdoRepoUrl();
                var targetBranch = options.TargetBranch;
                var buildId = environmentService.GetBuildId() ?? "";
                var stageContainerList = options.GetStageContainerList();
                var firstStageContainer = stageContainerList.Count > 0 ? stageContainerList[0] : "unknown";
                var prBranch = options.CreatePrBranchName($"update-deps-int-{firstStageContainer}", buildId);
                var committer = options.GetCommitterIdentity();

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
