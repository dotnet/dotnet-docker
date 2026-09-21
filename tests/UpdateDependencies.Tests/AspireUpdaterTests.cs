// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class AspireUpdaterTests
{
    [Fact]
    public async Task Channel_UsesLatestAspireBuild()
    {
        var build = CreateBuild();
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        barClient.Setup(client => client.GetLatestBuildAsync(AspireUpdater.PublicRepository, 5555)).ReturnsAsync(build);
        using var handler = new ArchiveHandler();
        using var httpClient = new HttpClient(handler);
        var updater = new AspireUpdater(barClient.Object, httpClient, Mock.Of<ILogger<AspireUpdater>>());
        var variables = CreateVariables();

        DependencyUpdate update = await updater.ResolveFromBarChannelAsync(5555, TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("aspire-dashboard|build-version").ShouldBe("13.6.0-preview.1.26453.4");
        barClient.Verify(client => client.GetLatestBuildAsync(AspireUpdater.PublicRepository, 5555), Times.Once);
    }

    [Fact]
    public async Task BarBuild_UsesStableTagsForPreviewBuild()
    {
        using var handler = new ArchiveHandler();
        using var httpClient = new HttpClient(handler);
        var updater = new AspireUpdater(Mock.Of<IBasicBarClient>(), httpClient, Mock.Of<ILogger<AspireUpdater>>());
        var variables = CreateVariables();

        DependencyUpdate update = await updater.ResolveFromBarBuildAsync(CreateBuild(), TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("aspire-dashboard|product-version").ShouldBe("13.6.0");
        variables.GetRawValue("aspire-dashboard|fixed-tag").ShouldBe("13.6.0");
        variables.GetRawValue("aspire-dashboard|minor-tag").ShouldBe("13.6");
        variables.GetRawValue("aspire-dashboard|major-tag").ShouldBe("13");
    }

    [Fact]
    public async Task BarBuild_HashesArchiveFromCurrentBaseUrl()
    {
        using var handler = new ArchiveHandler();
        using var httpClient = new HttpClient(handler);
        var updater = new AspireUpdater(Mock.Of<IBasicBarClient>(), httpClient, Mock.Of<ILogger<AspireUpdater>>());
        var variables = CreateVariables();
        DependencyUpdate update = await updater.ResolveFromBarBuildAsync(CreateBuild(), TestContext.Current.CancellationToken);
        variables.SetValue("aspire-dashboard|base-url|nightly", "https://example.invalid/updated");

        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        handler.Requests.ShouldBe(["https://example.invalid/updated/aspire-dashboard-linux-x64.zip"]);
        variables.GetRawValue("aspire-dashboard|linux|x64|sha").ShouldBe(
            Convert.ToHexStringLower(SHA512.HashData(Encoding.UTF8.GetBytes("archive"))));
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
    public async Task BarBuild_RejectsNonAspireRepository()
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
            "aspire-dashboard|build-version": "12.5.0",
            "aspire-dashboard|product-version": "12.5.0",
            "aspire-dashboard|fixed-tag": "12.5.0",
            "aspire-dashboard|minor-tag": "12.5",
            "aspire-dashboard|major-tag": "12",
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
