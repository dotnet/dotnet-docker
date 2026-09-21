// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class MonitorUpdaterTests
{
    [Theory]
    [InlineData("9.0.5-servicing.25556.2", "9.0.5")]
    [InlineData("9.0.0-rtm.12345.1", "9.0.0")]
    [InlineData("9.0.3-preview.1.26303.1", "9.0.3-preview.1")]
    [InlineData("9.0.5", "9.0.5")]
    public async Task Version_WritesBuildVersionAndProductTag(string version, string productVersion)
    {
        var variables = new ManifestVariables("""
            {
                "variables": {
                    "branch": "nightly",
                    "monitor|9.0|build-version": "9.0.1",
                    "monitor|9.0|product-version": "9.0.1"
                }
            }
            """);
        using var httpClient = new HttpClient();
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync(version, TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor|9.0|build-version").ShouldBe(version);
        variables.GetRawValue("monitor|9.0|product-version").ShouldBe(productVersion);
    }

    [Fact]
    public async Task Version_PreservesAliasesAndOtherVersionChecksums()
    {
        var variables = new ManifestVariables("""
            {
                "variables": {
                    "branch": "nightly",
                    "monitor|9.0|build-version": "9.0.1",
                    "monitor|9.0|product-version": "9.0.1",
                    "monitor-base|9.0|build-version": "$(monitor|9.0|build-version)",
                    "monitor-base|9.0|product-version": "$(monitor|9.0|product-version)",
                    "monitor|8.0|linux|x64|sha": "unrelated"
                }
            }
            """);
        using var httpClient = new HttpClient();
        var updater = CreateUpdater(httpClient);

        DependencyUpdate update = await updater.ResolveFromVersionAsync("9.0.5", TestContext.Current.CancellationToken);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor-base|9.0|build-version").ShouldBe("$(monitor|9.0|build-version)");
        variables.GetRawValue("monitor-base|9.0|product-version").ShouldBe("$(monitor|9.0|product-version)");
        variables.GetRawValue("monitor|8.0|linux|x64|sha").ShouldBe("unrelated");
    }

    private static MonitorUpdater CreateUpdater(HttpClient httpClient) =>
        new(
            Mock.Of<IPipelineArtifactProvider>(),
            httpClient,
            new UpdateDependenciesConfiguration(),
            Mock.Of<ILogger<MonitorUpdater>>());
}
