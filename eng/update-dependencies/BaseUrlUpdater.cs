// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Updates the baseUrl variables in the manifest.versions.json file.
/// </summary>
public static class BaseUrlUpdater
{
    /// <summary>
    /// Replaces active base URLs with the requested internal URL.
    /// Missing and empty variables are left alone.
    /// </summary>
    public static void Update(
        ManifestVariables manifestVariables,
        string dockerfileVersion,
        string internalBaseUrl,
        bool sdkOnlyRelease)
    {
        var upstreamBranch = manifestVariables.GetValue("branch");
        var baseUrlVarNames = ManifestHelper.GetBaseUrlVariableNames(
            dockerfileVersion: dockerfileVersion,
            branch: upstreamBranch,
            sdkOnlyRelease: sdkOnlyRelease);

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
            if (!string.IsNullOrEmpty(internalBaseUrl))
            {
                manifestVariables.SetValue(variableName, internalBaseUrl);
            }
        }
    }
}
