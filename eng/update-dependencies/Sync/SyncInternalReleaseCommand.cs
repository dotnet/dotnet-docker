// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading.Tasks;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging;

namespace Dotnet.Docker.Sync;

public sealed class SyncInternalReleaseOptions : IOptions
{
    public string SourceBranch { get; set; } = string.Empty;

    public static List<Option> Options => [];

    public static List<Argument> Arguments => [
        new Argument<string>("source-branch")
        {
            Description = "The source branch to sync from (e.g. release/foo)."
                + " Must match the pattern 'release/*'."
                + " It will be synced to internal/release/*.",
        },
    ];
}

public sealed class SyncInternalReleaseCommand(
    // IGitRepoFactory gitRepoFactory,
    ILocalGitRepoFactory localGitRepoFactory,
    ILogger<SyncInternalReleaseCommand> logger) : BaseCommand<SyncInternalReleaseOptions>
{
    // private readonly IGitRepoFactory _gitRepoFactory = gitRepoFactory;
    private readonly ILocalGitRepoFactory _localGitRepoFactory = localGitRepoFactory;
    private readonly ILogger<SyncInternalReleaseCommand> _logger = logger;

    public override async Task<int> ExecuteAsync(SyncInternalReleaseOptions options)
    {
        // var repo = _gitRepoFactory.CreateClient(".");

        var localRepo = _localGitRepoFactory.Create(new NativePath("."));

        var thisCommit = await localRepo.GetGitCommitAsync();
        var thisBranch = await localRepo.GetCheckedOutBranchAsync();

        if (!thisBranch.StartsWith("release/"))
        {
            throw new WrongBranchException($"Current branch '{thisBranch}' is not a release branch.");
        }

        _logger.LogInformation("Current commit: {thisCommit}", thisCommit);
        return 0;
    }
}

public sealed class WrongBranchException(string message) : InvalidOperationException(message);
