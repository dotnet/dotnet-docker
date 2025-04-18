// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Hosting;
using System.Net.Http;
using Dotnet.Docker;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var rootCommand = new RootCommand()
{
    FromChannelCommand.Create(
        name: "from-channel",
        description: "Update dependencies using the latest build from a channel"),
    SpecificCommand.Create(
        name: "specific",
        description: "Update dependencies using specific product versions"),
};

var config = new CommandLineConfiguration(rootCommand);

config.UseHost(
    _ => Host.CreateDefaultBuilder(),
    host => host.ConfigureServices(services =>
        {
            services.AddSingleton<IBasicBarClient>(_ =>
                new BarApiClient(null, null, disableInteractiveAuth: true));
            services.AddSingleton<IBuildAssetService, BuildAssetService>();
            services.AddSingleton<HttpClient>();

            FromChannelCommand.Register<FromChannelCommand>(services);
            SpecificCommand.Register<SpecificCommand>(services);
        })
    );

return await config.InvokeAsync(args);
