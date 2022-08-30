// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Updates the NuGet.config test app artifact to add or remove an internal package feed.
/// </summary>
internal class NuGetConfigUpdater : IDependencyUpdater
{
    private const string PkgSrcSuffix = "_internal";
    private readonly string _repoRoot;
    private readonly Options _options;
    private readonly string _configPath;

    public NuGetConfigUpdater(string repoRoot, Options options)
    {
        _repoRoot = repoRoot;
        _options = options;

        string configSuffix = (_options.SourceBranch == "nightly" ? ".nightly" : string.Empty);
        _configPath = Path.Combine(_repoRoot, $"tests/Microsoft.DotNet.Docker.Tests/TestAppArtifacts/NuGet.config{configSuffix}");
    }

    public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos)
    {
        string existingContent = File.ReadAllText(_configPath);
        IDependencyInfo? sdkInfo = dependencyInfos
            .FirstOrDefault(info => info.SimpleName == "sdk");
        if (sdkInfo is not null)
        {
            string newContent = GetUpdatedNuGetConfigContent(sdkInfo.SimpleVersion);

            if (newContent != existingContent)
            {
                return new[]
                {
                    new DependencyUpdateTask(
                        () => File.WriteAllText(_configPath, newContent),
                        new[] { sdkInfo },
                        Enumerable.Empty<string>())
                };
            }
        }

        return Enumerable.Empty<DependencyUpdateTask>();
    }

    /// <summary>
    /// Updates the NuGet.config file to include a URL to the internal package feed of the specified version.
    /// </summary>
    private string GetUpdatedNuGetConfigContent(string sdkVersion)
    {
        string pkgSrcName = $"dotnet{_options.DockerfileVersion.Replace(".", "_")}{PkgSrcSuffix}";

        XDocument doc = XDocument.Load(_configPath);

        XElement configuration = doc.Root!;
        UpdatePackageSources(sdkVersion, pkgSrcName, configuration);
        UpdatePackageSourceCredentials(pkgSrcName, configuration);
        return ToStringWithDeclaration(doc) + Environment.NewLine;
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

    private void UpdatePackageSourceCredentials(string pkgSrcName, XElement configuration)
    {
        XElement? pkgSourceCreds = configuration.Element("packageSourceCredentials");
        if (_options.IsInternal)
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
            UpdateAddElement(pkgSrcCredsEntry, "ClearTextPassword", "%NuGetFeedPassword%");
        }
        else
        {
            pkgSourceCreds?.Remove();
        }
    }

    private void UpdatePackageSources(string sdkVersion, string pkgSrcName, XElement configuration)
    {      
        XElement? pkgSources = configuration.Element("packageSources");
        if (_options.IsInternal)
        {
            pkgSources = GetOrCreateXObject(
                pkgSources,
                configuration,
                () => new XElement("packageSources"));

            string project = _options.DockerfileVersion != "3.1" ? "/internal" : string.Empty;

            UpdateAddElement(
                pkgSources,
                pkgSrcName,
                $"https://pkgs.dev.azure.com/dnceng{project}/_packaging/{sdkVersion}-shipping/nuget/v3/index.json");
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
#nullable disable
