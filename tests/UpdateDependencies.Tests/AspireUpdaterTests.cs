// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class AspireUpdaterTests
{
    [Fact]
    public async Task Channel_UpdatesSharedEditorUsingOneSingleton()
    {
        var build = CreateBuild();
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        barClient.Setup(client => client.GetLatestBuildAsync(AspireUpdater.PublicRepository, 5555)).ReturnsAsync(build);
        using var handler = new ArchiveHandler();
        using var httpClient = new HttpClient(handler);
        using var services = new ServiceCollection()
            .AddSingleton(barClient.Object)
            .AddSingleton(httpClient)
            .AddLogging()
            .AddSingleton<AspireUpdater>()
            .BuildServiceProvider();
        var updater = services.GetRequiredService<AspireUpdater>();
        var channelUpdater = (IBarChannelUpdater)updater;
        var buildUpdater = (IBarBuildUpdater)updater;
        Assert.Same(buildUpdater, channelUpdater);
        updater.ShouldBeSameAs(services.GetRequiredService<AspireUpdater>());
        var variables = CreateVariables();
        variables.SetValue("aspire-dashboard|base-url|nightly", "https://example.invalid/shared-editor");
        await (await channelUpdater.ResolveFromBarChannelAsync(5555, TestContext.Current.CancellationToken))
            .ApplyAsync(variables, "missing-workspace", TestContext.Current.CancellationToken);

        variables.GetRawValue("aspire-dashboard|build-version").ShouldBe("13.6.0-preview.1.26453.4");
        variables.GetRawValue("aspire-dashboard|product-version").ShouldBe("13.6.0");
        variables.GetRawValue("aspire-dashboard|fixed-tag").ShouldBe("13.6.0");
        variables.GetRawValue("aspire-dashboard|minor-tag").ShouldBe("13.6");
        variables.GetRawValue("aspire-dashboard|major-tag").ShouldBe("13");
        variables.GetRawValue("aspire-dashboard|linux|x64|sha").ShouldBe(
            Convert.ToHexStringLower(SHA512.HashData(Encoding.UTF8.GetBytes("archive"))));
        handler.Requests.ShouldBe(["https://example.invalid/shared-editor/aspire-dashboard-linux-x64.zip"]);
        barClient.Verify(client => client.GetLatestBuildAsync(AspireUpdater.PublicRepository, 5555), Times.Once);
        barClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task BarBuild_PropagatesArchiveCancellationWithoutMutatingEditor()
    {
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        using var cancellation = new CancellationTokenSource();
        using var handler = new ArchiveHandler(cancellation);
        using var httpClient = new HttpClient(handler);
        var updater = new AspireUpdater(barClient.Object, httpClient, Mock.Of<ILogger<AspireUpdater>>());
        var variables = CreateVariables();
        string original = variables.Content;

        DependencyUpdate update = await updater.ResolveFromBarBuildAsync(CreateBuild(), cancellation.Token);
        await Should.ThrowAsync<OperationCanceledException>(() =>
            update.ApplyAsync(variables, "missing-workspace", cancellation.Token));

        variables.Content.ShouldBe(original);
        handler.Requests.Count.ShouldBe(1);
        barClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task BarBuild_RejectsNonAspireBuildBeforeReadingManifest()
    {
        var build = CreateBuild();
        build.GitHubRepository = "https://github.com/dotnet/dotnet";
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        using var handler = new ArchiveHandler();
        using var httpClient = new HttpClient(handler);
        var updater = new AspireUpdater(barClient.Object, httpClient, Mock.Of<ILogger<AspireUpdater>>());

        var exception = await Should.ThrowAsync<ArgumentException>(() =>
            updater.ResolveFromBarBuildAsync(build, TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("not an Aspire build");
        handler.Requests.ShouldBeEmpty();
        barClient.VerifyNoOtherCalls();
    }

    private static ManifestVariables CreateVariables() => new("""
        {
          "variables": {
            "branch": "nightly",
            "aspire-dashboard|base-url|nightly": "https://example.invalid/original",
            "aspire-dashboard|build-version": "13.5.0",
            "aspire-dashboard|product-version": "13.5.0",
            "aspire-dashboard|fixed-tag": "13.5.0",
            "aspire-dashboard|minor-tag": "13.5",
            "aspire-dashboard|major-tag": "13",
            "aspire-dashboard|linux|x64|sha": "old"
          }
        }
        """);

    private static Build CreateBuild() => new(
        id: 123,
        dateProduced: DateTimeOffset.UtcNow,
        staleness: 0,
        released: false,
        stable: false,
        commit: "commit",
        channels: [],
        assets:
        [
            new Asset(
                id: 1,
                buildId: 123,
                nonShipping: false,
                name: "aspire-dashboard-linux-x64.zip",
                version: "13.6.0-preview.1.26453.4",
                locations: []),
        ],
        dependencies: [],
        incoherencies: [])
    {
        GitHubRepository = AspireUpdater.PublicRepository,
    };

    private sealed class ArchiveHandler(CancellationTokenSource? cancellation = null) : HttpMessageHandler
    {
        public List<string> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request.RequestUri!.AbsoluteUri);
            if (cancellation is not null)
            {
                cancellation.Cancel();
                return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("archive"),
            });
        }
    }
}
