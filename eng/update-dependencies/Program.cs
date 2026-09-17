// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Maestro.Common;
using Maestro.Common.AzureDevOpsTokens;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.GitAutomation;
using Microsoft.DotNet.GitAutomation.AzureDevOps;
using Microsoft.DotNet.GitAutomation.GitHub;
using AzureDevOpsClient = Microsoft.DotNet.DarcLib.AzureDevOpsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var rootCommand = new RootCommand()
{
    FromBuildCommand.Create(
        name: "from-build",
        description: "Update dependencies using a specific BAR build"),
    FromChannelCommand.Create(
        name: "from-channel",
        description: "Update dependencies using the latest build from a channel"),
    FromStagingPipelineCommand.Create(
        name: "from-staging-pipeline",
        description: "Update dependencies using a specific staging pipeline run"),
    FromComponentCommand.Create(
        name: "from-component",
        description: "Update a single image component"),
    MonitorCommand.Create(
        name: "monitor",
        description: "Update .NET Monitor using a version or Azure DevOps pipeline run"),
    AspireCommand.Create(
        name: "aspire",
        description: "Update Aspire Dashboard using a BAR build or channel"),
    SyncInternalReleaseCommand.Create(
        name: "sync-internal-release",
        description: "Sync release/* branch to internal/release/* branch"),
};

var config = new CommandLineConfiguration(rootCommand);

config.UseHost(
    hostBuilderFactory: unmatchedArgs =>
        {
            if (unmatchedArgs.Length > 0)
            {
                var helpBuilder = new HelpBuilder();
                using var stringWriter = new StringWriter();
                helpBuilder.Write(rootCommand, stringWriter);
                Console.WriteLine(stringWriter.ToString());
                throw new InvalidOperationException($"Unmatched tokens: {string.Join(" ", unmatchedArgs)}");
            }

            return Host.CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory);
        },
    configureHost: host => host
        .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = false;
                });
            })
        .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration
                    .GetRequiredSection("UpdateDependencies")
                    .Get<UpdateDependenciesConfiguration>()
                        ?? throw new InvalidOperationException("Failed to bind UpdateDependencies configuration.");

                services.AddSingleton(configuration);

                switch (configuration.PullRequestDestination)
                {
                    case GitRemote.GitHub:
                        var githubConfig = configuration.GitHub;
                        var accessProvider = CreateGitHubAccessProvider(configuration);
                        var repo = new GitHubRepo(githubConfig.Owner, githubConfig.Repository);
                        services.AddGitHubPullRequestAutomation(accessProvider, repo);
                        services.AddTransient<IPullRequestEndpoint>(sp =>
                            sp.GetRequiredService<GitHubPullRequestEndpoint>());
                        break;

                    case GitRemote.AzureDevOps:
                        var azdoConfig = configuration.AzureDevOps;
                        string organization = new Uri(azdoConfig.Organization).Segments.Last().TrimEnd('/');
                        services.AddAzureDevOpsPullRequestAutomation(
                            organization,
                            azdoConfig.Project,
                            azdoConfig.Repository,
                            new AutomationIdentity(configuration.User, configuration.Email),
                            azdoConfig.Token);
                        services.AddTransient<IPullRequestEndpoint>(sp =>
                            sp.GetRequiredService<AzureDevOpsPullRequestEndpoint>());
                        break;

                    default:
                        throw new InvalidOperationException(
                            $"Unsupported PR destination: {configuration.PullRequestDestination}");
                }

                services.AddSingleton<DependencyUpdateRunner>();

                // Local services needed for DarcLib git operations
                services.AddSingleton<ITelemetryRecorder, NoTelemetryRecorder>();
                services.AddSingleton<IProcessManager>(sp =>
                    new ProcessManager(sp.GetRequiredService<ILogger<ProcessManager>>(), "git"));
                services.AddTransient<IFileSystem, FileSystem>();

                // Auth services needed for DarcLib remote git operations
                services.AddSingleton<IRemoteTokenProvider>(sp =>
                {
                    var azdoTokenProvider = sp.GetRequiredService<IAzureDevOpsTokenProvider>();
                    var gitHubTokenProvider = new ResolvedTokenProvider(configuration.GitHub.Token);
                    return new RemoteTokenProvider(
                        azdoTokenProvider: azdoTokenProvider,
                        gitHubTokenProvider: gitHubTokenProvider);
                });
                services.AddSingleton<IAzureDevOpsTokenProvider, AzureDevOpsTokenProvider>();
                services.Configure<AzureDevOpsTokenProviderOptions>(options =>
                    {
                        options["default"] = new AzureDevOpsCredentialResolverOptions
                        {
                            Token = configuration.AzureDevOps.Token,
                            DisableInteractiveAuth = configuration.AzureDevOps.DisableInteractiveAuth
                        };
                    }
                );

                services.AddKeyedSingleton<IRemoteGitRepo, AzureDevOpsClient>(GitRemote.AzureDevOps);
                services.AddKeyedSingleton<IRemoteGitRepo, GitHubClient>(GitRemote.GitHub);
                services.AddSingleton<IRemoteGitRepoFactory, RemoteGitRepoFactory>();

                // Process-based git client
                services.AddSingleton<ILocalGitClient, LocalGitClient>();
                // LocalGitClient wants a non-generic ILogger, for some reason.
                services.AddSingleton<ILogger>(sp =>
                    sp.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(LocalGitClient)));
                // Git repo cloner that calls out to the `git` executable. It is lighter on memory
                // than the LibGit2Sharp-based implementation.
                services.AddSingleton<IGitRepoCloner, GitNativeRepoCloner>();
                // LibGit2Sharp-based git client - has some operations that are not supported by
                // the process-based client (namely "push" operations).
                services.AddSingleton<ILocalLibGit2Client, LocalLibGit2Client>();
                services.AddSingleton<ILocalGitRepoFactory, LocalGitRepoFactory>();

                // Finally, this project's own Git client abstraction that abstracts over the
                // various DarcLib implementations
                services.AddSingleton<IGitRepoHelperFactory, GitRepoHelperFactory>();

                // Services needed for BAR build access/updates
                services.AddSingleton<IBasicBarClient>(_ =>
                        new BarApiClient(null, null, disableInteractiveAuth: true));
                services.AddSingleton<IBuildAssetService, BuildAssetService>();

                services.AddEnvironmentService();
                services.AddBuildLabelService();
                services.AddPipelineArtifactProvider();
                services.AddSingleton<IInternalVersionsService, InternalVersionsService>();

                services.AddSingleton(_ =>
                {
                    var productHeader = new Octokit.ProductHeaderValue("dotnet-docker-update-dependencies");
                    var client = new Octokit.GitHubClient(productHeader);
                    if (!string.IsNullOrWhiteSpace(configuration.GitHub.Token))
                    {
                        client.Credentials = new Octokit.Credentials(configuration.GitHub.Token);
                    }

                    return client.Repository.Release;
                });

                // Each dependency has one singleton, exposing only its supported update capabilities.
                services.AddKeyedSingleton<IUpdater, DotNetUpdater>("dotnet");
                services.AddKeyedSingleton<IUpdater, AspireUpdater>("aspire");
                services.AddKeyedSingleton<IUpdater, MonitorUpdater>("monitor");
                services.AddKeyedSingleton<IUpdater, ChiselUpdater>("chisel");
                services.AddKeyedSingleton<IUpdater, SyftUpdater>("syft");
                services.AddKeyedSingleton<IUpdater, RocksToolboxUpdater>("rocks-toolbox");
                services.AddKeyedSingleton<IUpdater, MinGitUpdater>("mingit");

                // Commands
                services.AddCommand<FromBuildCommand, FromBuildOptions>();
                services.AddCommand<FromChannelCommand, FromChannelOptions>();
                services.AddCommand<FromStagingPipelineCommand, FromStagingPipelineOptions>();
                services.AddCommand<FromComponentCommand, FromComponentOptions>();
                services.AddCommand<MonitorCommand, MonitorOptions>();
                services.AddCommand<AspireCommand, AspireOptions>();
                services.AddCommand<SyncInternalReleaseCommand, SyncInternalReleaseOptions>();
            }
        )
    );

return await config.InvokeAsync(args);

static IGitHubAccessProvider CreateGitHubAccessProvider(UpdateDependenciesConfiguration configuration)
{
    var github = configuration.GitHub;
    if (!string.IsNullOrEmpty(github.Token))
    {
        var identity = new AutomationIdentity(configuration.User, configuration.Email);
        return new StaticGitHubAccessProvider(github.Token, identity);
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(github.AppClientId);
    ArgumentException.ThrowIfNullOrWhiteSpace(github.AppKeyUri);
    var credential = new AzureCliCredential();
    var cryptographyClient = new CryptographyClient(new Uri(github.AppKeyUri), credential);
    return new GitHubAppAccessProvider(github.AppClientId, cryptographyClient);
}
