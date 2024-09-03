// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "pre-build")]
    public class StaticTagTests
    {
        private const string LatestTagValue = "latest";
        private ITestOutputHelper OutputHelper { get; }

        public StaticTagTests(ITestOutputHelper outputHelper) => OutputHelper = outputHelper;

        public static IEnumerable<object[]> GetRepoObjects() => ManifestHelper.GetManifest().Repos.Select(repo => new object[] { repo }).ToArray();

        [Theory]
        [MemberData(nameof(GetRepoObjects))]
        public void ValidateLatestTag(ManifestHelper.Repo repo)
        {
            LatestTag_OnePerRepo(repo);
            LatestTag_IsNotPlatformSpecific(repo);
            LatestTag_OnCorrectMajorVersion(repo);
        }

        private void LatestTag_OnePerRepo(ManifestHelper.Repo repo)
        {
            var latestTagImages = repo.Images
                .Where(image => image.GetSharedTags().Contains(LatestTagValue))
                .ToList();

            Assert.True(
                latestTagImages.Count == 1,
                $"Expected a single image with the shared latest tag, but found {latestTagImages.Count}.");
        }

        private void LatestTag_IsNotPlatformSpecific(ManifestHelper.Repo repo)
        {
            foreach (var image in repo.Images)
            {
                var latestTagPlatforms = image.Platforms
                    .Where(platform => platform.GetTags().Contains(LatestTagValue))
                    .ToList();

                Assert.True(
                    latestTagPlatforms.Count == 0,
                    $"Expected no platforms with a specific latest tag, but found {latestTagPlatforms.Count}: \n" +
                    $"  {string.Join("\n", latestTagPlatforms.Select(platform => platform.Dockerfile))}");
            }
        }

        private void LatestTag_OnCorrectMajorVersion(ManifestHelper.Repo repo)
        {
            var latestImage = repo.Images
                .Where(image => image.GetSharedTags().Contains(LatestTagValue))
                .First();

            var expectedMajorVersion = GetExpectedMajorVersion(repo);
            var actualMajorVersion = latestImage.ProductVersion.Split('.')[0];

            Assert.Equal(expectedMajorVersion, actualMajorVersion);
        }

        private string GetExpectedMajorVersion(ManifestHelper.Repo repo)
        {
            List<string> productVersions = repo.Images.Select(image => image.ProductVersion.Split('.')[0]).Distinct().ToList();

            Assert.True(productVersions.Count > 0, "No product versions found for repo.");

            if (productVersions.Count == 1)
            {
                return productVersions[0];
            }
            else if (Config.IsNightlyRepo)
            {
                return productVersions.Max()!;
            }
            else
            {
                return productVersions.OrderByDescending(version => version).ElementAt(1);
            }
        }
    }
}
