// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies;

// Carry the version resolved before publishing with this invocation, not in the singleton updater.
public sealed record MonitorPipelineBuildReference(
    string Organization,
    string Project,
    int RunId,
    string Version)
        : PipelineBuildReference(Organization, Project, RunId)
{
    public static async Task<MonitorPipelineBuildReference> ResolveAsync(
        IPipelineArtifactProvider artifactProvider,
        PipelineBuildReference build,
        CancellationToken cancellationToken)
    {
        if (build is MonitorPipelineBuildReference prepared)
        {
            return prepared;
        }

        var versionFile = new PipelineArtifactFile("Build_Info", "dotnet-monitor.nupkg.buildversion");
        string version = await artifactProvider.GetArtifactTextContentAsync(
            build.Organization,
            build.Project,
            build.RunId,
            versionFile).WaitAsync(cancellationToken);

        return new MonitorPipelineBuildReference(build.Organization, build.Project, build.RunId, version.Trim());
    }
}
