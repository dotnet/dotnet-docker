// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;

namespace Dotnet.Docker;

internal sealed class ChiselVersionSource : IDependencyVersionSource
{
    public async Task<ComponentVersionInfo> GetVersionInfoAsync(ComponentVersionChannel channel)
    {
        GitHubReleaseInfo chiselReleaseInfo = await ChiselUpdater.GetBuildInfoAsync();
        return new ComponentVersionInfo(chiselReleaseInfo.SimpleVersion);
    }
}
