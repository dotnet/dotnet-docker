#nullable enable

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class AspireDashboardBasicScenario : ITestScenario
{
    private const int DashboardPort = 18888;

    private readonly DockerHelper _dockerHelper;

    private readonly ITestOutputHelper _outputHelper;

    private readonly ProductImageData _imageData;

    private readonly string _imageTag;

    public AspireDashboardBasicScenario(
        ProductImageData imageData,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper)
    {
        _dockerHelper = dockerHelper;
        _imageData = imageData;
        _outputHelper = outputHelper;

        _imageTag = _imageData.GetImage(DotNetImageRepo.Aspire_Dashboard, _dockerHelper);
    }

    public async Task ExecuteAsync()
    {
        string containerName = _imageData.GetIdentifier(nameof(AspireDashboardBasicScenario));

        try
        {
            _dockerHelper.Run(
                image: _imageTag,
                name: containerName,
                detach: true,
                optionalRunArgs: $"-p {DashboardPort}",
                skipAutoCleanup: true);

            await WebScenario.VerifyHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                DashboardPort);
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName);
        }
    }
}
