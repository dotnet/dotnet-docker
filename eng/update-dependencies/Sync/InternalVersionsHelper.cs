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

internal static class InternalVersionsHelper
{
    /// <summary>
    /// Records the staging pipeline run ID in an easy to parse format. This
    /// can be used by the sync-internal-release pipeline to record and
    /// re-apply the same staging builds after resetting the state of the repo
    /// to match the public release branch.
    /// </summary>
    /// <remarks>
    /// This will only store one staging pipeline run ID per dockerfileVersion
    /// </remarks>
    /// <param name="dockerfileVersion">major-minor version</param>
    /// <param name="stagingPipelineRunId">the build ID of the staging pipeline run</param>
    public static void RecordInternalVersion(string dockerfileVersion, int stagingPipelineRunId)
    {
        const string InternalVersionsFile = "internal-versions.txt";

        // Internal versions file should have one line per dockerfileVersion
        // Each line should be formatted as: <dockerfileVersion>=<stagingPipelineRunId>
        //
        // The preferable way to do this would be to record the version in
        // manifest.versions.json, however that would require one of the following:
        // 1) round-trip serialization, which would remove any whitespace/blank lines - which are
        //    important for keeping the file readable and reducing git merge conflicts
        // 2) lots of regex JSON manipulation which is error-prone and harder to maintain
        //
        // So for now, the separate file and format is a compromise.

        var versionsFilePath = Path.GetFullPath(SpecificCommand.VersionsFilename);
        var versionsFileDir = Path.GetDirectoryName(versionsFilePath) ?? "";
        var internalVersionFile = Path.Combine(versionsFileDir, InternalVersionsFile);

        InternalStagingBuilds builds;
        try
        {
            // File already exists - read existing versions
            var fileContents = File.ReadAllLines(internalVersionFile);
            builds = InternalStagingBuilds.Parse(fileContents);
        }
        catch (FileNotFoundException)
        {
            // File doesn't exist - it will be created
            builds = new InternalStagingBuilds(ImmutableDictionary<string, int>.Empty);
        }

        builds = builds.Add(dockerfileVersion, stagingPipelineRunId);
        File.WriteAllText(internalVersionFile, builds.ToString());
    }
}
