// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Dotnet.Docker.Model.Release;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal class StorageAccountBuildManifestProvider
{
    private readonly Lazy<TokenCredential> _azureCredential;
    private readonly ILogger<StorageAccountBuildManifestProvider> _logger;

    /// <summary>
    /// Creates a new instance of <see cref="StorageAccountBuildManifestProvider"/>.
    /// </summary>
    /// <param name="url">The url of the storage account, for example <c>https://foo.blob.core.windows.net/</c></param>
    public StorageAccountBuildManifestProvider(ILogger<StorageAccountBuildManifestProvider> logger)
    {
        _azureCredential = new(() => new DefaultAzureCredential());
        _logger = logger;
    }

    /// <summary>
    /// Gets the build manifest from the specified staging pipeline run.
    /// </summary>
    /// <param name="stagingPipelineRunId">The staging pipeline run ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The build manifest.</returns>
    public async Task<BuildManifest> GetBuildManifestAsync(
        string storageAccountUrl,
        int stagingPipelineRunId,
        CancellationToken cancellationToken = default)
    {
        // Each pipeline run has a corresponding blob container named stage-${options.StagingPipelineRunId}.
        // Release metadata is stored in metadata/ReleaseManifest.json.
        // Release assets are stored individually under in assets/shipping/assets/[Sdk|Runtime|aspnetcore|...].
        string containerName = $"stage-{stagingPipelineRunId}";
        string blobName = "metadata/ReleaseManifest.json";

        _logger.LogInformation(
            "Downloading build manifest from storage account {storageAccountUrl}/{containerName}/{blobName}",
            storageAccountUrl, containerName, blobName);

        var storageAccountUri = new Uri(storageAccountUrl);
        var blobServiceClient = new BlobServiceClient(storageAccountUri, _azureCredential.Value);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DownloadContentAsync(cancellationToken);

        var releaseManifestJson = response.Value.Content.ToString();
        return BuildManifest.FromJson(releaseManifestJson);
    }
}
