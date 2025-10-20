// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker.Git;
using Dotnet.Docker.Sync;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal partial class FromStagingPipelineCommand(
    ILogger<FromStagingPipelineCommand> logger,
    PipelineArtifactProvider pipelineArtifactProvider,
    IInternalVersionsService internalVersionsService,
    IGitRepoHelperFactory gitRepoHelperFactory)
    : BaseCommand<FromStagingPipelineOptions>
{
    /// <summary>
    /// Callback that stages all changes, commits them, pushes them to the
    /// remote, and creates a pull request.
    /// </summary>
    private delegate Task CommitAndCreatePullRequest(string commitMessage, string prTitle, string prBody);

    private readonly ILogger<FromStagingPipelineCommand> _logger = logger;
    private readonly PipelineArtifactProvider _pipelineArtifactProvider = pipelineArtifactProvider;
    private readonly IInternalVersionsService _internalVersionsService = internalVersionsService;
    private readonly IGitRepoHelperFactory _gitRepoHelperFactory = gitRepoHelperFactory;

    public override async Task<int> ExecuteAsync(FromStagingPipelineOptions options)
    {
        // Delegate all git responsibilities to the SetupRepoAsync method.
        // Depending on what options were passed in, we may or may not want to
        // actually perform git operations. The setup method will decide that
        // and return an appropriate delegate. This keeps all the git-related
        // logic in one place.
        // The key is to call the setup method before making any changes, and
        // use the delegate when changes are ready to be committed/pushed.
        var repoContext = await GitRepoContext.CreateAsync(_logger, _gitRepoHelperFactory, options);

        _logger.LogInformation(
            "Updating dependencies based on staging pipeline run ID {options.StagingPipelineRunId}",
            options.StagingPipelineRunId);

        string internalBaseUrl = string.Empty;
        if (options.Internal)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(
                options.StagingStorageAccount,
                $"{FromStagingPipelineOptions.StagingStorageAccountOption} must be set when using the {FromStagingPipelineOptions.InternalOption} option."
            );

            // Each pipeline run has a corresponding blob container named stage-${options.StagingPipelineRunId}.
            // Release metadata is stored in metadata/ReleaseManifest.json.
            // Release assets are stored individually under in assets/shipping/assets/[Sdk|Runtime|aspnetcore|...].
            // Full example: https://dotnetstagetest.blob.core.windows.net/stage-2XXXXXX/assets/shipping/assets/Runtime/10.0.0-preview.N.XXXXX.YYY/dotnet-runtime-10.0.0-preview.N.XXXXX.YYY-linux-arm64.tar.gz
            internalBaseUrl = NormalizeStorageAccountUrl(options.StagingStorageAccount)
                + $"/stage-{options.StagingPipelineRunId}/assets/shipping/assets";
        }

        var releaseConfig = await _pipelineArtifactProvider.GetReleaseConfigAsync(
            options.AzdoOrganization,
            options.AzdoProject,
            options.StagingPipelineRunId);

        string dotnetProductVersion = VersionHelper.ResolveProductVersion(releaseConfig.RuntimeBuild);
        string dockerfileVersion = VersionHelper.ResolveMajorMinorVersion(releaseConfig.RuntimeBuild).ToString();

        // Record pipeline run ID for this dockerfileVersion, for later use by sync-internal-release command
        _internalVersionsService.RecordInternalStagingBuild(
            repoRoot: repoContext.LocalRepoPath,
            dockerfileVersion: dockerfileVersion,
            stagingPipelineRunId: options.StagingPipelineRunId);

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
        var buildUrl = $"{options.AzdoOrganization}/{options.AzdoProject}/_build/results?buildId={options.StagingPipelineRunId}";
        _logger.LogInformation(
            "Applying internal build {BuildNumber} ({BuildUrl})",
            options.StagingPipelineRunId, buildUrl);

        _logger.LogInformation(
            "Ignore any git-related logging output below, because git "
            + "operations are being managed by a different command.");

        // Run old update-dependencies command using the resolved versions.
        // Do not use the old command to submit a pull request.
        var updateDependencies = new SpecificCommand();
        var updateDependenciesOptions = new SpecificCommandOptions()
        {
            RepoRoot = repoContext.LocalRepoPath,
            DockerfileVersion = dockerfileVersion.ToString(),
            ProductVersions = productVersions,
            InternalBaseUrl = internalBaseUrl,
        };
        var exitCode = await updateDependencies.ExecuteAsync(updateDependenciesOptions);
        if (exitCode != 0)
        {
            _logger.LogError(
                "Failed to apply staging pipeline run ID {StagingPipelineRunId}. "
                + "Command exited with code {ExitCode}.",
                options.StagingPipelineRunId, exitCode);
            return exitCode;
        }

        var commitMessage = $"Update .NET {dockerfileVersion} to {productVersions["sdk"]} SDK / {productVersions["runtime"]} Runtime";
        var prTitle = $"[{options.TargetBranch}] {commitMessage}";
        var prBody = $"""
            This pull request updates .NET {dockerfileVersion} to the following versions:

            - SDK: {productVersions["sdk"]}
            - Runtime: {productVersions["runtime"]}
            - ASP.NET Core: {productVersions["aspnet"]}

            These versions are from .NET staging pipeline run [#{options.StagingPipelineRunId}]({buildUrl}).
            """;
        await repoContext.CreatePullRequest(commitMessage, prTitle, prBody);

        return 0;
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
    /// Sets up the remote/local git repository based on <paramref name="options"/>.
    /// Call this before making changes to the repo at <paramref name="options.RepoRoot"/>, and
    /// then use the returned delegate to commit all changes and create a pull request.
    /// </summary>
    /// <returns>
    /// A delegate that can be used to commit changes and create a pull request.
    /// </returns>
    /// <remarks>
    /// If <see cref="FromStagingPipelineOptions.Mode"/> is <see cref="ChangeMode.Local"/>,
    /// no git operations will be performed.
    /// </remarks>
    private record GitRepoContext(string LocalRepoPath, CommitAndCreatePullRequest CreatePullRequest)
    {
        public static async Task<GitRepoContext> CreateAsync(
            ILogger logger,
            IGitRepoHelperFactory gitRepoFactory,
            FromStagingPipelineOptions options)
        {
            CommitAndCreatePullRequest createPullRequest;
            string localRepoPath;

            if (options.Mode == ChangeMode.Remote)
            {
                var remoteUrl = options.GetAzdoRepoUrl();
                var targetBranch = options.TargetBranch;
                var prBranch = options.CreatePrBranchName($"update-deps-int-{options.StagingPipelineRunId}");
                var committer = options.GetCommitterIdentity();

                // Clone the repo
                var git = await gitRepoFactory.CreateAndCloneAsync(remoteUrl);
                // Ensure the branch we want to modify exists, then check it out
                await git.Remote.EnsureBranchExistsAsync(targetBranch);
                // Create a new branch to push changes to and create a PR from
                await git.CheckoutRemoteBranchAsync(targetBranch);
                await git.Local.CreateAndCheckoutLocalBranchAsync(prBranch);

                localRepoPath = git.Local.LocalPath;
                createPullRequest = async (commitMessage, prTitle, prBody) =>
                {
                    await git.Local.StageAsync(".");
                    await git.Local.CommitAsync(commitMessage, committer);
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
                createPullRequest = async (commitMessage, prTitle, prBody) =>
                {
                    logger.LogInformation("Skipping commit and pull request creation in {Mode} mode.", options.Mode);
                };
            }

            return new GitRepoContext(localRepoPath, createPullRequest);
        }
    }
}
