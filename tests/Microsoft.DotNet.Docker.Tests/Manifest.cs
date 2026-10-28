// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.DotNet.Docker.Tests;

// Simplified version of the ImageBuilder manifest model
// See https://github.com/dotnet/docker-tools/tree/main/src/Microsoft.DotNet.ImageBuilder/src/Models/Manifest
public class Manifest
{
    public List<Repo> Repos { get; set; } = [];
}

public class Repo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<Image> Images { get; set; } = [];
}

public class Image
{
    public Dictionary<string, object> SharedTags { get; set; } = [];
    public string ProductVersion { get; set; } = string.Empty;
    public string Syndication { get; set; } = string.Empty;
    public List<Platform> Platforms { get; set; } = [];
}

public class Platform
{
    public string Dockerfile { get; set; } = string.Empty;
    public Dictionary<string, object> Tags { get; set; } = [];
}
