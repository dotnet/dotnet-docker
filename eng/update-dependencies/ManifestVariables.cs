// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json.Linq;

namespace Dotnet.Docker;

/// <summary>
/// Object that holds variables defined in a manifest file.
/// </summary>
public class ManifestVariables(JObject variables) : IManifestVariables
{
    public static ManifestVariables FromFile(string filePath)
    {
        var manifestObject = ManifestHelper.LoadManifest(filePath);
        var manifestVariables = (JObject?)manifestObject["variables"] ?? [];
        return new ManifestVariables(manifestVariables);
    }

    /// <summary>
    /// The raw JSON object containing the variables from the manifest.
    /// </summary>
    public JObject Variables { get; } = variables;

    /// <summary>
    /// Checks whether the manifest contains a value for <paramref name="variableName"/>.
    /// </summary>
    public bool HasValue(string variableName) => Variables.ContainsKey(variableName);

    /// <summary>
    /// Resolves the value of a manifest variable, recursively resolving any
    /// variables referenced in the value.
    /// </summary>
    public string GetValue(string variableName)
    {
        string variableValue = ManifestHelper.GetVariableValue(variableName, Variables);
        return ManifestHelper.ResolveVariables(variableValue, Variables);
    }
}
