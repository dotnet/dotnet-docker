// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable
namespace Dotnet.Docker;

internal class ChiselUpdater : ChiselToolUpdater
{
    public ChiselUpdater(string repoRoot, string dockerfileVersion, string newRef)
        : base(repoRoot, $"chisel|{dockerfileVersion}|ref", dockerfileVersion, newRef) { }
}
