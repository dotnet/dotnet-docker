// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace Dotnet.Docker
{
    public class SpecificCommand : BaseCommand<SpecificCommandOptions>
    {
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
                    await ApplyUpdatesAsync(options);
                    Trace.TraceInformation("Local updates completed without publishing.");
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

        private async Task ApplyUpdatesAsync(
            SpecificCommandOptions options,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GitHubReleaseInfo[] toolReleases = await Task.WhenAll(options.Tools.Select(Tools.GetReleaseAsync));
            cancellationToken.ThrowIfCancellationRequested();

            string manifestFilePath = options.GetManifestVersionsFilePath();
            var manifestVariables = ManifestVariables.FromFile(manifestFilePath);
            string originalContent = manifestVariables.Content;

            if (options.ProductVersions.Count != 0)
            {
                NuGetConfigUpdater.Update(manifestVariables, options);
                BaseUrlUpdater.Update(manifestVariables, options);

                foreach (var (productName, version) in options.ProductVersions)
                {
                    VersionUpdater.Update(manifestVariables, productName, version, options);
                }

                foreach (string productName in options.ProductVersions.Keys.Intersect(DockerfileShaUpdater.SupportedProducts))
                {
                    var checksumUpdater = new DockerfileShaUpdater(productName, options, manifestVariables);
                    await checksumUpdater.UpdateAsync(cancellationToken);
                }
            }

            foreach (GitHubReleaseInfo release in toolReleases)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Tools.UpdateAsync(manifestVariables, release, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            string updatedContent = manifestVariables.Content;
            if (updatedContent != originalContent)
            {
                await File.WriteAllTextAsync(manifestFilePath, updatedContent, cancellationToken);
            }

            // Generators read the saved manifest and may change files even when the manifest is unchanged.
            await ScriptRunner.GenerateDockerfilesAsync(options.RepoRoot, cancellationToken);
            await ScriptRunner.GenerateReadmesAsync(options.RepoRoot, cancellationToken);
        }
    }
}
