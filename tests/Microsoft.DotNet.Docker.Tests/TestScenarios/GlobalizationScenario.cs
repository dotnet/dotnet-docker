// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;

namespace Microsoft.DotNet.Docker.Tests.TestScenarios;

#nullable enable
public sealed class GlobalizationScenario : ITestScenario, IDisposable
{
    private const string Dockerfile = """
        ARG sdk_image
        ARG runtime_image

        FROM ${sdk_image} AS sdk
        RUN dotnet new console -n App -o /src --no-restore
        WORKDIR /src
        COPY Program.cs /src/Program.cs
        RUN dotnet restore
        RUN dotnet publish --no-restore -o /app

        FROM ${runtime_image} AS runtime
        COPY --from=sdk /app /app/
        ENTRYPOINT ["/app/App"]
        """;

    private readonly TempFolderContext _tempFolderContext = FileHelper.UseTempFolder();
    private readonly ProductImageData _imageData;
    private readonly DotNetImageRepo _repo;
    private readonly DockerHelper _dockerHelper;

    public GlobalizationScenario(
        ProductImageData imageData,
        DotNetImageRepo repo,
        DockerHelper dockerHelper)
    {
        _imageData = imageData;
        _repo = repo;
        _dockerHelper = dockerHelper;
    }

    public async Task ExecuteAsync()
    {
        // Setup project in temp dir
        string dockerfilePath = Path.Combine(_tempFolderContext.Path, "Dockerfile");
        await File.WriteAllTextAsync(path: dockerfilePath, contents: Dockerfile);

        File.Copy(
            sourceFileName: Path.Combine(DockerHelper.TestArtifactsDir, "GlobalizationTest.cs"),
            destFileName: Path.Combine(_tempFolderContext.Path, "Program.cs"));

        string tag = nameof(GlobalizationScenario).ToLowerInvariant();
        _dockerHelper.Build(
            tag: tag,
            dockerfile: dockerfilePath,
            contextDir: _tempFolderContext.Path,
            pull: Config.PullImages,
            buildArgs:
            [
                $"sdk_image={_imageData.GetImage(DotNetImageRepo.SDK, _dockerHelper)}",
                $"runtime_image={_imageData.GetImage(_repo, _dockerHelper)}",
            ]);

        string containerName = ImageData.GenerateContainerName(nameof(GlobalizationScenario));
        string output = _dockerHelper.Run(tag, containerName);

        output.Should().NotBeNullOrWhiteSpace();
        output.Should().ContainEquivalentOf("Globalization test succeeded", Exactly.Once());
    }

    public void Dispose() => _tempFolderContext.Dispose();
}
