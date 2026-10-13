// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public sealed class UpdateDependenciesConfiguration
{
    public GitRemote PullRequestDestination { get; set; } = GitRemote.GitHub;
    public string User { get; set; } = "";
    public string Email { get; set; } = "";
    public string BarToken { get; set; } = "";
    public AzureDevOpsConfiguration AzureDevOps { get; set; } = new();
    public GitHubConfiguration GitHub { get; set; } = new();

    public (string Name, string Email) GetCommitterIdentity()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(User);
        ArgumentException.ThrowIfNullOrWhiteSpace(Email);
        return (User, Email);
    }
}

public sealed class AzureDevOpsConfiguration
{
    public string Organization { get; set; } = "";
    public string Project { get; set; } = "";
    public string Repository { get; set; } = "";
    public string Token { get; set; } = "";
    public bool DisableInteractiveAuth { get; set; } = true;

    public string GetRepoUrl()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Organization);
        ArgumentException.ThrowIfNullOrWhiteSpace(Project);
        ArgumentException.ThrowIfNullOrWhiteSpace(Repository);
        return $"{Organization.TrimEnd('/')}/{Uri.EscapeDataString(Project)}/_git/{Uri.EscapeDataString(Repository)}";
    }
}

public sealed class GitHubConfiguration
{
    public string Owner { get; set; } = "";
    public string Repository { get; set; } = "";
    public string Token { get; set; } = "";
    public string AppClientId { get; set; } = "";
    public string AppKeyUri { get; set; } = "";
}
