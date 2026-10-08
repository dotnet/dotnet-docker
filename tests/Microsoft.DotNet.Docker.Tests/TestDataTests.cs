// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

#nullable enable
namespace Microsoft.DotNet.Docker.Tests;

[Trait("Category", "pre-build")]
public class TestDataTests(ITestOutputHelper outputHelper)
{
    private readonly DockerHelper _dockerHelper = new(outputHelper);

    private readonly ITestOutputHelper _outputHelper = outputHelper;

    private readonly Manifest _manifest = ManifestHelper.GetManifest();

    public static readonly IEnumerable<object[]> ImageRepos =
        Enum.GetValues<DotNetImageRepo>()
            .Select(repo => new object[] { repo })
            .ToArray();

    // Verifies that all images described in the manifest are present in the test data
    [Theory]
    [MemberData(nameof(ImageRepos))]
    public void VerifyTestData(DotNetImageRepo imageRepo)
    {
        var testData = GetTestData(imageRepo);

        string repoName = ImageData.GetRepoName(ProductImageData.GetRepoName(imageRepo));
        Repo? manifestRepo = _manifest.Repos.FirstOrDefault(r => r.Name == repoName);

        if (manifestRepo == null)
        {
            testData.ShouldBeEmpty($"Expected TestData to be empty because repo {repoName} is not in the manifest.");
            return;
        }

        List<string> testDataTags =
            testData
                .Select(productImageData =>
                    productImageData
                        .GetImage(imageRepo, _dockerHelper, skipPull: true)
                        .Split(':')[1])
                .ToList();

        // Account for SDK AOT images. They are tested based on the runtime-deps images.
        if (imageRepo == DotNetImageRepo.SDK)
        {
            testDataTags =
            [
                ..testDataTags,
                ..GetTestData(DotNetImageRepo.Runtime_Deps)
                    .Where(productImageData => productImageData.SdkImageVariant.HasFlag(DotNetImageVariant.AOT))
                    .Select(productImageData =>
                        productImageData
                            .GetImage(imageRepo, _dockerHelper, skipPull: true)
                            .Split(':')[1])
            ];
        }

        IEnumerable<List<string>> manifestTagsByPlatform = ManifestHelper.GetDockerfileTags(manifestRepo).Values;

        Action[] conditions = manifestTagsByPlatform
            .Select<List<string>, Action>(imageTags => () =>
                imageTags.ShouldContain(
                    tag => testDataTags.Contains(tag),
                    "Expected one of the following tags to be represented in test data: " + string.Join(", ", imageTags)))
            .ToArray();

        manifestTagsByPlatform.ShouldSatisfyAllConditions(conditions);
    }

    // Verifies that test data maps to Dockerfile paths in the manifest. Otherwise, path-filtered test runs
    // silently skip the test data. SDK and runtime-deps are excluded because their test data intentionally
    // includes entries without a corresponding Dockerfile (e.g. distroless SDKs, Windows runtime-deps).
    [Theory]
    [InlineData(DotNetImageRepo.Runtime)]
    [InlineData(DotNetImageRepo.Aspnet)]
    [InlineData(DotNetImageRepo.Monitor)]
    [InlineData(DotNetImageRepo.Monitor_Base)]
    [InlineData(DotNetImageRepo.Aspire_Dashboard)]
    [InlineData(DotNetImageRepo.Yarp)]
    public void VerifyTestDataDockerfilePaths(DotNetImageRepo imageRepo)
    {
        string repoName = ImageData.GetRepoName(ProductImageData.GetRepoName(imageRepo));
        Repo? manifestRepo = _manifest.Repos.FirstOrDefault(r => r.Name == repoName);

        // If the repo is not defined in the manifest, there are no images to test, so it's safe to return early.
        // This can happen when an image repo is only published from the main branch or only from the nightly branch.
        if (manifestRepo is null)
        {
            return;
        }

        HashSet<string> manifestDockerfilePaths = manifestRepo.Images
            .SelectMany(image => image.Platforms)
            .Select(platform => platform.Dockerfile)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Action[] conditions = GetTestData(imageRepo)
            .Select(productImageData => productImageData.GetDockerfilePath(imageRepo))
            .Distinct()
            .Select<string, Action>(dockerfilePath => () =>
                manifestDockerfilePaths.ShouldContain(
                    dockerfilePath,
                    $"Expected test data Dockerfile path '{dockerfilePath}' to exist in the manifest."))
            .ToArray();

        manifestDockerfilePaths.ShouldSatisfyAllConditions(conditions);
    }

    private static IEnumerable<ProductImageData> GetTestData(DotNetImageRepo repo) =>
        TestData.AllImageData.Where(p => p.SupportedImageRepos.HasFlag(repo));
}
