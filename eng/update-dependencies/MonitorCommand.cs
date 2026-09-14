// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Dotnet.Docker;

internal sealed class MonitorCommand(
    IPipelineArtifactProvider pipelineArtifactProvider,
    HttpClient httpClient,
    ILogger<MonitorCommand> logger)
        : BaseCommand<MonitorOptions>
{
    private static readonly (string Product, string ArchiveName)[] s_products =
    [
        ("monitor", "dotnet-monitor"),
        ("monitor-base", "dotnet-monitor-base"),
        ("monitor-ext-azureblobstorage", "dotnet-monitor-egress-azureblobstorage"),
        ("monitor-ext-s3storage", "dotnet-monitor-egress-s3storage"),
    ];

    public override async Task<int> ExecuteAsync(MonitorOptions options)
    {
        if (!ValidateOptions(options, logger))
        {
            return 1;
        }

        string? version = options.Version;
        if (options.PipelineRunId is int pipelineRunId)
        {
            version = await GetVersionFromPipelineAsync(options, pipelineRunId);
        }

        version = version?.Trim();
        if (string.IsNullOrEmpty(version) || !SemanticVersion.TryParse(version, out var parsedVersion))
        {
            logger.LogError("Invalid .NET Monitor version: '{Version}'.", version);
            return 1;
        }

        logger.LogInformation("Updating .NET Monitor to {Version}", version);

        options = options with
        {
            VersionSourceName = $"dotnet/dotnet-monitor/{parsedVersion.Major}.{parsedVersion.Minor}",
        };

        if (options.UpdateOnly)
        {
            await ApplyUpdatesAsync(
                repoRoot: Path.GetFullPath(options.RepoRoot),
                version: version,
                parsedVersion);

            logger.LogInformation("Local updates completed without publishing.");
        }
        else
        {
            var publisher = new DependencyUpdatePublisher(options);
            await publisher.PublishAsync((gitContext, cancellationToken) =>
                ApplyUpdatesAsync(
                    repoRoot: gitContext.WorkspaceDirectory,
                    version: version,
                    parsedVersion,
                    cancellationToken));
        }

        return 0;
    }

    private async Task ApplyUpdatesAsync(
        string repoRoot,
        string version,
        SemanticVersion parsedVersion,
        CancellationToken cancellationToken = default)
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

        string manifestPath = Path.Combine(repoRoot, "manifest.versions.json");
        var variables = ManifestVariables.FromFile(manifestPath);
        string originalContent = variables.Content;

        string branch = variables.GetValue("branch");
        string quality = branch == "main" ? "maintenance" : "preview";
        string baseUrlVariable = $"monitor|{dockerfileVersion}|base-url|{branch}";
        string checksumsBaseUrlVariable = $"monitor|{dockerfileVersion}|base-url|checksums|{branch}";

        VariableUpdater.Update(variables, baseUrlVariable, $"$(base-url|public|{quality}|{branch})");
        VariableUpdater.Update(variables, checksumsBaseUrlVariable, $"$(base-url|public-checksums|{quality}|{branch})");

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
                VariableUpdater.Update(variables, variableName, checksum);
            }
        }

        string updatedContent = variables.Content;
        if (updatedContent != originalContent)
        {
            await File.WriteAllTextAsync(manifestPath, updatedContent, cancellationToken);
        }

        // Generators can change files even when the manifest is unchanged.
        await ScriptRunner.GenerateDockerfilesAsync(repoRoot, cancellationToken);
        await ScriptRunner.GenerateReadmesAsync(repoRoot, cancellationToken);
    }

    private void UpdateVersion(ManifestVariables variables, string name, string value)
    {
        if (variables.Contains(name) && !SemanticVersion.TryParse(variables.GetRawValue(name), out _))
        {
            logger.LogInformation("Leaving manifest variable '{Variable}' unchanged.", name);
            return;
        }

        VariableUpdater.Update(variables, name, value);
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

    private Task<string> GetVersionFromPipelineAsync(MonitorOptions options, int pipelineRunId)
    {
        string organization = string.IsNullOrEmpty(options.AzdoOrganization)
            ? "https://dev.azure.com/dnceng"
            : options.AzdoOrganization;

        string project = string.IsNullOrEmpty(options.AzdoProject)
            ? "internal"
            : options.AzdoProject;

        var versionFile = new PipelineArtifactFile("Build_Info", "dotnet-monitor.nupkg.buildversion");

        return pipelineArtifactProvider.GetArtifactTextContentAsync(
            organization,
            project,
            pipelineRunId,
            versionFile);
    }

    private static bool ValidateOptions(MonitorOptions options, ILogger logger)
    {
        if ((options.Version is null) == (options.PipelineRunId is null))
        {
            logger.LogError("Specify either a Monitor version or --pipeline-run-id, but not both.");
            return false;
        }

        if (options.PipelineRunId is <= 0)
        {
            logger.LogError("--pipeline-run-id must be a positive Azure DevOps pipeline run ID.");
            return false;
        }

        return true;
    }

    private static void ValidateSha512Format(string hash)
    {
        if (hash.Length != 128 || !hash.All(char.IsAsciiHexDigit))
        {
            throw new FormatException("Invalid SHA512 checksum: expected 128 hexadecimal characters.");
        }
    }
}
