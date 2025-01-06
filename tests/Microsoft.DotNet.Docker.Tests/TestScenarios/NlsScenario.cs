// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;

namespace Microsoft.DotNet.Docker.Tests.TestScenarios;

#nullable enable
public sealed class NlsScenario : ITestScenario, IDisposable
{
    private const string DockerfileName = "NLSTest.Dockerfile";

    private const string TestSourceFileName = "NLSTest.cs";

    private readonly TempFolderContext _tempFolderContext = FileHelper.UseTempFolder();

    private readonly ProductImageData _imageData;

    private readonly DotNetImageRepo _repo;

    private readonly DockerHelper _dockerHelper;

    public NlsScenario(
        ProductImageData imageData,
        DotNetImageRepo repo,
        DockerHelper dockerHelper)
    {
        _imageData = imageData;
        _repo = repo;
        _dockerHelper = dockerHelper;
    }

    // ICU is not supported on Nano Server
    private bool IsIcuSupported => _imageData.OS.Contains(OS.ServerCore);

    public Task ExecuteAsync()
    {
        // Setup project in temp dir
        string dockerfilePath = Path.Combine(_tempFolderContext.Path, "Dockerfile");
        File.Copy(
            sourceFileName: Path.Combine(DockerHelper.TestArtifactsDir, DockerfileName),
            destFileName: dockerfilePath);
        File.Copy(
            sourceFileName: Path.Combine(DockerHelper.TestArtifactsDir, TestSourceFileName),
            destFileName: Path.Combine(_tempFolderContext.Path, TestSourceFileName));

        string sdkImage = _imageData.GetImage(DotNetImageRepo.SDK, _dockerHelper);
        string runtimeImage = _imageData.GetImage(_repo, _dockerHelper);
        string[] buildArgs =
        [
            $"sdk_image={sdkImage}",
            $"runtime_image={runtimeImage}",
        ];

        if (IsIcuSupported)
        {
            buildArgs = [..buildArgs, $"icu_expected={IsIcuSupported}"];
        }

        string tag = nameof(NlsScenario).ToLowerInvariant();
        _dockerHelper.Build(
            tag: tag,
            dockerfile: dockerfilePath,
            contextDir: _tempFolderContext.Path,
            pull: Config.PullImages,
            buildArgs: buildArgs);

        string containerName = ImageData.GenerateContainerName(nameof(NlsScenario));
        Func<string> runImage = () => _dockerHelper.Run(tag, containerName);

        string justification = $"image {runtimeImage} should{(IsIcuSupported ? "" : " not")} support ICU";
        runImage.Should().NotThrow(because: justification)
            .Which.Should().ContainEquivalentOf("All assertions passed", Exactly.Once());

        return Task.CompletedTask;
    }

    public void Dispose() => _tempFolderContext.Dispose();
}
