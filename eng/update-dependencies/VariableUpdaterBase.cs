// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.VersionTools.Dependencies;
using Newtonsoft.Json.Linq;

namespace Dotnet.Docker;

internal abstract class VariableUpdaterBase : FileRegexUpdater
{
    protected string VariableName { get; }
    protected Lazy<JObject> ManifestVariables { get; }

    public VariableUpdaterBase(string manifestVersionsFilePath, string variableName)
    {
        VariableName = variableName;
        Path = manifestVersionsFilePath;
        VersionGroupName = "val";
        Regex = ManifestHelper.GetManifestVariableRegex(variableName, @$"(?<{VersionGroupName}>\S*)");

        ManifestVariables = new Lazy<JObject>(
            () =>
            {
                const string VariablesProperty = "variables";
                JToken? variables = ManifestHelper.LoadManifest(manifestVersionsFilePath)[VariablesProperty];
                if (variables is null)
                {
                    throw new InvalidOperationException($"'{VariablesProperty}' property missing in '{manifestVersionsFilePath}'");
                }
                return (JObject)variables;
            });
    }
}
