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
        private static Lazy<JObject> Manifest { get; } = new Lazy<JObject>(() => LoadManifest());

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
        public static string Os { get; } =
            Environment.GetEnvironmentVariable("IMAGE_OS") ?? string.Empty;

        private static bool GetIsNightlyRepo()
        {
            string repo = (string)Manifest.Value["repos"][0]["name"];
            return repo.Contains("/nightly/");
        }

        private static JObject LoadManifest()
        {
            string manifestPath = Path.Combine(SourceRepoRoot, "manifest.json");
            string manifestJson = File.ReadAllText(manifestPath);
            return JObject.Parse(manifestJson);
        }

        public static string GetFilterRegexPattern(string filter) =>
            filter is null ? null : $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$";
    }
}
