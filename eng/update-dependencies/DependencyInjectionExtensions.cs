// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Docker;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers a command + its options binding as a singleton so that
    /// BaseCommand can resolve the concrete implementation at invocation time.
    /// </summary>
    public static void AddCommand<TCommand, TOptions>(
        this IServiceCollection serviceCollection)
            where TCommand : BaseCommand<TOptions>
            where TOptions : IOptions
    {
        serviceCollection.AddSingleton<ICommand<TOptions>, TCommand>();
    }

    /// <summary>
    /// Attempts to resolve a keyed service. Returns true and sets instance when found; otherwise false.
    /// This avoids wrapping GetRequiredKeyedService in try/catch at call sites when missing services are
    /// treated as a validation error.
    /// </summary>
    public static bool TryGetRequiredKeyedService<T>(
        this IServiceProvider serviceProvider,
        object? serviceKey,
        out T? instance)
        where T : notnull
    {
        try
        {
            instance = serviceProvider.GetRequiredKeyedService<T>(serviceKey);
            return true;
        }
        catch
        {
            instance = default;
            return false;
        }
    }
}
