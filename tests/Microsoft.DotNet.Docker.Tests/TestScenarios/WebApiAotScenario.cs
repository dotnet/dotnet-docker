#nullable enable

using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class WebApiAotScenario(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
    : WebScenario(imageData, dockerHelper, outputHelper)
{
    protected override string? Endpoint { get; } = "todos";
    protected override string SampleName { get; } = "webapiaot";
    protected override string BuildStageTarget { get; } = "publish_aot";
    protected override string? TestStageTarget { get; } = "test";
    protected override string[] AppStageTargets { get; } = [ "aot_app" ];
    protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime_Deps;
}
