// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.DotNet.Docker.Shared;

namespace Dotnet.Docker.Sync;

/// <inheritdoc/>
internal sealed class InternalVersionsService : IInternalVersionsService
{
    private const string InternalVersionsFileName = "stage-containers.txt";

    /// <inheritdoc/>
    public InternalStageContainers GetInternalStagingBuilds(string repoRoot)
    {
        var internalVersionFile = Path.Combine(repoRoot, InternalVersionsFileName);
        try
        {
            var fileContents = File.ReadAllLines(internalVersionFile);
            return InternalStageContainers.Parse(fileContents);
        }
        catch (FileNotFoundException)
        {
            return new InternalStageContainers(ImmutableDictionary<DotNetVersion, string>.Empty);
        }
    }

    /// <inheritdoc/>
    public void RecordInternalStagingBuild(string repoRoot, DotNetVersion dotNetVersion, string stageContainerName)
    {
        // Stage containers file should have one line per dockerfileVersion
        // Each line should be formatted as: <dockerfileVersion>=<stageContainerName>
        //
        // The preferable way to do this would be to record the version in
        // manifest.versions.json, however that would require one of the following:
        // 1) round-trip serialization, which would remove any whitespace/blank lines - which are
        //    important for keeping the file readable and reducing git merge conflicts
        // 2) lots of regex JSON manipulation which is error-prone and harder to maintain
        //
        // So for now, the separate file and format is a compromise.
        var builds = GetInternalStagingBuilds(repoRoot);
        builds = builds.Add(dotNetVersion, stageContainerName);
        var internalVersionFile = Path.Combine(repoRoot, InternalVersionsFileName);
        File.WriteAllText(internalVersionFile, builds.ToString());
    }
}
