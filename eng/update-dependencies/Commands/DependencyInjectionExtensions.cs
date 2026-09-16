// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

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
}
