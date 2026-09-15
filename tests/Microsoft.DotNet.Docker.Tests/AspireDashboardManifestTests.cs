// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Xunit;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "pre-build")]
public sealed class AspireDashboardManifestTests
{
    private const string AspireDashboardId = "aspire-dashboard";
    private const string SyndicatedRepoVariable = "$(syndicatedAspireDashboardRepo)";

    [Theory]
    [InlineData("", "aspire/dashboard")]
    [InlineData("/nightly", "aspire/nightly/dashboard")]
    public void RepoName_UsesAspireProductFamily(string repoNameModifier, string expectedRepoName)
    {
        Assert.Equal(expectedRepoName, ImageData.GetRepoName(AspireDashboardId, repoNameModifier));
    }

    [Fact]
    public void Manifest_UsesCanonicalRepoName()
    {
        string branch = Config.GetVariableValue("branch");
        string repoNameModifier = branch == "nightly" ? "/nightly" : string.Empty;

        Assert.Equal(
            ImageData.GetRepoName(AspireDashboardId, repoNameModifier),
            GetAspireDashboardRepo().Value<string>("name"));
    }

    [Fact]
    public void LegacySyndication_IsLimitedToAspire13()
    {
        int majorVersion = int.Parse(Config.GetVariableValue("aspire-dashboard|major-tag"));
        JObject aspireDashboardRepo = GetAspireDashboardRepo();
        List<(string Tag, string Repo)> syndications = GetSyndications(aspireDashboardRepo).ToList();

        if (majorVersion != 13)
        {
            Assert.Empty(syndications);
            return;
        }

        string branch = Config.GetVariableValue("branch");
        string expectedLegacyRepo = branch == "nightly"
            ? "dotnet/nightly/aspire-dashboard"
            : "dotnet/aspire-dashboard";

        Assert.Equal(
            expectedLegacyRepo,
            Config.Manifest.Value["variables"]?["syndicatedAspireDashboardRepo"]?.Value<string>());
        Assert.All(syndications, syndication => Assert.Equal(SyndicatedRepoVariable, syndication.Repo));

        string fixedTag = Config.GetVariableValue("aspire-dashboard|fixed-tag");
        string minorTag = Config.GetVariableValue("aspire-dashboard|minor-tag");
        string majorTag = Config.GetVariableValue("aspire-dashboard|major-tag");
        JObject sharedTags = (JObject)aspireDashboardRepo["images"]!.Single()["sharedTags"]!;
        Assert.Equal(
            [majorTag, "latest"],
            sharedTags["$(aspire-dashboard|major-tag)"]!["syndication"]!["destinationTags"]!
                .Values<string>()
                .Select(destinationTag => ResolveTag(destinationTag!)));
        Assert.Null(sharedTags["latest"]!["syndication"]);

        HashSet<string> expectedTags =
        [
            fixedTag,
            minorTag,
            majorTag,
            "latest",
            $"{fixedTag}-amd64",
            $"{minorTag}-amd64",
            $"{majorTag}-amd64",
            $"{fixedTag}-arm64v8",
            $"{minorTag}-arm64v8",
            $"{majorTag}-arm64v8"
        ];

        Assert.Equal(expectedTags.Count, syndications.Count);
        Assert.True(
            expectedTags.SetEquals(syndications.Select(syndication => syndication.Tag)),
            $"Expected syndication for: {string.Join(", ", expectedTags.Order())}");
    }

    private static JObject GetAspireDashboardRepo() =>
        Config.Manifest.Value["repos"]!
            .Children<JObject>()
            .Single(repo => repo.Value<string>("id") == AspireDashboardId);

    private static IEnumerable<(string Tag, string Repo)> GetSyndications(JObject repo)
    {
        foreach (JObject image in repo["images"]!.Children<JObject>())
        {
            IEnumerable<JObject> tagGroups =
            [
                (JObject)image["sharedTags"]!,
                .. image["platforms"]!
                    .Children<JObject>()
                    .Select(platform => (JObject)platform["tags"]!)
            ];

            foreach (JProperty tag in tagGroups.SelectMany(tagGroup => tagGroup.Properties()))
            {
                JToken? syndication = tag.Value["syndication"];
                string? syndicatedRepo = syndication?["repo"]?.Value<string>();
                if (syndicatedRepo is not null)
                {
                    IEnumerable<string> destinationTags = syndication!["destinationTags"] is JArray configuredDestinationTags
                        ? configuredDestinationTags.Values<string>().Select(destinationTag => destinationTag!)
                        : [tag.Name];

                    foreach (string destinationTag in destinationTags)
                    {
                        yield return (ResolveTag(destinationTag), syndicatedRepo);
                    }
                }
            }
        }
    }

    private static string ResolveTag(string tag) =>
        Regex.Replace(
            tag,
            @"\$\((?<variable>[\w:\-.|]+)\)",
            match => Config.GetVariableValue(match.Groups["variable"].Value));
}
