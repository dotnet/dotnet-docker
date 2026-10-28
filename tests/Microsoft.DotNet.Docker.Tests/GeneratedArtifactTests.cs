// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.DotNet.Docker.Tests.Extensions;
using Shouldly;
using Shouldly.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "pre-build")]
public class GeneratedArtifactTests(ITestOutputHelper outputHelper)
{
    private static readonly string GenerateDockerfilesScript =
        Path.Combine(Config.SourceRepoRoot, "eng", "dockerfile-templates", "Get-GeneratedDockerfiles.ps1");

    private static readonly string GenerateTagsDocumentationScript =
        Path.Combine(Config.SourceRepoRoot, "eng", "readme-templates", "Get-GeneratedReadmes.ps1");

    private ITestOutputHelper OutputHelper { get; } = outputHelper;

    [Fact]
    public void VerifyDockerfileTemplates()
    {
        ValidateGeneratedArtifacts(GenerateDockerfilesScript,
            $"The Dockerfiles are out of sync with the templates." +
            $"Update the Dockerfiles by running `{GenerateDockerfilesScript}`.");
    }

    [Fact]
    public void VerifyReadmeTemplates()
    {
        ValidateGeneratedArtifacts(GenerateTagsDocumentationScript,
            $"The Readmes are out of sync with the templates." +
            $"Update the Readmes by running `{GenerateTagsDocumentationScript}`.");
    }

    [Fact]
    public void VerifyInternalDockerfilesOutput()
    {
        if (Config.IsInternal)
        {
            OutputHelper.WriteLine(
                "Skipping test since it is not useful for internal build scenarios. " +
                "If there are issues with internal Dockerfiles, then internal builds will fail.");
            return;
        }

        const string internalDockerfilesSubfolder = "Baselines";

        using TempFolderContext outputDirectory = FileHelper.UseTempFolder();
        string dockerfilesOutputDirectory = Path.Combine(outputDirectory.Path, "src");
        string errorMessage = $"Failed to generate Dockerfiles using `{GenerateDockerfilesScript}`.";

        // Override base URLs to make templates generate internal versions of Dockerfiles
        const string internalBaseUrl = "https://dotnetstage.blob.core.windows.net";
        string customImageBuilderArgs =
            $" --var 'base-url|public|maintenance|main={internalBaseUrl}'" +
            $" --var 'base-url|public|maintenance|nightly={internalBaseUrl}'" +
            $" --var 'base-url|public|preview|main={internalBaseUrl}'" +
            $" --var 'base-url|public|preview|nightly={internalBaseUrl}'" +
            $" --var 'base-url|public-checksums|maintenance|main={internalBaseUrl}'" +
            $" --var 'base-url|public-checksums|maintenance|nightly={internalBaseUrl}'" +
            $" --var 'base-url|public-checksums|preview|main={internalBaseUrl}'" +
            $" --var 'base-url|public-checksums|preview|nightly={internalBaseUrl}'";

        // PowerShell's internal storage uses a different layout than its public storage
        const string internalPowerShellBaseUrl = "https://pscoretestdata.blob.core.windows.net";
        foreach (Match match in Config.GetVariableNames()
            .Select(name => Regex.Match(name, @"^powershell\|(?<dotnetVersion>[\d.]+)\|base-url$"))
            .Where(match => match.Success))
        {
            string dotnetVersion = match.Groups["dotnetVersion"].Value;
            string powershellVersion = Config.GetVariableValue($"powershell|{dotnetVersion}|build-version");
            string internalUrl =
                $"{internalPowerShellBaseUrl}/v{powershellVersion.Replace('.', '-')}-nuget/globaltool";
            customImageBuilderArgs += $" --var '{match.Value}={internalUrl}'";
        }

        // Generate internal Dockerfiles
        ExecuteScript(
            GenerateDockerfilesScript,
            $"-Output {outputDirectory.Path} -CustomImageBuilderArgs \"{customImageBuilderArgs}\"",
            errorMessage);

        string[] dockerfilePaths = DockerfileHelper.GetAllDockerfilesInDirectory(dockerfilesOutputDirectory);

        // Set up configuration for approval tests
        List<(Regex pattern, string replacement)> replacePatterns =
        [
            (DockerfileHelper.Sha512Regex, "{sha512_placeholder}"),
            (DockerfileHelper.Sha386Regex, "{sha386_placeholder}"),
            (DockerfileHelper.Sha256Regex, "{sha256_placeholder}"),
            (DockerfileHelper.VersionRegex, "0.0.0"),
            (DockerfileHelper.VersionWithVariableRegex, "0.0.0"),
            (DockerfileHelper.StringVersionWithVariableRegex, "0.0.0"),
            (DockerfileHelper.MinGitVersionRegex, "v0.0.0.windows.0"),
            (DockerfileHelper.AlpineVersionRegex, "alpine3.XX"),
        ];

        string DockerfileScrubber(string content)
        {
            foreach ((Regex pattern, string replacement) in replacePatterns)
            {
                content = pattern.Replace(content, replacement, count: 99);
            }

            return content;
        }

        string GetDiscriminator(string dockerfilePath) =>
            dockerfilePath
[(dockerfilesOutputDirectory.Length + 1)..]
                .Replace('/', '-')
                .Replace('\\', '-');

        string MatchTestFilenameGenerator(
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

        Action<ShouldMatchConfigurationBuilder> Configure(string dockerfilePath) =>
            config =>
            {
                config
                    // Don't launch diff tool automatically since we have a large number of files
                    .NoDiff()
                    // Use the caller location (this method) for the test method name,
                    // since we perform the comparison inside an extension method
                    .UseCallerLocation()
                    .SubFolder(internalDockerfilesSubfolder)
                    // Discriminator determines the name of the approved files,
                    // which needs to be unique per Dockerfile
                    .WithDiscriminator(GetDiscriminator(dockerfilePath))
                    .WithFilenameGenerator(MatchTestFilenameGenerator)
                    .WithScrubber(DockerfileScrubber);
            };

        // Validate the generated Dockerfiles
        dockerfilePaths.ShouldAllSatisfy(
            path => File.ReadAllText(path).ShouldMatchApproved(configureOptions: Configure(path)));
    }

    private void ValidateGeneratedArtifacts(string generateScriptPath, string errorMessage) => ExecuteScript(generateScriptPath, "-Validate", errorMessage);

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
