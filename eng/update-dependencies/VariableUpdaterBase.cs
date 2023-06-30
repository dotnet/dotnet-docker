// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

internal abstract class VariableUpdaterBase : FileRegexUpdater
{
    protected string VariableName { get; }
    protected Lazy<JObject> ManifestVariables { get; }

    public VariableUpdaterBase(string repoRoot, string variableName)
    {
        VariableName = variableName;
        Path = System.IO.Path.Combine(repoRoot, UpdateDependencies.VersionsFilename);
        VersionGroupName = "val";
        Regex = ManifestHelper.GetManifestVariableRegex(variableName, @$"(?<{VersionGroupName}>\S*)");

        ManifestVariables = new Lazy<JObject>(
            () =>
            {
                const string VariablesProperty = "variables";
                JToken? variables = ManifestHelper.LoadManifest(UpdateDependencies.VersionsFilename)[VariablesProperty];
                if (variables is null)
                {
                    throw new InvalidOperationException($"'{VariablesProperty}' property missing in '{UpdateDependencies.VersionsFilename}'");
                }
                return (JObject)variables;
            });
    }
}
