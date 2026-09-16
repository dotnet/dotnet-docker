// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

internal record AspireOptions : CreatePullRequestOptions, IOptions
{
    public int? FromBuildId { get; init; }
    public int? FromChannel { get; init; }

    public static new List<Option> Options { get; } =
    [
        new Option<int?>("--from-build-id")
        {
            Description = "The Aspire BAR build ID to update from (not an Azure DevOps pipeline run ID).",
        },
        new Option<int?>("--from-channel")
        {
            Description = "The BAR channel ID to read the latest microsoft/aspire build from.",
        },
        ..CreatePullRequestOptions.Options,
    ];
}
