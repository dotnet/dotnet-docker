// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.DotNet.Docker.UpdateDependencies;

namespace UpdateDependencies.Tests;

public sealed class ManifestVariablesTests
{
    [Theory]
    [InlineData("missing", false)]
    [InlineData("empty", false)]
    [InlineData("reference", false)]
    [InlineData("literal", true)]
    public void ShouldUpdateLiteral_ExcludesMissingEmptyAndReferenceValues(string name, bool expected)
    {
        var variables = new ManifestVariables("""
            {"variables":{"empty":"","reference":"$(literal)","literal":"1.2.3"}}
            """);

        variables.ShouldUpdateLiteral(name).ShouldBe(expected);
    }

    [Fact]
    public void SetValue_PreservesEverythingOutsideTheValueToken()
    {
        // Escapes make the CRLFs and tab explicit, independent of this source file's line endings.
        // A raw multiline literal would inherit those line endings instead.
        const string content =
            "{\r\n  // keep this comment\r\n  \"other\": {\"version\":\"old\"},\r\n"
            + "  \"variables\": { \"version\" \t:  \"old\", \"empty\": \"\", },\r\n}\r\n";
        var variables = new ManifestVariables(content);

        variables.SetValue("version", "new").ShouldBeTrue();

        string expected = content.Replace("\"old\", \"empty\"", "\"new\", \"empty\"");
        variables.Content.ShouldBe(expected);
        variables.GetRawValue("version").ShouldBe("new");
        variables.Contains("empty").ShouldBeTrue();
        variables.GetValue("empty").ShouldBeEmpty();
        variables.Names.ShouldBe(["version", "empty"]);
    }

    [Fact]
    public void SetValue_HandlesEscapingUnicodeAndRepeatedEdits()
    {
        const string content = """
            {"note":"\u00e9","variables":{"na\u006de":"old","later":"end"}}
            """;
        // Use actual multibyte characters before the edited token.
        string original = content.Replace("\\u00e9", "\u00e9\U0001F600");
        var variables = new ManifestVariables(original);
        const string replacement = "quote: \" slash: \\ newline:\n dollar: $1 \u00e9";

        variables.SetValue("name", replacement).ShouldBeTrue();
        variables.SetValue("later", "longer value").ShouldBeTrue();
        variables.SetValue("name", replacement).ShouldBeFalse();

        var reparsed = new ManifestVariables(variables.Content);
        reparsed.GetRawValue("name").ShouldBe(replacement);
        reparsed.GetRawValue("later").ShouldBe("longer value");
        variables.Content.ShouldStartWith("{\"note\":\"\u00e9\U0001F600\",");
        variables.Content.ShouldContain("\"na\\u006de\":");

        variables.SetValue("name", "old");
        variables.SetValue("later", "end");
        variables.Content.ShouldBe(original);
    }

    [Fact]
    public void SetValue_PreservesOffsetsAcrossMultibyteValuesAndGaps()
    {
        const string content =
            "{\"variables\":{\"first\":\"\u00e9\U0001F600\",/* \u00e9 */"
            + "\"\u00e9\":\"middle\",\"last\":\"\U0001F600\"}}";
        var variables = new ManifestVariables(content);

        // Edit out of source order, with different replacement lengths.
        variables.SetValue("last", "long replacement");
        variables.SetValue("first", "");
        variables.SetValue("\u00e9", "new");

        const string expected =
            "{\"variables\":{\"first\":\"\",/* \u00e9 */"
            + "\"\u00e9\":\"new\",\"last\":\"long replacement\"}}";
        variables.Content.ShouldBe(expected);

        variables.SetValue("first", "\u00e9\U0001F600");
        variables.SetValue("\u00e9", "middle");
        variables.SetValue("last", "\U0001F600");
        variables.Content.ShouldBe(content);
    }

    [Fact]
    public void SetValue_UnchangedDecodedValuePreservesOriginalEscapes()
    {
        const string content = """{"variables":{"version":"\u0031"}}""";
        var variables = new ManifestVariables(content);

        variables.SetValue("version", "1").ShouldBeFalse();

        variables.Content.ShouldBe(content);
    }

    [Fact]
    public void GetValue_ResolvesCurrentValuesWithoutRewritingReferences()
    {
        var variables = new ManifestVariables("""
            {"variables":{
              "version":"1",
              "alias":"$(version)",
              "url":"https://example/$(alias)/$(version)"
            }}
            """);

        variables.GetValue("url").ShouldBe("https://example/1/1");
        variables.SetValue("version", "2");
        variables.GetValue("url").ShouldBe("https://example/2/2");
        variables.GetRawValue("alias").ShouldBe("$(version)");

        variables.SetValue("alias", "3");
        variables.GetValue("url").ShouldBe("https://example/3/2");
        variables.GetValue("version").ShouldBe("2");

        variables.SetValue("alias", "$(version)");
        variables.GetValue("alias").ShouldBe("2");
    }

    [Fact]
    public void MissingVariables_AreNotTreatedAsEmptyOrInserted()
    {
        const string content = """{"variables":{"empty":"","alias":"$(missing)"}}""";
        var variables = new ManifestVariables(content);

        variables.Contains("missing").ShouldBeFalse();
        Should.Throw<KeyNotFoundException>(() => variables.GetRawValue("missing"));
        Should.Throw<KeyNotFoundException>(() => variables.GetValue("missing"));
        variables.SetValue("missing", "value").ShouldBeFalse();
        var error = Should.Throw<KeyNotFoundException>(() => variables.GetValue("alias"));

        error.Message.ShouldContain("alias -> missing");
        variables.Content.ShouldBe(content);
    }

    [Theory]
    [InlineData("""{"variables":{"a":"$(a)"}}""", "a -> a")]
    [InlineData("""{"variables":{"a":"$(b)","b":"$(c)","c":"$(a)"}}""", "a -> b -> c -> a")]
    public void GetValue_ReportsReferenceCycles(string content, string expectedChain)
    {
        var variables = new ManifestVariables(content);

        var error = Should.Throw<InvalidOperationException>(() => variables.GetValue("a"));

        error.Message.ShouldContain(expectedChain);
        variables.SetValue("a", "fixed");
        variables.GetValue("a").ShouldBe("fixed");
    }

    [Theory]
    [InlineData("")]
    [InlineData("[]")]
    [InlineData("{}")]
    [InlineData("""{"nested":{"variables":{}}}""")]
    [InlineData("""{"variables":null}""")]
    [InlineData("""{"variables":[]}""")]
    [InlineData("""{"variables":{"a":1}}""")]
    [InlineData("""{"variables":{"a":null}}""")]
    [InlineData("""{"variables":{"a":{}}}""")]
    [InlineData("""{"variables":{"a":"1","a":"2"}}""")]
    [InlineData("""{"variables":{},"variables":{}}""")]
    [InlineData("""{"variables":{}} {}""")]
    [InlineData("""{"variables":{"a":"1"}""")]
    public void Constructor_RejectsInvalidOrAmbiguousManifests(string content)
    {
        Should.Throw<JsonException>(() => new ManifestVariables(content));
    }

    [Fact]
    public void Constructor_AcceptsEmptyVariablesAndSkipsUnrelatedStructures()
    {
        const string content = """
            {"before":[{"variables":{"a":1}}],"variables":{},"after":{"a":[1,2]}}
            """;
        var variables = new ManifestVariables(content);

        variables.Names.ShouldBeEmpty();
        variables.Content.ShouldBe(content);
    }
}
