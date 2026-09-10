// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal abstract class VariableUpdaterBase(ManifestVariables variables, string variableName) : IDependencyUpdater
{
    protected string VariableName { get; } = variableName;
    protected ManifestVariables Variables { get; } = variables;

    public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos)
    {
        if (!Variables.Contains(VariableName))
        {
            Trace.TraceInformation($"Skipping absent manifest variable '{VariableName}'.");
            return [];
        }

        string currentValue = Variables.GetRawValue(VariableName);
        if (!ShouldUpdate(currentValue))
        {
            Trace.TraceInformation($"Leaving manifest variable '{VariableName}' unchanged.");
            return [];
        }

        string newValue = TryGetDesiredValue(dependencyInfos, out var usedInfos)
            ?? throw new InvalidOperationException($"No replacement value found for manifest variable '{VariableName}'.");

        if (currentValue == newValue)
        {
            return [];
        }

        // Keep the VersionTools execution contract until orchestration is migrated.
        // The action edits shared memory; the caller owns the eventual file write.
        return
        [
            new DependencyUpdateTask(
                updateAction: () => Variables.SetValue(VariableName, newValue),
                usedInfos: usedInfos,
                readableDescriptionLines: [$"Update '{VariableName}' from '{currentValue}' to '{newValue}'"])
        ];
    }

    protected virtual bool ShouldUpdate(string currentValue) => true;

    protected abstract string? TryGetDesiredValue(
        IEnumerable<IDependencyInfo> dependencyInfos,
        out IEnumerable<IDependencyInfo> usedDependencyInfos);
}
