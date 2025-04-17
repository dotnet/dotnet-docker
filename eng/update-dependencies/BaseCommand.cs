// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dotnet.Docker;

public abstract class BaseCommand<TOptions>() where TOptions : IOptions
{
    public abstract Task<int> ExecuteAsync(TOptions options);

    public static void Register<T>(IServiceCollection serviceCollection) where T : BaseCommand<TOptions>
    {
        serviceCollection.AddSingleton<BaseCommand<TOptions>, T>();
    }

    public static Command Create(string name, string description)
    {
        var command = new Command(name, description);

        foreach (var option in TOptions.Options)
        {
            command.Add(option);
        }

        foreach (var argument in TOptions.Arguments)
        {
            command.Add(argument);
        }

        command.Action = Handler;
        return command;
    }

    private static BindingHandler Handler =>
        CommandHandler.Create<TOptions, IHost>(async (options, host) =>
            {
                var thisCommand = host.Services.GetRequiredService<BaseCommand<TOptions>>();
                return await thisCommand.ExecuteAsync(options);
            });
}
