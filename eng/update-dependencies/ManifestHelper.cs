// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

/// <summary>
/// Helper class for interacting with manifest files.
/// </summary>
public static class ManifestHelper
{
    private const string VariableGroupName = "variable";
    private const string VariablePattern = $"\\$\\((?<{VariableGroupName}>[\\w:\\-.|]+)\\)";

    /// <summary>
    /// Gets the base URL based on the configured context.
    /// </summary>
    /// <param name="manifestVariables">JSON object of the variables from the manifest.</param>
    /// <param name="options">Configured options from the app.</param>
    public static string GetBaseUrl(JObject manifestVariables, Options options) =>
        GetVariableValue(GetBaseUrlVariableName(options.DockerfileVersion, options.SourceBranch, options.VersionSourceName), manifestVariables);

    /// <summary>
    /// Consstructs the name of the base URL variable.
    /// </summary>
    /// <param name="dockerfileVersion">Dockerfile version.</param>
    /// <param name="branch">Name of the branch.</param>
    public static string GetBaseUrlVariableName(string dockerfileVersion, string branch, string versionSourceName)
    {
        string version = versionSourceName?.Contains("dotnet-monitor") == true ? $"{dockerfileVersion}-monitor" : dockerfileVersion;
        return $"base-url|{version}|{branch}";
    }

    /// <summary>
    /// Gets the value of a manifest variable.
    /// </summary>
    /// <param name="variableName">Name of the variable.</param>
    /// <param name="variables">JSON object of the variables from the manifest.</param>
    public static string GetVariableValue(string variableName, JObject variables) =>
        ResolveVariables((string)variables[variableName], variables);

    /// <summary>
    /// Loads the manifest from the given filename.
    /// </summary>
    /// <param name="filename">Name, not path, of the manifest file located at the root of the repo.</param>
    public static JObject LoadManifest(string filename)
    {
        string path = Path.Combine(UpdateDependencies.RepoRoot, filename);
        string contents = File.ReadAllText(path);
        return JObject.Parse(contents);
    }

    /// <summary>
    /// Gets the regex that identifies a manifest variable.
    /// </summary>
    /// <param name="variableName">Name of the variable.</param>
    /// <param name="valuePattern">Regex pattern that identifies the value of the variable.</param>
    /// <param name="options">Configured options from the app.</param>
    public static Regex GetManifestVariableRegex(string variableName, string valuePattern, RegexOptions options = RegexOptions.None) =>
        new($"\"{Regex.Escape(variableName)}\": \"{valuePattern}\"", options);

    /// <summary>
    /// Resolves the value of a variable, recursively resolving any variables referenced in the value.
    /// </summary>
    /// <param name="value">Variable value to be resolved.</param>
    /// <param name="variables">JSON object of the variables from the manifest.</param>
    private static string ResolveVariables(string value, JObject variables)
    {
        MatchCollection matches = Regex.Matches(value, VariablePattern);
        foreach (Match match in matches)
        {
            string variableName = match.Groups[VariableGroupName].Value;
            string variableValue = GetVariableValue(variableName, variables);
            value = value.Replace(match.Value, variableValue);
        }

        return value;
    }
}
#nullable disable
