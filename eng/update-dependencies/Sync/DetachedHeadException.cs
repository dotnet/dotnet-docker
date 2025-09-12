// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Dotnet.Docker.Sync;

public sealed class DetachedHeadException(string message) : InvalidOperationException(message);
