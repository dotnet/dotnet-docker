// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Docker.Model.Release;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Build.WebApi;

namespace Dotnet.Docker;

/// <summary>
/// Represents a single file in an artifact from a pipeline run.
/// </summary>
/// <param name="ArtifactName">
/// The name of the pipeline artifact that the file resides in.
/// </param>
/// <param name="SubPath">
/// The sub-path of the specific file within the Artifact.
/// </param>
internal record PipelineArtifactFile(string ArtifactName, string SubPath);

internal class PipelineArtifactProvider(
    AzdoAuthProvider azdoAuthProvider,
    ILogger<PipelineArtifactProvider> logger,
    AzdoHttpClient azdoHttpClient
)
{
    private readonly AzdoAuthProvider _azdoAuthProvider = azdoAuthProvider;
    private readonly ILogger<PipelineArtifactProvider> _logger = logger;
    private readonly AzdoHttpClient _azdoHttpClient = azdoHttpClient;

    private static readonly IEnumerable<PipelineArtifactFile> s_releaseConfigFiles =
    [
        // Newest version of the staging pipeline
        new PipelineArtifactFile("metadata", "ReleaseConfig.json"),
        // Old version
        new PipelineArtifactFile("drop", "config.json"),
    ];

    /// <summary>
    /// Gets the .NET release config from a run of the staging pipeline.
    /// </summary>
    public async Task<ReleaseConfig> GetReleaseConfigAsync(
        string azdoOrganization,
        string azdoProject,
        int stagingPipelineRunId)
    {
        var releaseConfigJson = await GetArtifactTextContentAsync(
            azdoOrganization,
            azdoProject,
            stagingPipelineRunId,
            s_releaseConfigFiles);

        return ReleaseConfig.FromJson(releaseConfigJson);
    }

    /// <summary>
    /// Downloads text content from a pipeline artifact file.
    /// </summary>
    /// <param name="azdoOrganization">
    /// The URI of the Azure DevOps organization or collection.
    /// For example: https://dev.azure.com/fabrikamfiber/.
    /// </param>
    /// <param name="azdoProject">Azure DevOps project</param>
    /// <param name="stagingPipelineRunId">Pipeline run ID</param>
    /// <param name="artifactsToTry">The collection of artifact files to try, in order.</param>
    /// <returns>The artifact content as a string.</returns>
    private async Task<string> GetArtifactTextContentAsync(
        string azdoOrganization,
        string azdoProject,
        int stagingPipelineRunId,
        IEnumerable<PipelineArtifactFile> artifactsToTry)
    {
        if (string.IsNullOrWhiteSpace(azdoOrganization))
        {
            throw new ArgumentException("--azdo-organization is required", nameof(azdoOrganization));
        }
        if (string.IsNullOrWhiteSpace(azdoProject))
        {
            throw new ArgumentException("--azdo-project is required", nameof(azdoProject));
        }

        var connection = _azdoAuthProvider.GetVssConnection(azdoOrganization);
        var buildsClient = connection.GetClient<BuildHttpClient>();

        List<Exception> exceptions = [];

        foreach (PipelineArtifactFile pipelineArtifact in artifactsToTry)
        {
            _logger.LogInformation(
                "Trying to get artifact from pipeline run {stagingPipelineRunId} using {artifactFile}",
                stagingPipelineRunId, pipelineArtifact);

            try
            {
                // Attempt to get the artifact for the specified pipeline run
                BuildArtifact resolvedArtifact = await buildsClient.GetArtifactAsync(
                    azdoProject,
                    stagingPipelineRunId,
                    pipelineArtifact.ArtifactName);

                // If the artifact is found, download the specific file within it
                string downloadUrl = resolvedArtifact.Resource.DownloadUrl;
                downloadUrl = downloadUrl.Replace(
                    "/content?format=zip",
                    $"/content?format=file&subPath=%2F{pipelineArtifact.SubPath}");

                _logger.LogInformation(
                    "Downloading artifact {pipelineArtifact} from {downloadUrl}",
                    pipelineArtifact, downloadUrl);
                using var response = await _azdoHttpClient.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();
                var textContent = await response.Content.ReadAsStringAsync();
                return textContent;
            }
            catch (Exception e)
            {
                // Log the exception and continue to the next artifact file
                _logger.LogError(e, "Failed to retrieve {artifactFile}: {message}", pipelineArtifact, e.Message);
                exceptions.Add(e);
            }
        }

        // If all attempts fail, throw an exception with the details
        throw new AggregateException("Failed to retrieve artifact content.", exceptions);
    }
}
