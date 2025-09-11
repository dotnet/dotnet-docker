// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;
using System.Threading.Tasks;

namespace Dotnet.Docker.Sync;

internal sealed class SyncInternalReleaseOptions : IOptions
{
    public static List<Option> Options => [];
    public static List<Argument> Arguments => [];
}

internal sealed class SyncInternalReleaseCommand(

) : BaseCommand<SyncInternalReleaseOptions>
{
    public override Task<int> ExecuteAsync(SyncInternalReleaseOptions options) => throw new System.NotImplementedException();
}
