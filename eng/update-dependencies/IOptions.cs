// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

public interface IOptions
{
    public static abstract List<Option> Options { get; }
    public static abstract List<Argument> Arguments { get; }
}
