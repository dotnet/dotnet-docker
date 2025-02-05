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
        private static Lazy<JObject> ManifestVersions { get; } =
            new Lazy<JObject>(() => LoadManifest("manifest.versions.json"));

        public static Lazy<JObject> Manifest { get; } = new Lazy<JObject>(() => LoadManifest("manifest.json"));
        public static string SourceRepoRoot { get; } =
            GetEnvironmentVariableOrDefault("SOURCE_REPO_ROOT", string.Empty);
        public static bool IsHttpVerificationDisabled { get; } =
            Environment.GetEnvironmentVariable("DISABLE_HTTP_VERIFICATION") != null;
        public static bool PullImages { get; } =
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PULL_IMAGES"));
        public static bool IsNightlyRepo { get; } = GetIsNightlyRepo();
        public static bool IsRunningInContainer { get; } =
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RUNNING_TESTS_IN_CONTAINER"));
        public static string RepoPrefix { get; } = GetEnvironmentVariableOrDefault("REPO_PREFIX", string.Empty);
        public static string Registry { get; } =
            GetEnvironmentVariableOrDefault("REGISTRY", (string)Manifest.Value["registry"]);
        public static string CacheRegistry { get; } =
            GetEnvironmentVariableOrDefault("CACHE_REGISTRY", string.Empty);
        public static string[] OsNames { get; } =
            GetEnvironmentVariableOrDefault("IMAGE_OS_NAMES", string.Empty)
                .Split(",", StringSplitOptions.RemoveEmptyEntries);
        public static string SourceBranch { get; } =
            GetEnvironmentVariableOrDefault("SOURCE_BRANCH", string.Empty);
        public static string InternalAccessToken { get; } =
            GetEnvironmentVariableOrDefault("INTERNAL_ACCESS_TOKEN", string.Empty);
        public static string[] Paths { get; } =
            Environment.GetEnvironmentVariable("DOCKERFILE_PATHS")?
                .Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];

        public static bool IsInternal { get; } =
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("INTERNAL_TESTING"));

        private static string GetEnvironmentVariableOrDefault(string variableName, string defaultValue)
        {
            string value = Environment.GetEnvironmentVariable(variableName);
            return !string.IsNullOrEmpty(value) ? value : defaultValue;
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

        public static string GetVariableValue(string variableName) =>
            GetVariableValue(variableName, (JObject)ManifestVersions.Value["variables"]);

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
            GetVariableValue($"dotnet|{dotnetVersion}|base-url|{Config.SourceBranch}", (JObject)ManifestVersions.Value["variables"]);

        public static string GetBuildVersion(DotNetImageRepo imageRepo, string dotnetVersion)
        {
            if (imageRepo == DotNetImageRepo.Runtime_Deps)
            {
                throw new NotSupportedException("Runtime deps has no associated build version");
            }

            return GetVariableValue(
                $"{imageRepo.ToString().ToLower()}|{dotnetVersion}|build-version",
                (JObject)ManifestVersions.Value["variables"]);
        }

        public static string GetFilterRegexPattern(string filter) =>
            filter is null ? null : $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$";
    }
}
