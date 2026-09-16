// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Dotnet.Docker;
using Octokit;
using Octokit.Internal;

namespace UpdateDependencies.Tests;

public sealed class ManifestUpdaterTests
{
    [Fact]
    public async Task UpdateBatch_WritesBeforeGenerationAndSkipsUnchangedContent()
    {
        using var repo = new TempRepo();
        string repoRoot = Path.Combine(repo.LocalPath, "workspace with spaces");
        Directory.CreateDirectory(repoRoot);
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        const string original = """{"variables": { "branch":"nightly", "runtime|11.0|build-version" : "11.0.1" }}""";
        File.WriteAllText(manifestPath, original);

        // Lightweight stand-ins for the generators assert that disk already contains the edit.
        const string generator = """
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            if ($manifest.variables.'runtime|11.0|build-version' -ne '11.0.2') { throw 'Manifest was not saved before generation' }
            Set-Content ./generated.txt 'generated'
            """;
        WriteGenerators(repoRoot, generator);

        string workingDirectory = Directory.GetCurrentDirectory();

        Task ApplyAsync(ManifestVariables variables, string root, CancellationToken token)
        {
            variables.SetValue("runtime|11.0|build-version", "11.0.2");
            return Task.CompletedTask;
        }

        await DependencyUpdateRunner.ApplyAsync(repoRoot, ApplyAsync, TestContext.Current.CancellationToken);
        string expected = original.Replace("\"11.0.1\"", "\"11.0.2\"");
        byte[] expectedBytes = Encoding.UTF8.GetBytes(expected);
        File.ReadAllBytes(manifestPath).ShouldBe(expectedBytes);

        string generatedPath = Path.Combine(repoRoot, "generated.txt");
        File.Exists(generatedPath).ShouldBeTrue();
        File.Delete(generatedPath);
        File.SetLastWriteTimeUtc(manifestPath, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        DateTime originalWriteTime = File.GetLastWriteTimeUtc(manifestPath);

        await DependencyUpdateRunner.ApplyAsync(repoRoot, ApplyAsync, TestContext.Current.CancellationToken);

        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
        File.Exists(generatedPath).ShouldBeTrue();
        Directory.GetCurrentDirectory().ShouldBe(workingDirectory);
    }

    [Fact]
    public async Task UpdateBatch_UpdatesNuGetConfigWithoutManifestChanges()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        const string content = """
            {"variables":{
              "branch":"nightly",
              "sdk|11.0|build-version":"11.0.100",
              "sdk|11.0|product-version":"11.0.100"
            }}
            """;
        File.WriteAllText(manifestPath, content);
        File.SetLastWriteTimeUtc(manifestPath, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        DateTime originalWriteTime = File.GetLastWriteTimeUtc(manifestPath);
        string configDirectory = Path.Combine(repo.LocalPath, "tests", "Microsoft.DotNet.Docker.Tests", "TestAppArtifacts");
        Directory.CreateDirectory(configDirectory);
        string configPath = Path.Combine(configDirectory, "NuGet.config.internal");
        File.WriteAllText(configPath, "<configuration><packageSources /></configuration>");
        WriteGenerators(repo.LocalPath, "exit 0");

        await DependencyUpdateRunner.ApplyAsync(repo.LocalPath, (variables, root, token) =>
        {
            NuGetConfigUpdater.Update(variables, root, "11.0", "11.0.100", isInternal: true);
            return Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
        File.ReadAllText(manifestPath).ShouldBe(content);
        string config = File.ReadAllText(configPath);
        config.ShouldContain("11.0.100-shipping/nuget/v3/index.json");
        config.ShouldContain("%InternalAccessToken%");
    }

    [Fact]
    public async Task UpdateBatch_ReportsGeneratorFailure()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        File.WriteAllText(manifestPath, """{"variables":{}}""");
        WriteGenerators(repo.LocalPath, "exit 1");

        await Should.ThrowAsync<InvalidOperationException>(() =>
            DependencyUpdateRunner.ApplyAsync(repo.LocalPath, (_, _, _) => Task.CompletedTask, TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData("main", "")]
    [InlineData("nightly", ".nightly")]
    public void NuGetConfigUpdater_RemovesInternalFeedAndPreservesOtherVersionCredentials(string branch, string suffix)
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

    [Fact]
    public void ManifestEdits_SkipMissingKeysAndPreserveFormatting()
    {
        const string content = """{"variables": { "value" : "old" }, "value":"unrelated"}""";
        var variables = new ManifestVariables(content);

        variables.SetValue("missing", "new").ShouldBeFalse();
        variables.SetValue("value", "new").ShouldBeTrue();

        variables.Contains("missing").ShouldBeFalse();
        variables.Content.ShouldBe(content.Replace("\"old\"", "\"new\""));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("$(alias)", "$(alias)")]
    [InlineData("old", "new")]
    public async Task ToolUpdater_PreservesEmptyValuesAndAliases(string current, string expected)
    {
        var variables = new ManifestVariables("""{"variables":{"syft|version":"old","rocks-toolbox|latest|version":"unchanged"}}""");
        variables.SetValue("syft|version", current);
        var release = new SimpleJsonSerializer().Deserialize<Release>("""{"tag_name":"new"}""");
        var updater = new SyftUpdater(CreateReleaseClient(release));
        await updater.UpdateFromGitHubReleaseAsync(variables, TestContext.Current.CancellationToken);

        variables.GetRawValue("syft|version").ShouldBe(expected);
        variables.GetRawValue("rocks-toolbox|latest|version").ShouldBe("unchanged");
    }

    [Fact]
    public async Task MinGitUpdater_DoesNotResolveAssetsForDisabledVariables()
    {
        const string content = """{"variables":{"mingit|latest|x64|url":"$(alias)","mingit|latest|x64|sha":""}}""";
        var variables = new ManifestVariables(content);

        var updater = new MinGitUpdater(CreateReleaseClient(new Release()));
        await updater.UpdateFromGitHubReleaseAsync(variables, TestContext.Current.CancellationToken);

        variables.Content.ShouldBe(content);
    }

    [Fact]
    public async Task MinGitUpdater_UpdatesUrlAndChecksumFromTheSameAsset()
    {
        var variables = new ManifestVariables("""{"variables":{"mingit|latest|x64|url":"old","mingit|latest|x64|sha":"old"}}""");
        var release = new SimpleJsonSerializer().Deserialize<Release>("""
            {
              "body":"MinGit-2.50.0-64-bit.zip | abcdef123456",
              "assets":[
                {"name":"MinGit-2.50.0-64-bit.zip","browser_download_url":"https://example/mingit.zip"}
              ]
            }
            """);

        var updater = new MinGitUpdater(CreateReleaseClient(release));
        await updater.UpdateFromGitHubReleaseAsync(variables, TestContext.Current.CancellationToken);

        variables.GetRawValue("mingit|latest|x64|url").ShouldBe("https://example/mingit.zip");
        variables.GetRawValue("mingit|latest|x64|sha").ShouldBe("abcdef123456");
    }

    [Fact]
    public async Task MinGitUpdater_RejectsMissingChecksum()
    {
        var variables = new ManifestVariables("""{"variables":{"mingit|latest|x64|sha":"old"}}""");
        var release = new SimpleJsonSerializer().Deserialize<Release>("""
            {
              "body":"",
              "assets":[
                {"name":"MinGit-2.50.0-64-bit.zip","browser_download_url":"https://example/mingit.zip"}
              ]
            }
            """);

        var updater = new MinGitUpdater(CreateReleaseClient(release));
        await Should.ThrowAsync<InvalidOperationException>(() =>
            updater.UpdateFromGitHubReleaseAsync(variables, TestContext.Current.CancellationToken));

        variables.GetRawValue("mingit|latest|x64|sha").ShouldBe("old");
    }

    [Fact]
    public async Task ChiselUpdater_SelectsArchitectureAndPreservesDisabledVariables()
    {
        var variables = new ManifestVariables("""
            {"variables":{
              "chisel|latest|x64|url":"old",
              "chisel|latest|x64|sha384":"",
              "chisel|latest|arm|url":"$(alias)",
              "chisel|latest|arm64|url":""
            }}
            """);
        var release = new SimpleJsonSerializer().Deserialize<Release>("""
            {
              "assets":[
                {"name":"chisel_v1.0.0_linux_amd64.tar.gz","browser_download_url":"https://example/chisel.tar.gz"}
              ]
            }
            """);

        using var httpClient = new HttpClient();
        var updater = new ChiselUpdater(CreateReleaseClient(release), httpClient);
        await updater.UpdateFromGitHubReleaseAsync(variables, TestContext.Current.CancellationToken);

        variables.GetRawValue("chisel|latest|x64|url").ShouldBe("https://example/chisel.tar.gz");
        variables.GetRawValue("chisel|latest|x64|sha384").ShouldBe("");
        variables.GetRawValue("chisel|latest|arm|url").ShouldBe("$(alias)");
        variables.GetRawValue("chisel|latest|arm64|url").ShouldBe("");
    }

    [Fact]
    public async Task UpdateBatch_DoesNotSaveManifestWhenUpdaterFails()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        const string content = """
            {"variables":{
              "runtime|11.0|build-version":"11.0.1"
            }}
            """;
        File.WriteAllText(manifestPath, content);
        WriteGenerators(repo.LocalPath, "Set-Content ./generated.txt 'generated'");

        await Should.ThrowAsync<InvalidOperationException>(() =>
            DependencyUpdateRunner.ApplyAsync(repo.LocalPath,
                (variables, _, _) =>
                {
                    variables.SetValue("runtime|11.0|build-version", "11.0.2");
                    throw new InvalidOperationException("Updater failed.");
                },
                TestContext.Current.CancellationToken));
        File.ReadAllText(manifestPath).ShouldBe(content);
        File.Exists(Path.Combine(repo.LocalPath, "generated.txt")).ShouldBeFalse();
    }

    private static IReleasesClient CreateReleaseClient(Release release)
    {
        var client = new Mock<IReleasesClient>(MockBehavior.Strict);
        client.Setup(source => source.GetLatest(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(release);
        return client.Object;
    }

    private static void WriteGenerators(string repoRoot, string script)
    {
        string dockerfileDirectory = Path.Combine(repoRoot, "eng", "dockerfile-templates");
        string readmeDirectory = Path.Combine(repoRoot, "eng", "readme-templates");
        Directory.CreateDirectory(dockerfileDirectory);
        Directory.CreateDirectory(readmeDirectory);
        File.WriteAllText(Path.Combine(dockerfileDirectory, "Get-GeneratedDockerfiles.ps1"), script);
        File.WriteAllText(Path.Combine(readmeDirectory, "Get-GeneratedReadmes.ps1"), script);
    }
}
