#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.Docker.Tests;

public class YarpBasicScenario : ITestScenario
{
    private readonly DockerHelper _dockerHelper;

    private readonly ITestOutputHelper _outputHelper;

    private readonly ProductImageData _imageData;

    private readonly string _imageTag;

    private readonly int _webPort;

    private const int OtelHttpPort = 8080;
    private const int OtelGrpcPort = 4317;
    private const int OtelTimeout = 1000;

    public YarpBasicScenario(
        int webPort,
        ProductImageData imageData,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper)
    {
        _dockerHelper = dockerHelper;
        _imageData = imageData;
        _outputHelper = outputHelper;
        _webPort = webPort;

        _imageTag = _imageData.GetImage(DotNetImageRepo.Yarp, _dockerHelper);
    }

    public async Task ExecuteAsync()
    {
        string containerName = _imageData.GetIdentifier(nameof(YarpBasicScenario));
        using TempFileContext configFile = FileHelper.UseTempFile();
        string sampleContainer = $"{containerName}_aspnetapp";
        string otelContainerTag = $"local-otlptestlistener";
        string otelContainer = $"{containerName}_otel";

        try
        {
            // Deploy opentelemetry endpoint
            string otelAppTag = "otlptestlistener";
            string sampleFolder = Path.Combine(DockerHelper.TestArtifactsDir, "otlptestlistener");
            string dockerfilePath = $"{sampleFolder}/Dockerfile";
            _dockerHelper.Build(otelContainerTag, dockerfilePath, contextDir: sampleFolder, pull: Config.PullImages);
            _dockerHelper.Run(
                image: otelContainerTag,
                name: otelContainer,
                detach: true,
                optionalRunArgs: "-P",
                skipAutoCleanup: true);
            string otelHostPort = _dockerHelper.GetContainerHostPort(otelContainer, OtelGrpcPort);

            // Deploy the aspnet sample app
            _dockerHelper.Run(
                image: "mcr.microsoft.com/dotnet/samples:aspnetapp",
                name: sampleContainer,
                detach: true,
                optionalRunArgs: "",
                skipAutoCleanup: true);

            // Check that otel endpoint report is empty
            HttpResponseMessage emptyTelemetryResponse = await WebScenario.GetHttpResponseFromContainerAsync(
                otelContainer,
                _dockerHelper,
                _outputHelper,
                OtelHttpPort,
                pathAndQuery: "/report");
            await CheckTelemetryResponse(emptyTelemetryResponse, 0, 0, 0, 0);

            File.WriteAllText(configFile.Path, ConfigFileContent);

            _dockerHelper.Run(
                image: _imageTag,
                name: containerName,
                detach: true,
                optionalRunArgs: $"-e OTEL_EXPORTER_OTLP_ENDPOINT=http://host.docker.internal:{otelHostPort} -e OTEL_EXPORTER_OTLP_TIMEOUT={OtelTimeout}  -p {_webPort} -v {configFile.Path}:/etc/yarp.config --link {sampleContainer}:aspnetapp1",
                skipAutoCleanup: true);

            // base uri should return 404
            HttpResponseMessage notFoundResponse = await WebScenario.GetHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                _webPort,
                pathAndQuery: "/");

            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);

            // /aspnetapp should return a valid response
            HttpResponseMessage okResponse = await WebScenario.GetHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                _webPort,
                pathAndQuery: "/aspnetapp");
            Assert.Equal(HttpStatusCode.OK, okResponse.StatusCode);

            // Wait a bit for telemetry to arrive
            await Task.Delay(OtelTimeout * 10);

            // Some messages and some metrics should have been received
            HttpResponseMessage nonEmptyTelemetryResponse = await WebScenario.GetHttpResponseFromContainerAsync(
                otelContainer,
                _dockerHelper,
                _outputHelper,
                OtelHttpPort,
                pathAndQuery: "/report");
            await CheckTelemetryResponse(nonEmptyTelemetryResponse, minlogMessageCount: 10);
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName);
            _dockerHelper.DeleteContainer(sampleContainer);
            _dockerHelper.DeleteContainer(otelContainer);
            _dockerHelper.DeleteImage(otelContainerTag);
        }
    }

    private async Task CheckTelemetryResponse(
        HttpResponseMessage telemetryResponse,
        int? minlogMessageCount = null,
        int? minMetricNameCount = null,
        int? maxlogMessageCount = null,
        int? maxMetricNameCount = null)
    {
        Assert.Equal(HttpStatusCode.OK, telemetryResponse.StatusCode);
        TelemetryResults? response = await JsonSerializer.DeserializeAsync<TelemetryResults>(telemetryResponse.Content.ReadAsStream());

        Assert.NotNull(response);

        if (minlogMessageCount.HasValue)
        {
            Assert.True(
                minlogMessageCount.Value <= response!.LogMessageCount,
                $"Not enough messages received: expected minimum: {minlogMessageCount.Value}, actual: {response!.LogMessageCount}");
        }
        if (maxlogMessageCount.HasValue)
        {
            Assert.True(
                maxlogMessageCount.Value >= response!.LogMessageCount,
                $"Too many messages received: expected maximum: {maxlogMessageCount.Value}, actual: {response!.LogMessageCount}");
        }
        if (minMetricNameCount.HasValue)
        {
            Assert.True(
                minMetricNameCount.Value <= response!.MetricNameCount,
                $"Not enough metrics received: expected minimum: {minMetricNameCount.Value}, actual: {response!.MetricNameCount}");
        }
        if (maxMetricNameCount.HasValue)
        {
            Assert.True(
                maxMetricNameCount.Value >= response!.MetricNameCount,
                $"Too many metrics received: expected maximum: {maxMetricNameCount.Value}, actual: {response!.MetricNameCount}");
        }
    }

    private const string ConfigFileContent = """
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Information",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AllowedHosts": "*",
      "ReverseProxy": {
        "Routes": {
          "route1": {
            "ClusterId": "cluster1",
            "Match": {
              "Path": "/aspnetapp/{**catch-all}"
            },
            "Transforms": [
                { "PathRemovePrefix": "/aspnetapp" }
            ]
          }
        },
        "Clusters": {
          "cluster1": {
            "Destinations": {
              "destination1": {
                "Address": "http://aspnetapp1:8080"
              }
            }
          }
        }
      }
    }
    """;

    private class TelemetryResults
    {
        public int SpanIdCount { get; set; }
        public int LogMessageCount { get; set; }
        public List<string> MetricNames { get; init; } = [];
        public List<string> ResourceNames { get; init; } = [];
        public List<string> TraceIds { get; init; } = [];
        [JsonIgnore]
        public int MetricNameCount => MetricNames.Count;
        [JsonIgnore]
        public int TraceIdCount => TraceIds.Count;
    }
}
