#nullable enable

using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class BlazorWasmScenario(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
    : WebScenario(imageData, dockerHelper, outputHelper)
{
    protected override string SampleName { get; } = "blazorwasm";
    protected override string BuildStageTarget { get; } = "publish_self_contained";
    protected override string? TestStageTarget { get; } = null;

    // Known issue: Blazor ignores the ASPNETCORE_HTTP_PORTS environment variable that we set in runtime-deps.
    // We need to manually override it with ASPNETCORE_URLS.
    // https://github.com/dotnet/aspnetcore/issues/52494
    protected override int? PortOverride { get; } = 8080;

    // BlazorWASM publish output is a static site, so we don't need to run the app to verify it.
    // Endpoint access will be verified by `dotnet run` in the SDK image.
    protected override string[] AppStageTargets { get; } = [];
}
