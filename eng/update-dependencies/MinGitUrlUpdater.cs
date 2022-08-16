// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the MinGit version in the manifest.versions.json file.
/// </summary>
internal class MinGitUrlUpdater : MinGitUpdater
{
    public MinGitUrlUpdater(string repoRoot, JObject latestMinGitRelease)
        : base(
            repoRoot,
            latestMinGitRelease,
            "mingit|x64|url")
    {
    }

    protected override string GetValue()
    {
        JObject mingitAsset = GetMinGitAsset(LatestMinGitRelease);
        return mingitAsset.GetRequiredToken<JValue>("browser_download_url").ToString();
    }

    internal static JObject GetMinGitAsset(JObject release)
    {
        JArray assets = release.GetRequiredToken<JArray>("assets");

        JObject mingitAsset = (JObject)(assets.FirstOrDefault(asset => IsMinGit64BitAsset((JObject)asset)) ??
            throw new InvalidOperationException("Can't find 64-bit MinGit asset."));
        return mingitAsset;
    }

    private static bool IsMinGit64BitAsset(JObject asset)
    {
        string name = asset.GetRequiredToken<JValue>("name").ToString();
        return name.StartsWith("MinGit", StringComparison.OrdinalIgnoreCase) &&
            name.Contains("64-bit", StringComparison.OrdinalIgnoreCase) &&
            name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
    }
}
#nullable disable
