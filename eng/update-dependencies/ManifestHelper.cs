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
