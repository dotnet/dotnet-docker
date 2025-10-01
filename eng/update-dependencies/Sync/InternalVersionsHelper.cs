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

/// <summary>
/// Abstraction for recording and reading internal staging build information
/// local to the repo.
/// </summary>
/// <remarks>
/// This can be used by the sync-internal-release pipeline to record and
/// re-apply the same staging builds after resetting the state of the repo to
/// match the public release branch.
/// </remarks>
internal interface IInternalVersionsService
{
    /// <summary>
    /// Records a staging pipeline run ID in the repo.
    /// </summary>
    /// <remarks>
    /// This will only store one staging pipeline run ID per dockerfileVersion.
    /// If a version already exists for the same dockerfileVersion, it will be
    /// overwritten.
    /// </remarks>
    /// <param name="dockerfileVersion">major-minor version</param>
    /// <param name="stagingPipelineRunId">the build ID of the staging pipeline run</param>
    void RecordInternalStagingBuild(string repoRoot, string dockerfileVersion, int stagingPipelineRunId);

    /// <summary>
    /// Gets any previously recorded internal staging builds in the repo.
    /// </summary>
    InternalStagingBuilds GetInternalStagingBuilds(string repoRoot);
}

/// <inheritdoc/>
internal sealed class InternalVersionsService : IInternalVersionsService
{
    private const string InternalVersionsFileName = "internal-versions.txt";

    /// <inheritdoc/>
    public InternalStagingBuilds GetInternalStagingBuilds(string repoRoot)
    {
        var internalVersionFile = Path.Combine(repoRoot, InternalVersionsFileName);
        try
        {
            var fileContents = File.ReadAllLines(internalVersionFile);
            return InternalStagingBuilds.Parse(fileContents);
        }
        catch (FileNotFoundException)
        {
            return new InternalStagingBuilds(ImmutableDictionary<string, int>.Empty);
        }
    }

    /// <inheritdoc/>
    public void RecordInternalStagingBuild(string repoRoot, string dockerfileVersion, int stagingPipelineRunId)
    {
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

        // Internal versions file should have one line per dockerfileVersion
        // Each line should be formatted as: <dockerfileVersion>=<stagingPipelineRunId>
        var builds = GetInternalStagingBuilds(repoRoot).Add(dockerfileVersion, stagingPipelineRunId);
        var internalVersionFile = Path.Combine(repoRoot, InternalVersionsFileName);
        File.WriteAllText(internalVersionFile, builds.ToString());
    }
}
