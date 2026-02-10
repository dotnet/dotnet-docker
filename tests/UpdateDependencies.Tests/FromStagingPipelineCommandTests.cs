// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker;
using Dotnet.Docker.Git;
using Dotnet.Docker.Model.Release;
using Dotnet.Docker.Sync;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class FromStagingPipelineCommandTests
{
    [Fact]
    public async Task ExecuteAsync_InternalMode_AddsBuildTagWithStageContainer()
    {
        const string stageContainer = "stage-1234567";
        var expectedTag = $"Container - {stageContainer}";

        using var output = new StringWriter();
        var buildLabelService = new BuildLabelService(output);

        var command = CreateCommand(
            buildLabelService: buildLabelService,
            pipelineArtifactProvider: CreateMockPipelineArtifactProvider());

        var options = new FromStagingPipelineOptions
        {
            StageContainer = stageContainer,
            Internal = true,
            StagingStorageAccount = "https://dotnetstagetest.blob.core.windows.net/",
            Mode = ChangeMode.Local,
            RepoRoot = "/tmp/repo"
        };

        await command.ExecuteAsync(options);

        output.ToString().ShouldContain($"##vso[build.addbuildtag]{expectedTag}");
    }

    [Fact]
    public async Task ExecuteAsync_NotInternalMode_DoesNotAddBuildTag()
    {
        using var output = new StringWriter();
        var buildLabelService = new BuildLabelService(output);

        var command = CreateCommand(
            buildLabelService: buildLabelService,
            pipelineArtifactProvider: CreateMockPipelineArtifactProvider());

        var options = new FromStagingPipelineOptions
        {
            StageContainer = "stage-1234567",
            Internal = false,
            Mode = ChangeMode.Local,
            RepoRoot = "/tmp/repo"
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
        var options = new FromStagingPipelineOptions
        {
            StageContainer = stageContainer,
            RepoRoot = "/tmp/repo"
        };

        var result = options.GetStagingPipelineRunId();

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
        var options = new FromStagingPipelineOptions
        {
            StageContainer = stageContainer,
            RepoRoot = "/tmp/repo"
        };

        var exception = Should.Throw<ArgumentException>(() => options.GetStagingPipelineRunId());
        exception.Message.ShouldContain($"Invalid stage container name '{stageContainer}'");
        exception.Message.ShouldContain("Expected format: 'stage-{buildId}'");
    }

    private static FromStagingPipelineCommand CreateCommand(
        ILogger<FromStagingPipelineCommand>? logger = null,
        IPipelineArtifactProvider? pipelineArtifactProvider = null,
        IPipelinesService? pipelinesService = null,
        IInternalVersionsService? internalVersionsService = null,
        IEnvironmentService? environmentService = null,
        IBuildLabelService? buildLabelService = null,
        IGitRepoHelperFactory? gitRepoHelperFactory = null) =>
            new(logger ?? Mock.Of<ILogger<FromStagingPipelineCommand>>(),
                pipelineArtifactProvider ?? CreateMockPipelineArtifactProvider(),
                pipelinesService ?? CreateMockPipelinesService(),
                internalVersionsService ?? Mock.Of<IInternalVersionsService>(),
                environmentService ?? Mock.Of<IEnvironmentService>(),
                buildLabelService ?? Mock.Of<IBuildLabelService>(),
                gitRepoHelperFactory ?? Mock.Of<IGitRepoHelperFactory>());

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
