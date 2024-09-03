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
        private void LatestTag_OnePerRepo(ManifestHelper.Repo repo)
        {
            var latestTagImages = repo.Images
                .Where(image => image.GetSharedTags().Contains(LatestTagValue))
                .ToList();

            Assert.Single(latestTagImages);
        }

        [Theory]
        [MemberData(nameof(GetRepoObjects))]
        private void LatestTag_IsNotPlatformSpecific(ManifestHelper.Repo repo)
        {
            foreach (var image in repo.Images)
            {
                var latestTagPlatforms = image.Platforms
                    .Where(platform => platform.GetTags().Contains(LatestTagValue))
                    .ToList();

                Assert.Empty(latestTagPlatforms);
            }
        }

        [Theory]
        [MemberData(nameof(GetRepoObjects))]
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
            List<string> productVersions = repo.Images.Select(image => image.ProductVersion).Distinct().ToList();

            Assert.NotEmpty(productVersions);

            if (productVersions.Count == 1)
            {
                return productVersions[0].Split('.')[0];
            }
            else if (Config.IsNightlyRepo)
            {
                List<string> majorVersions = productVersions
                    .Select(version => version.Split('.')[0])
                    .Distinct()
                    .ToList();
                return majorVersions.Max()!;
            }
            else
            {
                List<string> gaVersions = productVersions
                    .Where(version => !version.Contains("-"))
                    .Select(version => version.Split('.')[0])
                    .Distinct()
                    .ToList();
                return gaVersions.Max()!;
            }
        }
    }
}
