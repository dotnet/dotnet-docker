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

namespace Dotnet.Docker
{
    public static class Program
    {
        private const string AspNetCoreBuildInfoName = "aspnet";
        private const string RuntimeBuildInfoName = "core-setup";
        private const string SdkBuildInfoName = "cli";

        private static Options Options { get; } = new Options();
        private static string RepoRoot { get; } = Directory.GetCurrentDirectory();

        public static async Task Main(string[] args)
        {
            try
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

                Options.Parse(args);

                IEnumerable<IDependencyInfo> buildInfos = await GetBuildInfoAsync();
                DependencyUpdateResults updateResults = UpdateFiles(buildInfos);
                if (updateResults.ChangesDetected())
                {
                    if (Options.UpdateOnly)
                    {
                        Trace.TraceInformation($"Changes made but no GitHub credentials specified, skipping PR creation");
                    }
                    else
                    {
                        await CreatePullRequestAsync(buildInfos);
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

        private static DependencyUpdateResults UpdateFiles(IEnumerable<IDependencyInfo> buildInfos)
        {
            string runtimeVersion = buildInfos.GetBuildVersion(RuntimeBuildInfoName);
            string dockerfileVersion = runtimeVersion.Substring(0, runtimeVersion.LastIndexOf('.'));
            IEnumerable<IDependencyUpdater> updaters = GetUpdaters(dockerfileVersion);

            return DependencyUpdateUtils.Update(updaters, buildInfos);
        }

        private static async Task<IEnumerable<IDependencyInfo>> GetBuildInfoAsync()
        {
            Trace.TraceInformation($"Retrieving build info from '{Options.BuildInfoUrl}'");

            XDocument buildInfoXml;
            if (File.Exists(Options.BuildInfoUrl.LocalPath))
            {
                buildInfoXml = XDocument.Load(Options.BuildInfoUrl.LocalPath);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                using (Stream stream = await client.GetStreamAsync(Options.BuildInfoUrl))
                {
                    buildInfoXml = XDocument.Load(stream);
                }
            }

            OrchestratedBuildModel buildInfo = OrchestratedBuildModel.Parse(buildInfoXml.Root);

            return new[]
            {
                CreateDependencyBuildInfo(SdkBuildInfoName, buildInfo.Builds),
                CreateDependencyBuildInfo(RuntimeBuildInfoName, buildInfo.Builds),
                CreateDependencyBuildInfo(AspNetCoreBuildInfoName, buildInfo.Builds),
            };
        }

        private static IDependencyInfo CreateDependencyBuildInfo(string name, IEnumerable<BuildIdentity> builds)
        {
            BuildIdentity buildId = builds.First(build => string.Equals(build.Name, name, StringComparison.OrdinalIgnoreCase));

            return new BuildDependencyInfo(
                new BuildInfo()
                {
                    Name = name,
                    LatestReleaseVersion = buildId.ProductVersion,
                    LatestPackages = new Dictionary<string, string>()
                },
                false,
                Enumerable.Empty<string>());
        }

        private static async Task CreatePullRequestAsync(IEnumerable<IDependencyInfo> buildInfos)
        {
            GitHubAuth gitHubAuth = new GitHubAuth(Options.GitHubPassword, Options.GitHubUser, Options.GitHubEmail);
            PullRequestCreator prCreator = new PullRequestCreator(gitHubAuth, Options.GitHubUser);
            PullRequestOptions prOptions = new PullRequestOptions()
            {
                BranchNamingStrategy = new SingleBranchNamingStrategy($"UpdateDependencies-{Options.GitHubUpstreamBranch}")
            };

            string sdkVersion = buildInfos.GetBuildVersion(SdkBuildInfoName);
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
                .Concat(dockerfiles.Select(path => CreateDockerfileEnvUpdater(path, "ASPNETCORE_VERSION", AspNetCoreBuildInfoName)))
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
