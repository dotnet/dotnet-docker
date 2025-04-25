// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker;

internal interface IBuildAssetService
{
    Task<string> GetAssetTextContentsAsync(Asset asset);
}

internal class BuildAssetService(
    HttpClient httpClient,
    ILogger<BuildAssetService> logger)
    : IBuildAssetService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<BuildAssetService> _logger = logger;

    public async Task<string> GetAssetTextContentsAsync(Asset asset)
    {
        string url = ResolveAssetUrl(asset);
        try
        {
            _logger.LogInformation("Fetching contents from {url}", url);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(
                """
                Successfully fetched contents from {url}:
                {content}
                """,
                url, content);
            // Remove trailing newlines
            return content.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch contents from {url}", url);
            throw;
        }
    }

    private string ResolveAssetUrl(Asset asset)
    {
        var blobStorageLocations = asset.Locations.Where(l => l.Location.Contains("blob.core.windows.net"));
        var azdoLocations = asset.Locations.Where(l => l.Location.Contains("dev.azure.com"));

        string allLocations =
            string.Join(Environment.NewLine, asset.Locations.Select(l => $"Type: {l.Type}; Url: {l.Location}"));
        _logger.LogInformation(
            """
            Asset {asset.Name} has {asset.Locations.Count} locations:
            {allLocations}
            """,
            asset.Name, asset.Locations.Count, allLocations);

        // Prefer public blob storage locations over azdo locations
        AssetLocation bestLocation =
            blobStorageLocations.FirstOrDefault()
            ?? azdoLocations.FirstOrDefault()
            ?? throw new InvalidOperationException(FormatErrorMessage(asset, "does not have any valid locations"));

        string url = $"{bestLocation.Location}/{asset.Name}";
        _logger.LogInformation("Using location {url}", url);
        return url;
    }

    private static string FormatErrorMessage(Asset asset, string message)
    {
        return $"Build {asset.BuildId} Asset {asset.Name} (version {asset.Version}) {message}";
    }
}
