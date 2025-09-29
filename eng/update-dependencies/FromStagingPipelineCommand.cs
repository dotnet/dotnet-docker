// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Linq;

namespace Dotnet.Docker;

internal partial class FromStagingPipelineCommand(
    ILogger<FromStagingPipelineCommand> logger,
    PipelineArtifactProvider pipelineArtifactProvider)
    : BaseCommand<FromStagingPipelineOptions>
{
    private readonly ILogger<FromStagingPipelineCommand> _logger = logger;
    private readonly PipelineArtifactProvider _pipelineArtifactProvider = pipelineArtifactProvider;

    public override async Task<int> ExecuteAsync(FromStagingPipelineOptions options)
    {
        _logger.LogInformation(
            "Updating dependencies based on staging pipeline run ID {options.StagingPipelineRunId}",
            options.StagingPipelineRunId);

        string internalBaseUrl = string.Empty;
        if (options.Internal)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(
                options.StagingStorageAccount,
                $"{FromStagingPipelineOptions.StagingStorageAccountOption} must be set when using the {FromStagingPipelineOptions.InternalOption} option."
            );

            // Each pipeline run has a corresponding blob container named stage-${options.StagingPipelineRunId}.
            // Release metadata is stored in metadata/ReleaseManifest.json.
            // Release assets are stored individually under in assets/shipping/assets/[Sdk|Runtime|aspnetcore|...].
            // Full example: https://dotnetstagetest.blob.core.windows.net/stage-2XXXXXX/assets/shipping/assets/Runtime/10.0.0-preview.N.XXXXX.YYY/dotnet-runtime-10.0.0-preview.N.XXXXX.YYY-linux-arm64.tar.gz
            internalBaseUrl = NormalizeStorageAccountUrl(options.StagingStorageAccount)
                + $"/stage-{options.StagingPipelineRunId}/assets/shipping/assets";
        }

        var releaseConfig = await _pipelineArtifactProvider.GetReleaseConfigAsync(
            options.AzdoOrganization,
            options.AzdoProject,
            options.StagingPipelineRunId);

        string dotnetProductVersion = VersionHelper.ResolveProductVersion(releaseConfig.RuntimeBuild);
        string dockerfileVersion = VersionHelper.ResolveMajorMinorVersion(releaseConfig.RuntimeBuild).ToString();

        // Record pipeline run ID for this dockerfileVersion, for later use by sync-internal-release command
        RecordInternalVersion(dockerfileVersion, options.StagingPipelineRunId.ToString());

        var productVersions = (options.Internal, releaseConfig.SdkOnly) switch
        {
            // SDK-only internal/staging release
            (true, true) => new Dictionary<string, string?>
            {
                // SDK-only releases are almost always one-off updates/bug
                // fixes on top of an existing release of the Runtime and
                // ASP.NET Core.
                //
                // If the release config tells us that this is an
                // SDK-only release, we can assume that we have already
                // released the runtime/aspnet versions that it's based on, and
                // therefore we shouldn't update them unnecessarily.
                { "sdk", VersionHelper.GetHighestSdkVersion(releaseConfig.SdkBuilds) }
            },
            // Internal/staging release
            (true, false) => new Dictionary<string, string?>
            {
                { "dotnet", dotnetProductVersion },
                { "runtime",  releaseConfig.RuntimeBuild },
                { "aspnet", releaseConfig.AspBuild },
                { "aspnet-composite", releaseConfig.AspBuild },
                { "sdk", VersionHelper.GetHighestSdkVersion(releaseConfig.SdkBuilds) },
            },
            // Public release - whether or not it's an SDK-only release doesn't
            // matter because the product versions will end up being the same.
            (false, _) => new Dictionary<string, string?>
            {
                { "dotnet", dotnetProductVersion },
                { "runtime", releaseConfig.Runtime },
                { "aspnet", releaseConfig.Asp },
                { "aspnet-composite", releaseConfig.Asp },
                { "sdk", VersionHelper.GetHighestSdkVersion(releaseConfig.Sdks) },
            }
        };

        _logger.LogInformation(
            "Resolved product versions: {productVersions}",
            string.Join(", ", productVersions.Select(kv => $"{kv.Key}: {kv.Value}")));

        // Run old update-dependencies command using the resolved versions
        var updateDependencies = new SpecificCommand();
        var updateDependenciesOptions = new SpecificCommandOptions()
        {
            DockerfileVersion = dockerfileVersion.ToString(),
            ProductVersions = productVersions,

            // Pass through all properties of CreatePullRequestOptions
            User = options.User,
            Email = options.Email,
            Password = options.Password,
            AzdoOrganization = options.AzdoOrganization,
            AzdoProject = options.AzdoProject,
            AzdoRepo = options.AzdoRepo,
            VersionSourceName = options.VersionSourceName,
            SourceBranch = options.SourceBranch,
            TargetBranch = options.TargetBranch,
            InternalBaseUrl = internalBaseUrl,
        };

        return await updateDependencies.ExecuteAsync(updateDependenciesOptions);
    }

    /// <summary>
    /// Records the staging pipeline run ID in an easy to parse format. This
    /// can be used by the sync-internal-release pipeline to record and
    /// re-apply the same staging builds after resetting the state of the repo
    /// to match the public release branch.
    /// </summary>
    /// <remarks>
    /// This will only store one staging pipeline run ID per dockerfileVersion
    /// </remarks>
    /// <param name="dockerfileVersion">major-minor version</param>
    /// <param name="stagingPipelineRunId">the build ID of the staging pipeline run</param>
    private void RecordInternalVersion(string dockerfileVersion, string stagingPipelineRunId)
    {
        const string InternalVersionsFile = "internal-versions.txt";

        // Internal versions file should have one line per dockerfileVersion
        // Each line should be formatted as: <dockerfileVersion>=<stagingPipelineRunId>
        //
        // The preferable way to do this would be to record the version in
        // manifest.versions.json, however that would require one of the following:
        // 1) round-trip serialization, which would remove any whitespace/blank lines - which are
        //    important for keeping the file readable and reducing git merge conflicts
        // 2) lots of regex JSON manipulation which is error-prone and harder to maintain
        //
        // So for now, the separate file and format is a compromise.

        var versionsFilePath = Path.GetFullPath(SpecificCommand.VersionsFilename);
        var versionsFileDir = Path.GetDirectoryName(versionsFilePath) ?? "";
        var internalVersionFile = Path.Combine(versionsFileDir, InternalVersionsFile);
        Dictionary<string, string> versions = [];

        _logger.LogInformation(
            "Recording staging pipeline build ID in {internalVersionFile}",
            internalVersionFile);

        try
        {
            // File already exists - read existing versions
            versions = File.ReadAllLines(internalVersionFile)
                .Select(line => line.Split('=', 2))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0], parts => parts[1]);
        }
        catch (FileNotFoundException)
        {
            // File doesn't exist - it will be created
        }

        versions[dockerfileVersion] = stagingPipelineRunId;
        var versionLines = versions.Select(kv => $"{kv.Key}={kv.Value}");
        File.WriteAllLines(internalVersionFile, versionLines);
    }

    /// <summary>
    /// Formats a storage account URL has a specific format:
    /// - Starts with "https://"
    /// - No trailing slash
    /// - Defaults to using blob.core.windows.net as the root domain
    /// </summary>
    private static string NormalizeStorageAccountUrl(string storageAccount)
    {
        if (string.IsNullOrWhiteSpace(storageAccount))
        {
            return storageAccount;
        }

        storageAccount = storageAccount.Trim();

        if (storageAccount.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return storageAccount.TrimEnd('/');
        }

        // If it's just the storage account name, construct the full URL
        return $"https://{storageAccount}.blob.core.windows.net";
    }
}
