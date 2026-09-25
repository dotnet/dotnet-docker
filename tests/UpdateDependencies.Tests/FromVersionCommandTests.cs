// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace UpdateDependencies.Tests;

public sealed class FromVersionCommandTests
{
    [Fact]
    public void BindsAllOptions()
    {
        ParseResult result = CreateCommand().Parse(
            ["version", "1.2.3", "--internal", "--base-url", "https://example.com/1.2.3", "--dotnet-version", "9.0"]);

        result.Errors.ShouldBeEmpty();
        FromVersionOptions options = FromVersionOptions.Bind(result);
        options.Version.ShouldBe("1.2.3");
        options.Internal.ShouldBeTrue();
        options.BaseUrl.ShouldBe("https://example.com/1.2.3");
        options.DotnetVersion.ShouldBe("9.0");
    }

    [Fact]
    public void DefaultsToPublicSource()
    {
        ParseResult result = CreateCommand().Parse(["version", "1.2.3"]);

        result.Errors.ShouldBeEmpty();
        FromVersionOptions options = FromVersionOptions.Bind(result);
        options.Internal.ShouldBeFalse();
        options.BaseUrl.ShouldBeNull();
        options.DotnetVersion.ShouldBeNull();
    }

    private static Command CreateCommand() =>
        DependencyCommand.CreateCliCommand<FakeUpdater>(new ServiceCollection().BuildServiceProvider());

    private sealed class FakeUpdater : IVersionUpdater
    {
        public static string Name => "fake";
        public static string VersionSourceName => "fake";

        public Task<DependencyUpdate> ResolveFromVersionAsync(
            string version,
            bool isInternal,
            string? baseUrl,
            string? dotnetVersion,
            CancellationToken cancellationToken) =>
            throw new NotImplementedException();
    }
}
