// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace Dotnet.Docker;

/// <summary>
/// Provides generic blob storage operations for Azure Storage Accounts.
/// </summary>
internal class StorageAccount
{
    private readonly BlobServiceClient _blobServiceClient;

    /// <summary>
    /// Creates a new instance of <see cref="StorageAccount"/>.
    /// </summary>
    /// <param name="url">The url of the storage account, for example <c>https://foo.blob.core.windows.net/</c></param>
    public StorageAccount(string url)
    {
        var credential = new DefaultAzureCredential();
        var storageAccountUri = new Uri(url);
        _blobServiceClient = new BlobServiceClient(storageAccountUri, credential);
    }

    /// <summary>
    /// Downloads a blob as text from the specified container.
    /// </summary>
    /// <param name="containerName">The name of the blob container</param>
    /// <param name="blobPath">The path to the blob within the container</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The blob content as a string</returns>
    public async Task<string> DownloadTextAsync(
        string containerName,
        string blobPath,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobPath);

        var response = await blobClient.DownloadContentAsync(cancellationToken);
        return response.Value.Content.ToString();
    }
}
