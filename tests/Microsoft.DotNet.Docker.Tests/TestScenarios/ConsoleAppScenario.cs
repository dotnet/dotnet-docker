// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests.TestScenarios;

public abstract class ConsoleAppScenario(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper) : ProjectTemplateTestScenario(imageData, dockerHelper, outputHelper)
{
    protected override string SampleName { get; } = "console";

    protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime;

    protected override DotNetImageRepo SdkImageRepo { get; } = DotNetImageRepo.SDK;

    protected override Task RunAsync(string image, string? user, string? command = null)
    {
        string containerName = ImageData.GetIdentifier("app-run");

        try
        {
            DockerHelper.Run(
                image: image,
                name: containerName,
                runAsUser: user,
                command: command);
        }
        finally
        {
            DockerHelper.DeleteContainer(containerName, captureLogs: true);
        }

        return Task.FromResult(0);
    }

    public class FxDependent(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
    {
        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.FxDependent);
    }

    public class SelfContained(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
    {
        protected override DotNetImageRepo RuntimeImageRepo { get; } = DotNetImageRepo.Runtime_Deps;

        protected override TestDockerfile Dockerfile =>
            TestDockerfileBuilder.GetDefaultDockerfile(PublishConfig.SelfContained);
    }

    public class TestProject(ProductImageData imageData, DockerHelper dockerHelper, ITestOutputHelper outputHelper)
        : ConsoleAppScenario(imageData, dockerHelper, outputHelper)
    {
        protected override bool NonRootUserSupported => false;
        protected override TestDockerfile Dockerfile { get; } =
            TestDockerfileBuilder.GetTestProjectDockerfile();
    }
}
