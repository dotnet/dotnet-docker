// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Linq;
using System.Text.RegularExpressions;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests
{
    public static class ManifestHelper
    {
        public static SimplifiedManifest GetManifest() =>
            JsonConvert.DeserializeObject<SimplifiedManifest>(Config.Manifest.Value.ToString())!;

        public class SimplifiedManifest
        {
            public List<Repo> Repos { get; set; } = new List<Repo>();
        }

        public class Repo
        {
            public string Name { get; set; } = string.Empty;
            public List<Image> Images { get; set; } = new List<Image>();

            [JsonIgnore]
            public Dictionary<string, List<string>> DockerfileTags { get; set; } = new Dictionary<string, List<string>>();

            [OnDeserialized]
            internal void SetDockerfileTags(StreamingContext context)
            {
                foreach (Image image in Images)
                {
                    foreach (Platform platform in image.Platforms)
                    {
                        if (!DockerfileTags.ContainsKey(platform.Dockerfile))
                        {
                            DockerfileTags[platform.Dockerfile] = new List<string>();
                        }

                        DockerfileTags[platform.Dockerfile].AddRange(platform.GetTags());
                        DockerfileTags[platform.Dockerfile].AddRange(image.GetSharedTags());
                        DockerfileTags[platform.Dockerfile] = DockerfileTags[platform.Dockerfile].Distinct().ToList();
                    }
                }
            }
        }

        public class Image
        {
            [JsonIgnore]
            private string _productVersion = string.Empty;

            [JsonProperty]
            [JsonConverter(typeof(TagsDictionaryConverter))]
            private Dictionary<string, object> SharedTags { get; set; } = new Dictionary<string, object>();
            public string ProductVersion { get => _productVersion; set => _productVersion = GetVariableValue(value); }
            public List<Platform> Platforms { get; set; } = new List<Platform>();
            public List<string> GetSharedTags() => SharedTags.Keys.ToList();
        }

        public class Platform
        {
            public string Dockerfile { get; set; } = string.Empty;

            [JsonProperty]
            [JsonConverter(typeof(TagsDictionaryConverter))]
            private Dictionary<string, object> Tags { get; set; } = new Dictionary<string, object>();
            public List<string> GetTags() => Tags.Keys.ToList();
        }

        internal class TagsDictionaryConverter : JsonConverter<Dictionary<string, object>>
        {
            public override Dictionary<string, object> ReadJson(
                JsonReader reader,
                Type objectType,
                Dictionary<string, object>? existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                var tagsDictionary = new Dictionary<string, object>();
                var jObject = JObject.Load(reader);

                foreach (var property in jObject.Properties())
                {
                    string modifiedKey = GetVariableValue(property.Name);
                    tagsDictionary.Add(modifiedKey, property.Value.ToObject<object>()!);
                }

                return tagsDictionary;
            }

            public override void WriteJson(
                JsonWriter writer,
                Dictionary<string, object>? value,
                JsonSerializer serializer)
            {
                writer.WriteStartObject();
                foreach (var kvp in value!)
                {
                    writer.WritePropertyName(kvp.Key);
                    serializer.Serialize(writer, kvp.Value);
                }
                writer.WriteEndObject();
            }
        }

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
    }
}
