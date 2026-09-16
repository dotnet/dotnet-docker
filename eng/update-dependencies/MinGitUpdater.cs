// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Octokit;

namespace Dotnet.Docker;

public sealed partial class MinGitUpdater(IReleasesClient releases) : IGitHubReleaseUpdater
{
    public const string ToolName = "mingit";

    private const string Owner = "git-for-windows";

    private const string Repo = "git";

    [GeneratedRegex(@"^MinGit.*64-bit.*\.zip$")]
    private static partial Regex UrlRegex { get; }

    private static string GetManifestVariableName(string type) => "mingit|latest|x64|" + type;

    public async Task UpdateFromGitHubReleaseAsync(
        ManifestVariables variables,
        CancellationToken cancellationToken)
    {
        Release release = await releases.GetLatest(Owner, Repo).WaitAsync(cancellationToken);

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
