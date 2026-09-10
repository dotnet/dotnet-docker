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
        /// Additional manifest values, resolved before choosing the workspace to update.
        /// </summary>
        internal List<VariableUpdateInfo> VariableUpdates { get; } = [];

        public override async Task<int> ExecuteAsync(SpecificCommandOptions options)
        {
            using TextWriterTraceListener consoleTraceListener = new(Console.Out);
            Trace.Listeners.Add(consoleTraceListener);

            try
            {
                string repoRoot = Path.GetFullPath(options.RepoRoot);
                options = options with { RepoRoot = repoRoot };

                // A supplied checksum file is an input from the caller's checkout, not the clone.
                if (!string.IsNullOrEmpty(options.ChecksumsFile))
                {
                    string checksumsFile = Path.GetFullPath(options.ChecksumsFile, repoRoot);
                    options = options with { ChecksumsFile = checksumsFile };
                }

                if (options.UpdateOnly)
                {
                    bool changesDetected = await ApplyUpdatesAsync(options);
                    if (changesDetected)
                    {
                        Trace.TraceInformation("Changes made but no credentials specified, skipping push to remote.");
                    }
                }
                else
                {
                    var publisher = new DependencyUpdatePublisher(options);
                    await publisher.PublishAsync(async (gitContext, cancellationToken) =>
                    {
                        var updateOptions = options with { RepoRoot = gitContext.WorkspaceDirectory };
                        await ApplyUpdatesAsync(updateOptions, cancellationToken);
                    });
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to update dependencies:{Environment.NewLine}{e}");
                return 1;
            }
            finally
            {
                Trace.Listeners.Remove(consoleTraceListener);
            }
        }

        private async Task<bool> ApplyUpdatesAsync(
            SpecificCommandOptions options,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using ErrorTraceListener errorTraceListener = new();
            Trace.Listeners.Add(errorTraceListener);

            try
            {
                // VersionTools and the generation scripts operate in the current directory.
                // Restore it before GitAutomation commits or cleans up its workspace.
                using var context = DirectoryStack.Push(options.RepoRoot);

                IDependencyInfo[] productBuildInfos = options.ProductVersions
                    .Select(kvp => CreateDependencyBuildInfo(kvp.Key, kvp.Value))
                    .ToArray();
                IDependencyInfo[] toolBuildInfos =
                    await Task.WhenAll(options.Tools.Select(Tools.GetToolBuildInfoAsync));

                cancellationToken.ThrowIfCancellationRequested();

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

                if (VariableUpdates.Count != 0)
                {
                    // Bind edits to this workspace, not the checkout used to resolve versions.
                    var variableUpdaters = VariableUpdates
                        .Select(update => new VariableUpdater(manifestFilePath, update));

                    DependencyUpdateResults customUpdateResults = UpdateFiles(VariableUpdates, variableUpdaters);
                    updateResults.Add(customUpdateResults);
                }

                IEnumerable<IDependencyUpdater> generatedContentUpdaters = GetGeneratedContentUpdaters(options.RepoRoot);
                IEnumerable<IDependencyInfo> allBuildInfos = [..productBuildInfos, ..toolBuildInfos];
                cancellationToken.ThrowIfCancellationRequested();
                UpdateFiles(allBuildInfos, generatedContentUpdaters);
                cancellationToken.ThrowIfCancellationRequested();

                if (errorTraceListener.Errors.Any())
                {
                    string errors = string.Join(Environment.NewLine, errorTraceListener.Errors);
                    throw new InvalidOperationException($"Dependency updates reported errors:{Environment.NewLine}{errors}");
                }

                bool changesDetected = updateResults.Any(result => result.ChangesDetected());
                if (!changesDetected)
                {
                    Trace.TraceInformation("No changes detected after updates.");
                }

                return changesDetected;
            }
            finally
            {
                Trace.Listeners.Remove(errorTraceListener);
            }
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
