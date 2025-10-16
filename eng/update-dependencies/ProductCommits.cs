// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Microsoft.DotNet.ProductConstructionService.Client.Models;

namespace Dotnet.Docker;

/// <summary>
/// Represents the product versions for the SDK, runtime, and ASP.NET Core.
/// </summary>
internal partial record ProductVersions(
    ProductVersion Sdk,
    ProductVersion Runtime,
    ProductVersion AspNetCore)
{
    // Example: Sdk/10.0.100-rtm.25515.111/productVersion.txt
    // Find full lists of assets at https://aka.ms/bar
    [GeneratedRegex("^Sdk/.*/productVersion.txt$")]
    public static partial Regex SdkAssetRegex { get; }

    [GeneratedRegex("^aspnetcore/Runtime/.*/productVersion.txt$")]
    public static partial Regex AspNetCoreAssetRegex { get; }

    [GeneratedRegex("^Runtime/.*/productVersion.txt$")]
    public static partial Regex RuntimeAssetRegex { get; }

    public static ProductVersions FromVmrBuildAssets(IEnumerable<Asset> buildAssets) =>
        new ProductVersions(
            Sdk: GetAssetVersionMatchingRegex(buildAssets, SdkAssetRegex),
            Runtime: GetAssetVersionMatchingRegex(buildAssets, RuntimeAssetRegex),
            AspNetCore: GetAssetVersionMatchingRegex(buildAssets, AspNetCoreAssetRegex));

    private static ProductVersion GetAssetVersionMatchingRegex(IEnumerable<Asset> buildAssets, Regex assetRegex)
    {
        Asset asset = buildAssets.FirstOrDefault(a => assetRegex.IsMatch(a.Name))
            ?? throw new InvalidOperationException(
                $"Could not find asset matching regex '{assetRegex}' in build assets.");

        return new ProductVersion(asset.Version);
    }
};

internal record struct ProductVersion(string Version);
