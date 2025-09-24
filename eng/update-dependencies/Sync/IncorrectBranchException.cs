// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Dotnet.Docker.Sync;

/// <summary>
/// Indicates that an incorrect branch was specified for a git operation.
/// </summary>
public sealed class IncorrectBranchException(string message) : InvalidOperationException(message);
