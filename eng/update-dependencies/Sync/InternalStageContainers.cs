// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.DotNet.Docker.Shared;

namespace Dotnet.Docker.Sync;

/// <summary>
/// Records information about what stage containers were used for which .NET Dockerfile versions.
/// </summary>
/// <param name="Versions">
/// Mapping of Major.Minor .NET version to stage container name.
/// </param>
internal sealed record InternalStageContainers(ImmutableDictionary<DotNetVersion, string> Versions)
{
    /// <summary>
    /// Parses <see cref=" InternalStageContainers"/> from lines of text.
    /// </summary>
    /// <remarks>
    /// Each line should be formatted as: &lt;dockerfileVersion&gt;=&lt;stageContainer&gt;
    /// </remarks>
    public static InternalStageContainers Parse(IEnumerable<string> lines)
    {
        var versions = lines
            .Select(line => line.Split('=', 2))
            .Where(parts => parts.Length == 2)
            .ToImmutableDictionary(
                // Reduce the version to major.minor only.
                // If we don't, we could end up with multiple entries for the same version.
                parts => DotNetVersion.Parse(parts[0]).ToMajorMinorVersion(),
                parts => parts[1]);

        return new InternalStageContainers(versions);
    }

    /// <summary>
    /// Returns a new <see cref="InternalStageContainers"/> with the specified
    /// version added.
    /// </summary>
    public InternalStageContainers Add(DotNetVersion dotNetVersion, string stageContainer) =>
        this with { Versions = Versions.SetItem(dotNetVersion.ToMajorMinorVersion(), stageContainer) };

    // Internal versions file should have one line per dockerfileVersion, and
    // each line should be formatted as: <dockerfileVersion>=<stageContainer>
    public override string ToString() =>
        string.Join(Environment.NewLine,
            Versions
                .OrderBy(kv => kv.Key)
                .Select(kv => $"{kv.Key.ToString(2)}={kv.Value}"));
}
