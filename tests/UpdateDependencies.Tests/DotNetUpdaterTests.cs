// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.Docker.UpdateDependencies.Model.Release;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UpdateDependencies.Tests;

public sealed class DotNetUpdaterTests
{
    [Fact]
    public async Task Channel_ResolvesAssetsFromLatestBuild()
    {
        var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
        bar.Setup(client => client.GetLatestBuildAsync(DotNetUpdater.PublicRepository, 42))
            .ReturnsAsync(CreateBuild());
        bar.Setup(client => client.GetAssetsAsync(null, null, 123, null)).ReturnsAsync(CreateAssets());

        await CreateUpdater(bar.Object).ResolveFromBarChannelAsync(42, TestContext.Current.CancellationToken);

        bar.Verify(client => client.GetLatestBuildAsync(DotNetUpdater.PublicRepository, 42), Times.Once);
        bar.Verify(client => client.GetAssetsAsync(null, null, 123, null), Times.Once);
    }

    [Fact]
    public async Task BarBuild_MapsAssetsToProductVersions()
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateManifest(repo.LocalPath);
        var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
        bar.Setup(client => client.GetAssetsAsync(null, null, 123, null)).ReturnsAsync(CreateAssets());
        var updater = CreateUpdater(bar.Object);

        DependencyUpdate update = await updater.ResolveFromBarBuildAsync(CreateBuild(), TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, repo.LocalPath, TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe("11.0.2");
        variables.GetRawValue("aspnet|11.0|build-version").ShouldBe("11.0.3");
        variables.GetRawValue("sdk|11.0|build-version").ShouldBe("11.0.101");
    }

    [Theory]
    [InlineData("", "11.0.101")]
    [InlineData("https://example/internal", "11.0.101-servicing.12345.1")]
    public async Task Staging_SelectsHighestSdkFromRequestedSource(string internalBaseUrl, string expectedSdk)
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateManifest(repo.LocalPath);
        ReleaseConfig release = CreateRelease(false);

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, release, internalBaseUrl, TestContext.Current.CancellationToken);

        variables.GetRawValue("sdk|11.0|build-version").ShouldBe(expectedSdk);
    }

    [Theory]
    [InlineData("", false, "11.0.2", "11.0.3", "11.0.2")]
    [InlineData("", true, "11.0.2", "11.0.3", "11.0.2")]
    [InlineData("https://example/internal", false, "11.0.2-servicing.12345.1", "11.0.3-servicing.12345.1", "11.0.2")]
    [InlineData("https://example/internal", true, "11.0.1", "11.0.1", "11.0.1")]
    public async Task Staging_PreservesRuntimeAndAspNetOnlyForInternalSdkOnlyRelease(
        string internalBaseUrl, bool sdkOnly, string expectedRuntime, string expectedAspNet, string expectedTag)
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateManifest(repo.LocalPath);

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, CreateRelease(sdkOnly), internalBaseUrl, TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(expectedRuntime);
        variables.GetRawValue("aspnet|11.0|build-version").ShouldBe(expectedAspNet);
        variables.GetRawValue("dotnet|11.0|product-version").ShouldBe(expectedTag);
    }

    [Theory]
    [InlineData(false, "https://example/internal", "old")]
    [InlineData(true, "old", "https://example/internal")]
    public async Task Staging_SelectsInternalBaseUrlForReleaseScope(
        bool sdkOnly, string expectedRuntimeBaseUrl, string expectedSdkBaseUrl)
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateManifest(repo.LocalPath);

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, CreateRelease(sdkOnly), "https://example/internal", TestContext.Current.CancellationToken);

        variables.GetRawValue("sdk|11.0|base-url|nightly").ShouldBe(expectedSdkBaseUrl);
        variables.GetRawValue("dotnet|11.0|base-url|nightly").ShouldBe(expectedRuntimeBaseUrl);
    }

    [Fact]
    public async Task Staging_UpdatesOnlyExistingManifestVariables()
    {
        using var repo = new TempRepo();
        var variables = CreateManifest(repo.LocalPath);
        variables.Contains("runtime|11.0|product-version").ShouldBeFalse();

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, CreateRelease(false), "", TestContext.Current.CancellationToken);

        variables.Contains("runtime|11.0|product-version").ShouldBeFalse();
        variables.GetRawValue("runtime|11.0|linux|x64|sha").ShouldBe("unchanged");
    }

    [Theory]
    [InlineData("11.0.2", "11.0.2")]
    [InlineData("11.0.2-servicing.12345.1", "11.0.2")]
    [InlineData("11.0.2-rtm.12345.1", "11.0.2")]
    [InlineData("11.0.2-preview.2.12345.1", "11.0.2-preview.2")]
    [InlineData("11.0.2-rc.1.12345.1", "11.0.2-rc.1")]
    [InlineData("11.0.2+build.123", "11.0.2")]
    public async Task Staging_WritesBuildVersionAndProductTag(string buildVersion, string productVersion)
    {
        using var repo = new TempRepo();
        var variables = CreateManifest(repo.LocalPath);
        var release = CreateRelease(false) with { RuntimeBuild = buildVersion };

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, release, "https://example/internal", TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(buildVersion);
        variables.GetRawValue("dotnet|11.0|product-version").ShouldBe(productVersion);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("$(alias)", "$(alias)")]
    [InlineData("not-a-version", "not-a-version")]
    [InlineData("11.0.1", "11.0.2")]
    public async Task Staging_PreservesDisabledVersionsAndAliases(string current, string expected)
    {
        using var repo = new TempRepo();
        var variables = CreateManifest(repo.LocalPath);
        variables.SetValue("runtime|11.0|build-version", current);

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, CreateRelease(false), "", TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(expected);
    }

    [Fact]
    public async Task Channel_CancelsPendingBuildLookup()
    {
        var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
        var pending = new TaskCompletionSource<Build>(TaskCreationOptions.RunContinuationsAsynchronously);
        bar.Setup(client => client.GetLatestBuildAsync("https://github.com/dotnet/dotnet", 42))
            .Returns(pending.Task);
        var updater = CreateUpdater(bar: bar.Object);
        using var cancellation = new CancellationTokenSource();
        Task<DependencyUpdate> lookup = updater.ResolveFromBarChannelAsync(42, cancellation.Token);
        lookup.IsCompleted.ShouldBeFalse();

        cancellation.Cancel();
        await Should.ThrowAsync<OperationCanceledException>(() =>
            lookup.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken));

        bar.Verify(client => client.GetLatestBuildAsync(DotNetUpdater.PublicRepository, 42), Times.Once);
        bar.VerifyNoOtherCalls();
    }

    private static Build CreateBuild() => JsonConvert.DeserializeObject<Build>("""
        {"id":123,"githubRepository":"https://github.com/dotnet/dotnet","commit":"abc"}
        """)!;

    private static List<Asset> CreateAssets() => JsonConvert.DeserializeObject<List<Asset>>("""
        [
          {"name":"Sdk/11.0.101/productVersion.txt","version":"11.0.101"},
          {"name":"Runtime/11.0.2/productVersion.txt","version":"11.0.2"},
          {"name":"aspnetcore/Runtime/11.0.3/productVersion.txt","version":"11.0.3"}
        ]
        """)!;

    private static DotNetUpdater CreateUpdater(
        IBasicBarClient? bar = null) =>
        new(
            bar ?? Mock.Of<IBasicBarClient>(),
            Mock.Of<ILogger<DotNetUpdater>>());

    private static ManifestVariables CreateManifest(string repoRoot)
    {
        const string content = """
            {"variables":{
              "branch":"nightly",
              "dotnet|11.0|product-version":"11.0.1",
              "runtime|11.0|build-version":"11.0.1",
              "runtime|11.0|linux|x64|sha":"unchanged",
              "aspnet|11.0|build-version":"11.0.1",
              "aspnet-composite|11.0|build-version":"$(aspnet|11.0|build-version)",
              "sdk|11.0|build-version":"11.0.100",
              "dotnet|11.0|base-url|nightly":"old",
              "sdk|11.0|base-url|nightly":"old"
            }}
            """;
        string configDirectory = Path.Combine(repoRoot, "tests", "Microsoft.DotNet.Docker.Tests", "TestAppArtifacts");
        Directory.CreateDirectory(configDirectory);
        File.WriteAllText(Path.Combine(configDirectory, "NuGet.config.nightly"), "<configuration />");
        File.WriteAllText(Path.Combine(configDirectory, "NuGet.config.internal"), "<configuration />");
        return new ManifestVariables(content);
    }

    private static ReleaseConfig CreateRelease(bool sdkOnly) => new()
    {
        Channel = "11.0",
        MajorVersion = "11",
        Release = "11.0.2",
        Runtime = "11.0.2",
        RuntimeBuild = "11.0.2-servicing.12345.1",
        Sdks = ["11.0.101", "11.0.100"],
        SdkBuilds = ["11.0.101-servicing.12345.1", "11.0.100-servicing.12345.1"],
        Asp = "11.0.3",
        AspBuild = "11.0.3-servicing.12345.1",
        Security = false,
        SupportPhase = "active",
        Internal = false,
        SdkOnly = sdkOnly,
    };
}
