// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

internal static class CreatePullRequestOptionsExtensions
{
    public static string GetManifestVersionsFilePath(this CreatePullRequestOptions options) =>
        Path.Combine(options.RepoRoot, "manifest.versions.json");
}
