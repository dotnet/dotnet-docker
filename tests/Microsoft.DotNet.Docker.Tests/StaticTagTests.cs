// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using FluentAssertions;
using Xunit;

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

        private const string AlpineOs = "alpine";
        private const string LatestTagValue = "latest";
        private const string SingleNumberRegex = @"\d+";
        private const string MajorVersionRegex = SingleNumberRegex;
        private const string MajorMinorVersionRegex = @$"{SingleNumberRegex}\.{SingleNumberRegex}";
        private static readonly string[] ApplianceRepos = { "monitor", "monitor-base", "aspire-dashboard" };

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
            Func<ManifestHelper.DockerfileInfo, bool> skipDockerFileOn)
        {

            if (!checkOs && !checkArchitecture)
            {
                throw new ArgumentException("At least one of 'checkOs' or 'checkArchitecture' must be true. " +
                    "Please use the VersionTag tests for this scenario.");
            }

            Dictionary<ManifestHelper.DockerfileInfo, List<string>> dockerfileTags = ManifestHelper.GetDockerfileTags(repo);
            foreach (KeyValuePair<ManifestHelper.DockerfileInfo, List<string>> dockerfileTag in dockerfileTags)
            {
                ManifestHelper.DockerfileInfo dockerfileInfo = dockerfileTag.Key;
                if (skipDockerFileOn(dockerfileInfo))
                {
                    continue;
                }

                IEnumerable<string> tags = dockerfileTag.Value
                    .Where(tag => IsTagOfFormat(
                        tag,
                        versionType,
                        dockerfileInfo.MajorMinor,
                        checkOs ? dockerfileInfo.Os : null,
                        checkArchitecture ? dockerfileInfo.Architecture : null));

                if (versionType == VersionType.Major && !IsExpectedMajorMinorVersion(repo, dockerfileInfo.MajorMinor))
                {
                    // Special case for major version tags
                    // These tags should be on the most up-to-date Major.Minor version for the respective major version
                    tags.Should().BeEmpty("expected tag to be on latest Major.Minor version for version " +
                        GetVersion(dockerfileInfo.MajorMinor).Major + ", but found the tag on " + dockerfileInfo);
                }
                else
                {
                    tags.Should().ContainSingle(dockerfileInfo + " requires exactly one tag that matches the expected format");
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
            Dictionary<ManifestHelper.DockerfileInfo, List<string>> dockerfileTags = ManifestHelper.GetDockerfileTags(repo);

            IEnumerable<ManifestHelper.DockerfileInfo> alpineDockerfiles = dockerfileTags.Keys
                .Where(dockerfileInfo => dockerfileInfo.Os.Contains(AlpineOs));

            if (!alpineDockerfiles.Any())
            {
                // The repo doesn't have any alpine dockerfiles
                return;
            }

            // It's possible that we don't specify the alpine version if
            // there's only a single alpine dockerfile in the repo.
            // In this is the case, a null string for the version will be used
            Version? latestAlpineVersion = alpineDockerfiles
                .Select(GetAlpineVersion)
                .Where(version => version != null)
                .OrderByDescending(version => version)
                .FirstOrDefault();

            foreach (ManifestHelper.DockerfileInfo dockerfileInfo in alpineDockerfiles)
            {
                IEnumerable<string> alpineFloatingTags = dockerfileTags[dockerfileInfo]
                    .Where(tag => IsTagOfFormat(
                        tag,
                        versionType,
                        majorMinor: dockerfileInfo.MajorMinor,
                        os: AlpineOs,
                        architecture: checkArchitecture ? dockerfileInfo.Architecture : null));

                if(!alpineFloatingTags.Any())
                {
                    dockerfileInfo.Os.Should().NotBeEquivalentTo(AlpineOs + latestAlpineVersion,
                        $"{dockerfileInfo} does not have an alpine floating tag. " +
                        "This dockerfile should have a floating tag because it uses the latest alpine version");
                }
                else
                {
                    alpineFloatingTags.Should().ContainSingle("expected exactly one alpine floating tag for " + dockerfileInfo);
                    dockerfileInfo.Os.Should().BeEquivalentTo(AlpineOs + latestAlpineVersion, $"{dockerfileInfo} has an alpine floating tag." +
                        "This dockerfile not have a floating tag because it doesn't use the latest alpine version");
                }
            }
        }

        // - <Major.Minor.Patch>
        // - <Major.Minor>
        // - <Major>                                                Appliance repos only
        [Theory]
        [MemberData(nameof(GetTagTestObjects), TestType.Version)]
        public void VersionTag_SameOsAndVersion(Repo repo, VersionType versionType)
        {
            // Group tags -> dockerfiles
            // Skip .NET 6 dockerfiles because they include linux and windows OSes for the tags
            Dictionary<string, List<ManifestHelper.DockerfileInfo>> tagsToDockerfiles = ManifestHelper.GetDockerfileTags(repo)
                .Where(pair => !IsDotNet6(pair.Key))
                .SelectMany(pair => pair.Value
                    .Where(tag => IsTagOfFormat(
                        tag,
                        versionType,
                        majorMinor: pair.Key.MajorMinor,
                        os: null,
                        architecture: null))
                    .Select(tag => (tag, pair.Key)))
                .GroupBy(pair => pair.tag, pair => pair.Key)
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach (KeyValuePair<string, List<ManifestHelper.DockerfileInfo>> tagToDockerfiles in tagsToDockerfiles)
            {
                string tag = tagToDockerfiles.Key;
                List<ManifestHelper.DockerfileInfo> dockerfiles = tagToDockerfiles.Value;

                List<string> dockerfileVersions = dockerfiles
                    .Select(dockerfile => dockerfile.MajorMinor)
                    .Distinct()
                    .ToList();

                dockerfileVersions.Should().ContainSingle(
                    "all dockerfiles for tag " + tag + " should have the same Major.Minor version");

                if (versionType == VersionType.Major)
                {
                    string dockerfileVersion = dockerfileVersions.First();

                    // Special case for major version tags
                    // These tags should be on the most up-to-date Major.Minor version for the respective major version
                    IsExpectedMajorMinorVersion(repo, dockerfileVersion).Should().BeTrue(
                        "expected tag to be on the latest Major.Minor version for the major version " +
                        GetVersion(dockerfileVersion).Major + ", but found the tag on " + dockerfileVersion);
                }

                List<string> dockerfileOses = dockerfiles
                    .Select(dockerfile => dockerfile.Os)
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
            Func<ManifestHelper.DockerfileInfo, bool>? skipDockerfileOn = null)
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

        private static bool IsWindows(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.Os.Contains("windowsservercore") || dockerfileInfo.Os.Contains("nanoserver");

        // Certain versions of appliance repos use a new tag schema.
        // This new schema excludes the OS from all tags.
        // The aspire-dashboard repo uses this schema for all versions.
        // The monitor and monitor-base repos use this schema for versions 9 and above.
        private static bool IsApplianceVersionUsingOldSchema(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.Repo.Contains("monitor") && GetVersion(dockerfileInfo.MajorMinor).Major <= 8;

        // <cref="IsApplianceVersionUsingOldSchema"/>
        private static bool IsApplianceVersionUsingNewSchema(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            !IsApplianceVersionUsingOldSchema(dockerfileInfo);

        private static bool IsDotNet6(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.MajorMinor.StartsWith("6");

        private static bool IsExpectedMajorMinorVersion(Repo repo, string version)
        {
            IEnumerable<string> productVersions = ManifestHelper.GetResolvedProductVersions(repo);

            Assert.NotEmpty(productVersions);

            List<Version> majorMinorVersions = productVersions
                .GroupBy(version => GetVersion(version).Major)
                .Select(group =>
                {
                    if (!Config.IsNightlyRepo)
                    {
                        // Use the latest GA major version on the main branch
                        // Assumes that non-GA versions have a hyphen in them
                        // e.g. non GA: 5.0.0-preview.1, GA: 5.0.0
                        // If there are no GA versions, use the latest preview version
                        var gaVersions = group.Where(version => !version.Contains('-'));
                        return gaVersions.Any() ? gaVersions : group;
                    }
                    // Use the latest major version on the nightly branch
                    return group;
                })
                .Select(group => group.Select(version =>
                {
                    // Product versions are in the form of <Major.Minor.Patch>
                    // We only care about Major.Minor so we parse out the Patch
                    Version parsedVersion = GetVersion(version);
                    return new Version(parsedVersion.Major, parsedVersion.Minor);
                }).OrderByDescending(version => version).First())
                .ToList();

            Version inputVersion = GetVersion(version);
            return majorMinorVersions.Contains(new Version(inputVersion.Major, inputVersion.Minor));
        }

        private static bool IsTagOfFormat(
            string tag,
            VersionType versionType,
            string? majorMinor = null,
            string? os = null,
            string? architecture = null)
        {
            string tagRegex = versionType switch
            {
                VersionType.Major =>
                    majorMinor == null
                        ? MajorVersionRegex
                        : @$"{GetVersion(majorMinor).Major}(-preview)?",
                VersionType.MajorMinor =>
                    @$"{majorMinor ?? MajorMinorVersionRegex}(-preview)?",
                VersionType.MajorMinorPatch =>
                    @$"{majorMinor ?? MajorMinorVersionRegex}\.{SingleNumberRegex}(-{SingleNumberRegex})?(?:-(alpha|beta|preview|rc)\.{SingleNumberRegex})?",
                _ => throw new ArgumentException("Invalid version type", nameof(versionType)),
            };

            string patternRegex = $"^{tagRegex}" + (os != null ? $"-{os}" : string.Empty) + (architecture != null ? $"-{architecture}" : string.Empty) + "$";
            return Regex.IsMatch(tag, patternRegex);
        }

        private static Version? GetAlpineVersion(ManifestHelper.DockerfileInfo dockerfileInfo)
        {
            string? parsedOs = dockerfileInfo.Os.Replace(AlpineOs, string.Empty);
            if (string.IsNullOrEmpty(parsedOs))
            {
                // No version specified
                return null;
            }
            return GetVersion(parsedOs);
        }

        private static int GetExpectedLatestMajorVersion(Repo repo)
        {
            IEnumerable<string> productVersions = ManifestHelper.GetResolvedProductVersions(repo);

            Assert.NotEmpty(productVersions);

            if (productVersions.Count() == 1)
            {
                // Use the first product version if there is only one
                return GetVersion(productVersions.First()).Major;
            }

            // In non-nightly branches, preview versions should not have the latest tag
            if (!Config.IsNightlyRepo)
            {
                productVersions = productVersions
                    .Where(version => !version.Contains("-preview") && !version.Contains("-rc"));
            }

            return productVersions
                .Select(version => GetVersion(version).Major)
                .Max();
        }

        private static Version GetVersion(string input)
        {
            // Version in the input can be in the form of
            // <Major>, <Major.Minor>, or <Major.Minor.Patch>
            Match match = Regex.Match(input, @$"^({MajorVersionRegex})(\.{SingleNumberRegex})?(\.{SingleNumberRegex})?");
            if (!match.Success)
            {
                throw new ArgumentException($"Failed to parse version from '{input}'", nameof(input));
            }
            return new Version(match.Value);
        }
    }
}
