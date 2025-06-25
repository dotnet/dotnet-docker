// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Dotnet.Docker.Model.Release;

namespace Dotnet.Docker;

internal interface IBuildManifestProvider
{
    /// <summary>
    /// Gets the build manifest for a given staging pipeline run ID.
    /// </summary>
    /// <param name="stagingPipelineRunId">
    /// The staging pipeline run ID. This is usually a 6 or 7 digit long
    /// number. You can find it in the URL of the Azure DevOps pipeline run.
    /// For example, the build ID for the following URL is 1234567:
    /// https://dev.azure.com/${org}/${project}/_build/results?buildId=1234567
    /// </param>
    /// <returns>
    /// BuildManifest which contains metadata about the .NET build that was
    /// produced or staged in the given pipeline run.
    /// </returns>
    Task<BuildManifest> GetBuildManifestAsync(int stagingPipelineRunId);
}
