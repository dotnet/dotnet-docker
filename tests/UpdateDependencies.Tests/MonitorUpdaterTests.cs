// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class MonitorUpdaterTests
{
    private const string ArchiveContent = "monitor archive";
    private static readonly string s_publishedChecksum = new('a', 128);

    [Theory]
    [InlineData("monitor", "9.0.5-servicing.25556.2", "9.0.5")]
    [InlineData("monitor-base", "9.0.0-rtm.12345.1", "9.0.0")]
    [InlineData("monitor-ext-azureblobstorage", "9.0.3-preview.1.26303.1", "9.0.3-preview.1")]
    [InlineData("monitor-ext-s3storage", "9.0.5", "9.0.5")]
    public async Task Version_WritesBuildVersionAndProductTag(string product, string version, string productVersion)
    {
        var values = new Dictionary<string, string>
        {
            ["branch"] = "nightly",
            [$"{product}|9.0|build-version"] = "9.0.1",
            [$"{product}|9.0|product-version"] = "9.0.1",
        };
        var variables = new ManifestVariables(JsonSerializer.Serialize(new { variables = values }));
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync(version, TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue($"{product}|9.0|build-version").ShouldBe(version);
        variables.GetRawValue($"{product}|9.0|product-version").ShouldBe(productVersion);
    }

    [Theory]
    [InlineData("nightly", "$(base-url|public|preview|nightly)", "$(base-url|public-checksums|preview|nightly)")]
    [InlineData("main", "$(base-url|public|maintenance|main)", "$(base-url|public-checksums|maintenance|main)")]
    public async Task Version_SelectsPublicUrlsForBranch(string branch, string expectedBaseUrl, string expectedChecksumUrl)
    {
        var variables = CreateVariables(branch, checksumVariable: null);
        using var httpClient = new HttpClient(new ChecksumHandler());
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue($"monitor|9.0|base-url|{branch}").ShouldBe(expectedBaseUrl);
        variables.GetRawValue($"monitor|9.0|base-url|checksums|{branch}").ShouldBe(expectedChecksumUrl);
    }

    [Theory]
    [InlineData("monitor|9.0|linux|x64|sha", "dotnet-monitor-9.0.5-linux-x64.tar.gz")]
    [InlineData("monitor-base|9.0|win|x64|sha", "dotnet-monitor-base-9.0.5-win-x64.zip")]
    [InlineData("monitor-ext-azureblobstorage|9.0|linux-musl|arm64|sha", "dotnet-monitor-egress-azureblobstorage-9.0.5-linux-musl-arm64.tar.gz")]
    [InlineData("monitor-ext-s3storage|9.0|win|arm64|sha", "dotnet-monitor-egress-s3storage-9.0.5-win-arm64.zip")]
    public async Task Checksum_SelectsArchiveForProductAndPlatform(string variable, string archive)
    {
        var variables = CreateVariables(checksumVariable: variable);
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        handler.Requests.ShouldBe([$"https://example.invalid/public-checksums/diagnostics/monitor/9.0.5/{archive}.sha512"]);
        variables.GetRawValue(variable).ShouldBe(s_publishedChecksum);
    }

    [Theory]
    [InlineData("9.0.5-servicing.25556.2", "dotnet-monitor-9.0.5-linux-x64.tar.gz")]
    [InlineData("9.0.0-rtm.12345.1", "dotnet-monitor-9.0.0-linux-x64.tar.gz")]
    [InlineData("9.0.3-preview.1.26303.1", "dotnet-monitor-9.0.3-preview.1.26303.1-linux-x64.tar.gz")]
    public async Task Checksum_UsesStableArchiveNamesOnlyForServicingAndRtm(string version, string archive)
    {
        var variables = CreateVariables();
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync(version, TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        handler.Requests.ShouldBe([$"https://example.invalid/public-checksums/diagnostics/monitor/{version}/{archive}.sha512"]);
    }

    [Fact]
    public async Task Version_PreservesAliasesAndOtherVersionChecksums()
    {
        var variables = new ManifestVariables("""
            {"variables":{
              "branch":"nightly",
              "monitor|9.0|build-version":"9.0.1",
              "monitor|9.0|product-version":"9.0.1",
              "monitor-base|9.0|build-version":"$(monitor|9.0|build-version)",
              "monitor-base|9.0|product-version":"$(monitor|9.0|product-version)",
              "monitor|8.0|linux|x64|sha":"unrelated"
            }}
            """);
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor-base|9.0|build-version").ShouldBe("$(monitor|9.0|build-version)");
        variables.GetRawValue("monitor-base|9.0|product-version").ShouldBe("$(monitor|9.0|product-version)");
        variables.GetRawValue("monitor|8.0|linux|x64|sha").ShouldBe("unrelated");
        handler.Requests.ShouldBeEmpty();
    }

    [Fact]
    public async Task PublishedChecksum_IsNormalizedWithoutDownloadingArchive()
    {
        var variables = CreateVariables();
        using var handler = new ChecksumHandler(checksum: $" {s_publishedChecksum.ToUpperInvariant()}\r\n");
        using var httpClient = new HttpClient(handler);
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor|9.0|linux|x64|sha").ShouldBe(s_publishedChecksum);
        handler.Requests.ShouldBe([
            "https://example.invalid/public-checksums/diagnostics/monitor/9.0.5/dotnet-monitor-9.0.5-linux-x64.tar.gz.sha512",
        ]);
    }

    [Fact]
    public async Task MissingPublishedChecksum_HashesArchive()
    {
        var variables = CreateVariables();
        using var handler = new ChecksumHandler(HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        string expectedChecksum = Convert.ToHexStringLower(SHA512.HashData(Encoding.UTF8.GetBytes(ArchiveContent)));
        variables.GetRawValue("monitor|9.0|linux|x64|sha").ShouldBe(expectedChecksum);
        handler.Requests.ShouldBe([
            "https://example.invalid/public-checksums/diagnostics/monitor/9.0.5/dotnet-monitor-9.0.5-linux-x64.tar.gz.sha512",
            "https://example.invalid/public/diagnostics/monitor/9.0.5/dotnet-monitor-9.0.5-linux-x64.tar.gz",
        ]);
    }

    [Fact]
    public async Task Pipeline_UsesTrimmedVersionFromConfiguredBuildArtifact()
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        PipelineArtifactFile[] expectedFiles = [new("Build_Info", "dotnet-monitor.nupkg.buildversion")];
        artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
            "organization", "project", 42,
            It.Is<IEnumerable<PipelineArtifactFile>>(files => files.SequenceEqual(expectedFiles))))
            .ReturnsAsync(" 9.0.5-servicing.25556.2\r\n");
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());
        var variables = CreateVariables(checksumVariable: null);

        await (await updater.ResolveFromPipelineBuildAsync(
                42,
                TestContext.Current.CancellationToken))
            .ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetValue("monitor|9.0|build-version").ShouldBe("9.0.5-servicing.25556.2");
        handler.Requests.ShouldBeEmpty();
        artifacts.VerifyAll();
        artifacts.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExplicitVersions_DoNotFetchOrShareInvocationState()
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());
        var first = CreateVariables(checksumVariable: null);
        var second = CreateVariables(checksumVariable: null);

        var firstUpdate = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        var secondUpdate = await updater.ResolveFromVersionAsync("9.0.6", TestContext.Current.CancellationToken);

        await Task.WhenAll(
            firstUpdate.ApplyAsync(first, "", TestContext.Current.CancellationToken),
            secondUpdate.ApplyAsync(second, "", TestContext.Current.CancellationToken));

        first.GetRawValue("monitor|9.0|build-version").ShouldBe("9.0.5");
        second.GetRawValue("monitor|9.0|build-version").ShouldBe("9.0.6");
        artifacts.VerifyNoOtherCalls();
        handler.Requests.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("abc", 1)]
    [InlineData("g", 128)]
    public async Task InvalidPublishedChecksum_IsRejectedWithoutArchiveFallback(string value, int repeat)
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var handler = new ChecksumHandler(checksum: string.Concat(Enumerable.Repeat(value, repeat)));
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());
        var variables = CreateVariables();

        var exception = await Should.ThrowAsync<FormatException>(async () =>
            await (await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken))
                .ApplyAsync(variables, "", TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("128 hexadecimal characters");
        handler.Requests.Count.ShouldBe(1);
        handler.Requests.ShouldAllBe(url => url.EndsWith(".sha512"));
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task ChecksumHttpFailure_DoesNotFallBackToArchive(HttpStatusCode status)
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var handler = new ChecksumHandler(status);
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());

        await Should.ThrowAsync<HttpRequestException>(async () =>
            await (await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken))
                .ApplyAsync(CreateVariables(), "", TestContext.Current.CancellationToken));

        handler.Requests.Count.ShouldBe(1);
        handler.Requests.ShouldAllBe(url => url.EndsWith(".sha512"));
    }

    [Fact]
    public async Task MissingChecksumAndArchive_Throws()
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var handler = new ChecksumHandler(HttpStatusCode.NotFound, archiveStatus: HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());

        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
            await (await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken))
                .ApplyAsync(CreateVariables(), "", TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("Unable to retrieve checksum");
        handler.Requests.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Version_PropagatesChecksumCancellation()
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var cancellation = new CancellationTokenSource();
        using var handler = new ChecksumHandler(cancellation: cancellation);
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());

        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await (await updater.ResolveFromVersionAsync("9.0.5", cancellation.Token))
                .ApplyAsync(CreateVariables(), "", cancellation.Token));

        handler.Requests.Count.ShouldBe(1);
        artifacts.VerifyNoOtherCalls();
    }

    private static UpdateDependenciesConfiguration CreateConfiguration() =>
        new() { AzureDevOps = new() { Organization = "organization", Project = "project" } };

    private static MonitorUpdater CreateUpdater(HttpClient httpClient) =>
        new(Mock.Of<IPipelineArtifactProvider>(), httpClient, CreateConfiguration(), Mock.Of<ILogger<MonitorUpdater>>());

    private static ManifestVariables CreateVariables(
        string branch = "nightly", string? checksumVariable = "monitor|9.0|linux|x64|sha")
    {
        var variables = new Dictionary<string, string>
        {
            ["branch"] = branch,
            ["base-url|public|preview|nightly"] = "https://example.invalid/public",
            ["base-url|public-checksums|preview|nightly"] = "https://example.invalid/public-checksums",
            [$"monitor|9.0|base-url|{branch}"] = "old",
            [$"monitor|9.0|base-url|checksums|{branch}"] = "old",
            ["monitor|9.0|build-version"] = "9.0.1",
            ["monitor|9.0|product-version"] = "9.0.1",
        };
        if (checksumVariable is not null)
        {
            variables[checksumVariable] = "old";
        }

        return new ManifestVariables(JsonSerializer.Serialize(new { variables }));
    }

    private sealed class ChecksumHandler(
        HttpStatusCode checksumStatus = HttpStatusCode.OK,
        string? checksum = null,
        HttpStatusCode archiveStatus = HttpStatusCode.OK,
        CancellationTokenSource? cancellation = null) : HttpMessageHandler
    {
        public List<string> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string url = request.RequestUri!.AbsoluteUri;
            Requests.Add(url);
            if (cancellation is not null)
            {
                cancellation.Cancel();
                return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
            }
            bool isChecksum = url.EndsWith(".sha512");
            return Task.FromResult(new HttpResponseMessage(isChecksum ? checksumStatus : archiveStatus)
            {
                Content = new StringContent(isChecksum ? checksum ?? s_publishedChecksum : ArchiveContent),
            });
        }
    }
}
