// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.DotNet.Docker.Tests.TestScenarios;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "runtime-deps")]
public class RuntimeDepsImageTests(ITestOutputHelper outputHelper) : CommonRuntimeImageTests(outputHelper)
{
    protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Runtime_Deps;

    public static IEnumerable<object[]> GetImageData() =>
        GetImageData(DotNetImageRepo.Runtime_Deps);

    public static IEnumerable<object[]> GetAotImageData() =>
        GetImageData(DotNetImageRepo.Runtime_Deps, DotNetImageVariant.AOT);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifySelfContainedConsoleScenario(ProductImageData imageData)
    {
        if (imageData.ImageVariant.HasFlag(DotNetImageVariant.AOT))
        {
            OutputHelper.WriteLine(
                $"Test is not applicable to AOT images. See {nameof(VerifyAotWebScenario)} instead.");
            return;
        }

        ConsoleAppScenario testScenario =
            new ConsoleAppScenario.SelfContained(imageData, DockerHelper, OutputHelper);
        await testScenario.ExecuteAsync();
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public async Task VerifySelfContainedWebScenario(ProductImageData imageData)
    {
        if (imageData.ImageVariant.HasFlag(DotNetImageVariant.AOT))
        {
            OutputHelper.WriteLine(
                $"Test is not applicable to AOT images. See {nameof(VerifyAotWebScenario)} instead.");
            return;
        }

        WebScenario testScenario = new WebScenario.SelfContained(imageData, DockerHelper, OutputHelper);
        await testScenario.ExecuteAsync();
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetAotImageData))]
    public async Task VerifyAotWebScenario(ProductImageData imageData)
    {
        if (imageData.Arch == Arch.Arm)
        {
            OutputHelper.WriteLine("Skipping test due to https://github.com/dotnet/docker-tools/issues/1177. "
                    + "ImageBuilder is unable to queue arm32 AOT images together with the arm64 AOT SDKs. "
                    + "Re-enable once fixed.");
            return;
        }

        WebScenario scenario = new WebScenario.Aot(imageData, DockerHelper, OutputHelper);
        await scenario.ExecuteAsync();
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyEnvironmentVariables(ProductImageData imageData) => VerifyCommonEnvironmentVariables(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInsecureFiles(ProductImageData imageData) => VerifyCommonInsecureFiles(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyShellNotInstalledForDistroless(ProductImageData imageData) => VerifyCommonShellNotInstalledForDistroless(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyNoSasToken(ProductImageData imageData) => VerifyCommonNoSasToken(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyDefaultUser(ProductImageData imageData) => VerifyCommonDefaultUser(imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyDistrolessOSReleaseInfo(ProductImageData imageData)
    {
        if (!imageData.IsDistroless)
        {
            OutputHelper.WriteLine("This test is only relevant to distroless images.");
            return;
        }

        string osReleaseInfo = GetOSReleaseInfo(imageData, ImageRepo);
        OutputHelper.WriteLine($"OS Release Info: {osReleaseInfo}");
        osReleaseInfo.ShouldNotBeEmpty();
    }

    /// <summary>
    /// Verifies the presence of the Chisel manifest in Ubuntu Chiseled images. Chisel manifest documentation:
    /// https://discourse.ubuntu.com/t/chisel-manifest-is-supported-in-newly-released-v1-0-0/48944
    /// </summary>
    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyChiselManifest(ProductImageData imageData)
    {
        if (!(imageData.OS.Family == OSFamily.Ubuntu && imageData.IsDistroless))
        {
            OutputHelper.WriteLine("Test is only relevant to Ubuntu Chiseled images");
            return;
        }

        // https://github.com/dotnet/dotnet-docker/issues/5973#issuecomment-2501510550
        bool shouldContainManifest = imageData.Version.Major is not 8 and not 9;

        const string rootFs = "/rootfs";
        const string chiselManifestFileName = "/var/lib/chisel/manifest.wall";

        // Setup a distroless helper image to inspect the filesystem of the Chiseled image
        string distrolessHelperImageTag = DockerHelper.BuildDistrolessHelper(ImageRepo, imageData, rootFs);

        // Check for the presence of the Chisel manifest by listing the files in the directory
        // and then verifying the output.
        string actualOutput = DockerHelper.Run(
            image: distrolessHelperImageTag,
            name: imageData.GetIdentifier(nameof(VerifyChiselManifest)),
            command: $"find {rootFs}/var/lib/ -path *chisel* -type f");

        if (shouldContainManifest)
        {
            Assert.Contains(chiselManifestFileName, actualOutput);
        }
        else
        {
            Assert.DoesNotContain(chiselManifestFileName, actualOutput);
        }
    }

    private string GetOSReleaseInfo(
        ProductImageData imageData,
        DotNetImageRepo imageRepo)
    {
        JsonNode output = SyftHelper.Scan(imageData, imageRepo);
        JsonObject distro = (JsonObject)output["distro"];
        return (string)distro["version"];
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInstalledPackages(ProductImageData imageData) => VerifyInstalledPackagesBase(imageData, ImageRepo);
}
