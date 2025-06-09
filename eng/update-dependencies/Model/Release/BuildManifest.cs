// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Dotnet.Docker.Model.Release;

public record BuildManifest
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public required Build[] Builds { get; init; }
    public required Asset[] ExtraAssets { get; init; }

    public IEnumerable<Asset> AllAssets =>
        Builds
            .SelectMany(build => build.Assets)
            .Concat(ExtraAssets);

    public override string ToString() =>
        JsonSerializer.Serialize(this, s_jsonOptions);

    public static BuildManifest FromJson(string json) =>
        JsonSerializer.Deserialize<BuildManifest>(json, s_jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {nameof(BuildManifest)}: " + json);
}

public record Build
{
    public required Uri Repo { get; init; }
    public required string Commit { get; init; }
    public required string Branch { get; init; }
    public required DateTimeOffset Produced { get; init; }
    public required string BuildNumber { get; init; }
    public required long BarBuildId { get; init; }
    public required Channel[] Channels { get; init; }
    public required Asset[] Assets { get; init; }
}

public record Asset
{
    public required string Name { get; init; }
    public string? Origin { get; init; } = null;
    public bool? DotnetReleaseShipping { get; init; } = false;
    public required string Version { get; init; }
    public required bool NonShipping { get; init; }
    public required Uri Source { get; init; }
    public required long BarAssetId { get; init; }
}

public record Channel(long Id, string Name);
