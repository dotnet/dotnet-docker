// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Dotnet.Docker;

/// <summary>
/// Applies manifest edits with change logging, skipping keys absent from this branch.
/// </summary>
internal static class VariableUpdater
{
    public static void Update(ManifestVariables variables, string variableName, string value)
    {
        if (!variables.Contains(variableName))
        {
            Trace.TraceInformation($"Skipping absent manifest variable '{variableName}'.");
            return;
        }

        string previousValue = variables.GetRawValue(variableName);
        if (variables.SetValue(variableName, value))
        {
            Trace.TraceInformation($"Update '{variableName}' from '{previousValue}' to '{value}'.");
        }
    }
}
