// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker.Sync;

public sealed record SyncInternalReleaseOptions : IOptions
{
    public string RemoteUrl { get; set; } = string.Empty;
    public string SourceBranch { get; set; } = string.Empty;
    public string TargetBranch { get; set; } = string.Empty;

    public static List<Option> Options => [];

    public static List<Argument> Arguments =>
    [
        new Argument<string>("remote-url"),
        new Argument<string>("source-branch"),
        new Argument<string>("target-branch"),
    ];
}
