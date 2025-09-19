// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;

namespace Dotnet.Docker;

/// <summary>
/// Provides version information for a single component of an image.
/// </summary>
internal interface IDependencyVersionSource
{
    /// <summary>
    /// Gets version information for the component in the specified channel.
    /// </summary>
    /// <param name="channel">
    /// Logical version channel for the component. This will be passed from the
    /// command line. If a component has multiple different supported releases,
    /// this parameter should be used by the version source to choose between
    /// them.
    /// </param>
    /// <returns>
    /// The latest version for the component in the specified channel.
    /// </returns>
    Task<ComponentVersionInfo> GetVersionInfoAsync(ComponentVersionChannel channel);
}
