// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.DotNet.VersionTools.Dependencies;
using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dotnet.Docker.Nightly
{
    public static class Program
    {
        private static Options Options { get; set; } = new Options();
        private static string RepoRoot { get; set; } = Directory.GetCurrentDirectory();
        private const string RuntimeBuildInfoName = "Runtime";
        private const string SdkBuildInfoName = "Sdk";

        public static async Task Main(string[] args)
        {
            try
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

                Options.Parse(args);

                DependencyUpdateResults updateResults = await UpdateFilesAsync();
                if (updateResults.ChangesDetected())
                {
                    if (Options.UpdateOnly)
                    {
                        Trace.TraceInformation($"Changes made but no GitHub credentials specified, skipping PR creation");
                    }
                    else
                    {
                        await CreatePullRequestAsync(updateResults);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to update dependencies:{Environment.NewLine}{e.ToString()}");
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }

        private static async Task<DependencyUpdateResults> UpdateFilesAsync()
        {
                IEnumerable<IDependencyInfo> buildInfos = await GetBuildInfoAsync();
                string sdkVersion = buildInfos.GetBuildVersion(SdkBuildInfoName);
                string dockerfileVersion = sdkVersion.Substring(0, sdkVersion.LastIndexOf('.'));
                IEnumerable<IDependencyUpdater> updaters = GetUpdaters(dockerfileVersion);

                return DependencyUpdateUtils.Update(updaters, buildInfos);
        }

        private static async Task<IEnumerable<IDependencyInfo>> GetBuildInfoAsync()
        {
            Trace.TraceInformation($"Retrieving build info from '{Options.BuildInfoUrl}'");

            using (HttpClient client = new HttpClient())
            using (Stream stream = await client.GetStreamAsync(Options.BuildInfoUrl))
            {
                XDocument buildInfoXml = XDocument.Load(stream);
                OrchestratedBuildModel buildInfo = OrchestratedBuildModel.Parse(buildInfoXml.Root);
                BuildIdentity sdkBuild = buildInfo.Builds
                    .First(build => string.Equals(build.Name, "cli", StringComparison.OrdinalIgnoreCase));
                BuildIdentity coreSetupBuild = buildInfo.Builds
                    .First(build => string.Equals(build.Name, "core-setup", StringComparison.OrdinalIgnoreCase));

                return new[]
                {
                    // TODO use sdkBuild.ProductVersion once written to build-info
                    CreateDependencyBuildInfo(SdkBuildInfoName, sdkBuild.BuildId),
                    CreateDependencyBuildInfo(RuntimeBuildInfoName, coreSetupBuild.ProductVersion),
                };
            };
        }

        private static IDependencyInfo CreateDependencyBuildInfo(string name, string latestReleaseVersion)
        {
            return new BuildDependencyInfo(
                new BuildInfo()
                {
                    Name = name,
                    LatestReleaseVersion = latestReleaseVersion,
                    LatestPackages = new Dictionary<string, string>()
                },
                false,
                Enumerable.Empty<string>());
        }

        private static async Task CreatePullRequestAsync(DependencyUpdateResults updateResults)
        {
            GitHubAuth gitHubAuth = new GitHubAuth(Options.GitHubPassword, Options.GitHubUser, Options.GitHubEmail);
            PullRequestCreator prCreator = new PullRequestCreator(gitHubAuth, Options.GitHubUser);
            PullRequestOptions prOptions = new PullRequestOptions()
            {
                BranchNamingStrategy = new SingleBranchNamingStrategy($"UpdateDependencies-{Options.GitHubUpstreamBranch}")
            };

            string sdkVersion = updateResults.UsedInfos.GetBuildVersion(SdkBuildInfoName);
            string commitMessage = $"Update {Options.GitHubUpstreamBranch} SDK to {sdkVersion}";

            await prCreator.CreateOrUpdateAsync(
                commitMessage,
                commitMessage,
                string.Empty,
                new GitHubBranch(Options.GitHubUpstreamBranch, new GitHubProject(Options.GitHubProject, Options.GitHubUpstreamOwner)),
                new GitHubProject(Options.GitHubProject, gitHubAuth.User),
                prOptions);
        }

        private static string GetBuildVersion(this IEnumerable<IDependencyInfo> buildInfos, string name)
        {
            return buildInfos.First(bi => bi.SimpleName == name).SimpleVersion;
        }

        private static IEnumerable<IDependencyUpdater> GetUpdaters(string dockerfileVersion)
        {
            string[] dockerfiles = Directory.GetFiles(
                Path.Combine(RepoRoot, dockerfileVersion),
                "Dockerfile",
                SearchOption.AllDirectories);

            Trace.TraceInformation("Updating the following Dockerfiles:");
            Trace.TraceInformation(string.Join(Environment.NewLine, dockerfiles));

            return dockerfiles
                .Select(path => CreateDockerfileEnvUpdater(path, "DOTNET_SDK_VERSION", SdkBuildInfoName))
                .Concat(dockerfiles.Select(path => CreateDockerfileEnvUpdater(path, "DOTNET_VERSION", RuntimeBuildInfoName)))
                .Concat(dockerfiles.Select(path => new DockerfileShaUpdater(path)));
        }

        private static IDependencyUpdater CreateDockerfileEnvUpdater(string path, string envName, string buildInfoName)
        {
            return new FileRegexReleaseUpdater()
            {
                Path = path,
                BuildInfoName = buildInfoName,
                Regex = new Regex($"ENV {envName} (?<envValue>[^\r\n]*)"),
                VersionGroupName = "envValue"
            };
        }
    }
}
