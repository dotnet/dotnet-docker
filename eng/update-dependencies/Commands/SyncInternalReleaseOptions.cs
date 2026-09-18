// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

public sealed record SyncInternalReleaseOptions : CreatePullRequestOptions
{
    public string StagingStorageAccount { get; set; } = string.Empty;

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Options.Add(FromStagingPipelineOptions.StagingStorageAccountOption);
    }

    public static new SyncInternalReleaseOptions Bind(ParseResult result) =>
        Bind(result, new SyncInternalReleaseOptions
        {
            StagingStorageAccount = result.GetRequiredValue(FromStagingPipelineOptions.StagingStorageAccountOption),
        });
}
