// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Docker.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

using static Microsoft.DotNet.Docker.Tests.ManifestHelper;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "pre-build")]
    public class StaticTagTests
    {
        public enum VersionType
        {
            Major = 1,
            MajorMinor = 2,
            MajorMinorPatch = 3
        }

        private const string LatestTagValue = "latest";
        private const string SingleNumberRegex = @"\d+";
        private const string MajorVersionRegex = SingleNumberRegex;
        private const string MajorMinorVersionRegex = @$"{SingleNumberRegex}\.{SingleNumberRegex}";
        private static readonly string[] ApplianceRepos =
        [
            "monitor",
            "monitor-base",
            "aspire-dashboard",
            "yarp"
        ];

        private enum TestType
        {
            // Tests validating the 'latest' tags.
            // Tests begin with "LatestTag_"
            Latest,

            // Tests validating platform-specific tags
            // Tags have variations of versions, oses, and architectures.
            // Does not include tags with only versions specified (<cref="TestType.Version"/>).
            // Tests begin with "PlatformTag_"
            Platform,

            // Tests validating alpine floating tags
            // Tags have variations of versions and architectures.
            // Tests begin with "FloatingAlpineTag_"
            FloatingAlpine,

            // Tests validating version tags
            // Tags have variations of versions
            // Does not include tags with specified oses or architectures (<cref="TestType.Platform"/>).
            // Tests begin with "VersionTag_"
            Version
        }

        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.Latest)]
        public void LatestTag_OnePerRepo(Repo repo)
        {
            IEnumerable<Image> latestTagImages = repo.Images
                .Where(image => ManifestHelper.GetResolvedSharedTags(image).Contains(LatestTagValue));

            latestTagImages.Should().ContainSingle("expected exactly one image to have the latest tag");
        }

        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.Latest)]
        public void LatestTag_IsNotPlatformSpecific(Repo repo)
        {
            foreach (Image image in repo.Images)
            {
                IEnumerable<Platform> latestTagPlatforms = image.Platforms
                    .Where(platform => ManifestHelper.GetResolvedTags(platform).Contains(LatestTagValue));

                latestTagPlatforms.Should().BeEmpty("expected latest tag to be a shared tag, not a platform-specific tag");
            }
        }

        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.Latest)]
        public void LatestTag_OnCorrectMajorVersion(Repo repo)
        {
            IEnumerable<(Image Image, List<string> SharedTags)> imageDatas = repo.Images
                .Select(image => (Image: image, SharedTags: ManifestHelper.GetResolvedSharedTags(image)));

            (Image Image, List<string> SharedTags) latestImageData = imageDatas
                .Where(imageData => imageData.SharedTags.Contains(LatestTagValue))
                .First();

            int expectedMajorVersion = GetExpectedLatestMajorVersion(repo);

            latestImageData.SharedTags.Should().ContainMatch($"{expectedMajorVersion}.*",
                because: $"latest tag should be on .NET {expectedMajorVersion} in repo {repo.Name}");
        }

        // - <Major.Minor.Patch>-<os>-<architecture>                Non-windows only for non-Appliance repos, old schema only for Appliance repos
        // - <Major.Minor>-<os>-<architecture>                      Non-windows only for non-Appliance repos, old schema only for Appliance repos
        // - <Major>-<os>-<architecture>                            Old schema only for Appliance repos
        // - <Major.Minor.Patch>-<os>                               Non-Appliance repos, old schema only for Appliance repos
        // - <Major.Minor>-<os>                                     Non-Appliance repos, old schema only for Appliance repos
        // - <Major>-<os                                            Old schema only for Appliance repos
        // - <Major.Minor.Patch>-<architecture>                     New schema only for Appliance repos only
        // - <Major.Minor>-<architecture>                           New schema only for Appliance repos only
        // - <Major>-<architecture>                                 New schema and non-monitor for Appliance repos only
        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.Platform)]
        public void PlatformTag_TagExists(
            Repo repo,
            VersionType versionType,
            bool checkOs,
            bool checkArchitecture,
            Func<DockerfileInfo, bool> skipDockerFileOn)
        {
            if (!checkOs && !checkArchitecture)
            {
                throw new ArgumentException("At least one of 'checkOs' or 'checkArchitecture' must be true. " +
                    "Please use the VersionTag tests for this scenario.");
            }

            Dictionary<DockerfileInfo, List<string>> dockerfileTags = ManifestHelper.GetDockerfileTags(repo);
            foreach ((DockerfileInfo dockerfileInfo, List<string> tags) in dockerfileTags)
            {
                if (skipDockerFileOn(dockerfileInfo))
                {
                    continue;
                }

                var dockerfileVersion = dockerfileInfo.ProductVersion;

                bool hasPreviewInMajorVersionGroup = HasPreviewProductVersion(repo, dockerfileVersion.Major);

                // .NET Monitor 8 Azure Linux images do not have Azure Linux platform
                //  (e.g. *-amd64) tags but do have undocumented CBL-Mariner tags. The tag
                // pattern test assumes that a dockerfile will produce platform tags for the
                // underlying OS, which is true for most cases, but not for .NET Monitor 8
                // due to combination of undocumenting platform tags for appliance images
                // (which only .NET Monitor had at the time) and the update from CBL-Mariner
                // to Azure Linux. Rewrite the OS to match CBL-Mariner in this instance.
                string os = dockerfileInfo.OsDir;
                if (repo.Name.EndsWith("monitor") &&
                    dockerfileInfo.VersionDir.StartsWith("8.") &&
                    OS.AzureLinuxDistroless.Equals(os))
                {
                    os = OS.MarinerDistroless;
                }

                Regex expectedPattern = GetTagRegex(
                    versionType,
                    dockerfileInfo.ProductVersion,
                    checkOs ? os : null,
                    checkArchitecture ? dockerfileInfo.ArchitectureDir : null);

                var matchingTags = tags.Where(tag => expectedPattern.IsMatch(tag));

                if (versionType == VersionType.Major && !IsLatestInMajorVersionGroup(repo, dockerfileInfo.ProductVersion, hasPreviewInMajorVersionGroup))
                {
                    // Special case for major version tags
                    // These tags should be on the latest Major.Minor GA version for the respective major version
                    matchingTags.Should().BeEmpty("expected tag to be on latest Major.Minor version for version " +
                        dockerfileVersion.Major + ", but found the tag on " + dockerfileInfo);
                }
                else
                {
                    matchingTags
                        .Should()
                        .ContainSingle($"{dockerfileInfo} requires exactly one tag that matches the expected pattern {expectedPattern}");
                }
            }
        }

        // - <Major.Minor.Patch .NET Version>-alpine-<architecture>
        // - <Major.Minor.Patch .NET Version>-alpine
        // - <Major.Minor .NET Version>-alpine
        // - <Major.Minor .NET Version>-alpine-<architecture>
        // - <Major .NET Version>-alpine-<architecture>
        // - <Major .NET Version>-alpine                            Appliance repos only
        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.FloatingAlpine)]
        public void FloatingAlpineTag_OnLatestVersion(Repo repo, VersionType versionType, bool checkArchitecture)
        {
            IEnumerable<KeyValuePair<DockerfileInfo, List<string>>> alpineDockerfileTags =
                GetDockerfileTags(repo)
                    .Where(p => p.Key.OsDir.Contains(OS.Alpine));

            using (new AssertionScope())
            {
                foreach ((DockerfileInfo dockerfileInfo, List<string> tags) in alpineDockerfileTags)
                {
                    string alpineFloatingTagVersion = GetAlpineFloatingTagVersion(dockerfileInfo);
                    Regex pattern = GetFloatingTagRegex(dockerfileInfo);
                    if (dockerfileInfo.OsDir == alpineFloatingTagVersion)
                    {
                        tags.Should().ContainSingle(tag => pattern.IsMatch(tag),
                            because: $"image {dockerfileInfo} should have an {OS.Alpine} floating tag");
                    }
                    else
                    {
                        tags.Should().NotContain(tag => pattern.IsMatch(tag),
                            because: $"image {dockerfileInfo} should not have an {OS.Alpine} floating tag");
                    }
                }
            }

            string GetAlpineFloatingTagVersion(DockerfileInfo info) =>
                Config.GetVariableValue($"alpine|{info.VersionDir}|floating-tag-version");

            Regex GetFloatingTagRegex(DockerfileInfo info) =>
                GetTagRegex(
                    versionType,
                    productVersion: info.ProductVersion,
                    os: OS.Alpine,
                    architecture: checkArchitecture ? info.ArchitectureDir : null);
        }

        // - <Major.Minor.Patch>
        // - <Major.Minor>
        // - <Major>                                                Appliance repos only
        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.Version)]
        public void VersionTag_SameOsAndVersion(Repo repo, VersionType versionType)
        {
            // Group tags -> dockerfiles
            Dictionary<string, List<DockerfileInfo>> tagsToDockerfiles =
                GetDockerfileTags(repo)
                    .SelectMany(pair =>
                    {
                        var (dockerfileInfo, allTags) = pair;
                        return allTags
                            .Where(tag => IsTagOfFormat(
                                tag: tag,
                                versionType: versionType,
                                productVersion: dockerfileInfo.ProductVersion,
                                os: null,
                                architecture: null))
                            .Select(tag => (tag, pair.Key));
                    })
                    .GroupBy(pair => pair.tag, pair => pair.Key)
                    .ToDictionary(group => group.Key, group => group.ToList());

            foreach (KeyValuePair<string, List<DockerfileInfo>> tagToDockerfiles in tagsToDockerfiles)
            {
                string tag = tagToDockerfiles.Key;
                List<DockerfileInfo> dockerfiles = tagToDockerfiles.Value;

                var dockerfileProductVersions = dockerfiles
                    .Select(dockerfile => dockerfile.ProductVersion)
                    .Distinct()
                    .ToList();

                dockerfileProductVersions.Should().ContainSingle(
                    "all dockerfiles for tag " + tag + " should have the same version");

                if (versionType == VersionType.Major)
                {
                    var dockerfileVersion = dockerfileProductVersions.First();

                    // Special case for major version tags
                    // These tags should be on the most up-to-date Major.Minor version for the respective major version
                    IsLatestInMajorVersionGroup(repo, dockerfileVersion, tag.Contains("-preview")).Should().BeTrue(
                        "expected tag to be on the latest Major.Minor GA version for the major version " +
                        dockerfileVersion.Major + ", but found the tag on " + dockerfileVersion);
                }

                List<string> dockerfileOses = dockerfiles
                    .Select(dockerfile => dockerfile.OsDir)
                    .Distinct()
                    .ToList();

                dockerfileOses.Should().ContainSingle(
                    "all dockerfiles for tag " + tag + " should have the same os");
            }
        }

        private static IEnumerable<object[]> GetTagTestObjects(TestType testType)
        {
            List<object[]> testObjects = new List<object[]>();
            foreach (Repo repo in ManifestHelper.GetManifest().Repos)
            {
                switch (testType)
                {
                    case TestType.Latest:
                        testObjects.Add(GetTagTestInput(testType, repo));
                        break;

                    case TestType.Platform:
                        if (ApplianceRepos.Any(repo.Name.Contains))
                        {
                            // Only appliance repos have major version tags
                            // Only appliance repos have floating distro tags (<cref="IsApplianceVersionUsingOldSchema"/>)
                            testObjects.AddRange(new[]
                            {
                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkOs: true,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsApplianceVersionUsingNewSchema
                                ), // <Major.Minor.Patch>-<os>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkOs: true,
                                    checkArchitecture: false,
                                    skipDockerfileOn: IsApplianceVersionUsingNewSchema
                                ), // <Major.Minor.Patch>-<os>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkOs: false,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsApplianceVersionUsingOldSchema
                                ), // <Major.Minor.Patch>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinor,
                                    checkOs: true,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsApplianceVersionUsingNewSchema
                                ), // <Major.Minor>-<os>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinor,
                                    checkOs: true,
                                    checkArchitecture: false,
                                    skipDockerfileOn: IsApplianceVersionUsingNewSchema
                                ), // <Major.Minor>-<os>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinor,
                                    checkOs: false,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsApplianceVersionUsingOldSchema
                                ), // <Major.Minor>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.Major,
                                    checkOs: true,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsApplianceVersionUsingNewSchema
                                ), // <Major>-<os>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.Major,
                                    checkOs: true,
                                    checkArchitecture: false,
                                    skipDockerfileOn: IsApplianceVersionUsingNewSchema
                                ) // <Major>-<os>
                            });

                            if (!repo.Name.Contains("monitor"))
                            {
                                // Monitor repos don't have these tags
                                testObjects.Add(GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.Major,
                                    checkOs: false,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsApplianceVersionUsingOldSchema
                                )); // <Major>-<architecture>
                            }
                        }
                        else
                        {
                            testObjects.AddRange(new[]
                            {
                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkOs: true,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsWindows
                                ), // <Major.Minor.Patch>-<os>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkOs: true,
                                    checkArchitecture: false
                                ), // <Major.Minor.Patch>-<os>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinor,
                                    checkOs: true,
                                    checkArchitecture: true,
                                    skipDockerfileOn: IsWindows
                                ), // <Major.Minor>-<os>-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinor,
                                    checkOs: true,
                                    checkArchitecture: false
                                ) // <Major.Minor>-<os>
                            });
                        }
                        break;

                    case TestType.FloatingAlpine:
                        if (ApplianceRepos.Any(repo.Name.Contains))
                        {
                            // Only appliance repos have major version alpine floating tags
                            // Only appliance repos have major.minor.patch alpine floating tags
                            testObjects.AddRange(new[]
                            {
                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkArchitecture: true
                                ), // <Major.Minor.Patch>-alpine-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.MajorMinorPatch,
                                    checkArchitecture: false
                                ), // <Major.Minor.Patch>-alpine

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.Major,
                                    checkArchitecture: true
                                ), // <Major>-alpine-<architecture>

                                GetTagTestInput(
                                    testType,
                                    repo,
                                    VersionType.Major,
                                    checkArchitecture: false
                                ) // <Major>-alpine
                            });
                        }
                        testObjects.AddRange(new[]
                        {
                            GetTagTestInput(
                                testType,
                                repo,
                                VersionType.MajorMinor,
                                checkArchitecture: true
                            ), // <Major.Minor>-alpine-<architecture>

                            GetTagTestInput(
                                testType,
                                repo,
                                VersionType.MajorMinor,
                                checkArchitecture: false
                            ) // <Major.Minor>-alpine
                        });
                        break;

                    case TestType.Version:
                        if (ApplianceRepos.Any(repo.Name.Contains))
                        {
                            // Only appliance repos have major version tags
                            testObjects.Add(GetTagTestInput(
                                testType,
                                repo,
                                VersionType.Major
                            )); // <Major>
                        }
                        testObjects.AddRange(new[]
                        {
                            GetTagTestInput(
                                testType,
                                repo,
                                VersionType.MajorMinor
                            ), // <Major.Minor>

                            GetTagTestInput(
                                testType,
                                repo,
                                VersionType.MajorMinorPatch
                            ), // <Major.Minor.Patch>
                        });
                        break;

                    default:
                        throw new ArgumentException("Invalid tag type", nameof(testType));
                }
            }
            return testObjects.ToArray();
        }

        private static object[] GetTagTestInput(
            TestType testType,
            Repo repo,
            VersionType? versionType = null,
            bool? checkOs = false,
            bool? checkArchitecture = false,
            Func<DockerfileInfo, bool>? skipDockerfileOn = null)
        {
            switch (testType)
            {
                case TestType.Latest:
                    return [ repo ];

                case TestType.Platform:
                    if (versionType == null || checkOs == null || checkArchitecture == null)
                    {
                        throw new ArgumentException($"'{nameof(versionType)}', '{nameof(checkOs)}' and '{nameof(checkArchitecture)}' must be specified", nameof(testType));
                    }
                    return [ repo, versionType, checkOs, checkArchitecture, skipDockerfileOn ?? (_ => false) ];

                case TestType.FloatingAlpine:
                    if (versionType == null || checkArchitecture == null)
                    {
                        throw new ArgumentException($"'{nameof(versionType)}' and '{nameof(checkArchitecture)}' must be specified", nameof(testType));
                    }
                    return [ repo, versionType, checkArchitecture ];

                case TestType.Version:
                    if (versionType == null)
                    {
                        throw new ArgumentException($"'{nameof(versionType)}' must be specified", nameof(testType));
                    }
                    return [ repo, versionType ];

                default:
                    throw new ArgumentException("Invalid tag type", nameof(testType));
            }
        }

        private static bool IsWindows(DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.OsDir.Contains("windowsservercore") || dockerfileInfo.OsDir.Contains("nanoserver");

        // Certain versions of appliance repos use a new tag schema.
        // This new schema excludes the OS from all tags.
        // The aspire-dashboard repo uses this schema for all versions.
        // The monitor and monitor-base repos use this schema for versions 9 and above.
        private static bool IsApplianceVersionUsingOldSchema(DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.RepoDir.Contains("monitor") && dockerfileInfo.ProductVersion.Major <= 8;

        // <cref="IsApplianceVersionUsingOldSchema"/>
        private static bool IsApplianceVersionUsingNewSchema(DockerfileInfo dockerfileInfo) =>
            !IsApplianceVersionUsingOldSchema(dockerfileInfo);

        /// <summary>
        /// Determines if the <paramref name="productVersion"/> is the latest in its major version group.
        /// </summary>
        /// <remarks>
        /// By default, this will only consider GA versions within the major version group (unless there are no GA versions).
        /// The <paramref name="includePreviewVersions"/> parameter can be set to include preview versions in the check.
        /// </remarks>
        private static bool IsLatestInMajorVersionGroup(Repo repo, DotNetVersion productVersion, bool includePreviewVersions)
        {
            IEnumerable<DotNetVersion> productVersions = GetResolvedProductVersions(repo).Select(DotNetVersion.Parse);
            Assert.NotEmpty(productVersions);

            List<DotNetVersion> majorMinorVersions = productVersions
                .GroupBy(version => version.Major)
                .Select(group =>
                {
                    if (includePreviewVersions)
                    {
                        return group;
                    }
                    else
                    {
                        // Use the latest GA major version.
                        // If there are no GA versions, use the latest preview version.
                        var gaVersions = group.Where(version => version.IsGA);
                        return gaVersions.Any() ? gaVersions : group;
                    }
                })
                .Select(group => group
                    .OrderByDescending(version => version)
                    .First())
                .ToList();

            return majorMinorVersions.Contains(productVersion);
        }

        /// <summary>
        /// Determines if a major product version has a preview version.
        /// </summary>
        private static bool HasPreviewProductVersion(Repo repo, int majorVersion)
        {
            var productVersions = GetResolvedProductVersions(repo).Select(DotNetVersion.Parse);

            IGrouping<int, DotNetVersion>? matchingMajorVersionGroup = productVersions
                .GroupBy(version => version.Major)
                .SingleOrDefault(group => group.Key == majorVersion);

            if (matchingMajorVersionGroup is null)
            {
                return false;
            }

            return matchingMajorVersionGroup.Any(version => !version.IsGA);
        }

        private static bool IsTagOfFormat(
            string tag,
            VersionType versionType,
            DotNetVersion? productVersion,
            string? os = null,
            string? architecture = null)
        {
            Regex pattern = GetTagRegex(versionType, productVersion, os, architecture);
            return pattern.IsMatch(tag);
        }

        private static Regex GetTagRegex(VersionType versionType, DotNetVersion? productVersion, string? os, string? architecture)
        {
            string tagRegex = versionType switch
            {
                VersionType.Major =>
                    productVersion == null ? MajorVersionRegex : @$"{productVersion.Major}(-preview)?",
                VersionType.MajorMinor =>
                    @$"{productVersion?.ToString(2) ?? MajorMinorVersionRegex}(-preview)?",
                VersionType.MajorMinorPatch =>
                    @$"{productVersion?.ToString(3) ?? @"\d+\.\d+\.\d+"}(?:-(alpha|beta|preview|rc)\.{SingleNumberRegex})?",
                _ => throw new ArgumentException("Invalid version type", nameof(versionType)),
            };

            return new Regex($"^{tagRegex}" + (os != null ? $"-{os}" : string.Empty) + (architecture != null ? $"-{architecture}" : string.Empty) + "$");
        }

        private static Version GetAlpineVersion(DockerfileInfo dockerfileInfo)
        {
            string parsedOs = dockerfileInfo.OsDir.Replace(OS.Alpine, string.Empty);
            parsedOs.Should().NotBeNullOrWhiteSpace(
                because: $"{dockerfileInfo} should have a specific Alpine version for osVersion");
            return Version.Parse(parsedOs);
        }

        private static int GetExpectedLatestMajorVersion(Repo repo)
        {
            var productVersions = GetResolvedProductVersions(repo).Select(DotNetVersion.Parse);

            Assert.NotEmpty(productVersions);

            if (productVersions.Count() == 1)
            {
                // Use the first product version if there is only one
                return productVersions.First().Major;
            }

            // In non-nightly branches, preview versions should not have the latest tag
            if (!Config.IsNightlyRepo)
            {
                productVersions = productVersions
                    .Where(version => !version.Release.Contains("preview") && !version.Release.Contains("rc"));
            }

            return productVersions.Select(version => version.Major).Max();
        }
    }
}
