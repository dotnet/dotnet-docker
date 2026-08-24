// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class BlazorGatewayBasicScenario : ITestScenario
{
    private readonly DockerHelper _dockerHelper;
    private readonly ITestOutputHelper _outputHelper;
    private readonly ProductImageData _imageData;
    private readonly string _imageTag;

    private const int WebPort = 8080;
    private const string ClientConfig = "{}";

    public BlazorGatewayBasicScenario(
        ProductImageData imageData,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper)
    {
        _dockerHelper = dockerHelper;
        _imageData = imageData;
        _outputHelper = outputHelper;

        _imageTag = _imageData.GetImage(DotNetImageRepo.Blazor_Gateway, _dockerHelper);
    }

    public async Task ExecuteAsync()
    {
        string containerName = _imageData.GetIdentifier(nameof(BlazorGatewayBasicScenario));
        string sampleContainer = $"{containerName}_aspnetapp";

        try
        {
            _dockerHelper.Run(
                image: "mcr.microsoft.com/dotnet/samples:aspnetapp",
                name: sampleContainer,
                detach: true,
                skipAutoCleanup: true);

            _dockerHelper.Run(
                image: _imageTag,
                name: containerName,
                detach: true,
                optionalRunArgs:
                    $"-p {WebPort} " +
                    "-e ClientApps__app__PathPrefix=/gateway " +
                    "-e ClientApps__app__EndpointsManifest=/app/blazor-gateway.staticwebassets.endpoints.json " +
                    "-e ClientApps__app__ConfigEndpointPath=/gateway/_blazor/_configuration " +
                    $"-e ClientApps__app__ConfigResponse={ClientConfig} " +
                    "-e ReverseProxy__Routes__api__ClusterId=api " +
                    "-e ReverseProxy__Routes__api__Match__Path=/gateway/api/{**catch-all} " +
                    "-e ReverseProxy__Routes__api__Transforms__0__PathRemovePrefix=/gateway/api " +
                    "-e ReverseProxy__Clusters__api__Destinations__app__Address=http://aspnetapp:8080 " +
                    $"--link {sampleContainer}:aspnetapp",
                skipAutoCleanup: true);

            using HttpResponseMessage aliveResponse = await WebScenario.GetHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                WebPort,
                pathAndQuery: "/alive");

            aliveResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            using HttpResponseMessage configResponse = await GetResponseAsync(
                containerName,
                "/gateway/_blazor/_configuration",
                "identity");

            configResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            configResponse.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");
            (await configResponse.Content.ReadAsStringAsync()).ShouldBe(ClientConfig);

            using HttpResponseMessage assetResponse = await GetResponseAsync(
                containerName,
                "/gateway/_framework/blazor.web.js",
                "br");
            assetResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            assetResponse.Content.Headers.ContentType?.MediaType.ShouldBe("text/javascript");
            assetResponse.Content.Headers.ContentEncoding.ShouldContain("br");

            using HttpResponseMessage proxyResponse = await WebScenario.GetHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                WebPort,
                pathAndQuery: "/gateway/api/");

            proxyResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName, captureLogs: true);
            _dockerHelper.DeleteContainer(sampleContainer, captureLogs: true);
        }
    }

    private async Task<HttpResponseMessage> GetResponseAsync(
        string containerName,
        string path,
        string acceptEncoding)
    {
        string url = !Config.IsRunningInContainer && DockerHelper.IsLinuxContainerModeEnabled
            ? $"http://localhost:{_dockerHelper.GetContainerHostPort(containerName, WebPort)}{path}"
            : $"http://{_dockerHelper.GetContainerAddress(containerName)}:{WebPort}{path}";

        using HttpClient client = new(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.None,
        });
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        request.Headers.AcceptEncoding.ParseAdd(acceptEncoding);
        return await client.SendAsync(request);
    }
}
