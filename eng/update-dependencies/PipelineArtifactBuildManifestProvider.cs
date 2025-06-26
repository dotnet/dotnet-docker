// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dotnet.Docker.Model.Release;
using Microsoft.Build.Framework;
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

internal class PipelineArtifactBuildManifestProvider(
    AzdoAuthProvider azdoAuthProvider,
    ILogger<PipelineArtifactBuildManifestProvider> logger,
    AzdoHttpClient azdoHttpClient
)
{
    private readonly AzdoAuthProvider _azdoAuthProvider = azdoAuthProvider;
    private readonly ILogger<PipelineArtifactBuildManifestProvider> _logger = logger;
    private readonly AzdoHttpClient _azdoHttpClient = azdoHttpClient;

    // The release staging pipeline has multiple versions, and the build
    // manifest has a different location depending on whether it's using the
    // new or old version of the staging pipeline.
    // These files will be tried, in order, to retrieve the build manifest.
    private readonly IEnumerable<PipelineArtifactFile> _buildManifestFiles =
    [
        // Newest version of the staging pipeline
        new PipelineArtifactFile("metadata", "ReleaseManifest.json"),
        // Old version
        new PipelineArtifactFile("manifests", "manifest.json"),
    ];

    /// <inheritdoc/>
    public async Task<BuildManifest> GetBuildManifestAsync(
        string azdoOrganization,
        string azdoProject,
        int stagingPipelineRunId)
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

        foreach (var artifactFile in _buildManifestFiles)
        {
            _logger.LogInformation(
                "Trying to get build manifest from pipeline run {stagingPipelineRunId} using {artifactFile}",
                stagingPipelineRunId, artifactFile
            );

            try
            {
                // Attempt to get the artifact for the specified pipeline run
                BuildArtifact artifact = await buildsClient.GetArtifactAsync(
                    azdoProject,
                    stagingPipelineRunId,
                    artifactFile.ArtifactName
                );

                // If the artifact is found, download the specific file within it
                string downloadUrl = artifact.Resource.DownloadUrl;
                downloadUrl = downloadUrl.Replace("/content?format=zip", $"/content?format=file&subPath=%2F{artifactFile.SubPath}");

                _logger.LogInformation(
                    "Downloading build manifest from artifact {artifactName} at {downloadUrl}",
                    artifactFile.ArtifactName, downloadUrl
                );

                using var response = await _azdoHttpClient.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();
                var buildManifestJson = await response.Content.ReadAsStringAsync();

                return BuildManifest.FromJson(buildManifestJson);
            }
            catch (Exception e)
            {
                // Log the exception and continue to the next artifact file
                _logger.LogError(e, "Failed to retrieve {artifactFile}: {message}", artifactFile, e.Message);
                exceptions.Add(e);
            }
        }

        // If all attempts fail, throw an exception with the details
        throw new AggregateException("Failed to retrieve build manifest", exceptions);
    }
}
