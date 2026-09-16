// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UpdateDependencies.Tests;

public sealed class BarCommandTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Commands_ResolveSupportedCapabilityAndUseRequestedWorkspace(bool fromChannel)
    {
        using var repo = new TempRepo();
        string repoRoot = Path.Combine(repo.LocalPath, "workspace with spaces");
        Directory.CreateDirectory(repoRoot);
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        File.WriteAllText(manifestPath, """{"variables":{"runtime|11.0|build-version":"11.0.1"}}""");
        foreach (var (directory, file) in new[]
        {
            ("dockerfile-templates", "Get-GeneratedDockerfiles.ps1"),
            ("readme-templates", "Get-GeneratedReadmes.ps1"),
        })
        {
            string path = Path.Combine(repoRoot, "eng", directory);
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, file), "Add-Content ./generated.txt 'generated'");
        }
        Build build = JsonConvert.DeserializeObject<Build>("""
            {"id":123,"githubRepository":"https://github.com/dotnet/dotnet","commit":"abc"}
            """)!;
        var updater = new Mock<IBarBuildUpdater>(MockBehavior.Strict);
        var channelUpdater = updater.As<IBarChannelUpdater>();
        if (fromChannel)
        {
            channelUpdater.Setup(service => service.UpdateFromBarChannelAsync(
                It.IsAny<ManifestVariables>(), repoRoot, "https://github.com/dotnet/dotnet", 42, It.IsAny<CancellationToken>()))
                .Callback<ManifestVariables, string, string, int, CancellationToken>((variables, _, _, _, _) =>
                    variables.SetValue("runtime|11.0|build-version", "11.0.2"))
                .Returns(Task.CompletedTask);
        }
        else
        {
            updater.Setup(service => service.UpdateFromBarBuildAsync(
                It.IsAny<ManifestVariables>(), repoRoot, build, It.IsAny<CancellationToken>()))
                .Callback<ManifestVariables, string, Build, CancellationToken>((variables, _, _, _) =>
                    variables.SetValue("runtime|11.0|build-version", "11.0.2"))
                .Returns(Task.CompletedTask);
        }
        using var services = new ServiceCollection()
            .AddKeyedSingleton<IUpdater>(DotNetUpdater.Key, updater.Object)
            .BuildServiceProvider();

        int exitCode;
        if (fromChannel)
        {
            var command = new FromChannelCommand(services, Mock.Of<ILogger<FromChannelCommand>>());
            exitCode = await command.ExecuteAsync(new FromChannelOptions
            {
                Channel = 42,
                Repo = "https://github.com/dotnet/dotnet",
                RepoRoot = repoRoot,
            });
        }
        else
        {
            var bar = new Mock<IBasicBarClient>(MockBehavior.Strict);
            bar.Setup(client => client.GetBuildAsync(123)).ReturnsAsync(build);
            var command = new FromBuildCommand(bar.Object, Mock.Of<ILogger<FromBuildCommand>>(), services);
            exitCode = await command.ExecuteAsync(new FromBuildOptions { Id = 123, RepoRoot = repoRoot });
            bar.VerifyAll();
        }

        exitCode.ShouldBe(0);
        ManifestVariables.FromFile(manifestPath).GetRawValue("runtime|11.0|build-version").ShouldBe("11.0.2");
        File.ReadAllLines(Path.Combine(repoRoot, "generated.txt")).Length.ShouldBe(2);
        updater.VerifyAll();
        channelUpdater.VerifyAll();
    }

    [Fact]
    public void Resolution_DistinguishesUnknownUpdaterFromUnsupportedCapability()
    {
        using var services = new ServiceCollection()
            .AddKeyedSingleton<IUpdater>("unsupported", Mock.Of<IUpdater>())
            .BuildServiceProvider();

        var unknown = Should.Throw<InvalidOperationException>(() =>
            services.GetUpdater<IBarBuildUpdater>("missing"));
        var unsupported = Should.Throw<InvalidOperationException>(() =>
            services.GetUpdater<IBarBuildUpdater>("unsupported"));

        unknown.Message.ShouldContain("No updater registered for 'missing'");
        unsupported.Message.ShouldContain("Updater 'unsupported' does not support IBarBuildUpdater");
    }
}
