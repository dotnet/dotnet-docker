// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    private static readonly Lazy<string> s_syftImageTag = new(() =>
        $"{Config.GetVariableValue("syft|repo")}:{Config.GetVariableValue("syft|tag")}"
    );

    private readonly DockerHelper _dockerHelper = dockerHelper;
    private readonly ITestOutputHelper _outputHelper = outputHelper;

    public JsonNode Scan(
        ProductImageData imageData,
        DotNetImageRepo imageRepo,
        IEnumerable<string>? extraExcludePaths = null
    )
    {
        using var tempDir = FileHelper.UseTempFolder();
        var tempDirPath = tempDir.Path;

        var syftImage = _dockerHelper.PullDockerHubImage(s_syftImageTag.Value);
        var imageToInspect = imageData.GetImage(imageRepo, _dockerHelper);

        // Ignore the dotnet folder, or else syft will report all the packages in the .NET Runtime.
        // We only care about the packages from the linux distro for these tests.
        extraExcludePaths ??= [];
        extraExcludePaths = extraExcludePaths.Append("./usr/share/dotnet");

        // Create a Dockerfile tailored to this specific scan (allows us to inline the image name
        // in exec-form RUN)
        var dockerfilePath = Path.Combine(tempDirPath, "Dockerfile");
        var dockerfileContents = CreateDockerfileContents(syftImage, imageToInspect, extraExcludePaths);
        File.WriteAllText(dockerfilePath, dockerfileContents);

        _outputHelper.WriteLine(
            $"""
            Scanning image using generated Dockerfile content:

            {dockerfileContents}

            """
        );

        // Run docker build with --output to write syft.json to the output directory
        _dockerHelper.Build(dockerfile: dockerfilePath, contextDir: tempDirPath, output: tempDirPath);

        string syftOutputPath = Path.Combine(tempDirPath, "syft.json");
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
    /// <returns>
    /// The contents of a Dockerfile that, when built, will scan <see cref="imageToScan"/> using
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
        const string OutputFileName = "syft.json";

        IEnumerable<string> syftCommand =
        [
            "/syft/syft",
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
        IEnumerable<string> excludeArgs = excludePaths.SelectMany(path => new[] { "--exclude", path });

        syftCommand = [.. syftCommand, .. excludeArgs];
        var syftCommandString = string.Join(", ", syftCommand.Select(a => $"\"{a}\""));

        // Since syft is running outside its container, it can't access its copy of CA certificates,
        // meaning it can't make a secure HTTPS connection to its update server, so we need to
        // disable its update check. Also, we don't need it checking for updates during tests anyways.
        const string DisableSyftUpdateCheck = "ENV SYFT_CHECK_FOR_APP_UPDATE=0";

        return $"""
            FROM {syftImage} AS syft
            FROM {imageToScan} AS scan-image

            FROM syft AS run-scan
            {DisableSyftUpdateCheck}
            USER root
            RUN --mount=from=scan-image,source=/,target=/rootfs \
                [{syftCommandString}]

            FROM scratch AS output
            COPY --from=run-scan /{OutputFileName} /{OutputFileName}
            """;
    }
}
