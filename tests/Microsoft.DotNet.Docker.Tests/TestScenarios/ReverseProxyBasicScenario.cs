#nullable enable

using System.IO;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class ReverseProxyBasicScenario : ITestScenario
{
    private readonly DockerHelper _dockerHelper;

    private readonly ITestOutputHelper _outputHelper;

    private readonly ProductImageData _imageData;

    private readonly string _imageTag;

    private readonly int _webPort;

    public ReverseProxyBasicScenario(
        int webPort,
        ProductImageData imageData,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper)
    {
        _dockerHelper = dockerHelper;
        _imageData = imageData;
        _outputHelper = outputHelper;
        _webPort = webPort;

        _imageTag = _imageData.GetImage(DotNetImageRepo.Reverse_Proxy, _dockerHelper);
    }

    public async Task ExecuteAsync()
    {
        string containerName = _imageData.GetIdentifier(nameof(ReverseProxyBasicScenario));
        using TempFileContext configFile = FileHelper.UseTempFile();

        try
        {
            File.WriteAllText(configFile.Path, ConfigFileContent);

            _dockerHelper.Run(
                image: _imageTag,
                name: containerName,
                detach: true,
                optionalRunArgs: $"-p {_webPort} -v {configFile.Path}:/etc/reverse-proxy.config",
                skipAutoCleanup: true);

            await WebScenario.VerifyHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                _webPort);
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName);
        }
    }

    private const string ConfigFileContent = @"
{
 ""Logging"": {
   ""LogLevel"": {
     ""Default"": ""Information"",
     ""Microsoft"": ""Warning"",
     ""Microsoft.Hosting.Lifetime"": ""Information""
   }
 },
 ""AllowedHosts"": ""*"",
 ""ReverseProxy"": {
   ""Routes"": {
     ""route1"" : {
       ""ClusterId"": ""cluster1"",
       ""Match"": {
         ""Path"": ""/bing/{**catch-all}""
       }
     }
   },
   ""Clusters"": {
     ""cluster1"": {
       ""Destinations"": {
         ""destination1"": {
           ""Address"": ""https://bing.com/""
         }
       }
     }
   }
 }
}
";
}
