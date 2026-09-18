// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Octokit;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed partial class MinGitUpdater(IReleasesClient releases) : IGitHubReleaseUpdater
{
    private const string Owner = "git-for-windows";
    private const string Repo = "git";

    public static string Name => "mingit";
    public static string VersionSourceName => Name;

    [GeneratedRegex(@"^MinGit.*64-bit.*\.zip$")]
    private static partial Regex UrlRegex { get; }

    private static string GetManifestVariableName(string type) => "mingit|latest|x64|" + type;

    public async Task<DependencyUpdate> ResolveFromGitHubReleaseAsync(CancellationToken cancellationToken)
    {
        Release release = await releases.GetLatest(Owner, Repo).WaitAsync(cancellationToken);

        return new DependencyUpdate(
            Scope: "",
            Description: $"Update MinGit to {release.TagName}",
            ApplyAsync: (variables, _, _) =>
            {
                Apply(variables, release);
                return Task.CompletedTask;
            });
    }

    private static void Apply(ManifestVariables variables, Release release)
    {
        string urlVariable = GetManifestVariableName("url");
        string shaVariable = GetManifestVariableName("sha");
        bool updateUrl = variables.ShouldUpdateLiteral(urlVariable);
        bool updateSha = variables.ShouldUpdateLiteral(shaVariable);
        if (!updateUrl && !updateSha)
        {
            return;
        }

        ReleaseAsset asset = release.Assets.FirstOrDefault(asset => UrlRegex.IsMatch(asset.Name))
            ?? throw new InvalidOperationException($"Could not find MinGit release asset matching regex {UrlRegex}.");

        if (updateUrl)
        {
            variables.SetValue(urlVariable, asset.BrowserDownloadUrl);
        }

        if (updateSha)
        {
            Regex shaRegex = new(@$"{Regex.Escape(asset.Name)}\s\|\s(?<sha>[0-9a-f]+)");
            Match match = shaRegex.Match(release.Body);
            if (!match.Success)
            {
                throw new InvalidOperationException($"Could not find checksum for '{asset.Name}' in the MinGit release body.");
            }

            string sha = match.Groups["sha"].Value;
            variables.SetValue(shaVariable, sha);
        }
    }
}
