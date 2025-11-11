// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

namespace Dotnet.Docker;

internal static partial class MinGitUpdater
{
    public const string ToolName = "mingit";

    private const string Owner = "git-for-windows";

    private const string Repo = "git";

    public static IEnumerable<IDependencyUpdater> GetUpdaters(string manifestVersionsFilePath) =>
    [
        new GitHubReleaseUrlUpdater(
            manifestVersionsFilePath: manifestVersionsFilePath,
            toolName: ToolName,
            variableName: GetManifestVariableName("url"),
            owner: Owner,
            repo: Repo,
            assetRegex: UrlRegex),
        new MinGitShaUpdater(manifestVersionsFilePath)
    ];

    public static async Task<GitHubReleaseInfo> GetBuildInfoAsync()
    {
        Release minGitRelease = await GitHubHelper.GetLatestRelease(Owner, Repo);
        return new GitHubReleaseInfo(ToolName, minGitRelease);
    }

    [GeneratedRegex(@"^MinGit.*64-bit.*\.zip$")]
    private static partial Regex UrlRegex { get; }

    private static string GetManifestVariableName(string type) => "mingit|latest|x64|" + type;

    private class MinGitShaUpdater(string manifestVersionsFilePath)
        : GitHubReleaseUpdaterBase(
            manifestVersionsFilePath,
            MinGitUpdater.ToolName,
            GetManifestVariableName("sha"),
            MinGitUpdater.Owner,
            MinGitUpdater.Repo)
    {
        protected override string? GetValue(GitHubReleaseInfo dependencyInfo)
        {
            ReleaseAsset asset = dependencyInfo.Release.Assets
                .First(asset => UrlRegex.IsMatch(asset.Name))
                    ?? throw new Exception(
                        $"Could not find release asset for {GetManifestVariableName("sha")} matching regex {UrlRegex}");

            string body = dependencyInfo.Release.Body;
            const string ShaGroupName = "sha";
            Regex shaRegex = new(@$"{Regex.Escape(asset.Name)}\s\|\s(?<{ShaGroupName}>[0-9|a-f]+)");
            string sha = shaRegex.Match(body).Groups[ShaGroupName].Value;
            return sha;
        }
    }

}
