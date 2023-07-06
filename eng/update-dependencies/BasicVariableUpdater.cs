// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;

#nullable enable
namespace Dotnet.Docker;

internal class BasicVariableUpdater : VariableUpdaterBase
{
    private readonly string _productDependency;
    private readonly string _newValue;

    public BasicVariableUpdater(string repoRoot, string productDependency, string variableName, string newValue) : base(repoRoot, variableName)
    {
        _newValue = newValue;
        _productDependency = productDependency;
    }

    protected sealed override string TryGetDesiredValue(IEnumerable<IDependencyInfo> dependencyInfos, out IEnumerable<IDependencyInfo> usedDependencyInfos)
    {
        IDependencyInfo? productDependencyInfo = dependencyInfos.FirstOrDefault(info => info.SimpleName == _productDependency);

        if (productDependencyInfo is null)
        {
            usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();
            return ManifestHelper.GetVariableValue(VariableName, ManifestVariables.Value);
        }

        usedDependencyInfos = new[] { productDependencyInfo };
        return _newValue;
    }
}
