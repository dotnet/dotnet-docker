// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

internal enum ChangeMode
{
    /// <summary>
    /// Run the command locally, making changes directly to the local repo.
    /// Do not run any git operations.
    /// </summary>
    Local,

    /// <summary>
    /// The local repo is not modified. The command clones a remote repo into
    /// a local directory and makes changes there. A pull request is submitted
    /// with the changes.
    /// </summary>
    Remote,
}
