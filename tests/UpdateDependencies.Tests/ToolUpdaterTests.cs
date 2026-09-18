// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text.Json;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Octokit;
using Octokit.Internal;

namespace UpdateDependencies.Tests;

public sealed class ToolUpdaterTests
{
    [Theory]
    [InlineData("syft", "anchore", "syft", "syft|version", "v1.0.0")]
    [InlineData("rocks-toolbox", "canonical", "rocks-toolbox", "rocks-toolbox|latest|version", "v1.0.0")]
    [InlineData("chisel", "canonical", "chisel", "chisel|latest|x64|url", "https://example/chisel-amd64.tar.gz")]
    [InlineData("mingit", "git-for-windows", "git", "mingit|latest|x64|url", "https://example/mingit64.zip")]
    public async Task Updater_FetchesLatestOnEachCallAndOnlyEditsSuppliedManifest(
        string tool,
        string owner,
        string repo,
        string variable,
        string expected)
    {
        using var workspace = new Workspace();
        const string onDisk = """{"variables":{"sentinel":"unchanged"}}""";
        File.WriteAllText(workspace.ManifestPath, onDisk);
        var variables = CreateVariables((variable, "old"));
        var releases = new Mock<IReleasesClient>(MockBehavior.Strict);
        CancellationToken token = TestContext.Current.CancellationToken;
        releases.Setup(source => source.GetLatest(owner, repo)).ReturnsAsync(CreateRelease());
        using var handler = new ChecksumHandler([]);
        using var httpClient = new HttpClient(handler);
        IGitHubReleaseUpdater updater = CreateUpdater(tool, releases.Object, httpClient);

        await (await updater.ResolveFromGitHubReleaseAsync(token)).ApplyAsync(variables, workspace.Root, token);

        variables.GetRawValue(variable).ShouldBe(expected);
        var secondVariables = CreateVariables((variable, "old"));
        await (await updater.ResolveFromGitHubReleaseAsync(token)).ApplyAsync(secondVariables, workspace.Root, token);

        secondVariables.GetRawValue(variable).ShouldBe(expected);
        releases.Verify(source => source.GetLatest(owner, repo), Times.Exactly(2));
        File.ReadAllText(workspace.ManifestPath).ShouldBe(onDisk);
        Directory.GetFileSystemEntries(workspace.Root).ShouldBe([workspace.ManifestPath]);
        handler.Requests.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("syft", "syft|version", "unused")]
    [InlineData("rocks-toolbox", "rocks-toolbox|latest|version", "unused")]
    [InlineData("chisel", "chisel|latest|x64|url", "chisel|latest|x64|sha384")]
    [InlineData("mingit", "mingit|latest|x64|url", "mingit|latest|x64|sha")]
    public async Task Updater_PreservesMissingEmptyAndAliasVariablesWithoutResolvingAssets(
        string tool,
        string firstVariable,
        string secondVariable)
    {
        var releases = new Mock<IReleasesClient>();
        releases.Setup(source => source.GetLatest(
            It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Release());
        using var handler = new ChecksumHandler([]);
        using var httpClient = new HttpClient(handler);
        IGitHubReleaseUpdater updater = CreateUpdater(tool, releases.Object, httpClient);
        CancellationToken token = TestContext.Current.CancellationToken;
        DependencyUpdate update = await updater.ResolveFromGitHubReleaseAsync(token);

        foreach (string? value in new string?[] { null, "", "$(alias)" })
        {
            ManifestVariables variables = value is null
                ? new ManifestVariables("""{"variables":{}}""")
                : CreateVariables((firstVariable, value), (secondVariable, value));
            string original = variables.Content;

            await update.ApplyAsync(variables, "", token);

            variables.Content.ShouldBe(original);
        }

        handler.Requests.ShouldBeEmpty();
    }

    [Fact]
    public async Task ChiselUpdater_SelectsMatchingChecksumsAndSkipsDisabledChecksumDownloads()
    {
        var variables = CreateVariables(
            ("chisel|latest|x64|url", "old"),
            ("chisel|latest|x64|sha384", "old"),
            ("chisel|latest|arm|url", "$(alias)"),
            ("chisel|latest|arm|sha384", "old"),
            ("chisel|latest|arm64|url", "old"),
            ("chisel|latest|arm64|sha384", ""));
        var releases = new Mock<IReleasesClient>(MockBehavior.Strict);
        releases.Setup(source => source.GetLatest(
            "canonical", "chisel")).ReturnsAsync(CreateRelease());
        using var handler = new ChecksumHandler(new Dictionary<string, string>
        {
            ["https://example/chisel-amd64.tar.gz.sha384"] = "ABCDEF1234  chisel_v1.0.0_linux_amd64.tar.gz\n",
            ["https://example/chisel-arm.tar.gz.sha384"] = "FEDCBA5678  chisel_v1.0.0_linux_arm.tar.gz\n",
        });
        using var httpClient = new HttpClient(handler);
        var updater = new ChiselUpdater(releases.Object, httpClient);
        CancellationToken token = TestContext.Current.CancellationToken;

        await (await updater.ResolveFromGitHubReleaseAsync(token)).ApplyAsync(variables, "", token);

        variables.GetRawValue("chisel|latest|x64|url").ShouldBe("https://example/chisel-amd64.tar.gz");
        variables.GetRawValue("chisel|latest|x64|sha384").ShouldBe("abcdef1234");
        variables.GetRawValue("chisel|latest|arm|url").ShouldBe("$(alias)");
        variables.GetRawValue("chisel|latest|arm|sha384").ShouldBe("fedcba5678");
        variables.GetRawValue("chisel|latest|arm64|url").ShouldBe("https://example/chisel-arm64.tar.gz");
        variables.GetRawValue("chisel|latest|arm64|sha384").ShouldBe("");
        handler.Requests.ShouldBe([
            "https://example/chisel-amd64.tar.gz.sha384",
            "https://example/chisel-arm.tar.gz.sha384",
        ]);
    }

    [Fact]
    public async Task MinGitUpdater_SelectsChecksumForSelected64BitAsset()
    {
        var variables = CreateVariables(
            ("mingit|latest|x64|url", "old"),
            ("mingit|latest|x64|sha", "old"));
        var releases = new Mock<IReleasesClient>(MockBehavior.Strict);
        releases.Setup(source => source.GetLatest(
            "git-for-windows", "git")).ReturnsAsync(CreateRelease());
        var updater = new MinGitUpdater(releases.Object);
        CancellationToken token = TestContext.Current.CancellationToken;

        await (await updater.ResolveFromGitHubReleaseAsync(token)).ApplyAsync(variables, "", token);

        variables.GetRawValue("mingit|latest|x64|url").ShouldBe("https://example/mingit64.zip");
        variables.GetRawValue("mingit|latest|x64|sha").ShouldBe("abcdef123456");
    }

    [Fact]
    public async Task UpdateBatch_UsesOneEditorAndSavesBeforeGeneratingOnce()
    {
        using var workspace = new Workspace();
        const string original = """{"variables": { "first" : "old", "second" : "old" }}""";
        File.WriteAllText(workspace.ManifestPath, original);
        workspace.WriteGenerators("""
            $manifest = Get-Content ./manifest.versions.json -Raw | ConvertFrom-Json
            if ($manifest.variables.first -ne 'new-first') { throw 'First edit was not saved' }
            if ($manifest.variables.second -ne 'new-second') { throw 'Second edit was not saved' }
            """);
        ManifestVariables? sharedVariables = null;
        var calls = new List<string>();
        var first = new CallbackUpdater(async (variables, cancellationToken) =>
        {
            sharedVariables = variables;
            variables.SetValue("first", "new-first");
            await Task.Yield();
            calls.Add("first");
        });
        var second = new CallbackUpdater((variables, cancellationToken) =>
        {
            variables.ShouldBeSameAs(sharedVariables);
            variables.GetRawValue("first").ShouldBe("new-first");
            File.ReadAllText(workspace.ManifestPath).ShouldBe(original);
            calls.ShouldBe(["first"]);
            variables.SetValue("second", "new-second");
            calls.Add("second");
            return Task.CompletedTask;
        });
        CancellationToken token = TestContext.Current.CancellationToken;
        DependencyUpdate firstUpdate = await first.ResolveFromGitHubReleaseAsync(token);
        DependencyUpdate secondUpdate = await second.ResolveFromGitHubReleaseAsync(token);
        await DependencyUpdateRunner.ApplyAsync(workspace.Root, async (variables, repoRoot, applyToken) =>
        {
            await firstUpdate.ApplyAsync(variables, repoRoot, applyToken);
            await secondUpdate.ApplyAsync(variables, repoRoot, applyToken);
        }, token);

        calls.ShouldBe(["first", "second"]);
        File.ReadAllText(workspace.ManifestPath).ShouldBe(
            original.Replace("\"first\" : \"old\"", "\"first\" : \"new-first\"")
                .Replace("\"second\" : \"old\"", "\"second\" : \"new-second\""));
        File.ReadAllLines(Path.Combine(workspace.Root, "dockerfile-generations.txt")).ShouldBe(["generated"]);
        File.ReadAllLines(Path.Combine(workspace.Root, "readme-generations.txt")).ShouldBe(["generated"]);
    }

    private static IGitHubReleaseUpdater CreateUpdater(string tool, IReleasesClient releases, HttpClient httpClient) =>
        tool switch
        {
            "chisel" => new ChiselUpdater(releases, httpClient),
            "syft" => new SyftUpdater(releases),
            "rocks-toolbox" => new RocksToolboxUpdater(releases),
            "mingit" => new MinGitUpdater(releases),
            _ => throw new ArgumentException($"Unknown tool: {tool}", nameof(tool)),
        };

    private sealed class CallbackUpdater(Func<ManifestVariables, CancellationToken, Task> update) : IGitHubReleaseUpdater
    {
        public static string Name => "sample";
        public static string VersionSourceName => "sample";

        public Task<DependencyUpdate> ResolveFromGitHubReleaseAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new DependencyUpdate(
                Scope: "",
                Description: "Update sample",
                ApplyAsync: (variables, _, token) => update(variables, token)));
    }

    private static ManifestVariables CreateVariables(params (string Name, string Value)[] values) =>
        new(JsonSerializer.Serialize(new { variables = values.ToDictionary(value => value.Name, value => value.Value) }));

    private static Release CreateRelease() => new SimpleJsonSerializer().Deserialize<Release>("""
        {
          "tag_name":"v1.0.0",
          "body":"MinGit-2.50.0-32-bit.zip | 000000\nMinGit-2.50.0-64-bit.zip | abcdef123456",
          "assets":[
            {"name":"chisel_v1.0.0_windows_amd64.tar.gz","browser_download_url":"https://example/wrong-platform.tar.gz"},
            {"name":"chisel_v1.0.0_linux_amd64.tar.gz","browser_download_url":"https://example/chisel-amd64.tar.gz"},
            {"name":"chisel_v1.0.0_linux_arm.tar.gz","browser_download_url":"https://example/chisel-arm.tar.gz"},
            {"name":"chisel_v1.0.0_linux_arm64.tar.gz","browser_download_url":"https://example/chisel-arm64.tar.gz"},
            {"name":"MinGit-2.50.0-32-bit.zip","browser_download_url":"https://example/mingit32.zip"},
            {"name":"MinGit-2.50.0-64-bit.zip","browser_download_url":"https://example/mingit64.zip"}
          ]
        }
        """);

    private sealed class ChecksumHandler(Dictionary<string, string> responses) : HttpMessageHandler
    {
        public List<string> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string url = request.RequestUri!.AbsoluteUri;
            Requests.Add(url);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responses[url]),
            });
        }
    }

    private sealed class Workspace : IDisposable
    {
        public string Root { get; } = Path.Combine(Directory.GetCurrentDirectory(), $"tool-updater-tests-{Guid.NewGuid():N}");
        public string ManifestPath => Path.Combine(Root, "manifest.versions.json");

        public Workspace()
        {
            Directory.CreateDirectory(Root);
        }

        public void WriteGenerators(string assertions)
        {
            string dockerfileDirectory = Path.Combine(Root, "eng", "dockerfile-templates");
            string readmeDirectory = Path.Combine(Root, "eng", "readme-templates");
            Directory.CreateDirectory(dockerfileDirectory);
            Directory.CreateDirectory(readmeDirectory);
            File.WriteAllText(
                Path.Combine(dockerfileDirectory, "Get-GeneratedDockerfiles.ps1"),
                assertions + "\nAdd-Content ./dockerfile-generations.txt 'generated'");
            File.WriteAllText(
                Path.Combine(readmeDirectory, "Get-GeneratedReadmes.ps1"),
                assertions + "\nAdd-Content ./readme-generations.txt 'generated'");
        }

        public void Dispose() => Directory.Delete(Root, recursive: true);
    }
}
