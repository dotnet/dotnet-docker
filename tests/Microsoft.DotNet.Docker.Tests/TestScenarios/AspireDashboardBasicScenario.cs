#nullable enable

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class AspireDashboardBasicScenario : ITestScenario
{
    private readonly DockerHelper _dockerHelper;

    private readonly ITestOutputHelper _outputHelper;

    private readonly ProductImageData _imageData;

    private readonly string _imageTag;

    private readonly int _dashboardWebPort;

    public AspireDashboardBasicScenario(
        int dashboardWebPort,
        ProductImageData imageData,
        DockerHelper dockerHelper,
        ITestOutputHelper outputHelper)
    {
        _dockerHelper = dockerHelper;
        _imageData = imageData;
        _outputHelper = outputHelper;
        _dashboardWebPort = dashboardWebPort;

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
                optionalRunArgs: $"-p {_dashboardWebPort} -e DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true",
                skipAutoCleanup: true);

            await WebScenario.VerifyHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                _dashboardWebPort);
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName);
        }
    }
}
