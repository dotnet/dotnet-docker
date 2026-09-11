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
    public async Task SpecificCommand_WritesBeforeGenerationAndSkipsUnchangedContent()
    {
        using var repo = new TempRepo();
        string repoRoot = Path.Combine(repo.LocalPath, "workspace with spaces");
        Directory.CreateDirectory(repoRoot);
        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        const string original = """{"variables": { "value" : "old" }}""";
        File.WriteAllText(manifestPath, original);

        // Lightweight stand-ins for the generators assert that disk already contains the edit.
        const string generator = """
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            if ($manifest.variables.value -ne 'new') { throw 'Manifest was not saved before generation' }
            Set-Content ./generated.txt 'generated'
            """;
        WriteGenerators(repoRoot, generator);

        var command = new SpecificCommand();
        command.VariableUpdates.Add(new VariableUpdateInfo("value", "new"));
        var options = new SpecificCommandOptions { RepoRoot = repoRoot };
        string workingDirectory = Directory.GetCurrentDirectory();

        int exitCode = await command.ExecuteAsync(options);

        exitCode.ShouldBe(0);
        string expected = original.Replace("\"old\"", "\"new\"");
        byte[] expectedBytes = Encoding.UTF8.GetBytes(expected);
        File.ReadAllBytes(manifestPath).ShouldBe(expectedBytes);

        string generatedPath = Path.Combine(repoRoot, "generated.txt");
        File.Exists(generatedPath).ShouldBeTrue();
        File.Delete(generatedPath);
        File.SetLastWriteTimeUtc(manifestPath, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        DateTime originalWriteTime = File.GetLastWriteTimeUtc(manifestPath);

        int noOpExitCode = await command.ExecuteAsync(options);

        noOpExitCode.ShouldBe(0);
        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
        File.Exists(generatedPath).ShouldBeTrue();
        Directory.GetCurrentDirectory().ShouldBe(workingDirectory);
    }

    [Fact]
    public async Task SpecificCommand_UpdatesNuGetConfigWithoutManifestChanges()
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
        var options = new SpecificCommandOptions
        {
            RepoRoot = repo.LocalPath,
            DockerfileVersion = "11.0",
            InternalBaseUrl = "https://example/internal",
            ProductVersions = new Dictionary<string, string?> { ["sdk"] = "11.0.100" },
        };

        int exitCode = await new SpecificCommand().ExecuteAsync(options);

        exitCode.ShouldBe(0);
        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
        File.ReadAllText(manifestPath).ShouldBe(content);
        string config = File.ReadAllText(configPath);
        config.ShouldContain("11.0.100-shipping/nuget/v3/index.json");
        config.ShouldContain("%InternalAccessToken%");
    }

    [Fact]
    public async Task SpecificCommand_ReportsGeneratorFailure()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        File.WriteAllText(manifestPath, """{"variables":{}}""");
        WriteGenerators(repo.LocalPath, "exit 1");

        int exitCode = await new SpecificCommand().ExecuteAsync(new SpecificCommandOptions { RepoRoot = repo.LocalPath });

        exitCode.ShouldBe(1);
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
        var options = new SpecificCommandOptions
        {
            RepoRoot = repo.LocalPath,
            DockerfileVersion = "11.0",
            ProductVersions = new Dictionary<string, string?> { ["sdk"] = "11.0.100" },
        };

        NuGetConfigUpdater.Update(variables, options);

        string config = File.ReadAllText(configPath);
        config.ShouldContain("https://api.nuget.org/v3/index.json");
        config.ShouldContain("dotnet10_0_internal");
        config.ShouldNotContain("dotnet11_0_internal");
    }

    [Fact]
    public void ProductUpdates_ShareEditorAndPreserveAliases()
    {
        var variables = new ManifestVariables("""
            {"variables":{
              "branch":"nightly",
              "sdk|11.0|build-version":"11.0.100-preview.1.12345.1",
              "sdk|11.0|product-version":"11.0.100-preview.1",
              "aspnet-composite|11.0|build-version":"$(sdk|11.0|build-version)",
              "dotnet|11.0|base-url|nightly":"$(public-url)",
              "public-url":"https://example/public"
            }}
            """);
        var options = new SpecificCommandOptions
        {
            DockerfileVersion = "11.0",
            InternalBaseUrl = "https://example/internal",
        };

        BaseUrlUpdater.Update(variables, options);
        VersionUpdater.Update(variables, "sdk", "11.0.100-preview.2.12345.2", options);
        VersionUpdater.Update(variables, "aspnet-composite", "11.0.0-preview.2.12345.2", options);

        variables.GetRawValue("sdk|11.0|build-version").ShouldBe("11.0.100-preview.2.12345.2");
        variables.GetRawValue("sdk|11.0|product-version").ShouldBe("11.0.100-preview.2");
        variables.GetRawValue("aspnet-composite|11.0|build-version").ShouldBe("$(sdk|11.0|build-version)");
        ManifestHelper.GetBaseUrls(variables, options).ShouldBe(["https://example/internal"]);

        string content = variables.Content;
        BaseUrlUpdater.Update(variables, options);
        VersionUpdater.Update(variables, "sdk", "11.0.100-preview.2.12345.2", options);
        VersionUpdater.Update(variables, "aspnet-composite", "11.0.0-preview.2.12345.2", options);
        variables.Content.ShouldBe(content);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("$(alias)", "$(alias)")]
    [InlineData("not-a-version", "not-a-version")]
    [InlineData("11.0.1", "11.0.2")]
    public void VersionUpdater_PreservesValueSelection(string current, string expected)
    {
        var variables = new ManifestVariables("""{"variables":{"runtime|11.0|build-version":"11.0.1"}}""");
        variables.SetValue("runtime|11.0|build-version", current);
        var options = new SpecificCommandOptions { DockerfileVersion = "11.0" };

        VersionUpdater.Update(variables, "runtime", "11.0.2", options);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(expected);
    }

    [Fact]
    public void VariableUpdater_SkipsMissingKeysAndPreservesFormatting()
    {
        const string content = """{"variables": { "value" : "old" }, "value":"unrelated"}""";
        var variables = new ManifestVariables(content);

        VariableUpdater.Update(variables, "missing", "new");
        VariableUpdater.Update(variables, "value", "new");

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
        var info = new GitHubReleaseInfo(SyftUpdater.ToolName, release);

        await Tools.UpdateAsync(variables, info, TestContext.Current.CancellationToken);

        variables.GetRawValue("syft|version").ShouldBe(expected);
        variables.GetRawValue("rocks-toolbox|latest|version").ShouldBe("unchanged");
    }

    [Fact]
    public void MinGitUpdater_DoesNotResolveAssetsForDisabledVariables()
    {
        const string content = """{"variables":{"mingit|latest|x64|url":"$(alias)","mingit|latest|x64|sha":""}}""";
        var variables = new ManifestVariables(content);

        MinGitUpdater.Update(variables, new Release());

        variables.Content.ShouldBe(content);
    }

    [Fact]
    public void MinGitUpdater_UpdatesUrlAndChecksumFromTheSameAsset()
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

        MinGitUpdater.Update(variables, release);

        variables.GetRawValue("mingit|latest|x64|url").ShouldBe("https://example/mingit.zip");
        variables.GetRawValue("mingit|latest|x64|sha").ShouldBe("abcdef123456");
    }

    [Fact]
    public void MinGitUpdater_RejectsMissingChecksum()
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

        Should.Throw<InvalidOperationException>(() => MinGitUpdater.Update(variables, release));

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

        await ChiselUpdater.UpdateAsync(variables, release, TestContext.Current.CancellationToken);

        variables.GetRawValue("chisel|latest|x64|url").ShouldBe("https://example/chisel.tar.gz");
        variables.GetRawValue("chisel|latest|x64|sha384").ShouldBe("");
        variables.GetRawValue("chisel|latest|arm|url").ShouldBe("$(alias)");
        variables.GetRawValue("chisel|latest|arm64|url").ShouldBe("");
    }

    [Fact]
    public async Task ShaUpdater_SelectsExactKeysAndUsesUpdatedManifest()
    {
        using var repo = new TempRepo();
        string checksumPath = Path.Combine(repo.LocalPath, "checksums.txt");
        File.WriteAllText(checksumPath, """
            AAAA dotnet-runtime-11.0.2-linux-x64.tar.gz
            BBBB dotnet-runtime-11.0.2-linux-arm64.tar.gz
            """);
        var variables = new ManifestVariables("""
            {"variables":{
              "branch":"nightly",
              "dotnet|11.0|base-url|nightly":"$(public-url)",
              "public-url":"https://example/public",
              "runtime|11.0|build-version":"11.0.1",
              "runtime|11.0|linux|x64|sha":"old",
              "runtime|11.0|linux|arm64|sha":"old",
              "runtime|11.0|linux|x64|sha384":"other",
              "runtime|11.01|linux|x64|sha":"other",
              "sdk|11.0|linux|x64|sha":"other"
            }}
            """);
        var options = new SpecificCommandOptions
        {
            DockerfileVersion = "11.0",
            ChecksumsFile = checksumPath,
            InternalBaseUrl = $"https://example/{Guid.NewGuid()}",
        };

        BaseUrlUpdater.Update(variables, options);
        VersionUpdater.Update(variables, "runtime", "11.0.2", options);
        var updater = new DockerfileShaUpdater("runtime", options, variables);
        await updater.UpdateAsync(TestContext.Current.CancellationToken);

        variables.GetRawValue("runtime|11.0|linux|x64|sha").ShouldBe("aaaa");
        variables.GetRawValue("runtime|11.0|linux|arm64|sha").ShouldBe("bbbb");
        variables.GetRawValue("runtime|11.0|linux|x64|sha384").ShouldBe("other");
        variables.GetRawValue("runtime|11.01|linux|x64|sha").ShouldBe("other");
        variables.GetRawValue("sdk|11.0|linux|x64|sha").ShouldBe("other");
    }

    [Fact]
    public async Task ShaUpdater_SkipsProductsWithoutChecksumVariables()
    {
        const string content = """{"variables":{}}""";
        var variables = new ManifestVariables(content);
        var updater = new DockerfileShaUpdater("runtime", new SpecificCommandOptions(), variables);

        await updater.UpdateAsync(TestContext.Current.CancellationToken);

        variables.Content.ShouldBe(content);
    }

    [Fact]
    public async Task SpecificCommand_DoesNotSaveManifestWhenChecksumResolutionFails()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        const string content = """
            {"variables":{
              "branch":"nightly",
              "dotnet|11.0|base-url|nightly":"old",
              "runtime|11.0|build-version":"11.0.1",
              "runtime|11.0|linux|x64|sha":"old"
            }}
            """;
        File.WriteAllText(manifestPath, content);
        string checksumPath = Path.Combine(repo.LocalPath, "checksums.txt");
        File.WriteAllText(checksumPath, "invalid checksum entry");
        WriteGenerators(repo.LocalPath, "Set-Content ./generated.txt 'generated'");
        var options = new SpecificCommandOptions
        {
            RepoRoot = repo.LocalPath,
            DockerfileVersion = "11.0",
            ChecksumsFile = checksumPath,
            InternalBaseUrl = $"https://example/{Guid.NewGuid()}",
            ProductVersions = new Dictionary<string, string?> { ["runtime"] = "11.0.2" },
        };

        int exitCode = await new SpecificCommand().ExecuteAsync(options);

        exitCode.ShouldBe(1);
        File.ReadAllText(manifestPath).ShouldBe(content);
        File.Exists(Path.Combine(repo.LocalPath, "generated.txt")).ShouldBeFalse();
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
