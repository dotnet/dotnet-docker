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

    private static IEnumerable<ProductImageData> GetTestData(DotNetImageRepo repo) =>
        TestData.AllImageData.Where(p => p.SupportedImageRepos.HasFlag(repo));
}
