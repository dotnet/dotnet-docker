// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// One single version for a single image component (could be a product or a tool).
/// </summary>
internal sealed record ComponentVersionInfo(string Version);
