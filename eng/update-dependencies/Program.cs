// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using Microsoft.DotNet.VersionTools;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.Dependencies;
using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput;

namespace Dotnet.Docker
{
    public static class Program
    {
        public const string VersionsFilename = "manifest.versions.json";

        private static Options Options { get; set; }
        private static string RepoRoot { get; } = Directory.GetCurrentDirectory();

        public static Task Main(string[] args)
        {
            RootCommand command = new RootCommand();
            foreach (Symbol option in Options.GetCliSymbols())
            {
                command.Add(option);
            };

            command.Handler = CommandHandler.Create<Options>(ExecuteAsync);

            return command.InvokeAsync(args);
        }

        private static async Task ExecuteAsync(Options options)
        {
            Options = options;

            try
            {
                ErrorTraceListener errorTraceListener = new ErrorTraceListener();
                Trace.Listeners.Add(errorTraceListener);
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

                IEnumerable<IDependencyInfo> buildInfos = Options.ProductVersions
                    .Select(kvp => CreateDependencyBuildInfo(kvp.Key, kvp.Value))
                    .ToArray();
                DependencyUpdateResults updateResults = UpdateFiles(buildInfos);
                if (updateResults.ChangesDetected())
                {
                    if (Options.UpdateOnly)
                    {
                        Trace.TraceInformation($"Changes made but no GitHub credentials specified, skipping PR creation");
                    }
                    else
                    {
                        await CreatePullRequestAsync();
                    }
                }

                if (errorTraceListener.Errors.Any())
                {
                    string errors = string.Join(Environment.NewLine, errorTraceListener.Errors);
                    Console.Error.WriteLine("Failed to update dependencies due to the following errors:");
                    Console.Error.WriteLine(errors);
                    Console.Error.WriteLine();
                    Console.Error.WriteLine("You may need to use the --compute-shas option if checksum files are missing.");
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to update dependencies:{Environment.NewLine}{e}");
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }

        private static DependencyUpdateResults UpdateFiles(IEnumerable<IDependencyInfo> buildInfos)
        {
            IEnumerable<IDependencyUpdater> updaters = GetUpdaters();

            return DependencyUpdateUtils.Update(updaters, buildInfos);
        }

        private static IDependencyInfo CreateDependencyBuildInfo(string name, string version)
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

        private static async Task CreatePullRequestAsync()
        {
            // Replace slashes with hyphens for use in naming the branch
            string versionSourceNameForBranch = Options.VersionSourceName.Replace("/", "-");

            GitHubAuth gitHubAuth = new GitHubAuth(Options.GitHubPassword, Options.GitHubUser, Options.GitHubEmail);
            PullRequestCreator prCreator = new PullRequestCreator(gitHubAuth, Options.GitHubUser);

            string branchSuffix = $"UpdateDependencies-{Options.GitHubUpstreamBranch}-From-{versionSourceNameForBranch}";
            PullRequestOptions prOptions = new PullRequestOptions()
            {
                BranchNamingStrategy = new SingleBranchNamingStrategy(branchSuffix)
            };

            string commitMessage = $"[{Options.GitHubUpstreamBranch}] Update dependencies from {Options.VersionSourceName}";
            GitHubProject upstreamProject = new GitHubProject(Options.GitHubProject, Options.GitHubUpstreamOwner);
            GitHubBranch upstreamBranch = new GitHubBranch(Options.GitHubUpstreamBranch, upstreamProject);

            using (GitHubClient client = new GitHubClient(gitHubAuth))
            {
                GitHubPullRequest pullRequestToUpdate = await client.SearchPullRequestsAsync(
                    upstreamProject,
                    upstreamBranch.Name,
                    await client.GetMyAuthorIdAsync());

                if (pullRequestToUpdate == null || pullRequestToUpdate.Head.Ref != $"{upstreamBranch.Name}-{branchSuffix}")
                {
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
                    UpdateExistingPullRequest(gitHubAuth, prOptions, commitMessage, upstreamBranch);
                }
            }
        }

        private static void UpdateExistingPullRequest(
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

            string tempRepoPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            try
            {
                string branchName = prOptions.BranchNamingStrategy.Prefix(upstreamBranch.Name);
                CloneOptions cloneOptions = new CloneOptions
                {
                    BranchName = branchName
                };

                // Clone the PR's repo/branch to a temp location
                Repository.Clone($"https://github.com/{gitHubAuth.User}/{Options.GitHubProject}", tempRepoPath, cloneOptions);

                // Remove all existing directories and files from the temp repo
                ClearRepoContents(tempRepoPath);

                // Copy contents of local repo changes to temp repo
                DirectoryCopy(".", tempRepoPath);

                using Repository repo = new Repository(tempRepoPath);
                RepositoryStatus status = repo.RetrieveStatus(new StatusOptions());

                // If there are any changes from what exists in the PR
                if (status.IsDirty)
                {
                    Commands.Stage(repo, "*");

                    Signature signature = new Signature(Options.GitHubUser, Options.GitHubEmail, DateTimeOffset.Now);
                    repo.Commit(commitMessage, signature, signature);

                    Branch branch = repo.Branches[$"origin/{branchName}"];

                    PushOptions pushOptions = new PushOptions
                    {
                        CredentialsProvider = (url, user, credTypes) => new UsernamePasswordCredentials
                        {
                            Username = Options.GitHubPassword,
                            Password = string.Empty
                        }
                    };

                    Remote remote = repo.Network.Remotes["origin"];
                    string pushRefSpec = $@"refs/heads/{branchName}";

                    repo.Network.Push(remote, pushRefSpec, pushOptions);
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
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

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

        private static IEnumerable<IDependencyUpdater> GetUpdaters()
        {
            // NOTE: The order in which the updaters are returned/invoked is important as there are cross dependencies
            // (e.g. sha updater requires the version numbers to be updated within the Dockerfiles)
            foreach (string productName in Options.ProductVersions.Keys)
            {
                yield return new VersionUpdater(VersionType.Build, productName, Options.DockerfileVersion, RepoRoot);
                yield return new VersionUpdater(VersionType.Product, productName, Options.DockerfileVersion, RepoRoot);

                foreach (IDependencyUpdater shaUpdater in DockerfileShaUpdater.CreateUpdaters(productName, Options.DockerfileVersion, RepoRoot, Options))
                {
                    yield return shaUpdater;
                }
            }

            yield return ScriptRunnerUpdater.GetDockerfileUpdater(RepoRoot);
            yield return ScriptRunnerUpdater.GetReadMeUpdater(RepoRoot);
        }
    }
}
