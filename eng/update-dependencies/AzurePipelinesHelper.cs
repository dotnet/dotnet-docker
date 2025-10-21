// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// Helper methods for working in an Azure Pipelines environment.
/// </summary>
internal static class AzurePipelinesHelper
{
    // List of predefined Azure Pipelines variables:
    // https://learn.microsoft.com/azure/devops/pipelines/build/variables#system-variables

    /// <summary>
    /// Gets the current build ID if running in Azure Pipelines, or an empty string otherwise.
    /// </summary>
    public static string GetBuildId() => Environment.GetEnvironmentVariable("BUILD_BUILDID") ?? "";

    /// <summary>
    /// Determines if the code is running in an Azure Pipelines environment.
    /// </summary>
    public static bool IsRunningInAzurePipelines() =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"));
}
