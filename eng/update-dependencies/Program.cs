// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Hosting;
using Dotnet.Docker;
using Microsoft.Extensions.Hosting;

var rootCommand = new RootCommand()
{
    SpecificCommand.Create(
        name: "specific",
        description: "Update dependencies using specific product versions"),
};

var config = new CommandLineConfiguration(rootCommand);

config.UseHost(
    _ => Host.CreateDefaultBuilder(),
    host => host.ConfigureServices(services =>
        {
            SpecificCommand.Register<SpecificCommand>(services);
        })
    );

return await config.InvokeAsync(args);
