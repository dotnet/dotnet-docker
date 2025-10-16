// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

/// <summary>
/// Information about a variable to be updated in the manifest.versions file.
/// </summary>
/// <param name="SimpleName">The variable name that will be updated</param>
/// <param name="SimpleVersion">The variable will be updated to this version</param>
internal sealed record VariableUpdateInfo(string SimpleName, string SimpleVersion) : IDependencyInfo;
