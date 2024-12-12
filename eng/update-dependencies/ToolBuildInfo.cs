// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

internal record ToolBuildInfo(string SimpleName, string SimpleVersion) : IDependencyInfo;
