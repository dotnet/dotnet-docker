// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

/// <summary>
/// Updates the PowerShell global tool installed in SDK images.
/// </summary>
/// <remarks>
/// Each .NET version has its own PowerShell version, download location, and checksums. Checksums
/// come from the SHA512SUMS file published next to the nupkgs, which may contain SHA-256 hashes
/// despite its name.
/// </remarks>
public sealed class PowerShellUpdater(
    HttpClient httpClient,
    IAzureCredentialProvider credentialProvider,
    ILogger<PowerShellUpdater> logger)
        : IVersionUpdater
{
    private const string ChecksumsFileName = "SHA512SUMS";

    public static string Name => "powershell";
    public static string VersionSourceName => "PowerShell/PowerShell";

    public Task<DependencyUpdate> ResolveFromVersionAsync(
        string version,
        bool isInternal,
        string? baseUrl,
        string? dotnetVersion,
        CancellationToken cancellationToken)
    {
        string series = GetSeries(version);

        var update = new DependencyUpdate(
            Description: $"Update PowerShell {series} to {version}",
            ApplyAsync: (manifestVariables, _, ct) =>
                ApplyAsync(manifestVariables, version, series, isInternal, baseUrl, dotnetVersion, ct),
            Scope: series);

        return Task.FromResult(update);
    }

    private async Task ApplyAsync(
        ManifestVariables variables,
        string version,
        string series,
        bool isInternal,
        string? baseUrl,
        string? dotnetVersion,
        CancellationToken cancellationToken)
    {
        List<string> targetDotnetVersions = GetDotnetVersions(variables, series, dotnetVersion);

        foreach (string targetDotnetVersion in targetDotnetVersions)
        {
            variables.SetValue(GetManifestVariableName(targetDotnetVersion, "build-version"), version);

            string baseUrlValue = GetBaseUrlValue(targetDotnetVersion, version, isInternal, baseUrl);
            variables.SetValue(GetManifestVariableName(targetDotnetVersion, "base-url"), baseUrlValue);
        }

        // Every target uses the same PowerShell version, so they share one download location.
        string resolvedBaseUrl = variables.GetValue(GetManifestVariableName(targetDotnetVersions[0], "base-url"));
        string checksumsUrl = $"{resolvedBaseUrl}/{ChecksumsFileName}";
        logger.LogInformation("Downloading PowerShell checksums from {Url}", checksumsUrl);

        string content = isInternal
            ? await DownloadInternalAsync(checksumsUrl, cancellationToken)
            : await httpClient.GetStringAsync(checksumsUrl, cancellationToken);

        Dictionary<string, string> checksums = ParseChecksums(content);

        foreach (string targetDotnetVersion in targetDotnetVersions)
        {
            UpdateChecksums(variables, targetDotnetVersion, version, checksums);
        }
    }

    private async Task<string> DownloadInternalAsync(string url, CancellationToken cancellationToken)
    {
        TokenCredential credential = credentialProvider.GetCredential(ServiceConnectionNames.Staging);
        var options = new BlobClientOptions { Transport = new HttpClientTransport(httpClient) };
        var blobClient = new BlobClient(new Uri(url), credential, options);

        Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync(cancellationToken);
        return response.Value.Content.ToString();
    }

    /// <summary>
    /// Gets the .NET versions to update: the requested one, or else every .NET version whose
    /// current PowerShell version is in the same major.minor series.
    /// </summary>
    private static List<string> GetDotnetVersions(ManifestVariables variables, string series, string? dotnetVersion)
    {
        if (dotnetVersion is not null)
        {
            // SetValue skips missing variables, so fail here instead of silently doing nothing.
            if (!variables.Contains(GetManifestVariableName(dotnetVersion, "build-version")))
            {
                throw new InvalidOperationException($"No PowerShell version is defined for .NET {dotnetVersion}.");
            }

            return [dotnetVersion];
        }

        List<string> dotnetVersions = variables.Names
            .Where(name => name.Split('|') is ["powershell", _, "build-version"])
            .Where(name => GetSeries(variables.GetValue(name)) == series)
            .Select(name => name.Split('|')[1])
            .ToList();

        if (dotnetVersions.Count == 0)
        {
            throw new InvalidOperationException(
                $"No .NET version currently uses PowerShell {series}. Use --dotnet-version to choose one.");
        }

        return dotnetVersions;
    }

    private static string GetBaseUrlValue(string dotnetVersion, string version, bool isInternal, string? baseUrl)
    {
        if (baseUrl is not null)
        {
            return baseUrl;
        }

        // Internal storage uses the version with dots replaced by dashes, e.g. v7-5-11-nuget.
        return isInternal
            ? $"$(powershell|base-url|internal)/v{version.Replace('.', '-')}-nuget/globaltool"
            : $"$(powershell|base-url|public)/$(powershell|{dotnetVersion}|build-version)";
    }

    // Parses lines of the form "<hash> *<file name>", where '*' marks binary mode and is optional.
    private static Dictionary<string, string> ParseChecksums(string content)
    {
        const StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        var checksums = new Dictionary<string, string>();

        foreach (string line in content.Split('\n', SplitOptions))
        {
            if (line.Split(' ', 2, SplitOptions) is not [var hash, var file])
            {
                throw new FormatException($"Unexpected line in {ChecksumsFileName}: '{line}'");
            }

            checksums[file.TrimStart('*')] = hash;
        }

        return checksums;
    }

    private static void UpdateChecksums(
        ManifestVariables variables,
        string dotnetVersion,
        string version,
        Dictionary<string, string> checksums)
    {
        foreach (string variableName in variables.Names)
        {
            // powershell|9.0|Linux|x64|sha -> PowerShell.Linux.x64.7.5.11.nupkg
            if (variableName.Split('|') is not ["powershell", var variableDotnetVersion, .. var platformParts, "sha"]
                || variableDotnetVersion != dotnetVersion)
            {
                continue;
            }

            string fileName = $"PowerShell.{string.Join('.', platformParts)}.{version}.nupkg";

            if (!checksums.TryGetValue(fileName, out string? checksum))
            {
                throw new InvalidOperationException($"'{fileName}' is not listed in {ChecksumsFileName}.");
            }

            variables.SetValue(variableName, checksum);
        }

        // Every file in SHA512SUMS uses the same hash algorithm, so any checksum identifies it.
        string shaFunction = GetShaFunction(checksums.Values.First());
        variables.SetValue(GetManifestVariableName(dotnetVersion, "sha-function"), shaFunction);
    }

    private static string GetShaFunction(string checksum) =>
        checksum.Length switch
        {
            64 => "256",
            128 => "512",
            _ => throw new FormatException($"Unexpected checksum length {checksum.Length}: '{checksum}'"),
        };

    private static string GetSeries(string version)
    {
        var parsedVersion = SemanticVersion.Parse(version);
        return $"{parsedVersion.Major}.{parsedVersion.Minor}";
    }

    private static string GetManifestVariableName(string dotnetVersion, string type) =>
        $"powershell|{dotnetVersion}|{type}";
}
