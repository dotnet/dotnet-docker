#nullable enable

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class YarpBasicScenario : ITestScenario
{
    private readonly DockerHelper _dockerHelper;

    private readonly ITestOutputHelper _outputHelper;

    private readonly ProductImageData _imageData;

    private readonly string _imageTag;

    private readonly int _webPort;

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

        try
        {
            // Deploy the aspnet sample app
            _dockerHelper.Run(
                image: "mcr.microsoft.com/dotnet/samples:aspnetapp",
                name: sampleContainer,
                detach: true,
                optionalRunArgs: "",
                skipAutoCleanup: true);

            File.WriteAllText(configFile.Path, ConfigFileContent);

            _dockerHelper.Run(
                image: _imageTag,
                name: containerName,
                detach: true,
                optionalRunArgs: $"-p {_webPort} -v {configFile.Path}:/etc/yarp.config --link {sampleContainer}:aspnetapp1",
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
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName);
            _dockerHelper.DeleteContainer(sampleContainer);
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
}
