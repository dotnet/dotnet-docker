// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.Docker.UpdateDependencies.Git;
using Microsoft.DotNet.Docker.UpdateDependencies.Model.Release;
using Microsoft.DotNet.Docker.UpdateDependencies.Sync;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class FromStagingPipelineCommandTests
{
    [Fact]
    public async Task ExecuteAsync_InternalMode_AddsBuildTagWithStageContainer()
    {
        const string stageContainer = "stage-1234567";
        var expectedTag = $"Container - {stageContainer}";
        using var repo = CreateRepo();

        using var output = new StringWriter();
        var buildLabelService = new BuildLabelService(output);

        var command = CreateCommand(
            buildLabelService: buildLabelService,
            pipelineArtifactProvider: CreateMockPipelineArtifactProvider());

        var options = new FromStagingPipelineOptions
        {
            StageContainers = stageContainer,
            Internal = true,
            StagingStorageAccount = "https://dotnetstagetest.blob.core.windows.net/",
            Mode = ChangeMode.Local,
            RepoRoot = repo.LocalPath
        };

        await command.ExecuteAsync(options);

        output.ToString().ShouldContain($"##vso[build.addbuildtag]{expectedTag}");
    }

    [Fact]
    public async Task ExecuteAsync_NotInternalMode_DoesNotAddBuildTag()
    {
        using var repo = CreateRepo();
        using var output = new StringWriter();
        var buildLabelService = new BuildLabelService(output);

        var command = CreateCommand(
            buildLabelService: buildLabelService,
            pipelineArtifactProvider: CreateMockPipelineArtifactProvider());

        var options = new FromStagingPipelineOptions
        {
            StageContainers = "stage-1234567",
            Internal = false,
            Mode = ChangeMode.Local,
            RepoRoot = repo.LocalPath
        };

        await command.ExecuteAsync(options);

        output.ToString().ShouldNotContain("##vso[build.addbuildtag]");
    }

    [Theory]
    [InlineData("stage-1234567", 1234567)]
    [InlineData("stage-1", 1)]
    [InlineData("stage-999999999", 999999999)]
    public void GetStagingPipelineRunId_ValidStageContainer_ReturnsExpectedId(string stageContainer, int expectedId)
    {
        var result = StagingPipelineOptionsExtensions.GetStagingPipelineRunId(stageContainer);

        result.ShouldBe(expectedId);
    }

    [Theory]
    [InlineData("invalid-format")]
    [InlineData("stage-abc")]
    [InlineData("1234567")]
    [InlineData("stage-")]
    [InlineData("")]
    public void GetStagingPipelineRunId_InvalidStageContainer_ThrowsArgumentException(string stageContainer)
    {
        var exception = Should.Throw<ArgumentException>(() =>
            StagingPipelineOptionsExtensions.GetStagingPipelineRunId(stageContainer));
        exception.Message.ShouldContain($"Invalid stage container name '{stageContainer}'");
        exception.Message.ShouldContain("Expected format: 'stage-{buildId}'");
    }

    [Theory]
    [InlineData("stage-1234567", new[] { "stage-1234567" })]
    [InlineData("stage-1234567,stage-2345678", new[] { "stage-1234567", "stage-2345678" })]
    [InlineData("stage-1234567, stage-2345678, stage-3456789", new[] { "stage-1234567", "stage-2345678", "stage-3456789" })]
    [InlineData("  stage-1234567  ,  stage-2345678  ", new[] { "stage-1234567", "stage-2345678" })]
    public void GetStageContainerList_ParsesCommaDelimitedList(string input, string[] expected)
    {
        var options = new FromStagingPipelineOptions
        {
            StageContainers = input,
            RepoRoot = "/tmp/repo"
        };

        var result = options.GetStageContainerList();

        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(",,,")]
    public void GetStageContainerList_EmptyOrWhitespace_ReturnsEmptyList(string input)
    {
        var options = new FromStagingPipelineOptions
        {
            StageContainers = input,
            RepoRoot = "/tmp/repo"
        };

        var result = options.GetStageContainerList();

        result.ShouldBeEmpty();
    }

    private static FromStagingPipelineCommand CreateCommand(
        ILogger<FromStagingPipelineCommand>? logger = null,
        IPipelineArtifactProvider? pipelineArtifactProvider = null,
        IPipelinesService? pipelinesService = null,
        IInternalVersionsService? internalVersionsService = null,
        IEnvironmentService? environmentService = null,
        IBuildLabelService? buildLabelService = null,
        IGitRepoHelperFactory? gitRepoHelperFactory = null)
    {
        var updater = new DotNetUpdater(
            Mock.Of<IBasicBarClient>(),
            Mock.Of<ILogger<DotNetUpdater>>());
        return new(new UpdateDependenciesConfiguration(),
                logger ?? Mock.Of<ILogger<FromStagingPipelineCommand>>(),
                updater,
                pipelineArtifactProvider ?? CreateMockPipelineArtifactProvider(),
                pipelinesService ?? CreateMockPipelinesService(),
                internalVersionsService ?? Mock.Of<IInternalVersionsService>(),
                environmentService ?? Mock.Of<IEnvironmentService>(),
                buildLabelService ?? Mock.Of<IBuildLabelService>(),
                gitRepoHelperFactory ?? Mock.Of<IGitRepoHelperFactory>());
    }

    private static TempRepo CreateRepo()
    {
        var repo = new TempRepo();
        File.WriteAllText(Path.Combine(repo.LocalPath, "manifest.versions.json"), """
            {"variables":{"branch":"nightly","sdk|9.0|build-version":"9.0.100"}}
            """);
        string configDirectory = Path.Combine(repo.LocalPath, "tests", "Microsoft.DotNet.Docker.Tests", "TestAppArtifacts");
        Directory.CreateDirectory(configDirectory);
        File.WriteAllText(Path.Combine(configDirectory, "NuGet.config.internal"), "<configuration />");
        File.WriteAllText(Path.Combine(configDirectory, "NuGet.config.nightly"), "<configuration />");
        foreach (var (directory, file) in new[]
        {
            ("dockerfile-templates", "Get-GeneratedDockerfiles.ps1"),
            ("readme-templates", "Get-GeneratedReadmes.ps1"),
        })
        {
            string path = Path.Combine(repo.LocalPath, "eng", directory);
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, file), "exit 0");
        }
        return repo;
    }

    private static IPipelineArtifactProvider CreateMockPipelineArtifactProvider()
    {
        var mock = new Mock<IPipelineArtifactProvider>();
        mock.Setup(p => p.GetReleaseConfigAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(CreateReleaseConfig());
        return mock.Object;
    }

    private static IPipelinesService CreateMockPipelinesService()
    {
        var mock = new Mock<IPipelinesService>();
        mock.Setup(p => p.GetBuildTagsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new List<string>());
        return mock.Object;
    }

    private static ReleaseConfig CreateReleaseConfig() => new()
    {
        Channel = "9.0",
        MajorVersion = "9",
        Release = "9.0.0",
        Runtime = "9.0.0",
        RuntimeBuild = "9.0.0",
        Sdks = ["9.0.100"],
        SdkBuilds = ["9.0.100"],
        Security = false,
        SupportPhase = "active",
        Internal = false,
        SdkOnly = false,
        Asp = "9.0.0",
        AspBuild = "9.0.0"
    };
}
