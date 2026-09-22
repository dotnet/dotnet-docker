// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.DotNet.Docker.UpdateDependencies.Model.Release;
using Microsoft.DotNet.DarcLib;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class DotNetUpdaterTests
{
    [Theory]
    [InlineData("11.0.2", "11.0.2")]
    [InlineData("11.0.2-servicing.12345.1", "11.0.2")]
    [InlineData("11.0.2-rtm.12345.1", "11.0.2")]
    [InlineData("11.0.2-preview.2.12345.1", "11.0.2-preview.2")]
    [InlineData("11.0.2-rc.1.12345.1", "11.0.2-rc.1")]
    [InlineData("11.0.2+build.123", "11.0.2")]
    public async Task Staging_WritesBuildVersionAndProductTag(string buildVersion, string productVersion)
    {
        using var repo = new TempRepo();
        var variables = CreateManifest(repo.LocalPath);
        var release = CreateRelease() with { RuntimeBuild = buildVersion };

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, release, "https://example/internal", TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(buildVersion);
        variables.GetRawValue("dotnet|11.0|product-version").ShouldBe(productVersion);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("$(alias)", "$(alias)")]
    [InlineData("not-a-version", "not-a-version")]
    [InlineData("11.0.1", "11.0.2")]
    public async Task Staging_PreservesDisabledVersionsAndAliases(string current, string expected)
    {
        using var repo = new TempRepo();
        var variables = CreateManifest(repo.LocalPath);
        variables.SetValue("runtime|11.0|build-version", current);

        await CreateUpdater().UpdateFromStagingPipelineAsync(
            variables, repo.LocalPath, CreateRelease(), "", TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(expected);
    }

    private static DotNetUpdater CreateUpdater() =>
        new DotNetUpdater(
            barClient: Mock.Of<IBasicBarClient>(),
            logger: Mock.Of<ILogger<DotNetUpdater>>());

    private static ManifestVariables CreateManifest(string repoRoot)
    {
        const string content = """
            {
                "variables": {
                    "branch": "nightly",
                    "dotnet|11.0|product-version": "11.0.1",
                    "runtime|11.0|build-version": "11.0.1"
                }
            }
            """;

        string configDirectory = Path.Combine(repoRoot, "tests", "Microsoft.DotNet.Docker.Tests", "TestAppArtifacts");
        Directory.CreateDirectory(configDirectory);
        File.WriteAllText(Path.Combine(configDirectory, "NuGet.config.nightly"), "<configuration />");
        File.WriteAllText(Path.Combine(configDirectory, "NuGet.config.internal"), "<configuration />");
        return new ManifestVariables(content);
    }

    private static ReleaseConfig CreateRelease() => new()
    {
        Channel = "11.0",
        MajorVersion = "11",
        Release = "11.0.2",
        Runtime = "11.0.2",
        RuntimeBuild = "11.0.2-servicing.12345.1",
        Sdks = ["11.0.100"],
        SdkBuilds = ["11.0.100-servicing.12345.1"],
        Asp = "11.0.3",
        AspBuild = "11.0.3-servicing.12345.1",
        Security = false,
        SupportPhase = "active",
        Internal = false,
        SdkOnly = false,
    };
}
