// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Dotnet.Docker;

/// <summary>
/// Updates the baseUrl variables in the manifest.versions.json file.
/// </summary>
internal static class BaseUrlUpdater
{
    /// <summary>
    /// Replaces active base URLs with the requested internal URL or public release reference.
    /// Missing and empty variables are left alone.
    /// </summary>
    public static void Update(ManifestVariables manifestVariables, SpecificCommandOptions options)
    {
        var upstreamBranch = manifestVariables.GetValue("branch");
        var baseUrlVarNames = ManifestHelper.GetBaseUrlVariableNames(
            dockerfileVersion: options.DockerfileVersion,
            branch: upstreamBranch,
            versionSourceName: options.VersionSourceName,
            sdkOnlyRelease: options.IsSdkOnly);

        foreach (string variableName in baseUrlVarNames)
        {
            if (!manifestVariables.Contains(variableName))
            {
                Trace.TraceWarning($"BaseUrlUpdater: variable '{variableName}' not found - skipping base URL update.");
                continue;
            }

            if (manifestVariables.GetRawValue(variableName).Length == 0)
            {
                Trace.TraceInformation($"Leaving manifest variable '{variableName}' unchanged.");
                continue;
            }

            // Without a release state, we cannot infer which public URL an internal URL replaced.
            if (options.IsInternal)
            {
                VariableUpdater.Update(manifestVariables, variableName, options.InternalBaseUrl);
            }
            else if (options.ReleaseState.HasValue)
            {
                string referenceName = ManifestHelper.GetBaseUrlVariableName(options.ReleaseState.Value, options.TargetBranch);
                VariableUpdater.Update(manifestVariables, variableName, $"$({referenceName})");
            }
        }
    }
}
