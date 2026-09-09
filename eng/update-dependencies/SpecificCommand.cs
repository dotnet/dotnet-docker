// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Microsoft.DotNet.VersionTools;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Dependencies;
using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput;

namespace Dotnet.Docker
{
    public class SpecificCommand : BaseCommand<SpecificCommandOptions>
    {
        /// <summary>
        /// Custom dependency updaters to run in addition to those selected by
        /// <see cref="SpecificCommandOptions"/>.
        /// </summary>
        public List<IDependencyUpdater> CustomUpdaters { get; } = [];

        /// <summary>
        /// Custom build infos to use for the <see cref="CustomUpdaters"/>.
        /// </summary>
        public List<IDependencyInfo> CustomUpdateInfos { get; } = [];

        public override async Task<int> ExecuteAsync(SpecificCommandOptions options)
        {
            int exitCode = 0;

            ErrorTraceListener errorTraceListener = new();
            TextWriterTraceListener consoleTraceListener = new(Console.Out);
            Trace.Listeners.Add(errorTraceListener);
            Trace.Listeners.Add(consoleTraceListener);

            try
            {
                // VersionTools runs git commands in the current directory rather than accepting
                // a repo path. Keep both change detection and publishing in the requested repo.
                using var context = DirectoryStack.Push(options.RepoRoot);

                IDependencyInfo[] productBuildInfos = options.ProductVersions
                    .Select(kvp => CreateDependencyBuildInfo(kvp.Key, kvp.Value))
                    .ToArray();
                IDependencyInfo[] toolBuildInfos =
                    await Task.WhenAll(options.Tools.Select(Tools.GetToolBuildInfoAsync));

                // Load manifest variables once, up front.
                var manifestFilePath = options.GetManifestVersionsFilePath();
                var manifestVariables = ManifestVariables.FromFile(manifestFilePath);

                List<DependencyUpdateResults> updateResults = [];

                if (productBuildInfos.Length != 0)
                {
                    IEnumerable<IDependencyUpdater> productUpdaters = GetProductUpdaters(manifestVariables, options);
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

                IEnumerable<IDependencyUpdater> generatedContentUpdaters = GetGeneratedContentUpdaters(options.RepoRoot);
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

                if (options.UpdateOnly)
                {
                    Trace.TraceInformation("Changes made but no credentials specified, skipping push to remote.");
                    return 0;
                }

                var publisher = new DependencyUpdatePublisher(options);
                await publisher.PublishAsync();
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

        private static IEnumerable<IDependencyUpdater> GetProductUpdaters(
            ManifestVariables manifestVariables,
            SpecificCommandOptions options)
        {
            // Preserve updater order because later operations can depend on earlier edits.
            List<IDependencyUpdater> updaters =
            [
                new NuGetConfigUpdater(manifestVariables, options),
                ..BaseUrlUpdater.CreateUpdaters(manifestVariables, options)
            ];

            foreach (string productName in options.ProductVersions.Keys)
            {
                updaters.Add(new VersionUpdater(VersionType.Build, productName, options.DockerfileVersion, options));
                updaters.Add(new VersionUpdater(VersionType.Product, productName, options.DockerfileVersion, options));

                var shaUpdaters = DockerfileShaUpdater.CreateUpdaters(
                    productName: productName,
                    dockerfileVersion: options.DockerfileVersion,
                    options: options,
                    variables: manifestVariables);

                updaters.AddRange(shaUpdaters);
            }

            return updaters;
        }

        private static IEnumerable<IDependencyUpdater> GetGeneratedContentUpdaters(string repoRoot) =>
        [
            ScriptRunnerUpdater.GetDockerfileUpdater(repoRoot),
            ScriptRunnerUpdater.GetReadMeUpdater(repoRoot)
        ];
    }
}
