// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Azure.Core;
using Microsoft.DotNet.Docker.UpdateDependencies;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

public sealed class PowerShellUpdaterTests
{
    private const string PublicRoot = "https://public.example.com/tool";
    private const string InternalRoot = "https://pscoretestdata.blob.core.windows.net";

    private const string Manifest = $$"""
        {
            "variables": {
                "powershell|base-url|public": "{{PublicRoot}}",
                "powershell|base-url|internal": "{{InternalRoot}}",
                "powershell|9.0|build-version": "7.5.11",
                "powershell|9.0|base-url": "$(powershell|base-url|public)/$(powershell|9.0|build-version)",
                "powershell|9.0|sha-function": "256",
                "powershell|9.0|Linux.Alpine|sha": "old",
                "powershell|9.0|Linux|x64|sha": "old",
                "powershell|9.0|Windows|x64|sha": "old",
                "powershell|10.0|build-version": "7.6.6",
                "powershell|10.0|base-url": "$(powershell|base-url|public)/$(powershell|10.0|build-version)",
                "powershell|10.0|sha-function": "256",
                "powershell|10.0|Linux|x64|sha": "unchanged",
                "powershell|11.0|build-version": "7.7.0-preview.5",
                "powershell|11.0|base-url": "$(powershell|base-url|public)/$(powershell|11.0|build-version)",
                "powershell|11.0|sha-function": "512",
                "powershell|11.0|Linux|x64|sha": "old"
            }
        }
        """;

    private static readonly string s_alpineSha = new('a', 64);
    private static readonly string s_linuxSha = new('b', 64);
    private static readonly string s_windowsSha = new('c', 64);

    [Fact]
    public async Task Public_UpdatesMatchingSeries()
    {
        var handler = new StubHandler(new()
        {
            [$"{PublicRoot}/7.5.12/SHA512SUMS"] = CreateChecksums("7.5.12"),
        });

        var variables = new ManifestVariables(Manifest);

        await ApplyAsync(CreateUpdater(handler), variables, "7.5.12");

        variables.GetRawValue("powershell|9.0|build-version").ShouldBe("7.5.12");
        variables.GetRawValue("powershell|9.0|base-url").ShouldBe("$(powershell|base-url|public)/$(powershell|9.0|build-version)");
        variables.GetRawValue("powershell|9.0|Linux.Alpine|sha").ShouldBe(s_alpineSha);
        variables.GetRawValue("powershell|9.0|Linux|x64|sha").ShouldBe(s_linuxSha);
        variables.GetRawValue("powershell|9.0|Windows|x64|sha").ShouldBe(s_windowsSha);
        variables.GetRawValue("powershell|10.0|build-version").ShouldBe("7.6.6");
        variables.GetRawValue("powershell|10.0|Linux|x64|sha").ShouldBe("unchanged");
        handler.Requests.ShouldHaveSingleItem().Headers.Authorization.ShouldBeNull();
    }

    [Fact]
    public async Task Internal_UsesInternalLayoutAndAuthenticates()
    {
        var handler = new StubHandler(new()
        {
            [$"{InternalRoot}/v7-7-0-preview-6-nuget/globaltool/SHA512SUMS"] = CreateChecksums("7.7.0-preview.6"),
        });

        var credential = new Mock<TokenCredential>();
        credential
            .Setup(c => c.GetTokenAsync(It.IsAny<TokenRequestContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccessToken("test-token", DateTimeOffset.MaxValue));

        var credentialProvider = Mock.Of<IAzureCredentialProvider>(
            p => p.GetCredential(ServiceConnectionNames.Staging) == credential.Object);

        var variables = new ManifestVariables(Manifest);

        await ApplyAsync(CreateUpdater(handler, credentialProvider), variables, "7.7.0-preview.6", isInternal: true);

        variables.GetRawValue("powershell|11.0|build-version").ShouldBe("7.7.0-preview.6");
        variables.GetRawValue("powershell|11.0|base-url")
            .ShouldBe("$(powershell|base-url|internal)/v7-7-0-preview-6-nuget/globaltool");
        variables.GetRawValue("powershell|11.0|Linux|x64|sha").ShouldBe(s_linuxSha);
        variables.GetRawValue("powershell|11.0|sha-function").ShouldBe("256");
        handler.Requests.ShouldHaveSingleItem().Headers.Authorization.ShouldBe(new("Bearer", "test-token"));
    }

    [Fact]
    public async Task BaseUrl_IsWrittenLiterally()
    {
        const string baseUrl = "https://custom.example.com/powershell/7.5.12";
        var handler = new StubHandler(new()
        {
            [$"{baseUrl}/SHA512SUMS"] = CreateChecksums("7.5.12"),
        });

        var variables = new ManifestVariables(Manifest);

        await ApplyAsync(CreateUpdater(handler), variables, "7.5.12", baseUrl: baseUrl);

        variables.GetRawValue("powershell|9.0|base-url").ShouldBe(baseUrl);
        variables.GetRawValue("powershell|9.0|Linux|x64|sha").ShouldBe(s_linuxSha);
    }

    [Fact]
    public async Task DotnetVersion_SelectsImagesOutsideTheSeries()
    {
        var handler = new StubHandler(new()
        {
            [$"{PublicRoot}/7.8.0-preview.1/SHA512SUMS"] = CreateChecksums("7.8.0-preview.1"),
        });

        var variables = new ManifestVariables(Manifest);

        await ApplyAsync(CreateUpdater(handler), variables, "7.8.0-preview.1", dotnetVersion: "11.0");

        variables.GetRawValue("powershell|11.0|build-version").ShouldBe("7.8.0-preview.1");
        variables.GetRawValue("powershell|11.0|Linux|x64|sha").ShouldBe(s_linuxSha);
    }

    [Fact]
    public async Task NoMatchingSeries_Throws()
    {
        var variables = new ManifestVariables(Manifest);

        await Should.ThrowAsync<InvalidOperationException>(() =>
            ApplyAsync(CreateUpdater(new StubHandler([])), variables, "7.8.0"));
    }

    [Fact]
    public async Task MissingChecksum_Throws()
    {
        var handler = new StubHandler(new()
        {
            [$"{PublicRoot}/7.5.12/SHA512SUMS"] = $"{s_linuxSha} *PowerShell.Linux.x64.7.5.12.nupkg\r\n",
        });

        var variables = new ManifestVariables(Manifest);

        await Should.ThrowAsync<InvalidOperationException>(() => ApplyAsync(CreateUpdater(handler), variables, "7.5.12"));
    }

    private static async Task ApplyAsync(
        PowerShellUpdater updater,
        ManifestVariables variables,
        string version,
        bool isInternal = false,
        string? baseUrl = null,
        string? dotnetVersion = null)
    {
        DependencyUpdate update = await updater.ResolveFromVersionAsync(
            version, isInternal, baseUrl, dotnetVersion, TestContext.Current.CancellationToken);

        await update.ApplyAsync(variables, "", TestContext.Current.CancellationToken);
    }

    private static PowerShellUpdater CreateUpdater(StubHandler handler) =>
        CreateUpdater(handler, Mock.Of<IAzureCredentialProvider>());

    private static PowerShellUpdater CreateUpdater(StubHandler handler, IAzureCredentialProvider credentialProvider) =>
        new(new HttpClient(handler), credentialProvider, Mock.Of<ILogger<PowerShellUpdater>>());

    private static string CreateChecksums(string version) =>
        $"{s_alpineSha} *PowerShell.Linux.Alpine.{version}.nupkg\r\n"
        + $"{s_linuxSha} *PowerShell.Linux.x64.{version}.nupkg\r\n"
        + $"{s_windowsSha} *PowerShell.Windows.x64.{version}.nupkg\r\n";

    private sealed class StubHandler(Dictionary<string, string> responses) : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Requests.Add(request);

            HttpResponseMessage response = responses.TryGetValue(request.RequestUri!.ToString(), out string? content)
                ? new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) }
                : new HttpResponseMessage(HttpStatusCode.NotFound);

            return Task.FromResult(response);
        }
    }
}
