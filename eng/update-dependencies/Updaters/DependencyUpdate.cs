// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

/// <summary>
/// An update whose new versions are known but which has not been written to the manifest yet.
/// Resolving first lets the pull request describe the update it is about to make.
/// </summary>
/// <param name="Scope">
/// Distinguishes updates of the same dependency that may be open at the same time, such as a
/// .NET major.minor version. It becomes part of the pull request branch name, so updates sharing
/// a scope replace each other. Empty when only one update of the dependency can be open at once.
/// </param>
/// <param name="Description">
/// The update in one line, used as the pull request title and the commit message.
/// </param>
/// <param name="ApplyAsync">Writes the resolved versions to the manifest and the repo.</param>
public sealed record DependencyUpdate(
    string Scope,
    string Description,
    Func<ManifestVariables, string, CancellationToken, Task> ApplyAsync);
