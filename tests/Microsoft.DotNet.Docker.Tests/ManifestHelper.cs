// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests
{
    public static class ManifestHelper
    {
        public static SimplifiedManifest GetManifest() =>
            JsonConvert.DeserializeObject<SimplifiedManifest>(Config.Manifest.Value.ToString())!;

        public static string GetResolvedProductVersion(Image image) =>
            GetVariableValue(image.ProductVersion);

        public static List<string> GetResolvedSharedTags(Image image) =>
            image.SharedTags.Keys.Select(tag => GetVariableValue(tag)).ToList();

        public static List<string> GetResolvedTags(Platform platform) =>
            platform.Tags.Keys.Select(tag => GetVariableValue(tag)).ToList();

        private static string GetVariableValue(string input)
        {
            string variablePattern = @"\$\(.+\)";
            var match = Regex.Match(input, variablePattern);
            if (!match.Success)
            {
                return input;
            }
            string variableName = match.Value.Replace("$(", string.Empty).Replace(")", string.Empty);
            string remainingString = input.Split(match.Value)[1];
            return Config.GetVariableValue(variableName) + remainingString;
        }

        public class SimplifiedManifest
        {
            public List<Repo> Repos { get; set; } = new List<Repo>();
        }

        public class Repo
        {
            public string Name { get; set; } = string.Empty;
            public List<Image> Images { get; set; } = new List<Image>();
        }

        public class Image
        {
            public Dictionary<string, object> SharedTags { get; set; } = new Dictionary<string, object>();
            public string ProductVersion { get; set; } = string.Empty;
            public List<Platform> Platforms { get; set; } = new List<Platform>();
        }

        public class Platform
        {
            public string Dockerfile { get; set; } = string.Empty;
            public Dictionary<string, object> Tags { get; set; } = new Dictionary<string, object>();
        }
    }
}
