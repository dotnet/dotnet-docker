// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Docker;

public static class UpdaterServiceExtensions
{
    public static T GetUpdater<T>(this IServiceProvider services, string key)
        where T : class, IUpdater
    {
        var updater = services.GetKeyedService<IUpdater>(key)
            ?? throw new InvalidOperationException($"No updater registered for '{key}'.");

        return updater as T
            ?? throw new InvalidOperationException($"Updater '{key}' does not support {typeof(T).Name}.");
    }
}
