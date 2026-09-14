// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dotnet.Docker
{
    /// <summary>
    /// Updates literal build and Docker tag versions without replacing aliases or disabled values.
    /// </summary>
    internal static partial class VersionUpdater
    {
        private static readonly string[] s_excludedMonikers = { "servicing", "rtm" };

        public static void Update(
            ManifestVariables variables,
            string productName,
            string? buildVersion,
            SpecificCommandOptions options)
        {
            string buildVariable = ManifestHelper.GetVersionVariableName(VersionType.Build, productName, options.DockerfileVersion);
            string productVariable = ManifestHelper.GetVersionVariableName(VersionType.Product, productName, options.DockerfileVersion);
            string productVersion = GetProductVersion(buildVersion, options.StableBranding);

            UpdateVersion(variables, buildVariable, buildVersion ?? string.Empty);
            UpdateVersion(variables, productVariable, productVersion);
        }

        // Preserve the old selection rule: edit literal versions, not aliases or empty values.
        private static void UpdateVersion(ManifestVariables variables, string variableName, string version)
        {
            if (variables.Contains(variableName) && !VersionValueRegex.IsMatch(variables.GetRawValue(variableName)))
            {
                Trace.TraceInformation($"Leaving manifest variable '{variableName}' unchanged.");
                return;
            }

            VariableUpdater.Update(variables, variableName, version);
        }

        public static string GetBuildVersion(string productName, string dockerfileVersion, ManifestVariables variables)
        {
            string versionVariableName = ManifestHelper.GetVersionVariableName(VersionType.Build, productName, dockerfileVersion);
            string version = variables.GetRawValue(versionVariableName);
            if (!VersionValueRegex.IsMatch(version))
            {
                throw new InvalidOperationException($"Unable to retrieve {versionVariableName}");
            }

            return version;
        }

        private static string GetProductVersion(string? buildVersion, bool stableBranding)
        {
            if (buildVersion is null)
            {
                return string.Empty;
            }

            // Derive the Docker tag version from the product build version.
            // 5.0.0-preview.2.19530.9 => 5.0.0-preview.2
            string versionRegexPattern = "[\\d]+.[\\d]+.[\\d]+(-[\\w]+(.[\\d]+)?)?";
            Match versionMatch = Regex.Match(buildVersion, versionRegexPattern);
            string version = versionMatch.Success ? versionMatch.Value : buildVersion;

            foreach (string excludedMoniker in s_excludedMonikers)
            {
                int monikerIndex = version.IndexOf($"-{excludedMoniker}", StringComparison.OrdinalIgnoreCase);
                if (monikerIndex != -1)
                {
                    version = version.Substring(0, monikerIndex);
                }
            }

            return VersionHelper.ResolveProductVersion(version, stableBranding);
        }

        [GeneratedRegex(@"\Av?[\d]+.[\d]+.[\d]+(-[\w]+(.[\d]+)*)?\z")]
        private static partial Regex VersionValueRegex { get; }
    }
}
