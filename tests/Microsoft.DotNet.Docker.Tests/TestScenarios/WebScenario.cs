#nullable enable

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class WebScenario(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
    : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
{
    protected virtual int? PortOverride { get; } = null;

    protected virtual string? Endpoint { get; } = null;

    protected override string SampleName { get; } = "web";

    protected override string BuildStageTarget { get; } = "build";

    // Running a scenario of unit testing within the sdk container is identical between a console app and web app,
    // so we only want to execute it for one of those app types.
    protected override string? TestStageTarget { get; } = null;

    protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Aspnet;

    protected override async Task RunAsync(string image, string? user, string? command = null)
    {
        string containerName = ImageData.GetIdentifier("app-run");

        command ??= "dotnet run";

        int port = PortOverride ?? ImageData.DefaultPort;

        if (PortOverride != null || ImageData.Version.Major == 6 || ImageData.Version.Major == 7)
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
                skipAutoCleanup: true);

            await VerifyHttpResponseFromContainerAsync(
                containerName,
                DockerHelper,
                OutputHelper,
                port,
                pathAndQuery: Endpoint);
        }
        finally
        {
            DockerHelper.DeleteContainer(containerName);
        }
    }

    public static async Task<HttpResponseMessage> GetHttpResponseFromContainerAsync(
        string containerName,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper,
        int containerPort,
        string? pathAndQuery = null,
        Action<HttpResponseMessage>? validateCallback = null,
        AuthenticationHeaderValue? authorizationHeader = null)
    {
        int retries = 4;

        // Can't use localhost when running inside containers or Windows.
        string url = !Config.IsRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
            ? $"http://localhost:{dockerHelper.GetContainerHostPort(containerName, containerPort)}/{pathAndQuery}"
            : $"http://{dockerHelper.GetContainerAddress(containerName)}:{containerPort}/{pathAndQuery}";

        using (HttpClient client = new HttpClient())
        {
            if (null != authorizationHeader)
            {
                client.DefaultRequestHeaders.Authorization = authorizationHeader;
            }

            while (retries > 0)
            {
                retries--;
                await Task.Delay(TimeSpan.FromSeconds(2));

                HttpResponseMessage? result = null;
                try
                {
                    result = await client.GetAsync(url);
                    outputHelper.WriteLine($"HTTP {result.StatusCode}\n{await result.Content.ReadAsStringAsync()}");

                    if (null == validateCallback)
                    {
                        result.EnsureSuccessStatusCode();
                    }
                    else
                    {
                        validateCallback(result);
                    }

                    // Store response in local that will not be disposed
                    HttpResponseMessage returnResult = result;
                    result = null;
                    return returnResult;
                }
                catch (Exception ex)
                {
                    outputHelper.WriteLine($"Request to {url} failed - retrying: {ex}");
                }
                finally
                {
                    result?.Dispose();
                }
            }
        }

        throw new TimeoutException($"Timed out attempting to access the endpoint {url} on container {containerName}");
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
            pathAndQuery,
            validateCallback)).Dispose();
    }
}
