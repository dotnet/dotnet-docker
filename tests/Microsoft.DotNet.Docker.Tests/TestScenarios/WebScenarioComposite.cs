#nullable enable

using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class WebScenarioComposite(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
    : WebScenario(imageData, dockerHelper, outputHelper)
{
    protected override string[] AppStageTargets { get; } = [ "fx_dependent_app" ];
}
