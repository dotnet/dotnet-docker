// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class Config
    {
        private const string VariableGroupName = "variable";
        private const string VariablePattern = $"\\$\\((?<{VariableGroupName}>[\\w:\\-.|]+)\\)";
        private static Lazy<JObject> Manifest { get; } = new Lazy<JObject>(() => LoadManifest("manifest.json"));
        private static Lazy<JObject> ManifestVersions { get; } = new Lazy<JObject>(() => LoadManifest("manifest.versions.json"));

        public static string SourceRepoRoot { get; } = Environment.GetEnvironmentVariable("SOURCE_REPO_ROOT") ?? string.Empty;
        public static bool IsHttpVerificationDisabled { get; } =
            Environment.GetEnvironmentVariable("DISABLE_HTTP_VERIFICATION") != null;
        public static bool PullImages { get; } = Environment.GetEnvironmentVariable("PULL_IMAGES") != null;
        public static bool IsNightlyRepo { get; } = GetIsNightlyRepo();
        public static bool IsRunningInContainer { get; } =
            Environment.GetEnvironmentVariable("RUNNING_TESTS_IN_CONTAINER") != null;
        public static string RepoPrefix { get; } = Environment.GetEnvironmentVariable("REPO_PREFIX") ?? string.Empty;
        public static string Registry { get; } =
            Environment.GetEnvironmentVariable("REGISTRY") ?? (string)Manifest.Value["registry"];
        public static string[] OsNames { get; } =
            (Environment.GetEnvironmentVariable("IMAGE_OS_NAMES") ?? string.Empty).Split(",", StringSplitOptions.RemoveEmptyEntries);
        public static string SourceBranch { get; } =
            Environment.GetEnvironmentVariable("SOURCE_BRANCH") ?? string.Empty;
        public static string SasQueryString { get; } =
            Environment.GetEnvironmentVariable("SAS_QUERY_STRING") ?? string.Empty;
        public static string NuGetFeedPassword { get; } =
            Environment.GetEnvironmentVariable("NUGET_FEED_PASSWORD") ?? string.Empty;

        public static bool IsInternal(string dotnetVersion)
        {
            string versionBaseUrl = GetBaseUrl(dotnetVersion);
            return versionBaseUrl.Contains("msrc") || versionBaseUrl.Contains("internal");
        }

        private static bool GetIsNightlyRepo()
        {
            string repo = (string)Manifest.Value["repos"][0]["name"];
            return repo.Contains("/nightly/");
        }

        private static JObject LoadManifest(string manifestFile)
        {
            string manifestPath = Path.Combine(SourceRepoRoot, manifestFile);
            string manifestJson = File.ReadAllText(manifestPath);
            return JObject.Parse(manifestJson);
        }

        private static string GetVariableValue(string variableName, JObject variables) =>
            ResolveVariables((string)variables[variableName], variables);

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

        public static string GetBaseUrl(string dotnetVersion) =>
            GetVariableValue($"base-url|{dotnetVersion}|{Config.SourceBranch}", (JObject)ManifestVersions.Value["variables"]);

        public static string GetBuildVersion(DotNetImageType imageType, string dotnetVersion)
        {
            if (imageType == DotNetImageType.Runtime_Deps)
            {
                throw new NotSupportedException("Runtime deps has no associated build version");
            }

            return GetVariableValue(
                $"{imageType.ToString().ToLower()}|{dotnetVersion}|build-version",
                (JObject)ManifestVersions.Value["variables"]);
        }

        public static string GetFilterRegexPattern(string filter) =>
            filter is null ? null : $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$";
    }
}
