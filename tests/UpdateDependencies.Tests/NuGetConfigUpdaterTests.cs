// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

namespace UpdateDependencies.Tests;

public sealed class NuGetConfigUpdaterTests
{
    [Theory]
    [InlineData("main", "")]
    [InlineData("nightly", ".nightly")]
    public void Update_RemovesInternalFeedAndPreservesOtherVersionCredentials(string branch, string suffix)
    {
        using var repo = new TempRepo();
        var variables = new ManifestVariables("""{"variables":{"branch":"nightly"}}""");
        variables.SetValue("branch", branch);
        string configDirectory = Path.Combine(repo.LocalPath, "tests", "Microsoft.DotNet.Docker.Tests", "TestAppArtifacts");
        Directory.CreateDirectory(configDirectory);
        string configPath = Path.Combine(configDirectory, $"NuGet.config{suffix}");
        File.WriteAllText(configPath, """
            <configuration>
              <packageSources>
                <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
                <add key="dotnet11_0_internal" value="https://example/internal" />
              </packageSources>
              <packageSourceCredentials>
                <dotnet10_0_internal>
                  <add key="Username" value="dotnet" />
                </dotnet10_0_internal>
                <dotnet11_0_internal>
                  <add key="Username" value="dotnet" />
                </dotnet11_0_internal>
              </packageSourceCredentials>
            </configuration>
            """);

        NuGetConfigUpdater.Update(variables, repo.LocalPath, "11.0", "11.0.100", isInternal: false);

        string config = File.ReadAllText(configPath);
        config.ShouldContain("https://api.nuget.org/v3/index.json");
        config.ShouldContain("dotnet10_0_internal");
        config.ShouldNotContain("dotnet11_0_internal");
    }
}
