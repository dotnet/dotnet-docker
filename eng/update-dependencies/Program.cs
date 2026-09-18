// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Maestro.Common;
using Maestro.Common.AzureDevOpsTokens;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.GitAutomation;
using Microsoft.DotNet.GitAutomation.AzureDevOps;
using Microsoft.DotNet.GitAutomation.GitHub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AzureDevOpsClient = Microsoft.DotNet.DarcLib.AzureDevOpsClient;

var updaterServices = new ServiceCollection();
IHost? host = null;

var rootCommand = new RootCommand("Update dotnet-docker dependencies");
IServiceProvider GetServices() => (host ??= CreateHost(updaterServices)).Services;

AddUpdater<AspireUpdater>();
AddUpdater<ChiselUpdater>();
AddUpdater<DotNetUpdater>();
AddUpdater<MinGitUpdater>();
AddUpdater<MonitorUpdater>();
AddUpdater<RocksToolboxUpdater>();
AddUpdater<SyftUpdater>();

rootCommand.Subcommands.Add(SyncInternalReleaseCommand.Create(GetServices));

try
{
    var parseResult = rootCommand.Parse(args.Length == 0 ? ["--help"] : args);
    return await parseResult.InvokeAsync();
}
finally
{
    host?.Dispose();
}

void AddUpdater<TUpdater>() where TUpdater : class, IUpdater
{
    updaterServices.AddSingleton<TUpdater>();
    rootCommand.Subcommands.Add(DependencyCommand.Create<TUpdater>(GetServices));
}

static IHost CreateHost(IEnumerable<ServiceDescriptor> updaterServices)
{
    var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
    {
        ContentRootPath = AppContext.BaseDirectory,
    });

    builder.Logging.ClearProviders();
    builder.Logging.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = false;
    });

    var configuration = builder.Configuration
        .GetRequiredSection("UpdateDependencies")
        .Get<UpdateDependenciesConfiguration>()
            ?? throw new InvalidOperationException("Failed to bind UpdateDependencies configuration.");

    IServiceCollection services = builder.Services;
    services.AddSingleton(configuration);

    switch (configuration.PullRequestDestination)
    {
        case GitRemote.GitHub:
            var githubConfig = configuration.GitHub;
            var repo = new GitHubRepo(githubConfig.Owner, githubConfig.Repository);
            if (!string.IsNullOrEmpty(githubConfig.AppClientId)
                && !string.IsNullOrEmpty(githubConfig.AppKeyUri))
            {
                services.AddGitHubPullRequestAutomation(CreateGitHubAppAccessProvider(configuration), repo);
            }
            else
            {
                var identity = new AutomationIdentity(configuration.User, configuration.Email);
                services.AddGitHubPullRequestAutomation(repo, identity, githubConfig.Token);
            }
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

    services.AddSingleton(sp => new Lazy<IPullRequestEndpoint>(() => sp.GetRequiredService<IPullRequestEndpoint>()));
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
            DisableInteractiveAuth = configuration.AzureDevOps.DisableInteractiveAuth,
        };
    });

    services.AddKeyedSingleton<IRemoteGitRepo, AzureDevOpsClient>(GitRemote.AzureDevOps);
    services.AddKeyedSingleton<IRemoteGitRepo, GitHubClient>(GitRemote.GitHub);
    services.AddSingleton<IRemoteGitRepoFactory, RemoteGitRepoFactory>();

    // Process-based git client
    services.AddSingleton<ILocalGitClient, LocalGitClient>();
    // LocalGitClient wants a non-generic ILogger, for some reason.
    services.AddSingleton<ILogger>(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(LocalGitClient)));
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
    services.AddSingleton<IBasicBarClient>(_ => new BarApiClient(null, null, disableInteractiveAuth: true));
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
    foreach (ServiceDescriptor updater in updaterServices)
    {
        services.Add(updater);
    }

    // Commands
    services.AddSingleton<FromStagingPipelineCommand>();
    services.AddSingleton<ICommand<FromStagingPipelineOptions>>(sp =>
        sp.GetRequiredService<FromStagingPipelineCommand>());
    services.AddSingleton<SyncInternalReleaseCommand>();

    return builder.Build();
}

static IGitHubAccessProvider CreateGitHubAppAccessProvider(UpdateDependenciesConfiguration configuration)
{
    var github = configuration.GitHub;
    ArgumentException.ThrowIfNullOrWhiteSpace(github.AppClientId);
    ArgumentException.ThrowIfNullOrWhiteSpace(github.AppKeyUri);
    var credential = new AzureCliCredential();
    var cryptographyClient = new CryptographyClient(new Uri(github.AppKeyUri), credential);
    return new GitHubAppAccessProvider(github.AppClientId, cryptographyClient);
}
