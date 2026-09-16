// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Xml.Linq;
using Microsoft.DotNet.Docker.Shared;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Updates the NuGet.config test app artifact to add or remove an internal package feed.
/// </summary>
public static class NuGetConfigUpdater
{
    private const string PkgSrcSuffix = "_internal";

    public static void Update(
        ManifestVariables variables,
        string repoRoot,
        string dockerfileVersion,
        string? sdkVersion,
        bool isInternal)
    {
        // The upstream branch represents which GitHub branch the current
        // branch branched off of. This is either "nightly" or "main".
        string upstreamBranch = variables.GetValue("branch");

        string configSuffix = (isInternal, upstreamBranch) switch
        {
            (true, _) => ".internal",
            (false, "nightly") => ".nightly",
            _ => string.Empty
        };

        string configPath = Path.Combine(
            repoRoot, "tests", "Microsoft.DotNet.Docker.Tests", "TestAppArtifacts", $"NuGet.config{configSuffix}");
        string existingContent = File.ReadAllText(configPath);
        DotNetVersion parsedSdkVersion = sdkVersion
            ?? throw new InvalidOperationException("An SDK version is required to update NuGet.config.");
        string pkgSrcName = $"dotnet{dockerfileVersion.Replace(".", "_")}{PkgSrcSuffix}";

        XDocument doc = XDocument.Parse(existingContent);
        XElement configuration = doc.Root
            ?? throw new InvalidOperationException($"Missing configuration root in '{configPath}'.");
        UpdatePackageSources(parsedSdkVersion, pkgSrcName, configuration, isInternal);
        UpdatePackageSourceCredentials(pkgSrcName, configuration, isInternal);
        string newContent = ToStringWithDeclaration(doc) + Environment.NewLine;

        if (newContent != existingContent)
        {
            File.WriteAllText(configPath, newContent);
        }
    }

    private static string ToStringWithDeclaration(XDocument doc)
    {
        StringBuilder builder = new();
        using (TextWriter writer = new Utf8StringWriter(builder))
        {
            // Using the Save method preserves the XML declaration header. ToString doesn't do that.
            doc.Save(writer);
        }
        return builder.ToString();
    }

    /// <summary>
    /// Updates the packageSourceCredentials section of the NuGet.config for the current version's package source.
    /// Credentials are only needed for internal, non-public-preview builds which use authenticated feeds.
    /// Public preview builds use public feeds, so their credentials are removed if present.
    /// Only the current version's entry is modified — other versions' credentials are left intact.
    /// </summary>
    private static void UpdatePackageSourceCredentials(string pkgSrcName, XElement configuration, bool isInternal)
    {
        XElement? pkgSourceCreds = configuration.Element("packageSourceCredentials");
        if (isInternal)
        {
            pkgSourceCreds = GetOrCreateXObject(
                pkgSourceCreds,
                configuration,
                () => new XElement("packageSourceCredentials"));

            XElement pkgSrcCredsEntry = GetOrCreateXObject(
                pkgSourceCreds.Element(pkgSrcName),
                pkgSourceCreds,
                () => new XElement(pkgSrcName));
            UpdateAddElement(pkgSrcCredsEntry, "Username", "dotnet");
            UpdateAddElement(pkgSrcCredsEntry, "ClearTextPassword", "%InternalAccessToken%");
        }
        else
        {
            // Only remove the credentials entry for the current version's package source,
            // leaving credentials for other versions intact (e.g. a non-preview version
            // may still need authenticated access even if this preview version doesn't).
            pkgSourceCreds?.Element(pkgSrcName)?.Remove();
            if (pkgSourceCreds is not null && !pkgSourceCreds.HasElements)
            {
                pkgSourceCreds.Remove();
            }
        }
    }

    private static void UpdatePackageSources(
        DotNetVersion sdkVersion, string pkgSrcName, XElement configuration, bool isInternal)
    {
        XElement? pkgSources = configuration.Element("packageSources");
        if (isInternal)
        {
            pkgSources = GetOrCreateXObject(
                node: pkgSources,
                parent: configuration,
                createNode: () => new XElement("packageSources")
            );

            UpdateAddElement(
                parentElement: pkgSources,
                key: pkgSrcName,
                value: $"https://pkgs.dev.azure.com/dnceng/internal/_packaging/{sdkVersion}-shipping/nuget/v3/index.json"
            );
        }
        else
        {
            if (pkgSources is not null)
            {
                RemoveAllInternalPackageSources(pkgSources);
            }
        }
    }

    private static void RemoveAllInternalPackageSources(XElement packageSources)
    {
        IEnumerable<XElement> elements = packageSources.Elements()
            .Where(pkgSrc => pkgSrc.Attribute("key")?.Value.EndsWith(PkgSrcSuffix) == true)
            .ToList();
        foreach (XElement element in elements)
        {
            element.Remove();
        }
    }

    private static void UpdateAddElement(XElement parentElement, string key, string value)
    {
        XElement addElement = GetOrCreateXObject(
            parentElement.Elements("add").FirstOrDefault(element => HasKeyAttribute(element, key)),
            parentElement,
            () => CreateAddElement(key, value));

        UpdateValueAttribute(addElement, value);
    }

    private static XElement CreateAddElement(string key, string value) =>
        new("add",
            new XAttribute("key", key),
            new XAttribute("value", value));

    private static void UpdateValueAttribute(XElement parentElement, string value)
    {
        XAttribute valueAttrib = GetOrCreateXObject(
            parentElement.Attribute("value"),
            parentElement,
            () => new XAttribute("value", value));
        valueAttrib.Value = value;
    }

    private static bool HasKeyAttribute(XElement element, string value) =>
        element.Attribute("key")?.Value == value;

    private static T GetOrCreateXObject<T>(T? node, XContainer parent, Func<T> createNode)
        where T : XObject
    {
        if (node is null)
        {
            node = createNode();
            parent.Add(node);
        }

        return node;
    }

    private class Utf8StringWriter : StringWriter
    {
        public Utf8StringWriter(StringBuilder stringBuilder)
            : base(stringBuilder)
        {
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
