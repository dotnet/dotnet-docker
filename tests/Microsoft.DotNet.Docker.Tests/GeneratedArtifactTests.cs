// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Shouldly;
using Shouldly.Configuration;
using Xunit;
using Xunit.Abstractions;
using Microsoft.DotNet.Docker.Tests.Extensions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "pre-build")]
    public class GeneratedArtifactTests
    {
        private static readonly string s_generateDockerfilesScript =
            Path.Combine(Config.SourceRepoRoot, "eng", "dockerfile-templates", "Get-GeneratedDockerfiles.ps1");

        private static readonly string s_generateTagsDocumentationScript =
            Path.Combine(Config.SourceRepoRoot, "eng", "readme-templates", "Get-GeneratedReadmes.ps1");

        private ITestOutputHelper OutputHelper { get; }

        public GeneratedArtifactTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        [Fact]
        public void VerifyDockerfileTemplates()
        {
            ValidateGeneratedArtifacts(s_generateDockerfilesScript,
                $"The Dockerfiles are out of sync with the templates." +
                $"Update the Dockerfiles by running `{s_generateDockerfilesScript}`.");
        }

        [Fact]
        public void VerifyReadmeTemplates()
        {
            ValidateGeneratedArtifacts(s_generateTagsDocumentationScript,
                $"The Readmes are out of sync with the templates." +
                $"Update the Readmes by running `{s_generateTagsDocumentationScript}`.");
        }

        [Fact]
        public void VerifyInternalDockerfilesOutput()
        {
            const string InternalDockerfilesSubfolder = "Baselines";

            using TempFolderContext outputDirectory = FileHelper.UseTempFolder();
            string dockerfilesOutputDirectory = Path.Combine(outputDirectory.Path, "src");
            string errorMessage = $"Failed to generate Dockerfiles using `{s_generateDockerfilesScript}`.";

            // Generate internal Dockerfiles
            ExecuteScript(
                s_generateDockerfilesScript,
                $"-Output {outputDirectory.Path} -IsInternalOverride",
                errorMessage);

            string[] dockerfilePaths = DockerfileHelper.GetAllDockerfilesInDirectory(dockerfilesOutputDirectory);

            // Set up configuration for approval tests
            string GetDiscriminator(string dockerfilePath) =>
                dockerfilePath
                    .Remove(0, dockerfilesOutputDirectory.Length + 1)
                    .Replace('\\', '-');

            Action<ShouldMatchConfigurationBuilder> Configure(string dockerfilePath) =>
                (ShouldMatchConfigurationBuilder config) =>
                {
                    config
                        // Don't launch diff tool automatically since we have a large number of files
                        .NoDiff()
                        // Use the caller location (this method) for the test method name,
                        // since we perform the comparison inside an extension method
                        .UseCallerLocation()
                        .SubFolder(InternalDockerfilesSubfolder)
                        // Discriminator determines the name of the approved files,
                        // which needs to be unique per Dockerfile
                        .WithDiscriminator(GetDiscriminator(dockerfilePath))
                        .WithFilenameGenerator(MatchTestFilenameGenerator)
                        .WithScrubber(DockerfileHelper.DockerfileScrubber);
                };

            // Validate the generated Dockerfiles
            dockerfilePaths.ShouldAllSatisfy(
                path => File.ReadAllText(path).ShouldMatchApproved(configureOptions: Configure(path)));
        }

        private static string MatchTestFilenameGenerator(
            TestMethodInfo testMethodInfo,
            string discriminator,
            string fileType,
            string fileExtension) =>
                Path.Combine(
                    testMethodInfo.DeclaringTypeName,
                    testMethodInfo.MethodName,
                    // The discriminator identifies the Dockerfile.
                    // Remove the first character to remove the leading '.'
                    $"{discriminator[1..]}.{fileType}.{fileExtension}");

        private void ValidateGeneratedArtifacts(string generateScriptPath, string errorMessage)
        {
            ExecuteScript(generateScriptPath, "-Validate", errorMessage);
        }

        private void ExecuteScript(string generateScriptPath, string arguments, string errorMessage)
        {
            string powershellArgs = $"-File {generateScriptPath} {arguments}";
            (Process Process, string StdOut, string StdErr) executeResult;

            // Support both execution within Windows 10, Nano Server and Linux environments.
            try
            {
                executeResult = ExecuteHelper.ExecuteProcess("pwsh", powershellArgs, OutputHelper);
            }
            catch (Win32Exception)
            {
                executeResult = ExecuteHelper.ExecuteProcess("powershell", powershellArgs, OutputHelper);
            }

            if (executeResult.Process.ExitCode != 0)
            {
                OutputHelper.WriteLine(errorMessage);
            }

            Assert.Equal(0, executeResult.Process.ExitCode);
        }
    }
}
