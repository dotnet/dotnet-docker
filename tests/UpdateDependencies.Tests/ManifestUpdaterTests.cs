// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Dotnet.Docker;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

namespace UpdateDependencies.Tests;

public sealed class ManifestUpdaterTests
{
    [Fact]
    public async Task SpecificCommand_WritesBeforeGenerationAndSkipsUnchangedContent()
    {
        using var repo = new TempRepo();
        string manifestPath = Path.Combine(repo.LocalPath, "manifest.versions.json");
        const string original = """{"variables": { "value" : "old" }}""";
        File.WriteAllText(manifestPath, original);

        // Lightweight stand-ins for the generators assert that disk already contains the edit.
        string dockerfileDirectory = Path.Combine(repo.LocalPath, "eng", "dockerfile-templates");
        string readmeDirectory = Path.Combine(repo.LocalPath, "eng", "readme-templates");
        Directory.CreateDirectory(dockerfileDirectory);
        Directory.CreateDirectory(readmeDirectory);
        const string generator = """
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            if ($manifest.variables.value -ne 'new') { throw 'Manifest was not saved before generation' }
            """;
        File.WriteAllText(Path.Combine(dockerfileDirectory, "Get-GeneratedDockerfiles.ps1"), generator);
        File.WriteAllText(Path.Combine(readmeDirectory, "Get-GeneratedReadmes.ps1"), generator);

        var command = new SpecificCommand();
        command.VariableUpdates.Add(new VariableUpdateInfo("value", "new"));
        var options = new SpecificCommandOptions { RepoRoot = repo.LocalPath };

        int exitCode = await command.ExecuteAsync(options);

        exitCode.ShouldBe(0);
        string expected = original.Replace("\"old\"", "\"new\"");
        byte[] expectedBytes = Encoding.UTF8.GetBytes(expected);
        File.ReadAllBytes(manifestPath).ShouldBe(expectedBytes);

        // Change detection still uses VersionTools's git status check on no-op update passes.
        var cancellationToken = TestContext.Current.CancellationToken;
        await repo.LocalGitRepo.ExecuteGitCommand(["init"], cancellationToken: cancellationToken);
        await repo.LocalGitRepo.ExecuteGitCommand(["config", "user.name", "Test"], cancellationToken: cancellationToken);
        await repo.LocalGitRepo.ExecuteGitCommand(["config", "user.email", "test@example.com"], cancellationToken: cancellationToken);
        await repo.LocalGitRepo.StageAsync(["."], cancellationToken: cancellationToken);
        await repo.LocalGitRepo.CommitAsync("Baseline", allowEmpty: false, author: ("Test", "test@example.com"), cancellationToken: cancellationToken);
        File.SetLastWriteTimeUtc(manifestPath, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        DateTime originalWriteTime = File.GetLastWriteTimeUtc(manifestPath);

        int noOpExitCode = await command.ExecuteAsync(options);

        noOpExitCode.ShouldBe(0);
        File.GetLastWriteTimeUtc(manifestPath).ShouldBe(originalWriteTime);
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
        IDependencyInfo[] infos =
        [
            new VariableUpdateInfo("sdk", "11.0.100-preview.2.12345.2"),
            new VariableUpdateInfo("aspnet-composite", "11.0.0-preview.2.12345.2"),
        ];
        IDependencyUpdater[] updaters =
        [
            ..BaseUrlUpdater.CreateUpdaters(variables, options),
            new VersionUpdater(VersionType.Build, "sdk", "11.0", options, variables),
            new VersionUpdater(VersionType.Product, "sdk", "11.0", options, variables),
            new VersionUpdater(VersionType.Build, "aspnet-composite", "11.0", options, variables),
        ];

        DependencyUpdateUtils.Update(updaters, infos);

        variables.GetRawValue("sdk|11.0|build-version").ShouldBe("11.0.100-preview.2.12345.2");
        variables.GetRawValue("sdk|11.0|product-version").ShouldBe("11.0.100-preview.2");
        variables.GetRawValue("aspnet-composite|11.0|build-version").ShouldBe("$(sdk|11.0|build-version)");
        ManifestHelper.GetBaseUrls(variables, options).ShouldBe(["https://example/internal"]);

        string content = variables.Content;
        DependencyUpdateUtils.Update(updaters, infos).UsedInfos.ShouldBeEmpty();
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
        var options = new SpecificCommandOptions();
        var updater = new VersionUpdater(VersionType.Build, "runtime", "11.0", options, variables);
        var info = new VariableUpdateInfo("runtime", "11.0.2");

        DependencyUpdateUtils.Update([updater], [info]);

        variables.GetRawValue("runtime|11.0|build-version").ShouldBe(expected);
    }

    [Fact]
    public void VariableUpdater_SkipsMissingKeysAndPreservesFormatting()
    {
        const string content = """{"variables": { "value" : "old" }, "value":"unrelated"}""";
        var variables = new ManifestVariables(content);
        var missing = new VariableUpdateInfo("missing", "new");
        var update = new VariableUpdateInfo("value", "new");

        DependencyUpdateUtils.Update(
            [new VariableUpdater(variables, missing), new VariableUpdater(variables, update)],
            []);

        variables.Contains("missing").ShouldBeFalse();
        variables.Content.ShouldBe(content.Replace("\"old\"", "\"new\""));
    }

    [Theory]
    [InlineData("", "", false)]
    [InlineData("$(alias)", "$(alias)", false)]
    [InlineData("old", "new", true)]
    public void ToolUpdater_PreservesEmptyValuesAndAliases(string current, string expected, bool resolvesRelease)
    {
        var variables = new ManifestVariables("""{"variables":{"tool":"old"}}""");
        variables.SetValue("tool", current);
        var updater = new ReleaseUpdater(variables);
        var info = new GitHubReleaseInfo("tool", new Release());

        DependencyUpdateUtils.Update([updater], [info]);

        variables.GetRawValue("tool").ShouldBe(expected);
        updater.ResolvedRelease.ShouldBe(resolvesRelease);
    }

    [Fact]
    public void ToolUpdater_DoesNotResolveAnUnrequestedTool()
    {
        const string content = """{"variables":{"tool":"old"}}""";
        var variables = new ManifestVariables(content);
        var updater = new ReleaseUpdater(variables);

        DependencyUpdateUtils.Update([updater], []);

        updater.ResolvedRelease.ShouldBeFalse();
        variables.Content.ShouldBe(content);
    }

    [Fact]
    public void ShaUpdater_SelectsExactProductAndVersionKeysFromEditor()
    {
        var variables = new ManifestVariables("""
            {"variables":{
              "runtime|11.0|build-version":"11.0.1",
              "runtime|11.0|linux|x64|sha":"old",
              "runtime|11.0|linux|arm64|sha":"old",
              "runtime|11.0|linux|x64|sha384":"other",
              "runtime|11.01|linux|x64|sha":"other",
              "sdk|11.0|linux|x64|sha":"other"
            }}
            """);

        var updaters = DockerfileShaUpdater.CreateUpdaters(
            "runtime", "11.0", new SpecificCommandOptions(), variables);

        updaters.Count().ShouldBe(2);
    }

    private sealed class ReleaseUpdater(ManifestVariables variables)
        : GitHubReleaseUpdaterBase(variables, "tool", "tool", "owner", "repo")
    {
        public bool ResolvedRelease { get; private set; }

        protected override string GetValue(GitHubReleaseInfo dependencyInfo)
        {
            ResolvedRelease = true;
            return "new";
        }
    }
}
