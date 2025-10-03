// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Dotnet.Docker.Sync;

/// <summary>
/// Records information about what internal staging pipeline run IDs were used
/// for which .NET Dockerfile versions.
/// </summary>
/// <param name="Versions">
/// Mapping of Major.Minor .NET version to staging pipeline run ID.
/// </param>
internal sealed record InternalStagingBuilds(ImmutableDictionary<string, int> Versions)
{
    /// <summary>
    /// Parses <see cref=" InternalStagingBuilds"/> from lines of text.
    /// </summary>
    /// <remarks>
    /// Each line should be formatted as: <dockerfileVersion>=<stagingPipelineRunId>
    /// </remarks>
    public static InternalStagingBuilds Parse(IEnumerable<string> lines)
    {
        var versions = lines
            .Select(line => line.Split('=', 2))
            .Where(parts => parts.Length == 2)
            .ToImmutableDictionary(parts => parts[0], parts => int.Parse(parts[1]));

        return new InternalStagingBuilds(versions);
    }

    /// <summary>
    /// Returns a new <see cref="InternalStagingBuilds"/> with the specified
    /// version added.
    /// </summary>
    public InternalStagingBuilds Add(string dockerfileVersion, int stagingPipelineRunId) =>
        this with { Versions = Versions.SetItem(dockerfileVersion, stagingPipelineRunId) };

    public override string ToString() =>
        string.Join(Environment.NewLine, Versions.Select(kv => $"{kv.Key}={kv.Value}"));
}
