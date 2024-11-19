#nullable enable

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public abstract class ConsoleAppScenario : ProjectTemplateTestScenario
{
    protected override string SampleName { get; } = "console";

    protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime;

    protected override DotNetImageRepo SdkImageRepo { get; } = DotNetImageRepo.SDK;

    public ConsoleAppScenario(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : base(imageData, dockerHelper, outputHelper)
    {
    }

    protected override Task RunAsync(string image, string? user, string? command = null)
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
            DockerHelper.DeleteContainer(containerName, captureLogs: true);
        }

        return Task.FromResult(0);
    }

    public class FxDependent(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
    {
        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.FxDependent);
    }

    public class SelfContained(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
    {
        protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime_Deps;

        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.SelfContained);
    }

    public class TestProject(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
    {
        protected override bool NonRootUserSupported => false;
        protected override TestDockerfile Dockerfile { get; } =
            TestDockerfileBuilder.GetTestProjectDockerfile();
    }
}
