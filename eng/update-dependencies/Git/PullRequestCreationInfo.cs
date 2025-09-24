// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker.Git;

/// <summary>
/// Contains information needed to create a pull request.
/// </summary>
/// <param name="Title">The title of the pull request.</param>
/// <param name="Body">The description/body of the pull request.</param>
/// <param name="BaseBranch">The target branch that changes will be merged into.</param>
/// <param name="HeadBranch">The source branch containing the changes.</param>
public record PullRequestCreationInfo(string Title, string Body, string BaseBranch, string HeadBranch);
