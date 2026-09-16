// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Maestro.Common;
using Maestro.Common.AzureDevOpsTokens;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.DependencyInjection;
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

            return Host.CreateDefaultBuilder();
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
        .ConfigureServices(services =>
            {

                // Local services needed for DarcLib git operations
                services.AddSingleton<ITelemetryRecorder, NoTelemetryRecorder>();
                services.AddSingleton<IProcessManager>(sp =>
                    new ProcessManager(sp.GetRequiredService<ILogger<ProcessManager>>(), "git"));
                services.AddTransient<IFileSystem, FileSystem>();

                // Auth services needed for DarcLib remote git operations
                services.AddSingleton<IRemoteTokenProvider>(sp =>
                {
                    var azdoTokenProvider = sp.GetRequiredService<IAzureDevOpsTokenProvider>();
                    var gitHubTokenProvider = new ResolvedTokenProvider(null);
                    return new RemoteTokenProvider(
                        azdoTokenProvider: azdoTokenProvider,
                        gitHubTokenProvider: gitHubTokenProvider);
                });
                services.AddSingleton<IAzureDevOpsTokenProvider, AzureDevOpsTokenProvider>();
                services.Configure<AzureDevOpsTokenProviderOptions>(options =>
                    {
                        // TODO: Find a way to use the same Azure DevOps token/auth between here and CreatePullRequestOptions
                        options["default"] = new AzureDevOpsCredentialResolverOptions
                        {
                            // Interactive auth can be enabled in order to run locally using your own user identity.
                            // Use with caution. Disable by default since this tool runs in CI.
                            DisableInteractiveAuth = true
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

                services.AddSingleton<Octokit.IReleasesClient>(_ =>
                    new Octokit.GitHubClient(new Octokit.ProductHeaderValue("dotnet-docker-update-dependencies"))
                        .Repository.Release);

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
