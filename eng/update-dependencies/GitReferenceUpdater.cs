// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;

#nullable enable
namespace Dotnet.Docker;

internal class GitReferenceUpdater : BasicVersionUpdater
{
    private readonly string _reference;

    public GitReferenceUpdater(string repoRoot, string variableName, string reference) : base(repoRoot, variableName)
    {
        _reference = reference;
    }

    protected sealed override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();
        return _reference;
    }
}
