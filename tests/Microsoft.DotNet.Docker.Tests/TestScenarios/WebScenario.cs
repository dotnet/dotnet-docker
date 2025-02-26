#nullable enable

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public abstract class WebScenario(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
    : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
{
    protected virtual int? PortOverride { get; } = null;

    protected virtual string? Endpoint { get; } = null;

    protected override string SampleName { get; } = "web";

    protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Aspnet;

    protected override async Task RunAsync(string image, string? user, string? command = null)
    {
        string containerName = ImageData.GetIdentifier("app-run");

        command ??= "dotnet run";

        int port = PortOverride ?? ImageData.DefaultPort;

        if (PortOverride != null)
        {
            command += $" --urls http://*:{port}";
        }

        try
        {
            DockerHelper.Run(
                image: image,
                name: containerName,
                detach: true,
                optionalRunArgs: $"-p {port}",
                runAsUser: user,
                command: command,
                skipAutoCleanup: true,
                tty: false);

            await VerifyHttpResponseFromContainerAsync(
                containerName,
                DockerHelper,
                OutputHelper,
                port,
                pathAndQuery: Endpoint);
        }
        finally
        {
            DockerHelper.DeleteContainer(containerName, captureLogs: true);
        }
    }

    public static async Task<HttpResponseMessage> GetHttpResponseFromContainerAsync(
        string containerName,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper,
        int containerPort,
        string? pathAndQuery = null,
        AuthenticationHeaderValue? authorizationHeader = null)
    {
        const int RetryAttempts = 4;
        const int RetryDelaySeconds = 3;

        if (!string.IsNullOrEmpty(pathAndQuery) && !pathAndQuery.StartsWith('/'))
        {
            pathAndQuery = "/" + pathAndQuery;
        }

        // Can't use localhost when running inside containers or Windows.
        string url = !Config.IsRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
            ? $"http://localhost:{dockerHelper.GetContainerHostPort(containerName, containerPort)}{pathAndQuery}"
            : $"http://{dockerHelper.GetContainerAddress(containerName)}:{containerPort}{pathAndQuery}";

        ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
                {
                    BackoffType = DelayBackoffType.Exponential,
                    MaxRetryAttempts = RetryAttempts,
                    Delay = TimeSpan.FromSeconds(RetryDelaySeconds),

                    // If the container is still starting up, it will refuse connections until it's ready.
                    // If it crashed, stop retrying immediately.
                    ShouldHandle = new PredicateBuilder()
                        .Handle<HttpRequestException>(exception =>
                            DockerHelper.ContainerIsRunning(containerName)),
                })
            .Build();

        using HttpClient client = new();
        if (authorizationHeader is not null)
        {
            client.DefaultRequestHeaders.Authorization = authorizationHeader;
        }

        HttpResponseMessage? result = null;
        try
        {
            result = await pipeline.ExecuteAsync(async cancellationToken =>
                {
                    outputHelper.WriteLine($"Sending request: GET {url}");
                    return await client.GetAsync(url, cancellationToken);
                });

            outputHelper.WriteLine($"""
                Response: HTTP {result.StatusCode}
                Content: {await result.Content.ReadAsStringAsync()}
                """);

            // Store response in local that will not be disposed
            HttpResponseMessage returnResult = result;
            result = null;
            return returnResult;
        }
        finally
        {
            result?.Dispose();
        }
    }

    public static async Task VerifyHttpResponseFromContainerAsync(
        string containerName,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper,
        int containerPort,
        string? pathAndQuery = null,
        Action<HttpResponseMessage>? validateCallback = null)
    {
        (await GetHttpResponseFromContainerAsync(
            containerName,
            dockerHelper,
            outputHelper,
            containerPort,
            pathAndQuery)).Dispose();
    }

    public new class FxDependent(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : WebScenario(imageData, dockerHelper, outputHelper)
    {
        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.FxDependent);
    }

    public new class SelfContained(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : WebScenario(imageData, dockerHelper, outputHelper)
    {
        protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime_Deps;

        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.SelfContained);
    }

    public class Aot(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : WebScenario(imageData, dockerHelper, outputHelper)
    {
        protected override string? Endpoint { get; } = "todos";
        protected override string SampleName { get; } = "webapiaot";
        protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime_Deps;

        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.Aot);
    }
}
