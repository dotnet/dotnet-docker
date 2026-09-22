// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class MonitorUpdater(
    IPipelineArtifactProvider pipelineArtifactProvider,
    HttpClient httpClient,
    UpdateDependenciesConfiguration configuration,
    ILogger<MonitorUpdater> logger)
        : IPipelineBuildUpdater, IVersionUpdater
{
    public static string Name => "monitor";
    public static string VersionSourceName => "dotnet/dotnet-monitor";

    private static readonly (string Product, string ArchiveName)[] s_products =
    [
        ("monitor", "dotnet-monitor"),
        ("monitor-base", "dotnet-monitor-base"),
        ("monitor-ext-azureblobstorage", "dotnet-monitor-egress-azureblobstorage"),
        ("monitor-ext-s3storage", "dotnet-monitor-egress-s3storage"),
    ];

    public async Task<DependencyUpdate> ResolveFromPipelineBuildAsync(
        int pipelineRunId,
        CancellationToken cancellationToken)
    {
        string version = await GetVersionFromPipelineAsync(pipelineRunId, cancellationToken);
        return await ResolveFromVersionAsync(version, cancellationToken);
    }

    private async Task<string> GetVersionFromPipelineAsync(
        int pipelineRunId,
        CancellationToken cancellationToken)
    {
        var versionFile = new PipelineArtifactFile("Build_Info", "dotnet-monitor.nupkg.buildversion");
        string version = await pipelineArtifactProvider
            .GetArtifactTextContentAsync(
                configuration.AzureDevOps.Organization,
                configuration.AzureDevOps.Project,
                pipelineRunId,
                versionFile)
            .WaitAsync(cancellationToken);

        return version.Trim();
    }

    public Task<DependencyUpdate> ResolveFromVersionAsync(string version, CancellationToken cancellationToken)
    {
        version = version.Trim();
        var parsedVersion = SemanticVersion.Parse(version);
        string dockerfileVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}";

        return Task.FromResult(new DependencyUpdate(
            Description: $"Update .NET Monitor {dockerfileVersion} to {version}",
            ApplyAsync: (variables, _, token) => ApplyAsync(variables, version, parsedVersion, token),
            Scope: dockerfileVersion));
    }

    private async Task ApplyAsync(
        ManifestVariables variables,
        string version,
        SemanticVersion parsedVersion,
        CancellationToken cancellationToken)
    {
        string dockerfileVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}";
        bool stableBranding = parsedVersion.ReleaseLabels.FirstOrDefault() is "servicing" or "rtm";
        string productVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}.{parsedVersion.Patch}";
        string fileVersion = stableBranding ? productVersion : version;

        // Preview tags keep the release label and number; archive names keep the full build version.
        if (parsedVersion.IsPrerelease && !stableBranding)
        {
            productVersion += $"-{string.Join(".", parsedVersion.ReleaseLabels.Take(2))}";
        }

        string branch = variables.GetValue("branch");
        string quality = branch == "main" ? "maintenance" : "preview";
        string baseUrlVariable = $"monitor|{dockerfileVersion}|base-url|{branch}";
        string checksumsBaseUrlVariable = $"monitor|{dockerfileVersion}|base-url|checksums|{branch}";

        variables.SetValue(baseUrlVariable, $"$(base-url|public|{quality}|{branch})");
        variables.SetValue(checksumsBaseUrlVariable, $"$(base-url|public-checksums|{quality}|{branch})");

        foreach (var (product, _) in s_products)
        {
            UpdateVersion(variables, $"{product}|{dockerfileVersion}|build-version", version);
            UpdateVersion(variables, $"{product}|{dockerfileVersion}|product-version", productVersion);
        }

        foreach (var (product, archiveName) in s_products)
        {
            string prefix = $"{product}|{dockerfileVersion}|";
            var checksumVariables = variables.Names
                .Where(name => name.StartsWith(prefix, StringComparison.Ordinal))
                .Where(name => name.EndsWith("|sha", StringComparison.Ordinal));

            // Newer images download checksums at build time and have no pinned hashes to update.
            foreach (string variableName in checksumVariables)
            {
                string[] parts = variableName.Split('|');
                string os = parts[2];
                string arch = parts[3];
                string extension = os.Contains("win", StringComparison.Ordinal) ? "zip" : "tar.gz";
                string baseUrl = variables.GetValue(baseUrlVariable);
                string downloadUrl = $"{baseUrl}/diagnostics/monitor/{version}/{archiveName}-{fileVersion}-{os}-{arch}.{extension}";
                string checksum = await GetChecksumAsync(downloadUrl, cancellationToken);
                variables.SetValue(variableName, checksum);
            }
        }
    }

    private void UpdateVersion(ManifestVariables variables, string name, string value)
    {
        if (variables.Contains(name) && !SemanticVersion.TryParse(variables.GetRawValue(name), out _))
        {
            logger.LogInformation("Leaving manifest variable '{Variable}' unchanged.", name);
            return;
        }

        variables.SetValue(name, value);
    }

    private async Task<string> GetChecksumAsync(string downloadUrl, CancellationToken cancellationToken)
    {
        string checksumUrl = downloadUrl.Replace("/public/", "/public-checksums/") + ".sha512";
        logger.LogInformation("Downloading Monitor checksum from {Url}", checksumUrl);

        using HttpResponseMessage response = await httpClient.GetAsync(checksumUrl, cancellationToken);
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
            string checksum = await response.Content.ReadAsStringAsync(cancellationToken);
            checksum = checksum.Trim().ToLowerInvariant();
            ValidateSha512Format(checksum);
            return checksum;
        }

        // Older Monitor builds may not publish a checksum alongside the archive.
        logger.LogInformation("No published checksum found. Hashing Monitor archive {Url}", downloadUrl);
        return await ChecksumHelper.ComputeChecksumShaAsync(httpClient, downloadUrl, cancellationToken)
            ?? throw new InvalidOperationException($"Unable to retrieve checksum for '{downloadUrl}'.");
    }

    private static void ValidateSha512Format(string hash)
    {
        if (hash.Length != 128 || !hash.All(char.IsAsciiHexDigit))
        {
            throw new FormatException("Invalid SHA512 checksum: expected 128 hexadecimal characters.");
        }
    }
}
