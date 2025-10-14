// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker.Git;

/// <summary>
/// Represents information about a Git remote repository.
/// </summary>
/// <param name="Name">The name of the remote (e.g., "origin").</param>
/// <param name="Url">The URL of the remote repository.</param>
internal sealed record GitRemoteInfo(string Name, string Url);
