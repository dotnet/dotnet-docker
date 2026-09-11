// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Hosting;
using System.Net;
using System.Text;
using Dotnet.Docker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Build.WebApi;

namespace UpdateDependencies.Tests;

public sealed class MonitorCommandTests
{
    [Theory]
    [InlineData("9.0.5-servicing.25556.2", "9.0", true, "nightly", ReleaseState.Prerelease)]
    [InlineData("9.0.0-rtm.12345.1", "9.0", true, "main", ReleaseState.Release)]
    [InlineData("10.0.3-preview.1.26303.1", "10.0", false, "nightly", ReleaseState.Prerelease)]
    [InlineData("9.0.5", "9.0", false, "main", ReleaseState.Release)]
    [InlineData("9.0.5+build-rtm", "9.0", false, "main", ReleaseState.Release)]
    [InlineData("9.0.5-servicing.25556.2+build.123", "9.0", true, "nightly", ReleaseState.Prerelease)]
    public async Task ExplicitVersion_UpdatesAllMonitorProducts(
        string version,
        string dockerfileVersion,
        bool stableBranding,
        string branch,
        ReleaseState releaseState)
    {
        using var repo = new TempRepo();
        WriteManifest(repo, branch);
        var artifacts = new Mock<IPipelineArtifactProvider>(MockBehavior.Strict);
        var specific = new Mock<ICommand<SpecificCommandOptions>>();
        SpecificCommandOptions? appliedOptions = null;
        specific.Setup(command => command.ExecuteAsync(It.IsAny<SpecificCommandOptions>()))
            .Callback<SpecificCommandOptions>(options => appliedOptions = options)
            .ReturnsAsync(17);
        var command = new MonitorCommand(artifacts.Object, specific.Object, Mock.Of<ILogger<MonitorCommand>>());
        var options = new MonitorOptions
        {
            Version = version,
            RepoRoot = repo.LocalPath,
            TargetBranch = branch,
            User = "bot",
            Email = "bot@example.com",
            Password = "test-token",
        };

        int exitCode = await command.ExecuteAsync(options);

        exitCode.ShouldBe(17);
        var applied = Assert.IsType<SpecificCommandOptions>(appliedOptions);
        applied.RepoRoot.ShouldBe(repo.LocalPath);
        applied.TargetBranch.ShouldBe(branch);
        applied.User.ShouldBe(options.User);
        applied.Email.ShouldBe(options.Email);
        applied.Password.ShouldBe(options.Password);
        applied.DockerfileVersion.ShouldBe(dockerfileVersion);
        applied.VersionSourceName.ShouldBe($"dotnet/dotnet-monitor/{dockerfileVersion}");
        applied.StableBranding.ShouldBe(stableBranding);
        applied.ReleaseState.ShouldBe(releaseState);
        applied.ProductVersions.ShouldBe(new Dictionary<string, string?>
        {
            { "monitor", version },
            { "monitor-base", version },
            { "monitor-ext-azureblobstorage", version },
            { "monitor-ext-s3storage", version },
        });
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
        var specific = new Mock<ICommand<SpecificCommandOptions>>();
        specific.Setup(command => command.ExecuteAsync(It.IsAny<SpecificCommandOptions>()))
            .ReturnsAsync(0);
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
                services.AddSingleton(specific.Object);
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
        specific.Verify(command => command.ExecuteAsync(It.Is<SpecificCommandOptions>(
            options => options.RepoRoot == repo.LocalPath
                && options.ProductVersions["monitor"] == "9.0.5-servicing.25556.2"
                && options.StableBranding)), Times.Once);
    }

    [Fact]
    public async Task ExplicitVersion_BindsCliWithoutAzureAuthentication()
    {
        using var repo = new TempRepo();
        WriteManifest(repo);
        var auth = new Mock<IAzdoAuthProvider>(MockBehavior.Strict);
        var specific = new Mock<ICommand<SpecificCommandOptions>>();
        specific.Setup(command => command.ExecuteAsync(It.IsAny<SpecificCommandOptions>()))
            .ReturnsAsync(0);
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
                services.AddSingleton(specific.Object);
                services.AddCommand<MonitorCommand, MonitorOptions>();
            }));

        int exitCode = await config.InvokeAsync(
            ["monitor", "9.0.5", "--repo-root", repo.LocalPath],
            TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        auth.VerifyNoOtherCalls();
        specific.Verify(command => command.ExecuteAsync(It.Is<SpecificCommandOptions>(
            options => options.ProductVersions["monitor"] == "9.0.5")), Times.Once);
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
        var specific = new Mock<ICommand<SpecificCommandOptions>>(MockBehavior.Strict);
        var command = new MonitorCommand(artifacts.Object, specific.Object, Mock.Of<ILogger<MonitorCommand>>());
        var options = new MonitorOptions { Version = version, PipelineRunId = pipelineRunId };

        int exitCode = await command.ExecuteAsync(options);

        exitCode.ShouldBe(1);
        artifacts.VerifyNoOtherCalls();
        specific.VerifyNoOtherCalls();
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
        var specific = new Mock<ICommand<SpecificCommandOptions>>(MockBehavior.Strict);
        var command = new MonitorCommand(artifacts.Object, specific.Object, Mock.Of<ILogger<MonitorCommand>>());

        int exitCode = await command.ExecuteAsync(new MonitorOptions { PipelineRunId = 1234567 });

        exitCode.ShouldBe(1);
        specific.VerifyNoOtherCalls();
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

    private static void WriteManifest(TempRepo repo, string branch = "nightly") =>
        File.WriteAllText(
            Path.Combine(repo.LocalPath, "manifest.versions.json"),
            $$$"""{"variables":{"branch":"{{{branch}}}"}}""");

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
