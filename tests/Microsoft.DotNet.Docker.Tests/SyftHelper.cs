// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public sealed class SyftHelper(DockerHelper dockerHelper, ITestOutputHelper outputHelper)
{
    private const string OutputFileName = "syft-output.json";

    private static readonly Lazy<string> SyftImageTag = new(() =>
        $"{Config.GetVariableValue("syft|repo")}:{Config.GetVariableValue("syft|tag")}"
    );

    private readonly DockerHelper _dockerHelper = dockerHelper;
    private readonly ITestOutputHelper _outputHelper = outputHelper;

    public JsonNode Scan(
        ProductImageData imageData,
        DotNetImageRepo imageRepo,
        IEnumerable<string>? excludePaths = null
    )
    {
        using TempFolderContext tempDir = FileHelper.UseTempFolder();
        string tempDirPath = tempDir.Path;

        string syftImage = _dockerHelper.PullDockerHubImage(SyftImageTag.Value);
        string imageToInspect = imageData.GetImage(imageRepo, _dockerHelper);

        // Ignore the dotnet folder, or else syft will report all the packages in the .NET Runtime.
        // We only care about the packages from the linux distro for these tests.
        excludePaths ??= [];
        excludePaths = excludePaths.Append("/usr/share/dotnet");

        // Create a Dockerfile tailored to this specific scan (allows us to inline the image name
        // in exec-form RUN)
        string dockerfilePath = Path.Combine(tempDirPath, "Dockerfile");
        string dockerfileContents = CreateDockerfileContents(syftImage, imageToInspect, excludePaths);
        File.WriteAllText(dockerfilePath, dockerfileContents);

        _outputHelper.WriteLine(
            $"""
            Scanning image using generated Dockerfile content:

            {dockerfileContents}

            """
        );

        // Run docker build with --output to write syft.json to the output directory
        _dockerHelper.Build(dockerfile: dockerfilePath, contextDir: tempDirPath, output: tempDirPath);

        string syftOutputPath = Path.Combine(tempDirPath, OutputFileName);
        if (!File.Exists(syftOutputPath))
        {
            throw new FileNotFoundException($"Expected syft output file was not produced: {syftOutputPath}");
        }

        string outputContents = File.ReadAllText(syftOutputPath);
        return JsonNode.Parse(outputContents)
            ?? throw new JsonException(
                $"""
                Unable to parse syft output as JSON:

                {outputContents}

                """
            );
    }

    /// <summary>
    /// Creates the contents of a Dockerfile that scans an image using Syft
    /// </summary>
    /// <param name="syftImage">The Syft image tag to use for scanning</param>
    /// <param name="imageToScan">The image that will be scanned</param>
    /// <param name="excludePaths">
    /// Paths to exclude from the scan, relative to the root of <paramref name="imageToScan"/>
    /// </param>
    /// <returns>
    /// The contents of a Dockerfile that, when built, will scan <paramref name="imageToScan"/> using
    /// Syft and output the results in a scratch image layer. When used in combination with the
    /// docker build --output argument, the results will be written to a file named `syft.json`.
    /// </returns>
    /// <remarks>
    /// The Dockerfile is generated instead of reading a static Dockerfile from the disk because
    /// Dockerfile doesn't allow ARG substitution inside RUN instructions without invoking the
    /// shell, meaning that we would not be able to set the source name or other arguments
    /// programmatically.
    /// </remarks>
    private static string CreateDockerfileContents(
        string syftImage,
        string imageToScan,
        IEnumerable<string> excludePaths
    )
    {
        IEnumerable<string> syftCommand =
        [
            "/syft",
            "scan",
            "/rootfs/",
            "--source-name",
            imageToScan,
            "--select-catalogers",
            "image",
            "--output",
            $"json=/{OutputFileName}",
        ];

        excludePaths ??= [];
        IEnumerable<string> excludeArgs = excludePaths.SelectMany(path =>
        {
            // Syft expects relative paths. The paths are relative to the scan directory, which in
            // this case is /rootfs/.
            if (!path.StartsWith("./"))
            {
                path = "./" + path.TrimStart('/');
            }

            return new[] { "--exclude", path };
        });

        syftCommand = [.. syftCommand, .. excludeArgs];
        string syftCommandString = string.Join(", ", syftCommand.Select(a => $"\"{a}\""));

        // Since syft is running outside its container, it can't access its copy of CA certificates,
        // meaning it can't make a secure HTTPS connection to its update server, so we need to
        // disable its update check. Also, we don't need it checking for updates during tests anyways.
        const string disableSyftUpdateCheck = "ENV SYFT_CHECK_FOR_APP_UPDATE=0";

        return $"""
            FROM {syftImage} AS syft
            FROM {imageToScan} AS scan-image

            FROM syft AS run-scan
            {disableSyftUpdateCheck}
            USER root
            RUN --mount=from=scan-image,source=/,target=/rootfs \
                [{syftCommandString}]

            FROM scratch AS output
            COPY --from=run-scan /{OutputFileName} /{OutputFileName}
            """;
    }
}
