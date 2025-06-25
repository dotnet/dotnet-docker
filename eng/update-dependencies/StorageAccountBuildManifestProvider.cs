// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Dotnet.Docker.Model.Release;

namespace Dotnet.Docker;

internal class StorageAccountBuildManifestProvider(StorageAccount stagingStorageAccount) : IBuildManifestProvider
{
    private readonly StorageAccount _storageAccount = stagingStorageAccount;

    /// <inheritdoc/>
    public async Task<BuildManifest> GetBuildManifestAsync(int stagingPipelineRunId)
    {
        // Each pipeline run has a corresponding blob container named stage-${options.StagingPipelineRunId}.
        // Release metadata is stored in metadata/ReleaseManifest.json.
        // Release assets are stored individually under in assets/shipping/assets/[Sdk|Runtime|aspnetcore|...].
        string releaseManifestJson = await _storageAccount.DownloadTextAsync(
            containerName: $"stage-{stagingPipelineRunId}",
            blobPath: "metadata/ReleaseManifest.json"
        );

        return BuildManifest.FromJson(releaseManifestJson);
    }
}
