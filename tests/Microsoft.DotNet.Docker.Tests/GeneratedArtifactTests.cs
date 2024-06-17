// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "pre-build")]
    public class GeneratedArtifactTests
    {
        private ITestOutputHelper OutputHelper { get; }

        public GeneratedArtifactTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        [Fact]
        public void VerifyDockerfileTemplates()
        {
            string generateDockerfilesScript = Path.Combine(Config.SourceRepoRoot, "eng", "dockerfile-templates", "Get-GeneratedDockerfiles.ps1");
            ValidateGeneratedArtifacts(
                generateDockerfilesScript,
                $"The Dockerfiles are out of sync with the templates.  Update the Dockerfiles by running `{generateDockerfilesScript}`.");
        }

        [Fact]
        public void VerifyReadmeTemplates()
        {
            string generateTagsDocumentationScript = Path.Combine(Config.SourceRepoRoot, "eng", "readme-templates", "Get-GeneratedReadmes.ps1");
            ValidateGeneratedArtifacts(
                generateTagsDocumentationScript,
                $"The Readmes are out of sync with the templates.  Update the Readmes by running `{generateTagsDocumentationScript}`.");
        }

        private void ValidateGeneratedArtifacts(string generateScriptPath, string errorMessage)
        {
            string powershellArgs = $"-File {generateScriptPath} -Validate";
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
