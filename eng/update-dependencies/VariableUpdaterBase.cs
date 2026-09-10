// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal abstract class VariableUpdaterBase : FileRegexUpdater
{
    protected string VariableName { get; }
    protected Lazy<ManifestVariables> Variables { get; }

    public VariableUpdaterBase(string manifestVersionsFilePath, string variableName)
    {
        VariableName = variableName;
        Path = manifestVersionsFilePath;
        VersionGroupName = "val";
        Regex = ManifestHelper.GetManifestVariableRegex(variableName, @$"(?<{VersionGroupName}>\S*)");

        Variables = new Lazy<ManifestVariables>(
            () => ManifestVariables.FromFile(manifestVersionsFilePath));
    }
}
