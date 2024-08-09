// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;

#nullable enable
namespace Dotnet.Docker;

internal class SyftRefUpdater : VariableUpdaterBase
{
    private readonly string _newValue;

    public SyftRefUpdater(string repoRoot, string newRef) : base(repoRoot, "syft|tag")
    {
        _newValue = newRef;
    }

    protected override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();
        return _newValue;
    }
}
#nullable disable
