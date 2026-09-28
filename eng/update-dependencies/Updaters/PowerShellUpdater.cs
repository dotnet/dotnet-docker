// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
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
        : IVersionUpdater, ILatestVersionUpdater
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
        string series = GetSeries(SemanticVersion.Parse(version));

        var update = new DependencyUpdate(
            Description: $"Update PowerShell {series} to {version}",
            ApplyAsync: (manifestVariables, _, ct) =>
                ApplyAsync(manifestVariables, version, series, isInternal, baseUrl, dotnetVersion, ct),
            Scope: series);

        return Task.FromResult(update);
    }

    public async Task<IReadOnlyList<DependencyUpdate>> ResolveLatestAsync(
        ManifestVariables variables,
        bool isInternal,
        string? dotnetVersion,
        CancellationToken cancellationToken)
    {
        IEnumerable<string> dotnetVersions = dotnetVersion is null
            ? GetAllDotnetVersions(variables)
            : [dotnetVersion];

        // .NET versions on the same PowerShell series share one update. Starting from the newest of their versions
        // means the update never lowers any of them.
        IEnumerable<SemanticVersion> currentVersions = dotnetVersions
            .Select(version => GetCurrentVersion(variables, version))
            .OrderDescending()
            .DistinctBy(GetSeries);

        // The manifest's base-url values refer to the current build-version, so candidate URLs are built from the root.
        string rootVariableName = isInternal ? "powershell|base-url|internal" : "powershell|base-url|public";
        string root = variables.GetValue(rootVariableName);

        List<DependencyUpdate> updates = [];

        foreach (SemanticVersion currentVersion in currentVersions)
        {
            SemanticVersion latestVersion = await FindLatestAsync(root, currentVersion, isInternal, cancellationToken);

            if (latestVersion == currentVersion)
            {
                logger.LogInformation("PowerShell {Version} is the latest available version.", currentVersion);
                continue;
            }

            DependencyUpdate update = await ResolveFromVersionAsync(
                latestVersion.ToNormalizedString(),
                isInternal,
                baseUrl: null,
                dotnetVersion,
                cancellationToken);

            updates.Add(update);
        }

        return updates;
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
        BlobClient blobClient = CreateBlobClient(url);
        Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync(cancellationToken);

        return response.Value.Content.ToString();
    }

    // Walks forward from the version until no later version exists.
    private async Task<SemanticVersion> FindLatestAsync(
        string root,
        SemanticVersion version,
        bool isInternal,
        CancellationToken cancellationToken)
    {
        foreach (SemanticVersion candidate in GetNextVersions(version))
        {
            if (await ExistsAsync(root, candidate, isInternal, cancellationToken))
            {
                return await FindLatestAsync(root, candidate, isInternal, cancellationToken);
            }
        }

        return version;
    }

    private async Task<bool> ExistsAsync(
        string root,
        SemanticVersion version,
        bool isInternal,
        CancellationToken cancellationToken)
    {
        string normalizedVersion = version.ToNormalizedString();
        string directory = isInternal ? GetInternalDirectory(normalizedVersion) : normalizedVersion;
        string checksumsUrl = $"{root}/{directory}/{ChecksumsFileName}";

        if (isInternal)
        {
            return await CreateBlobClient(checksumsUrl).ExistsAsync(cancellationToken);
        }

        using var request = new HttpRequestMessage(HttpMethod.Head, checksumsUrl);
        using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    private BlobClient CreateBlobClient(string url)
    {
        TokenCredential credential = credentialProvider.GetCredential(ServiceConnectionNames.Staging);
        var options = new BlobClientOptions { Transport = new HttpClientTransport(httpClient) };

        return new BlobClient(new Uri(url), credential, options);
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

        List<string> dotnetVersions = GetAllDotnetVersions(variables)
            .Where(version => GetSeries(GetCurrentVersion(variables, version)) == series)
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

        return isInternal
            ? $"$(powershell|base-url|internal)/{GetInternalDirectory(version)}"
            : $"$(powershell|base-url|public)/$(powershell|{dotnetVersion}|build-version)";
    }

    // Internal storage uses the version with dots replaced by dashes, e.g. v7-5-11-nuget.
    private static string GetInternalDirectory(string version) => $"v{version.Replace('.', '-')}-nuget/globaltool";

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

    // Versions that can follow the given one within its series, in the order they would be released. A gap stops the walk.
    private static SemanticVersion[] GetNextVersions(SemanticVersion version)
    {
        var release = new SemanticVersion(version.Major, version.Minor, version.Patch);

        SemanticVersion Prerelease(string label, int number) =>
            new(version.Major, version.Minor, version.Patch, $"{label}.{number}");

        return version.ReleaseLabels.ToArray() switch
        {
            [] => [new SemanticVersion(version.Major, version.Minor, version.Patch + 1)],
            ["preview", var number] => [Prerelease("preview", int.Parse(number) + 1), Prerelease("rc", 1), release],
            ["rc", var number] => [Prerelease("rc", int.Parse(number) + 1), release],
            _ => throw new FormatException($"Unexpected PowerShell version '{version}'."),
        };
    }

    private static IEnumerable<string> GetAllDotnetVersions(ManifestVariables variables) =>
        variables.Names
            .Select(name => name.Split('|'))
            .Where(parts => parts is ["powershell", _, "build-version"])
            .Select(parts => parts[1]);

    private static SemanticVersion GetCurrentVersion(ManifestVariables variables, string dotnetVersion) =>
        SemanticVersion.Parse(variables.GetValue(GetManifestVariableName(dotnetVersion, "build-version")));

    private static string GetSeries(SemanticVersion version) => $"{version.Major}.{version.Minor}";

    private static string GetManifestVariableName(string dotnetVersion, string type) =>
        $"powershell|{dotnetVersion}|{type}";
}
