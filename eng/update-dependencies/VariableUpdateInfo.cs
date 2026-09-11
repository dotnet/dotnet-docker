// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// Information about a variable to be updated in the manifest.versions file.
/// </summary>
/// <param name="VariableName">The existing manifest key to update.</param>
/// <param name="Value">The replacement value, which may contain manifest references.</param>
internal sealed record VariableUpdateInfo(string VariableName, string Value);
