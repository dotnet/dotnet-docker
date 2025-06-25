// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Docker;

public static class ServiceCollectionExtensions
{
    public static void AddCommand<TCommand, TOptions>(
        this IServiceCollection serviceCollection)
            where TCommand : BaseCommand<TOptions>
            where TOptions : IOptions
    {
        serviceCollection.AddSingleton<BaseCommand<TOptions>, TCommand>();
    }
}
