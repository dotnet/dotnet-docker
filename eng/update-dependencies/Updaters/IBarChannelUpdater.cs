// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public interface IBarChannelUpdater : IUpdater
{
    Task<DependencyUpdate> ResolveFromBarChannelAsync(int channelId, CancellationToken cancellationToken);
}
