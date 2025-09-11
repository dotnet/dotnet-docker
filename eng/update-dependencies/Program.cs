// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.IO;
using Dotnet.Docker;
using Dotnet.Docker.Sync;
using Microsoft.DotNet.DarcLib;
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
                    options.SingleLine = true;
                });
            })
        .ConfigureServices(services =>
            {
                services.AddSingleton<IBuildUpdaterService, BuildUpdaterService>();
                services.AddSingleton<IBasicBarClient>(_ =>
                    new BarApiClient(null, null, disableInteractiveAuth: true));
                services.AddSingleton<IBuildAssetService, BuildAssetService>();

                services.AddKeyedSingleton<IDependencyVersionSource, ChiselVersionSource>("chisel");

                services.AddHttpClient();
                services.AddHttpClient<AzdoHttpClient>();

                services.AddSingleton<AzdoAuthProvider>();
                services.AddSingleton<PipelineArtifactProvider>();

                services.AddCommand<FromBuildCommand, FromBuildOptions>();
                services.AddCommand<FromChannelCommand, FromChannelOptions>();
                services.AddCommand<FromStagingPipelineCommand, FromStagingPipelineOptions>();
                services.AddCommand<FromComponentCommand, FromComponentOptions>();
                services.AddCommand<SpecificCommand, SpecificCommandOptions>();
                services.AddCommand<SyncInternalReleaseCommand, SyncInternalReleaseOptions>();
            })
    );

return await config.InvokeAsync(args);
