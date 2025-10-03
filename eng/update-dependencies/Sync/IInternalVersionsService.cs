// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker.Sync;

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
