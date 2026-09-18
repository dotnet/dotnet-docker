// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace UpdateDependencies.Tests;

public sealed class DependencyCommandTests
{
    [Fact]
    public void Create_AddsOnlySupportedSourceCommands()
    {
        Command command = DependencyCommand.Create<BarUpdater>(ThrowIfResolved);

        command.Subcommands.Select(subcommand => subcommand.Name)
            .ShouldBe(["build-id", "channel"]);
    }

    [Fact]
    public void Create_MakesGitHubReleaseUpdaterDirectlyExecutable()
    {
        Command command = DependencyCommand.Create<ReleaseUpdater>(ThrowIfResolved);

        command.Action.ShouldNotBeNull();
        command.Subcommands.ShouldBeEmpty();
    }

    [Fact]
    public void Parse_RejectsUnsupportedSourceForDependency()
    {
        var root = new RootCommand
        {
            DependencyCommand.Create<BarUpdater>(ThrowIfResolved),
        };

        root.Parse(["sample", "pipeline-build", "123"]).Errors.ShouldNotBeEmpty();
        root.Parse(["sample", "channel", "123"]).Errors.ShouldBeEmpty();
        root.Parse(["sample", "channel", "0"]).Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public void Create_ExposesExpectedMetadataAndSources()
    {
        Check<AspireUpdater>("aspire", "microsoft/aspire", "build-id", "channel");
        Check<DotNetUpdater>("dotnet", "dotnet/dotnet", "build-id", "channel", "staging-pipeline");
        Check<MonitorUpdater>("monitor", "dotnet/dotnet-monitor", "pipeline-build", "version");
        Check<ChiselUpdater>("chisel", "chisel");
        Check<MinGitUpdater>("mingit", "mingit");
        Check<RocksToolboxUpdater>("rocks-toolbox", "rocks-toolbox");
        Check<SyftUpdater>("syft", "syft");

        static void Check<TUpdater>(string name, string versionSourceName, params string[] sources)
            where TUpdater : class, IUpdater
        {
            Command command = DependencyCommand.Create<TUpdater>(ThrowIfResolved);

            command.Name.ShouldBe(name);
            TUpdater.VersionSourceName.ShouldBe(versionSourceName);
            command.Subcommands.Select(subcommand => subcommand.Name).ShouldBe(sources);
        }
    }

    [Theory]
    [InlineData("--user")]
    [InlineData("--email")]
    [InlineData("--password")]
    [InlineData("--azdo-organization")]
    [InlineData("--azdo-project")]
    [InlineData("--azdo-repo")]
    public async Task ConfigurationFlags_AreRejectedBeforeResolvingServices(string option)
    {
        var root = new RootCommand
        {
            DependencyCommand.Create<AspireUpdater>(ThrowIfResolved),
            DependencyCommand.Create<ChiselUpdater>(ThrowIfResolved),
            DependencyCommand.Create<MonitorUpdater>(ThrowIfResolved),
            SyncInternalReleaseCommand.Create(ThrowIfResolved),
        };

        foreach (string[] args in new string[][]
        {
            ["aspire", "channel", "123", option, "unused"],
            ["chisel", option, "unused"],
            ["monitor", "pipeline-build", "123", option, "unused"],
            ["sync-internal-release", option, "unused"],
        })
        {
            ParseResult result = root.Parse(args);
            result.Errors.ShouldNotBeEmpty();
            int exitCode = await result.InvokeAsync(cancellationToken: TestContext.Current.CancellationToken);
            exitCode.ShouldNotBe(0);
        }
    }

    [Theory]
    [InlineData("build-id", "123")]
    [InlineData("channel", "456")]
    public void BarOptions_BindSourceAndCommonValues(string source, string id)
    {
        Command command = DependencyCommand.Create<AspireUpdater>(ThrowIfResolved);
        ParseResult result = command.Parse([
            source, id, "--repo-root", "workspace with spaces", "--update-only",
            "--source-branch", "release/11.0", "--target-branch", "nightly"]);
        result.Errors.ShouldBeEmpty();

        CreatePullRequestOptions options;
        if (source == "build-id")
        {
            FromBuildOptions buildOptions = FromBuildOptions.Bind(result);
            buildOptions.Id.ShouldBe(int.Parse(id));
            options = buildOptions;
        }
        else
        {
            FromChannelOptions channelOptions = FromChannelOptions.Bind(result);
            channelOptions.Channel.ShouldBe(int.Parse(id));
            options = channelOptions;
        }

        options.RepoRoot.ShouldBe("workspace with spaces");
        options.UpdateOnly.ShouldBeTrue();
        options.SourceBranch.ShouldBe("release/11.0");
        options.TargetBranch.ShouldBe("nightly");
    }

    [Fact]
    public void MonitorOptions_BindPipelineAndVersionSeparately()
    {
        Command command = DependencyCommand.Create<MonitorUpdater>(ThrowIfResolved);
        ParseResult pipeline = command.Parse(["pipeline-build", "123"]);
        ParseResult version = command.Parse(["version", "9.0.5", "--update-only", "false"]);
        pipeline.Errors.ShouldBeEmpty();
        version.Errors.ShouldBeEmpty();

        FromPipelineBuildOptions.Bind(pipeline).RunId.ShouldBe(123);
        FromVersionOptions.Bind(version).Version.ShouldBe("9.0.5");
        FromVersionOptions.Bind(version).UpdateOnly.ShouldBeFalse();
    }

    [Fact]
    public void StagingAndSyncOptions_BindTheirOwnValues()
    {
        Command staging = FromStagingPipelineCommand.Create(ThrowIfResolved);
        ParseResult stagingResult = staging.Parse([
            "stage-123,stage-456", "--staging-storage-account", "dotnetstage",
            "--internal", "--mode", "Remote", "--target-branch", "internal/release/11.0"]);
        stagingResult.Errors.ShouldBeEmpty();
        FromStagingPipelineOptions stagingOptions = FromStagingPipelineOptions.Bind(stagingResult);

        stagingOptions.GetStageContainerList().ShouldBe(["stage-123", "stage-456"]);
        stagingOptions.StagingStorageAccount.ShouldBe("dotnetstage");
        stagingOptions.Internal.ShouldBeTrue();
        stagingOptions.Mode.ShouldBe(ChangeMode.Remote);
        stagingOptions.TargetBranch.ShouldBe("internal/release/11.0");

        Command sync = SyncInternalReleaseCommand.Create(ThrowIfResolved);
        ParseResult syncResult = sync.Parse([
            "--source-branch", "release/11.0", "--target-branch", "internal/release/11.0",
            "--staging-storage-account", "dotnetstage"]);
        syncResult.Errors.ShouldBeEmpty();
        SyncInternalReleaseOptions syncOptions = SyncInternalReleaseOptions.Bind(syncResult);

        syncOptions.SourceBranch.ShouldBe("release/11.0");
        syncOptions.TargetBranch.ShouldBe("internal/release/11.0");
        syncOptions.StagingStorageAccount.ShouldBe("dotnetstage");
    }

    [Fact]
    public void CommonOptions_BindDefaultsWithoutLeakingAcrossCommands()
    {
        Command first = DependencyCommand.Create<ChiselUpdater>(ThrowIfResolved);
        Command second = DependencyCommand.Create<SyftUpdater>(ThrowIfResolved);
        var root = new RootCommand { first, second };
        ParseResult firstResult = root.Parse(["chisel", "--repo-root", "first-workspace", "--update-only"]);
        ParseResult secondResult = root.Parse(["syft"]);
        firstResult.Errors.ShouldBeEmpty();
        secondResult.Errors.ShouldBeEmpty();

        CreatePullRequestOptions.Bind(firstResult).UpdateOnly.ShouldBeTrue();
        CreatePullRequestOptions defaults = CreatePullRequestOptions.Bind(secondResult);
        defaults.RepoRoot.ShouldBe(Directory.GetCurrentDirectory());
        defaults.UpdateOnly.ShouldBeFalse();
        defaults.SourceBranch.ShouldBe("");
        defaults.TargetBranch.ShouldBe("nightly");
    }

    [Theory]
    [InlineData("build-id 123", "build:123")]
    [InlineData("channel 456", "channel:456")]
    [InlineData("pipeline-build 789", "pipeline:789")]
    [InlineData("version 9.0.5", "version:9.0.5")]
    [InlineData("", "release")]
    public async Task SourceCommands_UpdateOnlyDoesNotRequirePublishingCredentials(string source, string expected)
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        File.WriteAllText(manifestPath, """{"variables":{"source":"old"}}""");
        foreach (var (directory, script) in new[]
        {
            ("dockerfile-templates", "Get-GeneratedDockerfiles.ps1"),
            ("readme-templates", "Get-GeneratedReadmes.ps1"),
        })
        {
            string path = Path.Combine(repo.LocalPath, "eng", directory);
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, script), "Add-Content ./generated.txt 'generated'");
        }

        var updater = new RecordingUpdater();
        var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
        bar.Setup(client => client.GetBuildAsync(123)).ReturnsAsync(
            JsonConvert.DeserializeObject<Build>("""{"id":123}""")!);
        var services = new ServiceCollection()
            .AddLogging()
            .AddSingleton(updater)
            .AddSingleton(bar.Object)
            .AddSingleton(new UpdateDependenciesConfiguration
            {
                AzureDevOps = new()
                {
                    Organization = "https://dev.azure.com/configured-org",
                    Project = "configured-project",
                },
            })
            .AddSingleton<DependencyUpdateRunner>();
        using ServiceProvider provider = services.BuildServiceProvider();
        Command command = DependencyCommand.Create<RecordingUpdater>(provider);
        string[] args =
        [
            ..source.Split(' ', StringSplitOptions.RemoveEmptyEntries),
            "--repo-root", repo.LocalPath, "--update-only",
        ];

        int exitCode = await command.Parse(args).InvokeAsync(cancellationToken: TestContext.Current.CancellationToken);

        exitCode.ShouldBe(0);
        updater.Calls.ShouldBe(1);
        ManifestVariables.FromFile(manifestPath).GetRawValue("source").ShouldBe(expected);
        File.ReadAllLines(Path.Combine(repo.LocalPath, "generated.txt")).Length.ShouldBe(2);
        if (source.StartsWith("pipeline-build"))
        {
            updater.Pipeline.ShouldBe(new PipelineBuildReference(
                "https://dev.azure.com/configured-org", "configured-project", 789));
        }
    }

    [Fact]
    public async Task RunAsync_NormalModeRequiresPublishingCredentials()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddSingleton(new UpdateDependenciesConfiguration())
            .AddSingleton<DependencyUpdateRunner>();
        using ServiceProvider provider = services.BuildServiceProvider();
        var runner = provider.GetRequiredService<DependencyUpdateRunner>();

        await Should.ThrowAsync<ArgumentException>(() => runner.RunAsync(
            new CreatePullRequestOptions(),
            "sample/source",
            (_, _, _) => Task.CompletedTask,
            TestContext.Current.CancellationToken));
    }

    private static readonly IServiceProvider ThrowIfResolved = new ThrowingServiceProvider();

    private sealed class ThrowingServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType) =>
            throw new InvalidOperationException("Parsing commands should not resolve services.");
    }

    private sealed class BarUpdater : IBarBuildUpdater, IBarChannelUpdater
    {
        public static string Name => "sample";
        public static string VersionSourceName => "sample/source";

        public Task UpdateFromBarBuildAsync(
            ManifestVariables variables,
            string repoRoot,
            Build build,
            CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task UpdateFromBarChannelAsync(
            ManifestVariables variables,
            string repoRoot,
            int channelId,
            CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    private sealed class ReleaseUpdater : IGitHubReleaseUpdater
    {
        public static string Name => "sample";
        public static string VersionSourceName => "sample";

        public Task UpdateFromGitHubReleaseAsync(
            ManifestVariables variables,
            CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    private sealed class RecordingUpdater : IBarBuildUpdater, IBarChannelUpdater,
        IPipelineBuildUpdater, IVersionUpdater, IGitHubReleaseUpdater
    {
        public static string Name => "sample";
        public static string VersionSourceName => "sample/source";

        public int Calls { get; private set; }
        public PipelineBuildReference? Pipeline { get; private set; }

        public Task UpdateFromBarBuildAsync(
            ManifestVariables variables, string repoRoot, Build build, CancellationToken cancellationToken) =>
            Record(variables, $"build:{build.Id}");

        public Task UpdateFromBarChannelAsync(
            ManifestVariables variables, string repoRoot, int channelId, CancellationToken cancellationToken) =>
            Record(variables, $"channel:{channelId}");

        public Task UpdateFromPipelineBuildAsync(
            ManifestVariables variables, PipelineBuildReference build, CancellationToken cancellationToken)
        {
            Pipeline = build;
            return Record(variables, $"pipeline:{build.RunId}");
        }

        public Task UpdateFromVersionAsync(
            ManifestVariables variables, string version, CancellationToken cancellationToken) =>
            Record(variables, $"version:{version}");

        public Task UpdateFromGitHubReleaseAsync(ManifestVariables variables, CancellationToken cancellationToken) =>
            Record(variables, "release");

        private Task Record(ManifestVariables variables, string source)
        {
            Calls++;
            variables.SetValue("source", source);
            return Task.CompletedTask;
        }
    }
}
