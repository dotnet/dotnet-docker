// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies.Updaters;

/// <summary>
/// Writes an update's resolved versions to the manifest, and to any related files in the repo.
/// </summary>
/// <param name="variables">
/// The manifest variables to update. The caller loads and saves the manifest, so an
/// implementation only edits variables.
/// </param>
/// <param name="repoRoot">
/// The absolute path of the repo to update. This is not always the repo the command was run
/// from: the same update is applied again in a separate workspace when publishing.
/// </param>
public delegate Task ApplyUpdateAsync(
    ManifestVariables variables,
    string repoRoot,
    CancellationToken cancellationToken);

/// <summary>
/// An update whose new versions are known but which has not been written to the manifest yet.
/// Resolving first lets the pull request describe the update it is about to make.
/// </summary>
/// <param name="Description">
/// The update in one line, used as the pull request title and the commit message.
/// </param>
/// <param name="ApplyAsync">Makes the update.</param>
/// <param name="Scope">
/// Distinguishes updates of the same dependency that may be open at the same time, such as a
/// .NET major.minor version. It becomes part of the pull request branch name, so updates sharing
/// a scope replace each other. Defaults to empty, meaning only one update of the dependency can
/// be open at once.
/// </param>
public sealed record DependencyUpdate(
    string Description,
    ApplyUpdateAsync ApplyAsync,
    string Scope = "");
