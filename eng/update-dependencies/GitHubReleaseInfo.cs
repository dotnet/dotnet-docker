// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

namespace Dotnet.Docker;

internal record GitHubReleaseInfo(string SimpleName, Release Release) : IDependencyInfo
{
    public string SimpleVersion { get; } = Release.TagName;
}
