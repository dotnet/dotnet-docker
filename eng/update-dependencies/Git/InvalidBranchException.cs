// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Dotnet.Docker.Git;

public sealed class InvalidBranchException(string message) : InvalidOperationException(message);
