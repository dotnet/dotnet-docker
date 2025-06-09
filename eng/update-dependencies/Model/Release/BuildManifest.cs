// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Dotnet.Docker.Model.Release;

// The models in this file are a subset of the BuildManifest models used by the .NET release infrastructure.
// They should stay in sync.
// https://dev.azure.com/dnceng/internal/_git/dotnet-release?path=/src/Microsoft.DotNet.Release/Microsoft.DotNet.ReleaseLib/Models/BuildManifest.cs

/// <summary>
/// Represents a single release of .NET. This can contain multiple channels but typically spans only one major version
/// of .NET. For example, a .NET 8.0 BuildManifest would contain all .NET 8.0.1XX/2XX/3XX SDKs, but no versions of .NET
/// 9.0.
/// </summary>
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

    public static BuildManifest FromJson(string json) =>
        JsonSerializer.Deserialize<BuildManifest>(json, s_jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {nameof(BuildManifest)}: " + json);
}

/// <summary>
/// Represents a single, unique build on the .NET build asset registry (BAR). This is a unique build of a single commit
/// of a single repository.
/// </summary>
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

/// <summary>
/// Represents one single downloadable asset from a .NET build. Can be a binary, NuGet package, text file, or any other
/// type of file that is produced by a .NET build.
/// </summary>
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

/// <summary>
/// Represents a .NET build asset registry (BAR) channel. Channels group together builds of different repositories that
/// must ship together. An example channel might be something like ".NET SDK 10.0.100-preview.1".
/// </summary>
/// <param name="Id">The ID of the channel</param>
/// <param name="Name">The name of the channel</param>
public record Channel(long Id, string Name);
