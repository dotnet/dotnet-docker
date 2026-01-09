// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// Interface for accessing variables defined in a manifest file.
/// </summary>
/// <remarks>
/// Also see <see cref="ManifestVariables"/>.
/// </remarks>
public interface IManifestVariables
{
    string GetValue(string variableName);
    bool HasValue(string variableName);
}
