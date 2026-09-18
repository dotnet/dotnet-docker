// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class MonitorUpdaterTests
{
    private const string ArchiveContent = "monitor archive";
    private static readonly string s_checksum = Convert.ToHexStringLower(
        SHA512.HashData(Encoding.UTF8.GetBytes(ArchiveContent)));

    private static readonly (string Product, string Archive, string Os, string Arch)[] s_products =
    [
        ("monitor", "dotnet-monitor", "linux", "x64"),
        ("monitor-base", "dotnet-monitor-base", "win", "x64"),
        ("monitor-ext-azureblobstorage", "dotnet-monitor-egress-azureblobstorage", "linux-musl", "arm64"),
        ("monitor-ext-s3storage", "dotnet-monitor-egress-s3storage", "win", "arm64"),
    ];

    [Theory]
    [InlineData("9.0.5-servicing.25556.2", "9.0.5", "9.0.5", "nightly", true)]
    [InlineData("9.0.5-servicing.25556.2", "9.0.5", "9.0.5", "nightly", false)]
    [InlineData("9.0.0-rtm.12345.1", "9.0.0", "9.0.0", "main", true)]
    [InlineData("9.0.3-preview.1.26303.1", "9.0.3-preview.1", "9.0.3-preview.1.26303.1", "nightly", true)]
    [InlineData("9.0.3-preview.1.26303.1", "9.0.3-preview.1", "9.0.3-preview.1.26303.1", "nightly", false)]
    [InlineData("9.0.5", "9.0.5", "9.0.5", "main", true)]
    public async Task Version_UpdatesSharedEditorAndChecksumsWithoutSavingOrGenerating(
        string version, string productVersion, string fileVersion, string branch, bool publishedChecksum)
    {
        using var repo = new TempRepo();
        ManifestVariables variables = CreateVariables(branch);
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        string originalContent = variables.Content;
        File.WriteAllText(manifestPath, originalContent);
        string quality = branch == "main" ? "maintenance" : "preview";
        variables.SetValue(
            $"base-url|public|{quality}|{branch}", "https://example.invalid/shared/public");
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var handler = new ChecksumHandler(publishedChecksum ? HttpStatusCode.OK : HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        var updater = new MonitorUpdater(artifacts.Object, httpClient, Mock.Of<ILogger<MonitorUpdater>>());
        await (await updater.ResolveFromVersionAsync(version, TestContext.Current.CancellationToken))
            .ApplyAsync(variables, repo.LocalPath, TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor|9.0|base-url|" + branch)
            .ShouldBe($"$(base-url|public|{quality}|{branch})");
        variables.GetRawValue("monitor|9.0|base-url|checksums|" + branch)
            .ShouldBe($"$(base-url|public-checksums|{quality}|{branch})");
        foreach (var (product, archive, os, arch) in s_products)
        {
            variables.GetValue($"{product}|9.0|build-version").ShouldBe(version);
            variables.GetValue($"{product}|9.0|product-version").ShouldBe(productVersion);
            variables.GetRawValue($"{product}|9.0|{os}|{arch}|sha").ShouldBe(s_checksum);
            string extension = os == "win" ? "zip" : "tar.gz";
            string archiveUrl = $"https://example.invalid/shared/public/diagnostics/monitor/{version}/{archive}-{fileVersion}-{os}-{arch}.{extension}";
            string checksumUrl = archiveUrl.Replace("/public/", "/public-checksums/") + ".sha512";
            handler.Requests.ShouldContain(checksumUrl);
            if (publishedChecksum)
            {
                handler.Requests.ShouldNotContain(archiveUrl);
            }
            else
            {
                handler.Requests.ShouldContain(archiveUrl);
            }
        }
        variables.GetRawValue("monitor-base|9.0|build-version").ShouldBe("$(monitor|9.0|build-version)");
        variables.GetRawValue("monitor-base|9.0|product-version").ShouldBe("$(monitor|9.0|product-version)");
        variables.GetRawValue("monitor|8.0|linux|x64|sha").ShouldBe("unrelated");
        File.ReadAllText(manifestPath).ShouldBe(originalContent);
        Directory.Exists(Path.Combine(repo.LocalPath, "eng")).ShouldBeFalse();
        handler.Requests.Count.ShouldBe(s_products.Length * (publishedChecksum ? 1 : 2));
        artifacts.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Pipeline_ResolvesVersionUsingSameSingletonAsExplicitVersion()
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        PipelineArtifactFile[] expectedFiles = [new("Build_Info", "dotnet-monitor.nupkg.buildversion")];
        artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
            "organization", "project", 42,
            It.Is<IEnumerable<PipelineArtifactFile>>(files => files.SequenceEqual(expectedFiles))))
            .ReturnsAsync(" 9.0.5-servicing.25556.2\r\n");
        using var handler = new ChecksumHandler();
        using var httpClient = new HttpClient(handler);
        using var services = new ServiceCollection()
            .AddSingleton(artifacts.Object)
            .AddSingleton(httpClient)
            .AddLogging()
            .AddSingleton<MonitorUpdater>()
            .BuildServiceProvider();
        var versionUpdater = services.GetRequiredService<MonitorUpdater>();
        var pipelineUpdater = (IPipelineBuildUpdater)versionUpdater;
        Assert.Same(versionUpdater, pipelineUpdater);
        versionUpdater.ShouldBeSameAs(services.GetRequiredService<MonitorUpdater>());
        var variables = CreateVariables(pinnedChecksums: false);

        await (await pipelineUpdater.ResolveFromPipelineBuildAsync(
                new PipelineBuildReference("organization", "project", 42),
                TestContext.Current.CancellationToken))
            .ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetValue("monitor|9.0|build-version").ShouldBe("9.0.5-servicing.25556.2");
        variables.GetValue("monitor|9.0|product-version").ShouldBe("9.0.5");
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
        var updater = new MonitorUpdater(artifacts.Object, httpClient, Mock.Of<ILogger<MonitorUpdater>>());
        var first = CreateVariables(pinnedChecksums: false);
        var second = CreateVariables(pinnedChecksums: false);

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
        var updater = new MonitorUpdater(artifacts.Object, httpClient, Mock.Of<ILogger<MonitorUpdater>>());
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
        var updater = new MonitorUpdater(artifacts.Object, httpClient, Mock.Of<ILogger<MonitorUpdater>>());

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
        var updater = new MonitorUpdater(artifacts.Object, httpClient, Mock.Of<ILogger<MonitorUpdater>>());

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
        var updater = new MonitorUpdater(artifacts.Object, httpClient, Mock.Of<ILogger<MonitorUpdater>>());

        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await (await updater.ResolveFromVersionAsync("9.0.5", cancellation.Token))
                .ApplyAsync(CreateVariables(), "", cancellation.Token));

        handler.Requests.Count.ShouldBe(1);
        artifacts.VerifyNoOtherCalls();
    }

    private static ManifestVariables CreateVariables(string branch = "nightly", bool pinnedChecksums = true)
    {
        string quality = branch == "main" ? "maintenance" : "preview";
        var variables = new Dictionary<string, string>
        {
            ["branch"] = branch,
            [$"base-url|public|{quality}|{branch}"] = "https://example.invalid/public",
            [$"base-url|public-checksums|{quality}|{branch}"] = "https://example.invalid/public-checksums",
            [$"monitor|9.0|base-url|{branch}"] = "old",
            [$"monitor|9.0|base-url|checksums|{branch}"] = "old",
            ["monitor|8.0|linux|x64|sha"] = "unrelated",
        };
        foreach (var (product, _, os, arch) in s_products)
        {
            variables[$"{product}|9.0|build-version"] = "9.0.1";
            variables[$"{product}|9.0|product-version"] = "9.0.1";
            if (pinnedChecksums)
            {
                variables[$"{product}|9.0|{os}|{arch}|sha"] = "old";
            }
        }
        variables["monitor-base|9.0|build-version"] = "$(monitor|9.0|build-version)";
        variables["monitor-base|9.0|product-version"] = "$(monitor|9.0|product-version)";

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
                Content = new StringContent(isChecksum ? checksum ?? $" {s_checksum.ToUpperInvariant()}\r\n" : ArchiveContent),
            });
        }
    }
}
