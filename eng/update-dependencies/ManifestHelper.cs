// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace Dotnet.Docker;

/// <summary>
/// Helper class for interacting with manifest files.
/// </summary>
public static partial class ManifestHelper
{
    /// <summary>
    /// Gets the base URLs based on the configured context.
    /// </summary>
    /// <param name="manifestVariables">Variables from the manifest.</param>
    /// <param name="options">Configured options from the app.</param>
    public static IEnumerable<string> GetBaseUrls(ManifestVariables manifestVariables, SpecificCommandOptions options)
    {
        // The upstream branch represents which GitHub branch the current
        // branch branched off of. This is either "nightly" or "main".
        var upstreamBranch = manifestVariables.GetValue("branch");

        var baseUrlVariableNames = GetBaseUrlVariableNames(
            dockerfileVersion: options.DockerfileVersion,
            branch: upstreamBranch,
            versionSourceName: options.VersionSourceName);

        var baseUrlValues = baseUrlVariableNames
            .Where(manifestVariables.Contains)
            .Select(manifestVariables.GetValue);

        return baseUrlValues;
    }

    /// <summary>
    /// Constructs the base URL variables for the given dockerfile, branch,
    /// and product combination.
    /// </summary>
    /// <param name="dockerfileVersion">
    /// Dockerfile version. This should be a major.minor version e.g. "8.0",
    /// "9.0", "10.0".
    /// </param>
    /// <param name="branch">
    /// Name of the branch. This is typically "main" or "nightly".
    /// </param>
    public static IEnumerable<string> GetBaseUrlVariableNames(
        string dockerfileVersion,
        string branch,
        string versionSourceName = "",
        bool sdkOnlyRelease = false)
    {
        string product;
        if (sdkOnlyRelease)
        {
            product = "sdk";
        }
        else
        {
            product = versionSourceName switch
            {
                string v when v.Contains("dotnet-monitor") => "monitor",
                string v when v.Contains("aspire-dashboard") => "aspire-dashboard",
                _ => "dotnet",
            };
        }

        return [
            $"{product}|{dockerfileVersion}|base-url|{branch}",
            $"{product}|{dockerfileVersion}|base-url|checksums|{branch}",
        ];
    }

    /// <summary>
    /// Constructs the name of the shared base URL variable.
    /// </summary>
    /// <param name="releaseState">Release state of the product assets.</param>
    /// <param name="branch">Name of the branch.</param>
    public static string GetBaseUrlVariableName(ReleaseState releaseState, string branch)
    {
        string qualityString = releaseState switch
        {
            ReleaseState.Prerelease => "preview",
            ReleaseState.Release => "maintenance",
            _ => throw new NotSupportedException()
        };

        return $"base-url|public|{qualityString}|{branch}";
    }

    public static string GetVersionVariableName(VersionType versionType, string productName, string dockerfileVersion) =>
        $"{productName}|{dockerfileVersion}|{versionType.ToString().ToLowerInvariant()}-version";

    /// <summary>
    /// Determines if the given value matches the pattern manifest variable. Does not check if the variable is defined
    /// in the manifest.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is a manifest variable, false otherwise.</returns>
    public static bool IsManifestVariable(string value) => AnyVariableRegex().IsMatch(value);

    [GeneratedRegex(@"^\$\(.*\)$")]
    private static partial Regex AnyVariableRegex();
}
