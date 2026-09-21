// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Octokit;
using Octokit.Internal;

namespace UpdateDependencies.Tests;

public sealed class ManifestUpdaterTests
{
    [Fact]
    public async Task UpdateBatch_SavesCompletedUpdateBeforeRunningEachGeneratorOnce()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        File.WriteAllText(manifestPath, """{"variables":{"version":"old"}}""");

        // Lightweight stand-ins for the generators assert that disk already contains the edit.
        WriteGenerators(repo.LocalPath, """
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            if ($manifest.variables.version -ne 'new') { throw 'Manifest was not saved before generation' }
            """);

        var completeUpdate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task apply = DependencyUpdateRunner.ApplyAsync(repo.LocalPath, async (variables, _, token) =>
        {
            await completeUpdate.Task.WaitAsync(token);
            variables.SetValue("version", "new");
        }, TestContext.Current.CancellationToken);
        completeUpdate.SetResult();
        await apply;

        File.ReadAllLines(Path.Combine(repo.LocalPath, "dockerfile-generations.txt")).ShouldBe(["generated"]);
        File.ReadAllLines(Path.Combine(repo.LocalPath, "readme-generations.txt")).ShouldBe(["generated"]);
    }

    [Fact]
    public async Task UpdateBatch_DoesNotRewriteUnchangedManifest()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        File.WriteAllText(manifestPath, """{"variables":{"version":"same"}}""");
        File.SetLastWriteTimeUtc(manifestPath, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        DateTime originalWriteTime = File.GetLastWriteTimeUtc(manifestPath);
        WriteGenerators(repo.LocalPath, "exit 0");

        await DependencyUpdateRunner.ApplyAsync(repo.LocalPath, (variables, _, _) =>
        {
            variables.SetValue("version", "same");
            return Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
    }

    [Fact]
    public async Task UpdateBatch_RunsGeneratorsEvenWhenManifestIsUnchanged()
    {
        using var repo = new TempRepo();
        File.WriteAllText(Path.Combine(repo.LocalPath, "manifest.versions.json"), """{"variables":{}}""");
        WriteGenerators(repo.LocalPath, "");

        await DependencyUpdateRunner.ApplyAsync(
            repo.LocalPath, (_, _, _) => Task.CompletedTask, TestContext.Current.CancellationToken);

        File.ReadAllLines(Path.Combine(repo.LocalPath, "dockerfile-generations.txt")).ShouldBe(["generated"]);
        File.ReadAllLines(Path.Combine(repo.LocalPath, "readme-generations.txt")).ShouldBe(["generated"]);
    }

    [Fact]
    public async Task UpdateBatch_RunsGeneratorsInWorkspaceWithSpaces()
    {
        using var repo = new TempRepo();
        string repoRoot = Path.Combine(repo.LocalPath, "workspace with spaces");
        Directory.CreateDirectory(repoRoot);
        File.WriteAllText(Path.Combine(repoRoot, "manifest.versions.json"), """{"variables":{}}""");
        WriteGenerators(repoRoot, "Set-Content ./generator-directory.txt (Get-Location).Path");
        string workingDirectory = Directory.GetCurrentDirectory();

        await DependencyUpdateRunner.ApplyAsync(
            repoRoot, (_, _, _) => Task.CompletedTask, TestContext.Current.CancellationToken);

        File.ReadAllText(Path.Combine(repoRoot, "generator-directory.txt")).Trim().ShouldBe(repoRoot);
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
        DependencyUpdate update = await updater.ResolveFromGitHubReleaseAsync(TestContext.Current.CancellationToken);
        await Should.ThrowAsync<InvalidOperationException>(() =>
            update.ApplyAsync(variables, "", TestContext.Current.CancellationToken));

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
        await (await updater.ResolveFromGitHubReleaseAsync(TestContext.Current.CancellationToken))
            .ApplyAsync(variables, "", TestContext.Current.CancellationToken);

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
        File.WriteAllText(Path.Combine(dockerfileDirectory, "Get-GeneratedDockerfiles.ps1"),
            script + "\nAdd-Content ./dockerfile-generations.txt 'generated'");
        File.WriteAllText(Path.Combine(readmeDirectory, "Get-GeneratedReadmes.ps1"),
            script + "\nAdd-Content ./readme-generations.txt 'generated'");
    }
}
