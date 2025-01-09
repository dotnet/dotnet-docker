// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using VerifyTests.DiffPlex;

namespace Microsoft.DotNet.Docker.Tests;

#nullable enable
[Trait("Category", "pre-build")]
public class VerifyChecksTests
{
    [Fact]
    public Task Run() =>
        VerifyChecks.Run();
}

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() =>
        VerifyDiffPlex.Initialize(OutputType.Compact);
}
