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
            string productVersion = buildInfos.GetBuildVersion(RuntimeBuildInfoName) ??
                buildInfos.GetBuildVersion(SdkBuildInfoName) ??
                buildInfos.GetBuildVersion(AspNetCoreBuildInfoName);
            string dockerfileVersion = productVersion.Substring(0, productVersion.LastIndexOf('.'));
            IEnumerable<IDependencyUpdater> updaters = GetUpdaters(dockerfileVersion, buildInfos);

            return DependencyUpdateUtils.Update(updaters, buildInfos);
        }

        private static async Task<IEnumerable<IDependencyInfo>> GetBuildInfoAsync()
        {
            IEnumerable<IDependencyInfo> buildInfo;

            if (Options.BuildInfoUrl != null)
            {
                buildInfo = await LoadBuildInfoXml();
            }
            else
            {
                List<IDependencyInfo> buildInfoList = new List<IDependencyInfo>();
                buildInfo = buildInfoList;

                if (Options.AspnetVersion != null)
                {
                    buildInfoList.Add(CreateDependencyBuildInfo(AspNetCoreBuildInfoName, Options.AspnetVersion));
                }
                if (Options.RuntimeVersion != null)
                {
                    buildInfoList.Add(CreateDependencyBuildInfo(RuntimeBuildInfoName, Options.RuntimeVersion));
                }
                if (Options.SdkVersion != null)
                {
                    buildInfoList.Add(CreateDependencyBuildInfo(SdkBuildInfoName, Options.SdkVersion));
                }
            }

            return buildInfo;
        }

        private static async Task<IEnumerable<IDependencyInfo>> LoadBuildInfoXml()
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
            return CreateDependencyBuildInfo(name, buildId.ProductVersion);
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

        private static async Task CreatePullRequestAsync(IEnumerable<IDependencyInfo> buildInfos)
        {
            GitHubAuth gitHubAuth = new GitHubAuth(Options.GitHubPassword, Options.GitHubUser, Options.GitHubEmail);
            PullRequestCreator prCreator = new PullRequestCreator(gitHubAuth, Options.GitHubUser);
            PullRequestOptions prOptions = new PullRequestOptions()
            {
                BranchNamingStrategy = new SingleBranchNamingStrategy($"UpdateDependencies-{Options.GitHubUpstreamBranch}")
            };

            string commitMessage = $"[{Options.GitHubUpstreamBranch}] Update dependencies from dotnet/core-sdk";

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
            return buildInfos.FirstOrDefault(bi => bi.SimpleName == name)?.SimpleVersion;
        }

        private static IEnumerable<IDependencyUpdater> GetUpdaters(
            string dockerfileVersion, IEnumerable<IDependencyInfo> buildInfos)
        {
            string[] dockerfiles = Directory.GetFiles(
                Path.Combine(RepoRoot, dockerfileVersion),
                "Dockerfile",
                SearchOption.AllDirectories);

            Trace.TraceInformation("Updating the following Dockerfiles:");
            Trace.TraceInformation(string.Join(Environment.NewLine, dockerfiles));

            // NOTE: The order in which the updaters are returned/invoked is important as there are cross dependencies 
            // (e.g. sha updater requires the version numbers to be updated within the Dockerfiles)
            List<IDependencyUpdater> manifestBasedUpdaters = new List<IDependencyUpdater>();
            CreateManifestUpdater(manifestBasedUpdaters, "Sdk", buildInfos, SdkBuildInfoName);
            CreateManifestUpdater(manifestBasedUpdaters, "Runtime", buildInfos, RuntimeBuildInfoName);
            manifestBasedUpdaters.Add(new ReadMeUpdater(RepoRoot));

            return CreateDockerfileEnvUpdaters(dockerfiles, buildInfos, "DOTNET_SDK_VERSION", SdkBuildInfoName)
                .Concat(CreateDockerfileEnvUpdaters(dockerfiles, buildInfos, "ASPNETCORE_VERSION", AspNetCoreBuildInfoName))
                .Concat(CreateDockerfileEnvUpdaters(dockerfiles, buildInfos, "DOTNET_VERSION", RuntimeBuildInfoName))
                .Concat(dockerfiles.Select(path => DockerfileShaUpdater.CreateProductShaUpdater(path)))
                .Concat(dockerfiles.Select(path => DockerfileShaUpdater.CreateLzmaShaUpdater(path)))
                .Concat(manifestBasedUpdaters);
        }

        private static IEnumerable<IDependencyUpdater> CreateDockerfileEnvUpdaters(
            string[] dockerfilePaths, IEnumerable<IDependencyInfo> buildInfos, string envName, string buildInfoName)
        {
            return GetBuildVersion(buildInfos, buildInfoName) == null 
                ? Enumerable.Empty<IDependencyUpdater>()
                : dockerfilePaths.Select(path => CreateDockerfileEnvUpdater(path, envName, buildInfoName));
        }

        private static IDependencyUpdater CreateDockerfileEnvUpdater(string dockerfilePath, string envName, string buildInfoName)
        {        
            return new FileRegexReleaseUpdater()
            {
                Path = dockerfilePath,
                BuildInfoName = buildInfoName,
                Regex = new Regex($"ENV {envName} (?<envValue>[^\r\n]*)"),
                VersionGroupName = "envValue"
            };
        }

        private static void CreateManifestUpdater(
            List<IDependencyUpdater> manifestUpdaters,
            string imageVariantName, 
            IEnumerable<IDependencyInfo> buildInfos, 
            string buildInfoName)
        {
            string version = GetBuildVersion(buildInfos, buildInfoName);
            if (version != null)
            {
                manifestUpdaters.Add(new ManifestUpdater(imageVariantName, version, RepoRoot));
            }
        }
    }
}
