// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;

namespace Dotnet.Docker;

public abstract class CreatePullRequestOptions
{
    private string? _targetBranch = null;

    public string? Owner { get; init; } = null;
    public string? Project { get; init; } = null;
    public string? User { get; init; } = null;
    public string? Email { get; init; } = null;
    public string? Password { get; init; } = null;

    public string SourceBranch { get; init; } = "nightly";
    public string TargetBranch
    {
        get => _targetBranch ?? SourceBranch;
        init => _targetBranch = value;
    }

    public static List<Option> Options =>
    [
        new Option<string>("--owner") { Description = "Owner of the fork where the branch should be pushed" },
        new Option<string>("--project") { Description = "Name of the repo" },
        new Option<string>("--user") { Description = "GitHub login used to create PR" },
        new Option<string>("--email") { Description = "GitHub email used to create PR" },
        new Option<string>("--password") { Description = "GitHub password used to create PR" },
        new Option<string>("--source-branch") { Description = "Source branch from which the changes are derived" },
        new Option<string>("--target-branch") { Description = "Target branch for the generated PR (defaults to --source-branch)" },
    ];

    public static List<Argument> Arguments => [];
}
