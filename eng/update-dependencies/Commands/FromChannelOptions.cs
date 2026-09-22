// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal record FromChannelOptions : CreatePullRequestOptions
{
    public required int Channel { get; init; }

    private static readonly PositiveIdArgument s_channel = new("The BAR channel ID to use as the update source");

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Arguments.Add(s_channel);
    }

    public static new FromChannelOptions Bind(ParseResult result) =>
        Bind(result, new FromChannelOptions { Channel = result.GetValue(s_channel) });
}
