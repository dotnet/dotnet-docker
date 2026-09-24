// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Docker.UpdateDependencies.Commands;
using Microsoft.DotNet.Docker.UpdateDependencies.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace UpdateDependencies.Tests;

public sealed class FromVersionCommandTests
{
    private static readonly string[] s_versionSourceOptions = ["--internal", "--base-url", "--dotnet-version"];

    [Fact]
    public void VersionUpdater_DoesNotHaveVersionSourceOptions()
    {
        Command command = FromVersionCommand.CreateCliCommand<FakeVersionUpdater>(new ServiceCollection().BuildServiceProvider());

        command.Options.Select(option => option.Name).ShouldNotContain(name => s_versionSourceOptions.Contains(name));
    }

    [Fact]
    public void VersionSourceUpdater_BindsVersionSourceOptions()
    {
        Command command = FromVersionCommand.CreateCliCommand<FakeVersionSourceUpdater>(new ServiceCollection().BuildServiceProvider());

        ParseResult result = command.Parse(
            ["1.2.3", "--internal", "--base-url", "https://example.com/1.2.3", "--dotnet-version", "9.0"]);

        result.Errors.ShouldBeEmpty();
        VersionSourceOptions.Bind(result).ShouldBe(
            new VersionSource(Internal: true, BaseUrl: "https://example.com/1.2.3", DotnetVersion: "9.0"));
    }

    [Fact]
    public void VersionSourceUpdater_DefaultsToPublicSource()
    {
        Command command = FromVersionCommand.CreateCliCommand<FakeVersionSourceUpdater>(new ServiceCollection().BuildServiceProvider());

        ParseResult result = command.Parse(["1.2.3"]);

        result.Errors.ShouldBeEmpty();
        VersionSourceOptions.Bind(result).ShouldBe(new VersionSource());
    }

    private sealed class FakeVersionUpdater : IVersionUpdater
    {
        public static string Name => "fake";
        public static string VersionSourceName => "fake";

        public Task<DependencyUpdate> ResolveFromVersionAsync(string version, CancellationToken cancellationToken) =>
            throw new NotImplementedException();
    }

    private sealed class FakeVersionSourceUpdater : IVersionSourceUpdater
    {
        public static string Name => "fake";
        public static string VersionSourceName => "fake";

        public Task<DependencyUpdate> ResolveFromVersionAsync(
            string version,
            VersionSource source,
            CancellationToken cancellationToken) =>
            throw new NotImplementedException();
    }
}
