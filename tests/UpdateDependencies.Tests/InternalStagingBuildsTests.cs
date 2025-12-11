// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using Dotnet.Docker.Sync;
using Microsoft.DotNet.Docker.Shared;
using Shouldly;
using Xunit;

namespace UpdateDependencies.Tests;

public sealed class InternalStagingBuildsTests
{
    [Fact]
    public void ToStringOrdersVersionsAscending()
    {
        var builds = new InternalStagingBuilds(
            ImmutableDictionary.CreateRange<DotNetVersion, int>(
                [
                    KeyValuePair.Create(DotNetVersion.Parse("10.0"), 1000),
                    KeyValuePair.Create(DotNetVersion.Parse("8.0"), 800),
                    KeyValuePair.Create(DotNetVersion.Parse("9.0"), 900)
                ]));

        var lines = builds.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        lines.ShouldBe(["8.0=800", "9.0=900", "10.0=1000"]);
    }

    [Fact]
    public void AddReplacesEntryForSameMajorMinorVersion()
    {
        var builds = new InternalStagingBuilds(
            ImmutableDictionary.CreateRange<DotNetVersion, int>(
                [KeyValuePair.Create(DotNetVersion.Parse("8.0"), 100)]));

        builds = builds.Add(DotNetVersion.Parse("8.0.101"), 200);
        builds = builds.Add(DotNetVersion.Parse("8.0.202"), 300);

        builds.Versions.Count.ShouldBe(1);
        builds.Versions.Single().ShouldBe(KeyValuePair.Create(DotNetVersion.Parse("8.0"), 300));
    }

    [Fact]
    public void ToStringOmitsPatchVersion()
    {
        var builds = new InternalStagingBuilds(ImmutableDictionary<DotNetVersion, int>.Empty);
        builds = builds.Add(DotNetVersion.Parse("8.0.101"), 100);
        builds = builds.Add(DotNetVersion.Parse("9.1.205-servicing.1"), 200);

        foreach (var line in builds.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            var versionPart = line.Split('=')[0];
            versionPart.Count(c => c == '.').ShouldBe(1);
        }
    }
}
