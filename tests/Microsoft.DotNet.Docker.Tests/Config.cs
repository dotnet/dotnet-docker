// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class Config
    {
        public static string RepoName { get; } = Environment.GetEnvironmentVariable("REPO") ?? GetManifestRepoName();
        public static bool IsHttpVerificationDisabled { get; } =
            Environment.GetEnvironmentVariable("DISABLE_HTTP_VERIFICATION") != null;
        public static bool IsLocalRun { get; } = Environment.GetEnvironmentVariable("LOCAL_RUN") != null;
        public static bool IsNightlyRepo { get; } = RepoName.Contains("nightly");
        public static bool IsRunningInContainer { get; } =
            Environment.GetEnvironmentVariable("RUNNING_TESTS_IN_CONTAINER") != null;

        private static string GetManifestRepoName()
        {
            string manifestJson = File.ReadAllText("manifest.json");
            JObject manifest = JObject.Parse(manifestJson);
            return (string)manifest["repos"][0]["name"];
        }
    }
}
