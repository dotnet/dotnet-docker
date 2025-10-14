// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using Dotnet.Docker;
using Dotnet.Docker.Git;
using Dotnet.Docker.Sync;
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
    SpecificCommand.Create(
        name: "specific",
        description: "Update dependencies using specific product versions"),
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

                // Individual build updater services that support different repos
                services.AddKeyedSingleton<IBuildUpdaterService, VmrBuildUpdaterService>(BuildRepo.Vmr);
                services.AddKeyedSingleton<IBuildUpdaterService, AspireBuildUpdaterService>(BuildRepo.Aspire);

                services.AddHttpClient();
                services.AddHttpClient<AzdoHttpClient>();

                services.AddSingleton<AzdoAuthProvider>();
                services.AddSingleton<PipelineArtifactProvider>();
                services.AddSingleton<IInternalVersionsService, InternalVersionsService>();

                // Dependencies that can be updated using the FromComponentCommand
                services.AddKeyedSingleton<IDependencyVersionSource, ChiselVersionSource>("chisel");
                // Factory method for reading variables from manifest.versions.json
                services.AddSingleton<Func<string, IManifestVariables>>(path => ManifestVariables.FromFile(path));

                // Commands
                services.AddCommand<FromBuildCommand, FromBuildOptions>();
                services.AddCommand<FromChannelCommand, FromChannelOptions>();
                services.AddCommand<FromStagingPipelineCommand, FromStagingPipelineOptions>();
                services.AddCommand<FromComponentCommand, FromComponentOptions>();
                services.AddCommand<SpecificCommand, SpecificCommandOptions>();
                services.AddCommand<SyncInternalReleaseCommand, SyncInternalReleaseOptions>();
            }
        )
    );

return await config.InvokeAsync(args);
