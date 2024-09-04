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
        public static Manifest GetManifest() =>
            JsonConvert.DeserializeObject<Manifest>(Config.Manifest.Value.ToString()) ??
            throw new Exception("Failed to deserialize manifest");

        public static string GetResolvedProductVersion(Image image) =>
            GetVariableValue(image.ProductVersion);

        public static List<string> GetResolvedSharedTags(Image image) =>
            image.SharedTags.Keys.Select(GetVariableValue).ToList();

        public static List<string> GetResolvedTags(Platform platform) =>
            platform.Tags.Keys.Select(GetVariableValue).ToList();

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
