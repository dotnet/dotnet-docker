// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Dotnet.Docker.Git;

/// <summary>
/// Exception thrown when a Git branch operation fails due to invalid branch conditions,
/// such as attempting to use a non-existent branch or an incorrectly named branch.
/// </summary>
/// <param name="message">The error message describing the invalid branch condition.</param>
internal sealed class InvalidBranchException(string message) : InvalidOperationException(message);
