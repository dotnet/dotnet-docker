// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.DotNet.VersionTools;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.Dependencies;
using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput;
using Newtonsoft.Json.Linq;

namespace Dotnet.Docker
{
    public class SpecificCommand : BaseCommand<SpecificCommandOptions>
    {
        private static SpecificCommandOptions? s_options;

        private static SpecificCommandOptions Options
        {
            get => s_options ?? throw new InvalidOperationException($"{nameof(Options)} has not been set.");
            set => s_options = value;
        }

        /// <summary>
        /// Custom dependency updaters to run in addition to what is
        /// automatically ran based on <see cref="Options"/> .
        /// </summary>
        public List<IDependencyUpdater> CustomUpdaters { get; } = [];

        /// <summary>
        /// Custom build infos to use for the <see cref="CustomUpdaters"/>
        /// </summary>
        public List<IDependencyInfo> CustomUpdateInfos { get; } = [];

        public override async Task<int> ExecuteAsync(SpecificCommandOptions options)
        {
            Options = options;
            int exitCode = 0;

            ErrorTraceListener errorTraceListener = new();
            TextWriterTraceListener consoleTraceListener = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(errorTraceListener);
            Trace.Listeners.Add(consoleTraceListener);

            try
            {
                // We control what files Microsoft.DotNet.VersionTools updates via the updaters we
                // provide, but it has no configuration options for what directory to run git
                // commands from. If the working directory isn't the same as the repo we want to
                // modify, then git commands might do unexpected things - for example, git might
                // report no changes were made when in fact we changed files in a different repo.
                // We need to explicitly change directories to the one passed in via options.
                // The previous working directory will be restored at the end of the current scope,
                // when context is disposed.
                using var context = DirectoryStack.Push(options.RepoRoot);

                IDependencyInfo[] productBuildInfos = Options.ProductVersions
                    .Select(kvp => CreateDependencyBuildInfo(kvp.Key, kvp.Value))
                    .ToArray();
                IDependencyInfo[] toolBuildInfos =
                    await Task.WhenAll(Options.Tools.Select(Tools.GetToolBuildInfoAsync));

                // Load manifest variables once, up front
                var manifestFilePath = options.GetManifestVersionsFilePath();
                var manifestVariables = ManifestVariables.FromFile(manifestFilePath);

                List<DependencyUpdateResults> updateResults = [];

                if (productBuildInfos.Length != 0)
                {
                    IEnumerable<IDependencyUpdater> productUpdaters = GetProductUpdaters(manifestVariables);
                    DependencyUpdateResults productUpdateResults = UpdateFiles(productBuildInfos, productUpdaters);
                    updateResults.Add(productUpdateResults);
                }

                if (toolBuildInfos.Length != 0)
                {
                    IEnumerable<IDependencyUpdater> toolUpdaters = Tools.GetToolUpdaters(manifestFilePath);
                    DependencyUpdateResults toolUpdateResults = UpdateFiles(toolBuildInfos, toolUpdaters);
                    updateResults.Add(toolUpdateResults);
                }

                if (CustomUpdaters.Count != 0)
                {
                    DependencyUpdateResults customUpdateResults = UpdateFiles(CustomUpdateInfos, CustomUpdaters);
                    updateResults.Add(customUpdateResults);
                }

                IEnumerable<IDependencyUpdater> generatedContentUpdaters = GetGeneratedContentUpdaters();
                IEnumerable<IDependencyInfo> allBuildInfos = [..productBuildInfos, ..toolBuildInfos];
                UpdateFiles(allBuildInfos, generatedContentUpdaters);

                if (errorTraceListener.Errors.Any())
                {
                    string errors = string.Join(Environment.NewLine, errorTraceListener.Errors);
                    Console.Error.WriteLine("Failed to update dependencies due to the following errors:");
                    Console.Error.WriteLine(errors);
                    Environment.Exit(1);
                }

                if (!updateResults.Any(result => result.ChangesDetected()))
                {
                    Trace.TraceInformation("No changes detected after updates.");
                    return 0;
                }

                if (string.IsNullOrEmpty(Options.Email)
                    || string.IsNullOrEmpty(Options.Password)
                    || string.IsNullOrEmpty(Options.User)
                    || string.IsNullOrEmpty(Options.TargetBranch))
                {
                    Trace.TraceInformation("Changes made but no credentials specified, skipping push to remote.");
                    return 0;
                }

                GitRemote remote = Options switch
                {
                    {
                        AzdoOrganization: not null and not "",
                        AzdoProject: not null and not "",
                        AzdoRepo: not null and not "",
                    } => GitRemote.AzureDevOps,
                    _ => GitRemote.GitHub,
                };

                string commitMessage = $"[{Options.TargetBranch}] Update dependencies from {Options.VersionSourceName}";

                switch (remote)
                {
                    case GitRemote.AzureDevOps:
                        PushToAzdoBranch(commitMessage, Options.TargetBranch);
                        break;

                    case GitRemote.GitHub:
                        var branchSuffix = FormatBranchName($"UpdateDependencies-{Options.TargetBranch}-From-{Options.VersionSourceName}");
                        var branchNamingStrategy = new SingleBranchNamingStrategy(branchSuffix);
                        var prOptions = new PullRequestOptions() { BranchNamingStrategy = branchNamingStrategy };
                        await CreateGitHubPullRequest(commitMessage, prOptions, branchSuffix);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to update dependencies:{Environment.NewLine}{e}");
                exitCode = 1;
            }
            finally
            {
                Trace.Listeners.Remove(errorTraceListener);
                Trace.Listeners.Remove(consoleTraceListener);
                errorTraceListener.Dispose();
                consoleTraceListener.Dispose();
                s_options = null;
            }

            return exitCode;
        }

        private static DependencyUpdateResults UpdateFiles(
            IEnumerable<IDependencyInfo> buildInfos,
            IEnumerable<IDependencyUpdater> updaters)
        {
            DependencyUpdateResults results = DependencyUpdateUtils.Update(updaters, buildInfos);
            Console.WriteLine(results.GetSuggestedCommitMessage());
            return results;
        }

        private static IDependencyInfo CreateDependencyBuildInfo(string name, string? version)
        {
            return new BuildDependencyInfo(
                new BuildInfo()
                {
                    Name = name,
                    LatestReleaseVersion = version,
                    LatestPackages = new Dictionary<string, string>()
                },
                false,
                Enumerable.Empty<string>());
        }

        // Replace slashes with hyphens for use in naming the branch
        private static string FormatBranchName(string branchName) => branchName.Replace('/', '-');

        private static void PushToAzdoBranch(string commitMessage, string targetBranch)
        {
            using Repository repo = new(Options.RepoRoot);

            // Commit the existing changes
            Commands.Stage(repo, "*");
            Signature signature = new(Options.User, Options.Email, DateTimeOffset.Now);
            repo.Commit(commitMessage, signature, signature);

            PushOptions pushOptions = new()
            {
                CredentialsProvider = (url, user, credTypes) => new UsernamePasswordCredentials
                {
                    Username = Options.Password, // it doesn't make sense but a PAT needs to be set as username
                    Password = string.Empty
                }
            };

            // Create a remote to AzDO
            string remoteName = GetUniqueName(
                repo.Network.Remotes.Select(remote => remote.Name).ToList(),
                "azuredevops"
            );

            var url = Options.GetAzdoRepoUrl();
            Trace.WriteLine($"Adding remote {remoteName} with URL {url}");
            Remote remote = repo.Network.Remotes.Add(remoteName, url);

            try
            {
                // Push the commit to AzDO
                string username = Options.Email.Substring(0, Options.Email.IndexOf('@'));
                string pushRefSpec = $@"refs/heads/{targetBranch}";

                Trace.WriteLine($"Pushing to {targetBranch}");

                // Force push
                repo.Network.Push(remote, "+HEAD", pushRefSpec, pushOptions);
            }
            finally
            {
                // Clean up the AzDO remote that was created
                repo.Network.Remotes.Remove(remote.Name);
            }
        }

        private static string GetUniqueName(IEnumerable<string> existingNames, string suggestedName, int? index = null)
        {
            string name = suggestedName + index?.ToString();
            if (existingNames.Any(val => val == name))
            {
                return GetUniqueName(existingNames, suggestedName, index is null ? 1 : ++index);
            }

            return name;
        }

        private static async Task CreateGitHubPullRequest(string commitMessage, PullRequestOptions prOptions, string branchSuffix)
        {
            GitHubAuth gitHubAuth = new(Options.Password, Options.User, Options.Email);
            PullRequestCreator prCreator = new(gitHubAuth, Options.User);

            GitHubProject upstreamProject = new(Options.GitHubProject, Options.GitHubUpstreamOwner);
            GitHubBranch upstreamBranch = new(Options.TargetBranch, upstreamProject);

            using (GitHubClient client = new(gitHubAuth))
            {
                GitHubPullRequest pullRequestToUpdate = await client.SearchPullRequestsAsync(
                    upstreamProject,
                    upstreamBranch.Name,
                    await client.GetMyAuthorIdAsync());

                if (pullRequestToUpdate == null || pullRequestToUpdate.Head.Ref != $"{upstreamBranch.Name}-{branchSuffix}")
                {
                    Trace.WriteLine("Didn't find a PR to update. Submitting a new one.");
                    await prCreator.CreateOrUpdateAsync(
                        commitMessage,
                        commitMessage,
                        string.Empty,
                        upstreamBranch,
                        new GitHubProject(Options.GitHubProject, gitHubAuth.User),
                        prOptions);
                }
                else
                {
                    UpdateExistingGitHubPullRequest(gitHubAuth, prOptions, commitMessage, upstreamBranch);
                }
            }
        }

        private static void UpdateExistingGitHubPullRequest(
            GitHubAuth gitHubAuth, PullRequestOptions prOptions, string commitMessage, GitHubBranch upstreamBranch)
        {
            // PullRequestCreator ends up force-pushing updates to an existing PR which is not great when the logic
            // gets called on a schedule (see https://github.com/dotnet/dotnet-docker/issues/1114). To avoid this,
            // it needs the ability to only update files that have changed from the existing PR.  Because the
            // PullRequestCreator class doesn't rely on there being a locally cloned repo, it doesn't have an
            // efficient way to determine whether files have changed or not. Update-dependencies would have to
            // implement logic which pulls down each file individually from the API and compare it to what exists
            // in the local repo.  Since that's not an efficient process, this method works by cloning the PR's
            // branch to a temporary repo location, grabbing the whole repo where the original updates from
            // update-dependencies were made and copying it into the temp repo, and committing and pushing
            // those changes in the temp repo back to the PR's branch.

            Trace.WriteLine($"Updating existing pull request for branch {upstreamBranch.Name}.");

            string tempRepoPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            try
            {
                string branchName = prOptions.BranchNamingStrategy.Prefix(upstreamBranch.Name);
                CloneOptions cloneOptions = new()
                {
                    BranchName = branchName
                };

                // Clone the PR's repo/branch to a temp location
                Repository.Clone($"https://github.com/{gitHubAuth.User}/{Options.GitHubProject}", tempRepoPath, cloneOptions);

                // Remove all existing directories and files from the temp repo
                ClearRepoContents(tempRepoPath);

                // Copy contents of local repo changes to temp repo
                DirectoryCopy(".", tempRepoPath);

                using Repository repo = new(tempRepoPath);
                RepositoryStatus status = repo.RetrieveStatus(new StatusOptions());

                // If there are any changes from what exists in the PR
                if (status.IsDirty)
                {
                    Trace.WriteLine($"Detected changes that don't currently exist in the PR.");

                    Commands.Stage(repo, "*");

                    Signature signature = new(Options.User, Options.Email, DateTimeOffset.Now);
                    repo.Commit(commitMessage, signature, signature);

                    Branch branch = repo.Branches[$"origin/{branchName}"];

                    PushOptions pushOptions = new()
                    {
                        CredentialsProvider = (url, user, credTypes) => new UsernamePasswordCredentials
                        {
                            Username = Options.Password,
                            Password = string.Empty
                        }
                    };

                    Remote remote = repo.Network.Remotes["origin"];
                    string pushRefSpec = $@"refs/heads/{branchName}";

                    repo.Network.Push(remote, pushRefSpec, pushOptions);
                }
                else
                {
                    Trace.WriteLine($"No new changes were detected - skipping PR update.");
                }
            }
            finally
            {
                // Cleanup temp repo
                DeleteRepoDirectory(tempRepoPath);
            }
        }

        private static void DeleteRepoDirectory(string repoPath)
        {
            if (Directory.Exists(repoPath))
            {
                IEnumerable<string> gitFiles = Directory.GetFiles(
                    Path.Combine(repoPath, ".git"), "*", SearchOption.AllDirectories);

                // Ensure all files in .git folder are writable
                foreach (string file in gitFiles)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }

                Directory.Delete(repoPath, true);
            }
        }

        private static void ClearRepoContents(string repoPath)
        {
            foreach (string file in Directory.GetFiles(repoPath))
            {
                File.Delete(file);
            }
            foreach (DirectoryInfo dir in new DirectoryInfo(repoPath).GetDirectories().Where(dir => dir.Name != ".git"))
            {
                Directory.Delete(dir.FullName, true);
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new(sourceDirName);

            DirectoryInfo[] dirs = dir.GetDirectories()
                .Where(dir => dir.Name != ".git")
                .ToArray();

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        private static IEnumerable<IDependencyUpdater> GetProductUpdaters(ManifestVariables manifestVariables)
        {
            // NOTE: The order in which the updaters are returned/invoked is important as there are cross dependencies
            // (e.g. sha updater requires the version numbers to be updated within the Dockerfiles)

            List<IDependencyUpdater> updaters =
            [
                new NuGetConfigUpdater(manifestVariables, Options),
                ..BaseUrlUpdater.CreateUpdaters(manifestVariables, Options)
            ];

            foreach (string productName in Options.ProductVersions.Keys)
            {
                updaters.Add(new VersionUpdater(VersionType.Build, productName, Options.DockerfileVersion, Options));
                updaters.Add(new VersionUpdater(VersionType.Product, productName, Options.DockerfileVersion, Options));

                var shaUpdaters = DockerfileShaUpdater.CreateUpdaters(
                    productName: productName,
                    dockerfileVersion: Options.DockerfileVersion,
                    options: Options,
                    variables: manifestVariables);

                updaters.AddRange(shaUpdaters);
            }

            return updaters;
        }

        private static IEnumerable<IDependencyUpdater> GetGeneratedContentUpdaters() =>
        [
            ScriptRunnerUpdater.GetDockerfileUpdater(Options.RepoRoot),
            ScriptRunnerUpdater.GetReadMeUpdater(Options.RepoRoot)
        ];
    }
}
