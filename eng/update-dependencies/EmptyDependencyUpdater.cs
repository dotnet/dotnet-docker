// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal class EmptyDependencyUpdater : IDependencyUpdater
{
    public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos) => [];
}
