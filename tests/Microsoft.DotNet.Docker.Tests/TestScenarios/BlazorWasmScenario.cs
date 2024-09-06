#nullable enable

using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class BlazorWasmScenario(
    ProductImageData imageData,
    DockerHelper dockerHelper,
    ITestOutputHelper outputHelper,
    bool useWasmTools)
    : WebScenario(imageData, dockerHelper, outputHelper)
{
    protected override TestDockerfile Dockerfile { get; } = TestDockerfileBuilder.GetBlazorWasmDockerfile(useWasmTools);
    protected override string SampleName { get; } = "blazorwasm";
    protected override bool OutputIsStatic { get; } = true;

    // Known issue: Blazor ignores the ASPNETCORE_HTTP_PORTS environment variable that we set in runtime-deps.
    // We need to manually override it with ASPNETCORE_URLS.
    // https://github.com/dotnet/aspnetcore/issues/52494
    protected override int? PortOverride { get; } = 8080;
}
