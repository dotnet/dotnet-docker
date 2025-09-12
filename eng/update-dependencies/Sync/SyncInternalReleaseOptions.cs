// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker.Sync;

public sealed class SyncInternalReleaseOptions : IOptions
{
    public string SourceBranch { get; set; } = string.Empty;

    public static List<Option> Options => [];

    public static List<Argument> Arguments => [
        new Argument<string>("source-branch")
        {
            Description = "The source branch to sync from. Must match the currently checked out branch."
        },
    ];
}
