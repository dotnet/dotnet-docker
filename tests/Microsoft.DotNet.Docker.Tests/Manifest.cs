// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.DotNet.Docker.Tests
{
    // Simplified version of the ImageBuilder manifest model
    // See https://github.com/dotnet/docker-tools/tree/main/src/Microsoft.DotNet.ImageBuilder/src/Models/Manifest
    public class Manifest
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
