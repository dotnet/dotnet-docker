// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Newtonsoft.Json.Linq;
using Xunit;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "pre-build")]
public sealed class AspireDashboardManifestTests
{
    private const string AspireDashboardId = "aspire-dashboard";
    [Theory]
    [InlineData("", "aspire/dashboard")]
    [InlineData("/nightly", "aspire/nightly/dashboard")]
    public void RepoName_UsesAspireProductFamily(string repoNameModifier, string expectedRepoName) => Assert.Equal(expectedRepoName, ImageData.GetRepoName(AspireDashboardId, repoNameModifier));

    [Fact]
    public void Manifest_UsesCanonicalRepoName()
    {
        string branch = Config.GetVariableValue("branch");
        string repoNameModifier = branch == "nightly" ? "/nightly" : string.Empty;

        Assert.Equal(
            ImageData.GetRepoName(AspireDashboardId, repoNameModifier),
            GetAspireDashboardRepo().Value<string>("name"));
    }

    private static JObject GetAspireDashboardRepo() =>
        Config.Manifest.Value["repos"]!
            .Children<JObject>()
            .Single(repo => repo.Value<string>("id") == AspireDashboardId);
}
