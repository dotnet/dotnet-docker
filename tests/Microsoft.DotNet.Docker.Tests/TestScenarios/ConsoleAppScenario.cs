#nullable enable

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class ConsoleAppScenario : ProjectTemplateTestScenario
{
    protected override string SampleName { get; } = "console";

    protected override string BuildStageTarget { get; } = "build";

    protected override string? TestStageTarget { get; } = "test";

    protected override string[] AppStageTargets { get; } = DockerHelper.IsLinuxContainerModeEnabled
        ? ["fx_dependent_app", "self_contained_app"]
        : ["fx_dependent_app"];

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
            DockerHelper.DeleteContainer(containerName);
        }

        return Task.FromResult(0);
    }
}
