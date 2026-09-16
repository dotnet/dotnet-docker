// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Hosting;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class AspireCommandTests
{
    private const string BuildVersion = "13.6.0-preview.1.26453.4";
    private const string X64Archive = "x64 archive";
    private const string Arm64Archive = "arm64 archive";

    [Theory]
    [InlineData("main")]
    [InlineData("nightly")]
    public async Task BuildId_UpdatesAspireAndGeneratesInRequestedWorkspace(string branch)
    {
        using var repo = new TempRepo();
        string repoRoot = Path.Combine(repo.LocalPath, "workspace with spaces");
        string manifestPath = WriteManifest(repoRoot, branch);
        WriteGenerators(repoRoot);
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var barClient = CreateBarClient();
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var options = new AspireOptions { FromBuildId = 123, RepoRoot = repoRoot };
        string workingDirectory = Directory.GetCurrentDirectory();

        int exitCode = await command.ExecuteAsync(options, TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        AssertUpdatedManifest(manifestPath);
        handler.Requests.ShouldBe(
        [
            $"https://example.invalid/{branch}/aspire/{BuildVersion}/aspire-dashboard-linux-x64.zip",
            $"https://example.invalid/{branch}/aspire/{BuildVersion}/aspire-dashboard-linux-arm64.zip",
        ], ignoreOrder: true);
        string dockerfileOutput = Path.Combine(repoRoot, "generated-dockerfiles.txt");
        string readmeOutput = Path.Combine(repoRoot, "generated-readmes.txt");
        File.ReadAllText(dockerfileOutput).Trim().ShouldBe(BuildVersion);
        File.ReadAllText(readmeOutput).Trim().ShouldBe("13.6.0");
        Directory.GetCurrentDirectory().ShouldBe(workingDirectory);

        File.Delete(dockerfileOutput);
        File.Delete(readmeOutput);
        byte[] originalBytes = File.ReadAllBytes(manifestPath);
        File.SetLastWriteTimeUtc(manifestPath, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        DateTime originalWriteTime = File.GetLastWriteTimeUtc(manifestPath);

        int noOpExitCode = await command.ExecuteAsync(options, TestContext.Current.CancellationToken);

        noOpExitCode.ShouldBe(0);
        File.ReadAllBytes(manifestPath).ShouldBe(originalBytes);
        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
        File.ReadAllText(dockerfileOutput).Trim().ShouldBe(BuildVersion);
        File.ReadAllText(readmeOutput).Trim().ShouldBe("13.6.0");
        barClient.Verify(client => client.GetBuildAsync(123), Times.Exactly(2));
        barClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Updater_UsesSharedManifestWithoutSavingOrGenerating()
    {
        using var callerRepo = new TempRepo();
        using var targetRepo = new TempRepo();
        string callerManifest = WriteManifest(callerRepo.LocalPath, "main");
        string targetManifest = WriteManifest(targetRepo.LocalPath, "nightly");
        string callerContent = File.ReadAllText(callerManifest);
        string targetContent = File.ReadAllText(targetManifest);
        var callerVariables = new ManifestVariables(callerContent);
        var targetVariables = new ManifestVariables(targetContent);
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        var updater = new AspireUpdater(barClient.Object, httpClient, Mock.Of<ILogger<AspireUpdater>>());
        var build = CreateBuild();

        await updater.UpdateFromBarBuildAsync(
            targetVariables, targetRepo.LocalPath, build, TestContext.Current.CancellationToken);

        AssertUpdatedVariables(targetVariables);
        File.ReadAllText(targetManifest).ShouldBe(targetContent);
        File.ReadAllText(callerManifest).ShouldBe(callerContent);
        handler.Requests.ShouldAllBe(url => url.StartsWith("https://example.invalid/nightly/"));
        handler.Requests.Clear();

        await updater.UpdateFromBarBuildAsync(
            callerVariables, callerRepo.LocalPath, build, TestContext.Current.CancellationToken);

        AssertUpdatedVariables(callerVariables);
        File.ReadAllText(callerManifest).ShouldBe(callerContent);
        handler.Requests.ShouldAllBe(url => url.StartsWith("https://example.invalid/main/"));
        barClient.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.Unauthorized)]
    public async Task ExecuteAsync_DoesNotSaveOrGenerateWhenChecksumFails(HttpStatusCode status)
    {
        using var repo = new TempRepo();
        string manifestPath = WriteManifest(repo.LocalPath);
        string originalContent = File.ReadAllText(manifestPath);
        WriteGenerators(repo.LocalPath);
        using var handler = new DashboardHandler(status);
        using var httpClient = new HttpClient(handler);
        var barClient = CreateBarClient();
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var options = new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath };

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            command.ExecuteAsync(options, TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("Unable to retrieve checksum");
        exception.Message.ShouldContain("aspire-dashboard-linux-arm64.zip");
        File.ReadAllText(manifestPath).ShouldBe(originalContent);
        File.Exists(Path.Combine(repo.LocalPath, "generated-dockerfiles.txt")).ShouldBeFalse();
        File.Exists(Path.Combine(repo.LocalPath, "generated-readmes.txt")).ShouldBeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_RejectsMissingDashboardAssets()
    {
        using var repo = new TempRepo();
        string manifestPath = WriteManifest(repo.LocalPath);
        string originalContent = File.ReadAllText(manifestPath);
        var build = CreateBuild();
        build.Assets.RemoveAll(asset => asset.Name.Contains("linux"));
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var barClient = CreateBarClient(build);
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var options = new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath };

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            command.ExecuteAsync(options, TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("Could not find aspire-dashboard-linux-* assets");
        File.ReadAllText(manifestPath).ShouldBe(originalContent);
        handler.Requests.ShouldBeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_RejectsUnsupportedRepositoryBeforeReadingWorkspace()
    {
        using var repo = new TempRepo();
        var build = CreateBuild();
        build.GitHubRepository = "https://github.com/dotnet/dotnet";
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var barClient = CreateBarClient(build);
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var options = new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath };

        int exitCode = await command.ExecuteAsync(options, TestContext.Current.CancellationToken);

        exitCode.ShouldBe(1);
        handler.Requests.ShouldBeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_PropagatesGeneratorFailure()
    {
        using var repo = new TempRepo();
        string manifestPath = WriteManifest(repo.LocalPath);
        WriteGenerators(repo.LocalPath, dockerfileScript: "exit 1");
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var barClient = CreateBarClient();
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var options = new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath };

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            command.ExecuteAsync(options, TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("exited with code 1");
        AssertUpdatedManifest(manifestPath);
        File.Exists(Path.Combine(repo.LocalPath, "generated-readmes.txt")).ShouldBeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_CancelsBuildLookupBeforeReadingWorkspace()
    {
        using var repo = new TempRepo();
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        var pending = new TaskCompletionSource<Build>();
        barClient.Setup(client => client.GetBuildAsync(123)).Returns(pending.Task);
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var cancellationToken = new CancellationToken(canceled: true);

        await Should.ThrowAsync<OperationCanceledException>(() => command.ExecuteAsync(
            new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath }, cancellationToken));

        handler.Requests.ShouldBeEmpty();
        barClient.VerifyAll();
        barClient.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("--from-build-id")]
    [InlineData("--from-channel")]
    public async Task SourceOptions_BindCliAndResolveBuild(string sourceOption)
    {
        using var repo = new TempRepo();
        string manifestPath = WriteManifest(repo.LocalPath);
        WriteGenerators(repo.LocalPath);
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        var build = CreateBuild();
        if (sourceOption == "--from-build-id")
        {
            barClient.Setup(client => client.GetBuildAsync(23456)).ReturnsAsync(build);
        }
        else
        {
            barClient.Setup(client => client.GetLatestBuildAsync("https://github.com/microsoft/aspire", 5555))
                .ReturnsAsync(build);
        }
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var config = CreateCommandLine(barClient.Object, httpClient);
        string id = sourceOption == "--from-build-id" ? "23456" : "5555";

        int exitCode = await config.InvokeAsync(
            ["aspire", sourceOption, id, "--repo-root", repo.LocalPath],
            TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        AssertUpdatedManifest(manifestPath);
        File.ReadAllText(Path.Combine(repo.LocalPath, "generated-dockerfiles.txt")).Trim().ShouldBe(BuildVersion);
        File.ReadAllText(Path.Combine(repo.LocalPath, "generated-readmes.txt")).Trim().ShouldBe("13.6.0");
        if (sourceOption == "--from-build-id")
        {
            barClient.Verify(client => client.GetBuildAsync(23456), Times.Once);
        }
        else
        {
            barClient.Verify(client => client.GetLatestBuildAsync("https://github.com/microsoft/aspire", 5555), Times.Once);
        }
        barClient.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("123", "5555")]
    [InlineData("0", null)]
    [InlineData("-1", null)]
    [InlineData(null, "0")]
    [InlineData(null, "-1")]
    [InlineData("not-an-id", null)]
    [InlineData(null, "not-an-id")]
    public async Task InvalidSource_DoesNotFetchOrApplyUpdates(string? buildId, string? channel)
    {
        using var repo = new TempRepo();
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        var config = CreateCommandLine(barClient.Object, httpClient);
        List<string> args = ["aspire", "--repo-root", repo.LocalPath];
        if (buildId is not null)
        {
            args.AddRange(["--from-build-id", buildId]);
        }
        if (channel is not null)
        {
            args.AddRange(["--from-channel", channel]);
        }

        int exitCode = await config.InvokeAsync(args.ToArray(), TestContext.Current.CancellationToken);

        exitCode.ShouldNotBe(0);
        handler.Requests.ShouldBeEmpty();
        barClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task BuildId_AcceptsAzureDevOpsAspireRepository()
    {
        using var repo = new TempRepo();
        string manifestPath = WriteManifest(repo.LocalPath);
        WriteGenerators(repo.LocalPath);
        var build = CreateBuild();
        build.GitHubRepository = null;
        build.AzureDevOpsRepository = "https://dev.azure.com/dnceng/internal/_git/microsoft-aspire";
        var barClient = CreateBarClient(build);
        using var handler = new DashboardHandler();
        using var httpClient = new HttpClient(handler);
        using var services = CreateServices(barClient.Object, httpClient);
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());
        var options = new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath };

        int exitCode = await command.ExecuteAsync(options, TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        AssertUpdatedManifest(manifestPath);
    }

    [Fact]
    public async Task ExecuteAsync_UsesKeyedBarBuildCapability()
    {
        using var repo = new TempRepo();
        string manifestPath = WriteManifest(repo.LocalPath);
        WriteGenerators(repo.LocalPath);
        var build = CreateBuild();
        var barClient = CreateBarClient(build);
        var updater = new Mock<IBarBuildUpdater>(MockBehavior.Strict);
        CancellationToken token = TestContext.Current.CancellationToken;
        updater.Setup(service => service.UpdateFromBarBuildAsync(
            It.IsAny<ManifestVariables>(), repo.LocalPath, build, token))
            .Callback<ManifestVariables, string, Build, CancellationToken>((variables, _, _, _) =>
                variables.SetValue("aspire-dashboard|build-version", BuildVersion))
            .Returns(Task.CompletedTask);
        using var services = new ServiceCollection()
            .AddKeyedSingleton<IUpdater>("aspire", updater.Object)
            .AddKeyedSingleton<IUpdater>("other", Mock.Of<IUpdater>())
            .BuildServiceProvider();
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());

        int exitCode = await command.ExecuteAsync(
            new AspireOptions { FromBuildId = 123, RepoRoot = repo.LocalPath }, token);

        exitCode.ShouldBe(0);
        ManifestVariables.FromFile(manifestPath).GetRawValue("aspire-dashboard|build-version").ShouldBe(BuildVersion);
        File.ReadAllText(Path.Combine(repo.LocalPath, "generated-dockerfiles.txt")).Trim().ShouldBe(BuildVersion);
        updater.VerifyAll();
        updater.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_RejectsUnsupportedCapabilityBeforeReadingWorkspace()
    {
        var barClient = CreateBarClient();
        using var services = new ServiceCollection()
            .AddKeyedSingleton<IUpdater>("aspire", Mock.Of<IUpdater>())
            .BuildServiceProvider();
        var command = new AspireCommand(barClient.Object, services, Mock.Of<ILogger<AspireCommand>>());

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            command.ExecuteAsync(
                new AspireOptions { FromBuildId = 123, RepoRoot = "missing-workspace" },
                TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("aspire");
        exception.Message.ShouldContain(nameof(IBarBuildUpdater));
    }

    private static CommandLineConfiguration CreateCommandLine(IBasicBarClient barClient, HttpClient httpClient)
    {
        var root = new RootCommand
        {
            AspireCommand.Create("aspire", "Update Aspire Dashboard"),
        };
        var config = new CommandLineConfiguration(root);
        config.UseHost(
            _ => Host.CreateDefaultBuilder(),
            host => host.ConfigureServices(services =>
            {
                services.AddSingleton(barClient);
                services.AddSingleton(httpClient);
                services.AddKeyedSingleton<IUpdater, AspireUpdater>("aspire");
                services.AddCommand<AspireCommand, AspireOptions>();
            }));
        return config;
    }

    private static ServiceProvider CreateServices(IBasicBarClient barClient, HttpClient httpClient) =>
        new ServiceCollection()
            .AddSingleton(barClient)
            .AddSingleton(httpClient)
            .AddLogging()
            .AddKeyedSingleton<IUpdater, AspireUpdater>("aspire")
            .BuildServiceProvider();

    private static Mock<IBasicBarClient> CreateBarClient(Build? build = null)
    {
        var barClient = new Mock<IBasicBarClient>(MockBehavior.Strict);
        barClient.Setup(client => client.GetBuildAsync(123)).ReturnsAsync(build ?? CreateBuild());
        return barClient;
    }

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
            CreateAsset("linux-x64"),
            CreateAsset("linux-arm64"),
            CreateAsset("win-x64"),
        ],
        dependencies: [],
        incoherencies: [])
    {
        GitHubRepository = "https://github.com/microsoft/aspire",
    };

    private static Asset CreateAsset(string platform) => new(
        id: 1,
        buildId: 123,
        nonShipping: false,
        name: $"aspire/{BuildVersion}/aspire-dashboard-{platform}.zip",
        version: BuildVersion,
        locations: []);

    private static string WriteManifest(string repoRoot, string branch = "nightly")
    {
        Directory.CreateDirectory(repoRoot);
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        File.WriteAllText(manifestPath, $$$"""
            {
              "variables": {
                "branch": "{{{branch}}}",
                "download-root": "https://example.invalid",
                "aspire-dashboard|base-url|main": "$(download-root)/main",
                "aspire-dashboard|base-url|nightly": "$(download-root)/nightly",

                "aspire-dashboard|build-version": "13.5.0-preview.1.26301.1",
                "aspire-dashboard|product-version": "13.5.0",
                "aspire-dashboard|fixed-tag": "13.5.0",
                "aspire-dashboard|minor-tag": "13.5",
                "aspire-dashboard|major-tag": "13",
                "aspire-dashboard|linux|x64|sha": "old-x64",
                "aspire-dashboard|linux|arm64|sha": "old-arm64",
                "runtime|11.0|build-version": "unchanged"
              }
            }
            """);
        return manifestPath;
    }

    private static void AssertUpdatedManifest(string manifestPath)
    {
        var variables = ManifestVariables.FromFile(manifestPath);
        AssertUpdatedVariables(variables);
        File.ReadAllText(manifestPath).ReplaceLineEndings("\n").ShouldContain("\n\n");
    }

    private static void AssertUpdatedVariables(ManifestVariables variables)
    {
        variables.GetRawValue("aspire-dashboard|build-version").ShouldBe(BuildVersion);
        variables.GetRawValue("aspire-dashboard|product-version").ShouldBe("13.6.0");
        variables.GetRawValue("aspire-dashboard|fixed-tag").ShouldBe("13.6.0");
        variables.GetRawValue("aspire-dashboard|minor-tag").ShouldBe("13.6");
        variables.GetRawValue("aspire-dashboard|major-tag").ShouldBe("13");
        variables.GetRawValue("aspire-dashboard|linux|x64|sha").ShouldBe(
            Convert.ToHexStringLower(SHA512.HashData(Encoding.UTF8.GetBytes(X64Archive))));
        variables.GetRawValue("aspire-dashboard|linux|arm64|sha").ShouldBe(
            Convert.ToHexStringLower(SHA512.HashData(Encoding.UTF8.GetBytes(Arm64Archive))));
        variables.GetRawValue("aspire-dashboard|base-url|main").ShouldBe("$(download-root)/main");
        variables.GetRawValue("aspire-dashboard|base-url|nightly").ShouldBe("$(download-root)/nightly");
        variables.GetRawValue("runtime|11.0|build-version").ShouldBe("unchanged");
    }

    private static void WriteGenerators(string repoRoot, string? dockerfileScript = null)
    {
        string dockerfileDirectory = Path.Combine(repoRoot, "eng", "dockerfile-templates");
        string readmeDirectory = Path.Combine(repoRoot, "eng", "readme-templates");
        Directory.CreateDirectory(dockerfileDirectory);
        Directory.CreateDirectory(readmeDirectory);
        File.WriteAllText(Path.Combine(dockerfileDirectory, "Get-GeneratedDockerfiles.ps1"), dockerfileScript ?? """
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            Set-Content ./generated-dockerfiles.txt $manifest.variables.'aspire-dashboard|build-version'
            """);
        File.WriteAllText(Path.Combine(readmeDirectory, "Get-GeneratedReadmes.ps1"), """
            if (!(Test-Path ./generated-dockerfiles.txt)) { throw 'Dockerfiles must be generated first' }
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            Set-Content ./generated-readmes.txt $manifest.variables.'aspire-dashboard|product-version'
            """);
    }

    private sealed class DashboardHandler(HttpStatusCode arm64Status = HttpStatusCode.OK) : HttpMessageHandler
    {
        public List<string> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string url = request.RequestUri!.AbsoluteUri;
            Requests.Add(url);
            var (status, content) = url switch
            {
                _ when url.EndsWith("aspire-dashboard-linux-x64.zip") => (HttpStatusCode.OK, X64Archive),
                _ when url.EndsWith("aspire-dashboard-linux-arm64.zip") => (arm64Status, Arm64Archive),
                _ => throw new InvalidOperationException($"Unexpected download: {url}"),
            };
            return Task.FromResult(new HttpResponseMessage(status)
            {
                Content = new StringContent(content),
            });
        }
    }
}
