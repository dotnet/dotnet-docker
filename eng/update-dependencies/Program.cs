// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Maestro.Common;
using Maestro.Common.AzureDevOpsTokens;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AzureDevOpsClient = Microsoft.DotNet.DarcLib.AzureDevOpsClient;

var serviceRegistrations = new ServiceCollection();
IHost? host = null;

var rootCommand = new RootCommand("Update dotnet-docker dependencies");
IServiceProvider GetServices() => (host ??= CreateHost(serviceRegistrations)).Services;

AddUpdater<AspireUpdater>();
AddUpdater<ChiselUpdater>();
Command dotnetCommand = AddUpdater<DotNetUpdater>();
AddCommand<FromStagingPipelineCommand>(
    dotnetCommand,
    FromStagingPipelineCommand.Create(GetServices));
serviceRegistrations.AddSingleton<ICommand<FromStagingPipelineOptions>>(services =>
    services.GetRequiredService<FromStagingPipelineCommand>());
AddUpdater<MinGitUpdater>();
AddUpdater<MonitorUpdater>();
AddUpdater<RocksToolboxUpdater>();
AddUpdater<SyftUpdater>();

AddCommand<SyncInternalReleaseCommand>(
    rootCommand,
    SyncInternalReleaseCommand.Create(GetServices));

try
{
    var parseResult = rootCommand.Parse(args.Length == 0 ? ["--help"] : args);
    return await parseResult.InvokeAsync();
}
finally
{
    host?.Dispose();
}

Command AddUpdater<TUpdater>() where TUpdater : class, IUpdater
{
    serviceRegistrations.AddSingleton<TUpdater>();
    Command command = DependencyCommand.Create<TUpdater>(GetServices);
    rootCommand.Subcommands.Add(command);
    return command;
}

void AddCommand<TCommand>(
    Command parent,
    Command command)
    where TCommand : class
{
    serviceRegistrations.AddSingleton<TCommand>();
    parent.Subcommands.Add(command);
}

static IHost CreateHost(IEnumerable<ServiceDescriptor> serviceRegistrations)
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
    foreach (ServiceDescriptor registration in serviceRegistrations)
    {
        services.Add(registration);
    }

    return builder.Build();
}
