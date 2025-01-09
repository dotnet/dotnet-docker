// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;
using Xunit.Abstractions;

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
        public async Task VerifyInternalDockerfilesOutput()
        {
            using TempFolderContext outputDirectory = FileHelper.UseTempFolder();
            string dockerfilesPath = Path.Combine(outputDirectory.Path, "src");

            string errorMessage = $"Failed to generate Dockerfiles using `{s_generateDockerfilesScript}`.";
            ExecuteScript(
                s_generateDockerfilesScript,
                $"-Output {outputDirectory.Path} -IsInternalOverride",
                errorMessage);

            await DockerfileHelper.ScrubDockerfilesAsync(dockerfilesPath);
            await VerifyDirectory(dockerfilesPath).UseDirectory("Baselines");
        }

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
