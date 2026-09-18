// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.Docker.UpdateDependencies.Model.Release;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UpdateDependencies.Tests;

public sealed class DotNetUpdaterTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BarBuildAndChannel_UpdateSharedEditorWithoutSavingOrGenerating(bool fromChannel)
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateManifest(repo.LocalPath);
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        string original = File.ReadAllText(manifestPath);
        Build build = JsonConvert.DeserializeObject<Build>("""
            {"id":123,"githubRepository":"https://github.com/dotnet/dotnet","commit":"abc"}
            """)!;
        List<Asset> assets = JsonConvert.DeserializeObject<List<Asset>>("""
            [
              {"name":"Sdk/11.0.100/productVersion.txt","version":"11.0.100"},
              {"name":"Runtime/11.0.2/productVersion.txt","version":"11.0.2"},
              {"name":"aspnetcore/Runtime/11.0.3/productVersion.txt","version":"11.0.3"}
            ]
            """)!;
        var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
        bar.Setup(client => client.GetAssetsAsync(null, null, 123, null)).ReturnsAsync(assets);
        if (fromChannel)
        {
            bar.Setup(client => client.GetLatestBuildAsync("https://github.com/dotnet/dotnet", 42))
                .ReturnsAsync(build);
        }
        var updater = CreateUpdater(bar: bar.Object);
        using var services = new ServiceCollection()
            .AddSingleton(updater)
            .BuildServiceProvider();

        var registeredUpdater = services.GetRequiredService<DotNetUpdater>();
        registeredUpdater.ShouldBeSameAs(updater);
        Assert.Same((IBarBuildUpdater)registeredUpdater, (IBarChannelUpdater)registeredUpdater);
        if (fromChannel)
        {
            await ((IBarChannelUpdater)registeredUpdater).UpdateFromBarChannelAsync(
                variables, repo.LocalPath, 42, TestContext.Current.CancellationToken);
        }
        else
        {
            await ((IBarBuildUpdater)registeredUpdater).UpdateFromBarBuildAsync(
                variables, repo.LocalPath, build, TestContext.Current.CancellationToken);
        }

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe("11.0.2");
        variables.GetRawValue("dotnet|11.0|product-version").ShouldBe("11.0.2");
        variables.GetRawValue("aspnet|11.0|build-version").ShouldBe("11.0.3");
        variables.GetRawValue("aspnet-composite|11.0|build-version").ShouldBe("$(aspnet|11.0|build-version)");
        variables.GetRawValue("sdk|11.0|build-version").ShouldBe("11.0.100");
        variables.GetRawValue("runtime|11.0|linux|x64|sha").ShouldBe("unchanged");
        File.ReadAllText(manifestPath).ShouldBe(original);
        bar.VerifyAll();
        bar.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task Staging_PreservesPublicInternalAndSdkOnlySemantics(bool internalBuild, bool sdkOnly)
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateManifest(repo.LocalPath);
        string original = variables.Content;
        ReleaseConfig release = CreateRelease(sdkOnly);
        var updater = CreateUpdater();
        string internalBaseUrl = internalBuild
            ? "https://dotnetstage.blob.core.windows.net/stage-123/assets/shipping/assets"
            : "";

        await updater.UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, release, internalBaseUrl, TestContext.Current.CancellationToken);

        string expectedSdk = internalBuild ? "11.0.101-servicing.12345.1" : "11.0.101";
        variables.GetRawValue("sdk|11.0|build-version").ShouldBe(expectedSdk);
        string expectedRuntime = internalBuild
            ? sdkOnly ? "11.0.1" : release.RuntimeBuild
            : release.Runtime;
        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(expectedRuntime);
        variables.GetRawValue("dotnet|11.0|product-version")
            .ShouldBe(internalBuild && sdkOnly ? "11.0.1" : "11.0.2");
        if (internalBuild)
        {
            string product = sdkOnly ? "sdk" : "dotnet";
            variables.GetRawValue($"{product}|11.0|base-url|nightly")
                .ShouldBe("https://dotnetstage.blob.core.windows.net/stage-123/assets/shipping/assets");
        }

        File.ReadAllText(Path.Combine(repo.LocalPath, "manifest.versions.json")).ShouldBe(original);
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
    public async Task CancelledBuildLookup_DoesNotMutateManifest()
    {
        var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
        var pending = new TaskCompletionSource<Build>();
        bar.Setup(client => client.GetLatestBuildAsync("https://github.com/dotnet/dotnet", 42))
            .Returns(pending.Task);
        var updater = CreateUpdater(bar: bar.Object);
        var variables = new ManifestVariables("""{"variables":{}}""");

        await Should.ThrowAsync<OperationCanceledException>(() => updater.UpdateFromBarChannelAsync(
            variables, "unused", 42, new CancellationToken(true)));

        bar.VerifyAll();
        bar.VerifyNoOtherCalls();
        variables.Content.ShouldBe("""{"variables":{}}""");
    }

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
        File.WriteAllText(Path.Combine(repoRoot, "manifest.versions.json"), content);
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
        Sdks = ["11.0.100", "11.0.101"],
        SdkBuilds = ["11.0.100-servicing.12345.1", "11.0.101-servicing.12345.1"],
        Asp = "11.0.3",
        AspBuild = "11.0.3-servicing.12345.1",
        Security = false,
        SupportPhase = "active",
        Internal = false,
        SdkOnly = sdkOnly,
    };
}
