// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
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
        JArray images = Assert.IsType<JArray>(aspireDashboardRepo["images"]);
        JObject image = Assert.IsType<JObject>(Assert.Single(images));
        string? syndicatedRepo = image.Value<string>("syndication");

        if (majorVersion != 13)
        {
            Assert.Null(syndicatedRepo);
            return;
        }

        string branch = Config.GetVariableValue("branch");
        string expectedLegacyRepo = branch == "nightly"
            ? "dotnet/nightly/aspire-dashboard"
            : "dotnet/aspire-dashboard";

        Assert.Equal(
            expectedLegacyRepo,
            Config.Manifest.Value["variables"]?["syndicatedAspireDashboardRepo"]?.Value<string>());
        Assert.Equal(SyndicatedRepoVariable, syndicatedRepo);
    }

    private static JObject GetAspireDashboardRepo() =>
        Config.Manifest.Value["repos"]!
            .Children<JObject>()
            .Single(repo => repo.Value<string>("id") == AspireDashboardId);
}
