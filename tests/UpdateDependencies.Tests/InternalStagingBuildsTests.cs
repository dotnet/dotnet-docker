// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Dotnet.Docker.Sync;
using Microsoft.DotNet.Docker.Shared;

namespace UpdateDependencies.Tests;

public sealed class InternalStagingBuildsTests
{
    [Fact]
    public void ToStringOrdersVersionsAscending()
    {
        var builds = new InternalStageContainers(
            ImmutableDictionary.CreateRange<DotNetVersion, string>(
                [
                    KeyValuePair.Create(DotNetVersion.Parse("10.0"), "stage-1000"),
                    KeyValuePair.Create(DotNetVersion.Parse("8.0"), "stage-800"),
                    KeyValuePair.Create(DotNetVersion.Parse("9.0"), "stage-900")
                ]));

        var lines = builds.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        lines.ShouldBe(["8.0=stage-800", "9.0=stage-900", "10.0=stage-1000"]);
    }

    [Fact]
    public void AddReplacesEntryForSameMajorMinorVersion()
    {
        var builds = new InternalStageContainers(
            ImmutableDictionary.CreateRange<DotNetVersion, string>(
                [KeyValuePair.Create(DotNetVersion.Parse("8.0"), "stage-100")]));

        builds = builds.Add(DotNetVersion.Parse("8.0.101"), "stage-200");
        builds = builds.Add(DotNetVersion.Parse("8.0.202"), "stage-300");

        builds.Versions.Count.ShouldBe(1);
        builds.Versions.Single().ShouldBe(KeyValuePair.Create(DotNetVersion.Parse("8.0"), "stage-300"));
    }

    [Fact]
    public void ToStringOmitsPatchVersion()
    {
        var builds = new InternalStageContainers(ImmutableDictionary<DotNetVersion, string>.Empty);
        builds = builds.Add(DotNetVersion.Parse("8.0.101"), "stage-100");
        builds = builds.Add(DotNetVersion.Parse("9.1.205-servicing.1"), "stage-200");

        foreach (var line in builds.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            var versionPart = line.Split('=')[0];
            versionPart.Count(c => c == '.').ShouldBe(1);
        }
    }
}
