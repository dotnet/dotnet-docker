// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests
{
    public static class ManifestHelper
    {
        public record DockerfileInfo(string Repo, string MajorMinor, string Os, string Architecture);

        public static Manifest GetManifest() =>
            JsonConvert.DeserializeObject<Manifest>(Config.Manifest.Value.ToString()) ??
            throw new Exception("Failed to deserialize manifest");

        public static string GetResolvedProductVersion(Image image) =>
            GetVariableValue(image.ProductVersion);

        public static IEnumerable<string> GetResolvedProductVersions(Repo repo) =>
            repo.Images.Select(GetResolvedProductVersion).Distinct();

        public static List<string> GetResolvedSharedTags(Image image) =>
            image.SharedTags.Keys.Select(GetVariableValue).ToList();

        public static List<string> GetResolvedTags(Platform platform) =>
            platform.Tags.Keys.Select(GetVariableValue).ToList();

        private static readonly string DockerfileRegex = @"src/(?<repo>.+)/(?<major_minor>\d+\.\d+)/(?<os>.+)/(?<architecture>.+)";

        public static Dictionary<DockerfileInfo, List<string>> GetDockerfileTags(Repo repo)
        {
            Dictionary<DockerfileInfo, List<string>> dockerfileTags = new Dictionary<DockerfileInfo, List<string>>();
            foreach (Image image in repo.Images)
            {
                foreach (Platform platform in image.Platforms)
                {
                    DockerfileInfo dockerfileInfo = GetDockerfileInfo(platform.Dockerfile);
                    if (!dockerfileTags.ContainsKey(dockerfileInfo))
                    {
                        dockerfileTags[dockerfileInfo] = new List<string>();
                    }
                    dockerfileTags[dockerfileInfo].AddRange(GetResolvedTags(platform));
                    dockerfileTags[dockerfileInfo].AddRange(GetResolvedSharedTags(image));
                }
            }
            return dockerfileTags;
        }

        public static DockerfileInfo GetDockerfileInfo(string dockerfile)
        {
            var match = Regex.Match(dockerfile, DockerfileRegex);
            if (!match.Success)
            {
                throw new Exception($"Failed to parse dockerfile: {dockerfile}");
            }
            return new DockerfileInfo(match.Groups["repo"].Value, match.Groups["major_minor"].Value, match.Groups["os"].Value, match.Groups["architecture"].Value);
        }

        private static string GetVariableValue(string input)
        {
            string variablePattern = @"\$\((?<variable>.+)\)";
            foreach (Match match in Regex.Matches(input, variablePattern))
            {
                string variableName = Config.GetVariableValue(match.Groups["variable"].Value);
                input = input.Replace(match.Value, variableName);
            }
            return input;
        }
    }
}
