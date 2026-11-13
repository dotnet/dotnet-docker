// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

public sealed class MonitorUpdater(
    IPipelineArtifactProvider pipelineArtifactProvider,
    UpdateDependenciesConfiguration configuration,
    ILogger<MonitorUpdater> logger)
{
    private static readonly string[] Products =
    [
        "monitor",
        "monitor-base",
        "monitor-ext-azureblobstorage",
        "monitor-ext-s3storage",
    ];

    public async Task<DependencyUpdate> ResolveFromPipelineBuildAsync(
        int pipelineRunId,
        CancellationToken cancellationToken)
    {
        string version = await GetVersionFromPipelineAsync(pipelineRunId, cancellationToken);
        return ResolveFromVersion(version);
    }

    private async Task<string> GetVersionFromPipelineAsync(int pipelineRunId, CancellationToken cancellationToken)
    {
        PipelineArtifactFile versionFile = new PipelineArtifactFile("Build_Info", "dotnet-monitor.nupkg.buildversion");
        string version = await pipelineArtifactProvider
            .GetArtifactTextContentAsync(
                configuration.AzureDevOps.Organization,
                configuration.AzureDevOps.Project,
                pipelineRunId,
                versionFile)
            .WaitAsync(cancellationToken);

        return version.Trim();
    }

    public DependencyUpdate ResolveFromVersion(string version)
    {
        version = version.Trim();
        SemanticVersion parsedVersion = SemanticVersion.Parse(version);
        string dockerfileVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}";

        return new DependencyUpdate(
            Description: $"Update .NET Monitor {dockerfileVersion} to {version}",
            ApplyAsync: (variables, _, _) =>
            {
                Apply(variables, version, parsedVersion);
                return Task.CompletedTask;
            },
            Scope: dockerfileVersion);
    }

    private void Apply(
        ManifestVariables variables,
        string version,
        SemanticVersion parsedVersion)
    {
        string dockerfileVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}";
        bool stableBranding = parsedVersion.ReleaseLabels.FirstOrDefault() is "servicing" or "rtm";
        string productVersion = $"{parsedVersion.Major}.{parsedVersion.Minor}.{parsedVersion.Patch}";

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

        foreach (string product in Products)
        {
            UpdateVersion(variables, $"{product}|{dockerfileVersion}|build-version", version);
            UpdateVersion(variables, $"{product}|{dockerfileVersion}|product-version", productVersion);
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
}
