#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public class AspireDashboardBasicScenario : ITestScenario
{
    private const string DashboardPortEnvVar = "ASPNETCORE_URLS";

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
        int dashboardPort = GetDashboardPort(_imageTag, _dockerHelper);

        try
        {
            _dockerHelper.Run(
                image: _imageTag,
                name: containerName,
                detach: true,
                optionalRunArgs: $"-p {dashboardPort}",
                skipAutoCleanup: true);

            await WebScenario.VerifyHttpResponseFromContainerAsync(
                containerName,
                _dockerHelper,
                _outputHelper,
                dashboardPort);
        }
        finally
        {
            _dockerHelper.DeleteContainer(containerName);
        }
    }

    private static int GetDashboardPort(string imageTag, DockerHelper dockerHelper)
    {
        IDictionary<string, string> envVars = dockerHelper.GetEnvironmentVariables(imageTag);
        string portString = envVars[DashboardPortEnvVar].Split(':')[^1];
        return int.Parse(portString);
    }
}
