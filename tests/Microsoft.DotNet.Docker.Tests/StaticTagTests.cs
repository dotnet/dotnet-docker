// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "pre-build")]
    public class StaticTagTests
    {
        private const string LatestTagValue = "latest";

        public static IEnumerable<object[]> GetRepoObjects() =>
            ManifestHelper.GetManifest().Repos.Select(repo => new object[] { repo }).ToArray();

        [Theory]
        [MemberData(nameof(GetRepoObjects))]
        private void LatestTag_OnePerRepo(Repo repo)
        {
            IEnumerable<Image> latestTagImages = repo.Images
                .Where(image => ManifestHelper.GetResolvedSharedTags(image).Contains(LatestTagValue));

            Assert.Single(latestTagImages);
        }

        [Theory]
        [MemberData(nameof(GetRepoObjects))]
        private void LatestTag_IsNotPlatformSpecific(Repo repo)
        {
            foreach (Image image in repo.Images)
            {
                IEnumerable<Platform> latestTagPlatforms = image.Platforms
                    .Where(platform => ManifestHelper.GetResolvedTags(platform).Contains(LatestTagValue));

                Assert.Empty(latestTagPlatforms);
            }
        }

        [Theory]
        [MemberData(nameof(GetRepoObjects))]
        private void LatestTag_OnCorrectMajorVersion(Repo repo)
        {
            Image latestImage = repo.Images
                .Where(image => ManifestHelper.GetResolvedSharedTags(image).Contains(LatestTagValue))
                .First();

            int expectedMajorVersion = GetExpectedMajorVersion(repo);
            int actualMajorVersion = GetMajorVersion(ManifestHelper.GetResolvedProductVersion(latestImage));

            Assert.Equal(expectedMajorVersion, actualMajorVersion);
        }

        private static int GetExpectedMajorVersion(Repo repo)
        {
            List<string> productVersions = repo.Images
                .Select(ManifestHelper.GetResolvedProductVersion)
                .Distinct()
                .ToList();

            Assert.NotEmpty(productVersions);

            if (productVersions.Count == 1)
            {
                // Use the first product version if there is only one
                return GetMajorVersion(productVersions[0]);
            }
            
            if (Config.IsNightlyRepo)
            {
                // Use the latest major version on the nightly branch
                List<int> majorVersions = productVersions
                    .Select(GetMajorVersion)
                    .Distinct()
                    .ToList();

                return majorVersions.Max();
            }

            // Use the latest GA major version on the main branch
            // Assumes that non-GA versions have a hyphen in them
            // e.g. non GA: 5.0.0-preview.1, GA: 5.0.0
            List<int> gaVersions = productVersions
                .Where(version => !version.Contains('-'))
                .Select(GetMajorVersion)
                .Distinct()
                .ToList();

            return gaVersions.Max();
        }

        private static int GetMajorVersion(string version) =>
            int.Parse(version.Split('.')[0]);
    }
}
