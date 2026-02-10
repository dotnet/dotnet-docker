// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Dotnet.Docker;

namespace UpdateDependencies.Tests;

public sealed class BuildLabelServiceTests
{
    [Fact]
    public void AddBuildTags_WritesSingleTag()
    {
        using var writer = new StringWriter();
        var service = new BuildLabelService(writer);

        service.AddBuildTags("my-tag");

        writer.ToString().ShouldBe(
            """
            ##vso[build.addbuildtag]my-tag

            """);
    }

    [Fact]
    public void AddBuildTags_WritesMultipleTags()
    {
        using var writer = new StringWriter();
        var service = new BuildLabelService(writer);

        service.AddBuildTags("tag1", "tag2", "tag3");

        var expected = """
            ##vso[build.addbuildtag]tag1
            ##vso[build.addbuildtag]tag2
            ##vso[build.addbuildtag]tag3

            """;
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public void AddBuildTags_WithEmptyEnumerable_WritesNothing()
    {
        using var writer = new StringWriter();
        var service = new BuildLabelService(writer);

        service.AddBuildTags([]);

        writer.ToString().ShouldBeEmpty();
    }
}
