#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class TestScenario
{
    protected static string AdminUser { get; } = DockerHelper.IsLinuxContainerModeEnabled ? "root" : "ContainerAdministrator";

    protected static string NonRootUser { get; } = DockerHelper.IsLinuxContainerModeEnabled ? "app" : "ContainerUser";

    protected DockerHelper DockerHelper { get; }

    protected ProductImageData ImageData { get; }

    protected ITestOutputHelper OutputHelper { get; }

    protected bool NonRootUserSupported { get; }

    protected TestSolution TestSolution { get; }

    protected virtual string SampleName { get; } = "console";

    // Target stages refer to stages in TestAppArtifacts/Dockerfile.linux
    protected virtual string BuildStageTarget { get; } = "build";

    protected virtual string? TestStageTarget { get; } = "test";

    protected virtual string[] AppStageTargets { get; } = [ "self_contained_app", "fx_dependent_app" ];

    protected virtual DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime;

    protected DotNetImageRepo SdkImageRepo { get; } = DotNetImageRepo.SDK;

    public TestScenario(
        ProductImageData imageData,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper)
    {
        DockerHelper = dockerHelper;
        ImageData = imageData;
        OutputHelper = outputHelper;
        NonRootUserSupported = DockerHelper.IsLinuxContainerModeEnabled && ImageData.Version.Major > 7;

        TestSolution = new(imageData, SampleName, dockerHelper, withTests: TestStageTarget != null);
    }

    protected string Build(string stageTarget, string[]? customBuildArgs)
    {
        string tag = ImageData.GetIdentifier(stageTarget);

        List<string> buildArgs =
        [
            $"sdk_image={ImageData.GetImage(SdkImageRepo, DockerHelper)}",
            $"runtime_image={ImageData.GetImage(RuntimeImageRepo, DockerHelper)}",
            $"port={ImageData.DefaultPort}"
        ];

        if (DockerHelper.IsLinuxContainerModeEnabled)
        {
            // Docker needs all FROM images to be defined even if we won't use them for testing Composite images
            ProductImageData runtimeDepsImageData = ImageData.ImageVariant.HasFlag(DotNetImageVariant.Composite)
                ?  new ProductImageData()
                    {
                        Version = ImageData.Version,
                        OS = ImageData.OS,
                        Arch = ImageData.Arch,
                        SdkOS = ImageData.SdkOS,
                        ImageVariant = DotNetImageVariant.None,
                        SupportedImageRepos = DotNetImageRepo.Runtime_Deps,
                    }
                : ImageData;

            buildArgs.Add($"runtime_deps_image={runtimeDepsImageData.GetImage(DotNetImageRepo.Runtime_Deps, DockerHelper)}");
        }

        if (customBuildArgs != null)
        {
            buildArgs.AddRange(customBuildArgs);
        }

        const string NuGetFeedPasswordVar = "NuGetFeedPassword";

        if (!string.IsNullOrEmpty(Config.NuGetFeedPassword))
        {
            buildArgs.Add(NuGetFeedPasswordVar);
            Environment.SetEnvironmentVariable(NuGetFeedPasswordVar, Config.NuGetFeedPassword);
        }

        try
        {
            DockerHelper.Build(
                tag: tag,
                target: stageTarget,
                contextDir: TestSolution.SolutionDir,
                platform: ImageData.Platform,
                buildArgs: buildArgs.ToArray());
        }
        finally
        {
            if (!string.IsNullOrEmpty(Config.NuGetFeedPassword))
            {
                Environment.SetEnvironmentVariable(NuGetFeedPasswordVar, null);
            }
        }

        return tag;
    }

    public async Task Execute()
    {
        List<string> tags = [];

        try
        {
            // Need to include the RID for all build stages because they all rely on "dotnet restore". We should
            // always provide RID when running restore because it's RID-dependent. If we don't then a call to the
            // publish command with a different RID than the default would end up restoring images. This is not
            // what we'd want and plus it would fail in that case if it was targeting a private NuGet feed because
            // the password isn't necessarily provided in that stage.
            string[] customBuildArgs = [ $"rid={ImageData.Rid}" ];

            // Build and run app on SDK image
            string buildTag = Build(BuildStageTarget, customBuildArgs);
            tags.Add(buildTag);
            await Run(buildTag, AdminUser, "dotnet run");

            // Build and run tests on SDK image
            // Tests must run as admin user in order to write the test results to the output directory in the
            // project directory
            if (!string.IsNullOrEmpty(TestStageTarget))
            {
                string unitTestTag = Build(TestStageTarget, customBuildArgs);
                tags.Add(unitTestTag);
                await Run(unitTestTag, AdminUser);
            }

            // Build and run all other projects for each user and target stage
            foreach (string target in AppStageTargets)
            {
                string tag = Build(target, customBuildArgs);
                tags.Add(tag);
                await Run(tag, AdminUser);

                if (NonRootUserSupported)
                {
                    await Run(tag, NonRootUser);
                }
            }
        }
        finally
        {
            tags.ForEach(tag => DockerHelper.DeleteImage(tag));
        }
    }

    protected virtual Task Run(string image, string user, string? command = null)
    {
        string containerName = ImageData.GetIdentifier("app-run");

        try
        {
            DockerHelper.Run(
                image: image,
                name: containerName,
                runAsUser: user,
                command: command);
        }
        finally
        {
            DockerHelper.DeleteContainer(containerName);
        }

        return Task.FromResult(0);
    }
}
