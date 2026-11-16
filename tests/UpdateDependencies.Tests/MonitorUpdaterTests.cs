// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class MonitorUpdaterTests
{
    [Theory]
    [InlineData("10.0.5-servicing.25556.2", "10.0.5")]
    [InlineData("10.0.0-rtm.12345.1", "10.0.0")]
    [InlineData("10.0.3-preview.1.26303.1", "10.0.3-preview.1")]
    [InlineData("10.0.5", "10.0.5")]
    public async Task Version_WritesBuildVersionAndProductTag(string version, string productVersion)
    {
        ManifestVariables variables = new ManifestVariables("""
            {
                "variables": {
                    "branch": "nightly",
                    "monitor|10.0|build-version": "10.0.1",
                    "monitor|10.0|product-version": "10.0.1"
                }
            }
            """);
        MonitorUpdater updater = CreateUpdater();

        DependencyUpdate update = updater.ResolveFromVersion(version);
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor|10.0|build-version").ShouldBe(version);
        variables.GetRawValue("monitor|10.0|product-version").ShouldBe(productVersion);
    }

    [Fact]
    public async Task Version_PreservesAliases()
    {
        ManifestVariables variables = new ManifestVariables("""
            {
                "variables": {
                    "branch": "nightly",
                    "monitor|10.0|build-version": "10.0.1",
                    "monitor|10.0|product-version": "10.0.1",
                    "monitor-base|10.0|build-version": "$(monitor|10.0|build-version)",
                    "monitor-base|10.0|product-version": "$(monitor|10.0|product-version)"
                }
            }
            """);
        MonitorUpdater updater = CreateUpdater();

        DependencyUpdate update = updater.ResolveFromVersion("10.0.5");
        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);

        variables.GetRawValue("monitor-base|10.0|build-version").ShouldBe("$(monitor|10.0|build-version)");
        variables.GetRawValue("monitor-base|10.0|product-version").ShouldBe("$(monitor|10.0|product-version)");
    }

    private static MonitorUpdater CreateUpdater() =>
        new(
            Mock.Of<IPipelineArtifactProvider>(),
            new UpdateDependenciesConfiguration(),
            Mock.Of<ILogger<MonitorUpdater>>());
}
