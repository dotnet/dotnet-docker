// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.RegularExpressions;
using Dotnet.Docker.Model.Release;
using System.Text;

namespace Dotnet.Docker;

internal partial class FromStagingPipelineCommand(
    ILogger<FromStagingPipelineCommand> logger,
    PipelineArtifactBuildManifestProvider pipelineArtifactBuildManifestProvider,
    StorageAccountBuildManifestProvider storageAccountBuildManifestProvider)
    : BaseCommand<FromStagingPipelineOptions>
{
    private readonly ILogger<FromStagingPipelineCommand> _logger = logger;
    private readonly PipelineArtifactBuildManifestProvider _pipelineArtifactBuildManifestProvider = pipelineArtifactBuildManifestProvider;
    private readonly StorageAccountBuildManifestProvider _storageAccountBuildManifestProvider = storageAccountBuildManifestProvider;

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

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(NormalizeStorageAccountUrl(options.StagingStorageAccount));
            urlBuilder.Append($"/stage-{options.StagingPipelineRunId}/assets/shipping/assets");
            internalBaseUrl = urlBuilder.ToString();
        }

        var buildManifest = await GetBuildManifest(options);
        var allAssets = buildManifest.AllAssets.ToList();

        // Look through all the assets and get the version of the highest SDK feature band
        var sdkAssets = allAssets
            .Where(asset => SdkRegex.IsMatch(asset.Name))
            .ToList();
        var sdkVersions = sdkAssets.Select(asset => asset.Version);
        string highestSdkVersion = VersionHelper.GetHighestSdkVersion(sdkVersions);
        string dockerfileVersion = VersionHelper.ResolveMajorMinorVersion(highestSdkVersion).ToString();

        string runtimeVersion = allAssets
            .Where(asset => RuntimeRegex.IsMatch(asset.Name))
            .Select(asset => asset.Version)
            .First();

        string aspnetVersion = allAssets
            .Where(asset => AspNetCoreRegex.IsMatch(asset.Name))
            .Select(asset => asset.Version)
            .First();

        _logger.LogInformation(
            """
            Resolved .NET versions:
            .NET: {dockerfileVersion}
            - SDK: {highestSdkVersion}
            - Runtime: {runtimeVersion}
            - ASP.NET Core: {aspnetVersion}
            """,
            dockerfileVersion, highestSdkVersion, runtimeVersion, aspnetVersion);

        // Run old update-dependencies command using the resolved versions
        var updateDependencies = new SpecificCommand();
        var updateDependenciesOptions = new SpecificCommandOptions()
        {
            DockerfileVersion = dockerfileVersion.ToString(),
            ProductVersions = new Dictionary<string, string?>()
            {
                // "dotnet" version is required. It sets the "dotnet|*|product-version"
                // variable which is used for runtime-deps, runtime, and aspnet tags.
                { "dotnet", runtimeVersion },
                { "runtime",  runtimeVersion },
                { "aspnet", aspnetVersion },
                { "aspnet-composite", aspnetVersion },
                { "sdk", highestSdkVersion },
            },

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

    private async Task<BuildManifest> GetBuildManifest(FromStagingPipelineOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.StagingStorageAccount))
        {
            try
            {
                _logger.LogInformation(
                    "Attempting to get build manifest from storage account: {StagingStorageAccount}",
                    options.StagingStorageAccount
                );

                return await _storageAccountBuildManifestProvider.GetBuildManifestAsync(
                    options.StagingStorageAccount,
                    options.StagingPipelineRunId
                );
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Failed to get build manifest from storage account. Falling back to pipeline artifact provider."
                );

                return await _pipelineArtifactBuildManifestProvider.GetBuildManifestAsync(
                    options.AzdoOrganization,
                    options.AzdoProject,
                    options.StagingPipelineRunId
                );
            }
        }
        else
        {
            _logger.LogInformation(
                "No staging storage account provided. Using pipeline artifact provider."
            );

            return await _pipelineArtifactBuildManifestProvider.GetBuildManifestAsync(
                options.AzdoOrganization,
                options.AzdoProject,
                options.StagingPipelineRunId
            );
        }
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

    // Examples:
    // - "Sdk/8.0.117-servicing.25269.14/dotnet-sdk-8.0.117-linux-x64.tar.gz"
    // - "Sdk/8.0.310-servicing.25113.14/dotnet-sdk-8.0.310-linux-musl-arm64.tar.gz"
    [GeneratedRegex(@"Sdk/.*/dotnet-sdk-.*-linux-x64.tar.gz$")]
    private static partial Regex SdkRegex { get; }

    // "aspnetcore/Runtime/8.0.14-servicing.25112.21/aspnetcore-runtime-8.0.14-linux-x64.tar.gz",
    [GeneratedRegex(@"aspnetcore/Runtime/.*/aspnetcore-runtime-.*-linux-x64.tar.gz")]
    private static partial Regex AspNetCoreRegex { get; }

    // "Runtime/8.0.14-servicing.25111.18/dotnet-runtime-8.0.14-linux-x64.tar.gz"
    [GeneratedRegex(@"Runtime/.*/dotnet-runtime-.*-linux-x64.tar.gz")]
    private static partial Regex RuntimeRegex { get; }
}
