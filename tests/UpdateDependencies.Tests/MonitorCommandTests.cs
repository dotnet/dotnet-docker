// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Hosting;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Build.WebApi;

namespace UpdateDependencies.Tests;

public sealed class MonitorCommandTests
{
    [Theory]
    [InlineData("9.0.5-servicing.25556.2", "9.0", "9.0.5", "nightly")]
    [InlineData("9.0.0-rtm.12345.1", "9.0", "9.0.0", "main")]
    [InlineData("10.0.3-preview.1.26303.1", "10.0", "10.0.3-preview.1", "nightly")]
    [InlineData("9.0.5", "9.0", "9.0.5", "main")]
    [InlineData("9.0.5+build-rtm", "9.0", "9.0.5", "main")]
    [InlineData("9.0.5-servicing.25556.2+build.123", "9.0", "9.0.5", "nightly")]
    public async Task ExplicitVersion_UpdatesAllMonitorProducts(
        string version,
        string dockerfileVersion,
        string productVersion,
        string branch)
    {
        using var repo = new TempRepo();
        WriteManifest(repo, branch, dockerfileVersion);
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var httpClient = new HttpClient();
        using var services = CreateServices(artifacts.Object, httpClient);
        var command = new MonitorCommand(artifacts.Object, services, Mock.Of<ILogger<MonitorCommand>>());
        var options = new MonitorOptions
        {
            Version = version,
            RepoRoot = repo.LocalPath,
            TargetBranch = branch,
        };

        int exitCode = await command.ExecuteAsync(options, TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        var variables = ManifestVariables.FromFile(Path.Combine(repo.LocalPath, "manifest.versions.json"));
        foreach (string product in Products)
        {
            variables.GetRawValue($"{product}|{dockerfileVersion}|build-version").ShouldBe(version);
            variables.GetRawValue($"{product}|{dockerfileVersion}|product-version").ShouldBe(productVersion);
        }
        string quality = branch == "main" ? "maintenance" : "preview";
        variables.GetRawValue($"monitor|{dockerfileVersion}|base-url|{branch}")
            .ShouldBe($"$(base-url|public|{quality}|{branch})");
        variables.GetRawValue($"monitor|{dockerfileVersion}|base-url|checksums|{branch}")
            .ShouldBe($"$(base-url|public-checksums|{quality}|{branch})");
        File.ReadAllText(Path.Combine(repo.LocalPath, "generated-version.txt")).Trim().ShouldBe(version);
        artifacts.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null, null, "https://dev.azure.com/dnceng", "internal")]
    [InlineData("https://dev.azure.com/example", "public", "https://dev.azure.com/example", "public")]
    public async Task PipelineRunId_BindsCliOptionsAndReadsVersion(
        string? organization,
        string? project,
        string expectedOrganization,
        string expectedProject)
    {
        using var repo = new TempRepo();
        WriteManifest(repo);
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        PipelineArtifactFile[] expectedFiles = [new("Build_Info", "dotnet-monitor.nupkg.buildversion")];
        artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
            expectedOrganization,
            expectedProject,
            1234567,
            It.Is<IEnumerable<PipelineArtifactFile>>(files => files.SequenceEqual(expectedFiles))))
            .ReturnsAsync("  9.0.5-servicing.25556.2\r\n");
        var root = new RootCommand
        {
            MonitorCommand.Create("monitor", "Update Monitor"),
        };
        var config = new CommandLineConfiguration(root);
        config.UseHost(
            _ => Host.CreateDefaultBuilder(),
            host => host.ConfigureServices(services =>
            {
                services.AddSingleton(artifacts.Object);
                services.AddHttpClient();
                services.AddKeyedSingleton<IUpdater, MonitorUpdater>("monitor");
                services.AddCommand<MonitorCommand, MonitorOptions>();
            }));
        List<string> args = ["monitor", "--pipeline-run-id", "1234567", "--repo-root", repo.LocalPath];
        if (organization is not null)
        {
            args.AddRange(["--azdo-organization", organization]);
        }
        if (project is not null)
        {
            args.AddRange(["--azdo-project", project]);
        }

        int exitCode = await config.InvokeAsync(args.ToArray(), TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        artifacts.VerifyAll();
        artifacts.Verify(provider => provider.GetArtifactTextContentAsync(
            expectedOrganization,
            expectedProject,
            1234567,
            It.IsAny<IEnumerable<PipelineArtifactFile>>()), Times.Once);
        var variables = ManifestVariables.FromFile(Path.Combine(repo.LocalPath, "manifest.versions.json"));
        variables.GetRawValue("monitor|9.0|build-version").ShouldBe("9.0.5-servicing.25556.2");
        variables.GetRawValue("monitor|9.0|product-version").ShouldBe("9.0.5");
    }

    [Fact]
    public async Task ExplicitVersion_BindsCliWithoutAzureAuthentication()
    {
        using var repo = new TempRepo();
        WriteManifest(repo);
        var auth = new Mock<IAzdoAuthProvider>(MockBehavior.Strict);
        var root = new RootCommand
        {
            MonitorCommand.Create("monitor", "Update Monitor"),
        };
        var config = new CommandLineConfiguration(root);
        config.UseHost(
            _ => Host.CreateDefaultBuilder(),
            host => host.ConfigureServices(services =>
            {
                services.AddPipelineArtifactProvider();
                services.AddSingleton(auth.Object);
                services.AddKeyedSingleton<IUpdater, MonitorUpdater>("monitor");
                services.AddCommand<MonitorCommand, MonitorOptions>();
            }));

        int exitCode = await config.InvokeAsync(
            ["monitor", "9.0.5", "--repo-root", repo.LocalPath],
            TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        auth.VerifyNoOtherCalls();
        var variables = ManifestVariables.FromFile(Path.Combine(repo.LocalPath, "manifest.versions.json"));
        variables.GetRawValue("monitor|9.0|build-version").ShouldBe("9.0.5");
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("9.0.5", 1234567)]
    [InlineData(null, 0)]
    [InlineData(null, -1)]
    [InlineData("", null)]
    [InlineData("not-a-version", null)]
    [InlineData("9.0", null)]
    [InlineData("9.0.5\n9.0.6", null)]
    public async Task InvalidInput_DoesNotReadArtifactsOrApplyUpdates(string? version, int? pipelineRunId)
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        using var httpClient = new HttpClient();
        using var services = CreateServices(artifacts.Object, httpClient);
        var command = new MonitorCommand(artifacts.Object, services, Mock.Of<ILogger<MonitorCommand>>());
        var options = new MonitorOptions { Version = version, PipelineRunId = pipelineRunId };

        int exitCode = await command.ExecuteAsync(options, TestContext.Current.CancellationToken);

        exitCode.ShouldBe(1);
        artifacts.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" \r\n")]
    [InlineData("not-a-version")]
    public async Task InvalidArtifactVersion_DoesNotApplyUpdates(string version)
    {
        var artifacts = new Mock<IPipelineArtifactProvider>();
        artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
            "https://dev.azure.com/dnceng",
            "internal",
            1234567,
            It.IsAny<IEnumerable<PipelineArtifactFile>>()))
            .ReturnsAsync(version);
        using var httpClient = new HttpClient();
        using var services = CreateServices(artifacts.Object, httpClient);
        var command = new MonitorCommand(artifacts.Object, services, Mock.Of<ILogger<MonitorCommand>>());

        int exitCode = await command.ExecuteAsync(
            new MonitorOptions { PipelineRunId = 1234567 }, TestContext.Current.CancellationToken);

        exitCode.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_UsesKeyedPipelineCapability()
    {
        using var repo = new TempRepo();
        WriteManifest(repo);
        CancellationToken token = TestContext.Current.CancellationToken;
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        var updater = new Mock<IPipelineBuildUpdater>(MockBehavior.Strict);
        artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
            "https://dev.azure.com/dnceng", "internal", 1234567, It.IsAny<IEnumerable<PipelineArtifactFile>>()))
            .ReturnsAsync(" 9.0.5 \r\n");
        updater.Setup(service => service.UpdateFromPipelineBuildAsync(
                It.IsAny<ManifestVariables>(),
                It.Is<PipelineBuildReference>(build => build is MonitorPipelineBuildReference
                    && ((MonitorPipelineBuildReference)build).Version == "9.0.5"
                    && build.Organization == "https://dev.azure.com/dnceng"
                    && build.Project == "internal"
                    && build.RunId == 1234567),
                token))
                .Returns(Task.CompletedTask);
        using var services = new ServiceCollection()
            .AddKeyedSingleton<IUpdater>("monitor", updater.Object)
            .AddKeyedSingleton<IUpdater>("other", Mock.Of<IUpdater>())
            .BuildServiceProvider();
        var command = new MonitorCommand(artifacts.Object, services, Mock.Of<ILogger<MonitorCommand>>());
        var options = new MonitorOptions
        {
            PipelineRunId = 1234567,
            RepoRoot = repo.LocalPath,
        };

        int exitCode = await command.ExecuteAsync(options, token);

        exitCode.ShouldBe(0);
        updater.VerifyAll();
        updater.VerifyNoOtherCalls();
        artifacts.VerifyAll();
    }

    [Theory]
    [InlineData(false, nameof(MonitorUpdater))]
    [InlineData(true, nameof(IPipelineBuildUpdater))]
    public async Task ExecuteAsync_RejectsUnsupportedCapabilityBeforeReadingWorkspace(
        bool fromPipeline, string capability)
    {
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        if (fromPipeline)
        {
            artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
                "https://dev.azure.com/dnceng", "internal", 1234567, It.IsAny<IEnumerable<PipelineArtifactFile>>()))
                .ReturnsAsync("9.0.5");
        }
        using var services = new ServiceCollection()
            .AddKeyedSingleton<IUpdater>("monitor", Mock.Of<IBarBuildUpdater>())
            .BuildServiceProvider();
        var command = new MonitorCommand(artifacts.Object, services, Mock.Of<ILogger<MonitorCommand>>());
        var options = new MonitorOptions
        {
            Version = fromPipeline ? null : "9.0.5",
            PipelineRunId = fromPipeline ? 1234567 : null,
            RepoRoot = "missing-workspace",
        };

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            command.ExecuteAsync(options, TestContext.Current.CancellationToken));

        exception.Message.ShouldContain("monitor");
        exception.Message.ShouldContain(capability);
    }

    [Fact]
    public async Task ExecuteAsync_CancelsWhileReadingPipelineVersion()
    {
        using var cancellation = new CancellationTokenSource();
        var pending = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        artifacts.Setup(provider => provider.GetArtifactTextContentAsync(
            "https://dev.azure.com/dnceng", "internal", 1234567, It.IsAny<IEnumerable<PipelineArtifactFile>>()))
            .Returns(pending.Task);
        using var services = new ServiceCollection().BuildServiceProvider();
        var command = new MonitorCommand(artifacts.Object, services, Mock.Of<ILogger<MonitorCommand>>());

        Task<int> execution = command.ExecuteAsync(
            new MonitorOptions { PipelineRunId = 1234567, RepoRoot = "missing-workspace" }, cancellation.Token);
        cancellation.Cancel();

        await Should.ThrowAsync<OperationCanceledException>(() => execution);
        pending.SetResult("9.0.5");
        artifacts.VerifyAll();
    }

    [Fact]
    public async Task ArtifactProvider_ReadsBuildInfoAndAuthenticatesOnlyOnRequest()
    {
        var auth = new Mock<IAzdoAuthProvider>(MockBehavior.Strict);
        auth.SetupGet(provider => provider.AccessToken).Returns("test-token");
        var pipelines = new Mock<IPipelinesService>(MockBehavior.Strict);
        pipelines.Setup(service => service.GetArtifactAsync("https://dev.azure.com/dnceng", "internal", 1234567, "Build_Info"))
            .ReturnsAsync(new BuildArtifact
            {
                Resource = new ArtifactResource
                {
                    DownloadUrl = "https://example.invalid/artifact/content?format=zip",
                },
            });
        const string versionFile = "9.0.5-servicing.25556.2\r\n";
        using var handler = new ArtifactHandler(versionFile);
        using var httpClient = new HttpClient(handler);
        var client = new AzdoHttpClient(auth.Object, httpClient);
        var provider = new PipelineArtifactProvider(pipelines.Object, Mock.Of<ILogger<PipelineArtifactProvider>>(), client);
        auth.VerifyGet(provider => provider.AccessToken, Times.Never);

        var artifactFile = new PipelineArtifactFile("Build_Info", "dotnet-monitor.nupkg.buildversion");
        string version = await provider.GetArtifactTextContentAsync(
            "https://dev.azure.com/dnceng",
            "internal",
            1234567,
            artifactFile);

        version.ShouldBe(versionFile);
        handler.RequestUri.ShouldBe("https://example.invalid/artifact/content?format=file&subPath=%2Fdotnet-monitor.nupkg.buildversion");
        string encodedToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(":test-token"));
        handler.Authorization.ShouldBe($"Basic {encodedToken}");
        auth.VerifyGet(provider => provider.AccessToken, Times.Once);
        pipelines.VerifyAll();
    }

    private static readonly string[] Products =
    [
        "monitor", "monitor-base", "monitor-ext-azureblobstorage", "monitor-ext-s3storage",
    ];

    private static ServiceProvider CreateServices(IPipelineArtifactProvider artifacts, HttpClient httpClient) =>
        new ServiceCollection()
            .AddSingleton(artifacts)
            .AddSingleton(httpClient)
            .AddLogging()
            .AddKeyedSingleton<IUpdater, MonitorUpdater>("monitor")
            .BuildServiceProvider();

    private static void WriteManifest(TempRepo repo, string branch = "nightly", string dockerfileVersion = "9.0")
    {
        var variables = new Dictionary<string, string>
        {
            ["branch"] = branch,
            [$"monitor|{dockerfileVersion}|base-url|{branch}"] = "old",
            [$"monitor|{dockerfileVersion}|base-url|checksums|{branch}"] = "old",
        };
        foreach (string product in Products)
        {
            variables[$"{product}|{dockerfileVersion}|build-version"] = "9.0.1";
            variables[$"{product}|{dockerfileVersion}|product-version"] = "9.0.1";
        }
        File.WriteAllText(Path.Combine(repo.LocalPath, "manifest.versions.json"), JsonSerializer.Serialize(new { variables }));

        string generator = $$"""
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            Set-Content ./generated-version.txt $manifest.variables.'monitor|{{dockerfileVersion}}|build-version'
            """;
        foreach (var (directory, script) in new[]
        {
            ("dockerfile-templates", "Get-GeneratedDockerfiles.ps1"),
            ("readme-templates", "Get-GeneratedReadmes.ps1"),
        })
        {
            string path = Path.Combine(repo.LocalPath, "eng", directory);
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, script), generator);
        }
    }

    private sealed class ArtifactHandler(string content) : HttpMessageHandler
    {
        public string? RequestUri { get; private set; }
        public string? Authorization { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri?.AbsoluteUri;
            Authorization = request.Headers.Authorization?.ToString();
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content),
            });
        }
    }
}
