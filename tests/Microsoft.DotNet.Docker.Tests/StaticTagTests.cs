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
        public enum TestType
        {
            Latest,
            Platform,
            Alpine,
            Version
        }
        private const string AlpineOs = "alpine";
        private const string LatestTagValue = "latest";
        private const string SingleNumberRegex = @"\d+";
        private const string DoubleNumberRegex = @$"{SingleNumberRegex}\.{SingleNumberRegex}";
        private static readonly string[] ApplianceRepos = { "monitor", "monitor-base", "aspire-dashboard" };

        public static IEnumerable<object[]> GetTagTestObjects(TestType testType)
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
                            // Only appliance repos have floating distro tags (new schema only)
                            testObjects.AddRange(new[]
                            {
                                GetTagTestInput(testType, repo, 3, true, true, IsApplianceVersionUsingNewSchema),     // <Major.Minor.Patch>-<os>-<architecture>
                                GetTagTestInput(testType, repo, 3, true, false, IsApplianceVersionUsingNewSchema),    // <Major.Minor.Patch>-<os>
                                GetTagTestInput(testType, repo, 3, false, true, IsApplianceVersionUsingOldSchema),    // <Major.Minor.Patch>-<architecture>
                                GetTagTestInput(testType, repo, 2, true, true, IsApplianceVersionUsingNewSchema),     // <Major.Minor>-<os>-<architecture>
                                GetTagTestInput(testType, repo, 2, true, false, IsApplianceVersionUsingNewSchema),    // <Major.Minor>-<os>
                                GetTagTestInput(testType, repo, 2, false, true, IsApplianceVersionUsingOldSchema),    // <Major.Minor>-<architecture>
                                GetTagTestInput(testType, repo, 1, true, true, IsApplianceVersionUsingNewSchema),     // <Major>-<os>-<architecture>
                                GetTagTestInput(testType, repo, 1, true, false, IsApplianceVersionUsingNewSchema),    // <Major>-<os>
                            });

                            if (!repo.Name.Contains("monitor"))
                            {
                                // Monitor repos don't have these tags
                                testObjects.Add(GetTagTestInput(testType, repo, 1, false, true, IsApplianceVersionUsingOldSchema)); // <Major>-<architecture>
                            }
                        }
                        else
                        {
                            testObjects.AddRange(new[]
                            {
                                GetTagTestInput(testType, repo, 3, true, true, IsWindows),                            // <Major.Minor.Patch>-<os>-<architecture>
                                GetTagTestInput(testType, repo, 3, true, false),                                      // <Major.Minor.Patch>-<os>
                                GetTagTestInput(testType, repo, 2, true, true, IsWindows),                            // <Major.Minor>-<os>-<architecture>
                                GetTagTestInput(testType, repo, 2, true, false),                                      // <Major.Minor>-<os>
                            });
                        }
                        break;

                    case TestType.Alpine:
                        if (ApplianceRepos.Any(repo.Name.Contains))
                        {
                            // Only appliance repos have major version alpine floating tags
                            // Only appliance repos have major.minor.patch alpine floating tags
                            testObjects.AddRange(new[]
                            {
                                GetTagTestInput(testType, repo, 3, true),                                               // <Major.Minor.Patch>-alpine-<architecture>
                                GetTagTestInput(testType, repo, 3, false),                                              // <Major.Minor.Patch>-alpine
                                GetTagTestInput(testType, repo, 1, true),                                               // <Major>-alpine-<architecture>
                                GetTagTestInput(testType, repo, 1, false),                                              // <Major>-alpine
                            });
                        }
                        testObjects.AddRange(new[]
                        {
                            GetTagTestInput(testType, repo, 2, true),                                               // <Major.Minor>-alpine-<architecture>
                            GetTagTestInput(testType, repo, 2, false),                                              // <Major.Minor>-alpine
                        });
                        break;

                    case TestType.Version:
                        if (ApplianceRepos.Any(repo.Name.Contains))
                        {
                            // Only appliance repos have major version tags
                            testObjects.Add(GetTagTestInput(testType, repo, 1));                                   // <Major>
                        }
                        testObjects.AddRange(new[]
                        {
                            GetTagTestInput(testType, repo, 2),                                                   // <Major.Minor>
                            GetTagTestInput(testType, repo, 3),                                                   // <Major.Minor.Patch>
                        });
                        break;

                    default:
                        throw new ArgumentException("Invalid tag type", nameof(testType));
                }
            }
            return testObjects.ToArray();
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
            Image latestImage = repo.Images
                .Where(image => ManifestHelper.GetResolvedSharedTags(image).Contains(LatestTagValue))
                .First();

            int expectedMajorVersion = GetExpectedMajorVersion(repo);
            int actualMajorVersion = GetMajorVersion(ManifestHelper.GetResolvedProductVersion(latestImage));

            actualMajorVersion.Should().Be(expectedMajorVersion, "expected latest tag to be on the latest major version");
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
        private void PlatformTag_TagExists(
            Repo repo,
            int versionParts,
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
                        versionParts,
                        dockerfileInfo.MajorMinor,
                        checkOs ? dockerfileInfo.Os : null,
                        checkArchitecture ? dockerfileInfo.Architecture : null));

                if (versionParts == 1 && !IsExpectedMajorMinorVersion(repo, dockerfileInfo.MajorMinor))
                {
                    // Special case for major version tags
                    // These tags should be on the most up-to-date Major.Minor version for the respective major version
                    tags.Should().BeEmpty("expected tag to be on latest Major.Minor version for version " +
                        GetMajorVersion(dockerfileInfo.MajorMinor) + ", but found the tag on " + dockerfileInfo);
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
        [MemberData(nameof(GetTagTestObjects), TestType.Alpine)]
        public void AlpineTag_OnLatestVersion(Repo repo, int versionParts, bool checkArchitecture)
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
            string? latestAlpineVersion = alpineDockerfiles
                .Select(GetAlpineVersion)
                .Where(version => !string.IsNullOrEmpty(version))
                .OrderByDescending(version => new Version(version))
                .FirstOrDefault();

            foreach (ManifestHelper.DockerfileInfo dockerfileInfo in alpineDockerfiles)
            {
                IEnumerable<string> alpineFloatingTags = dockerfileTags[dockerfileInfo]
                    .Where(tag => IsTagOfFormat(tag, versionParts, dockerfileInfo.MajorMinor, AlpineOs, checkArchitecture ? dockerfileInfo.Architecture : null));

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
        public void VersionTag_SameOsAndVersion(Repo repo, int versionParts)
        {
            // Group tags -> dockerfiles
            // Skip .NET 6 dockerfiles because they include linux and windows OSes for the tags
            Dictionary<string, List<ManifestHelper.DockerfileInfo>> tagsToDockerfiles = ManifestHelper.GetDockerfileTags(repo)
                .Where(pair => !IsDotNet6(pair.Key))
                .SelectMany(pair => pair.Value
                    .Where(tag => IsTagOfFormat(tag, versionParts, majorMinor: pair.Key.MajorMinor, os: null, architecture: null))
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
                
                if (versionParts == 1)
                {
                    string dockerfileVersion = dockerfileVersions.First();
    
                    // Special case for major version tags
                    // These tags should be on the most up-to-date Major.Minor version for the respective major version
                    IsExpectedMajorMinorVersion(repo, dockerfileVersion).Should().BeTrue(
                        "expected tag to be on the latest Major.Minor version for the major version " +
                        GetMajorVersion(dockerfileVersion) + ", but found the tag on " + dockerfileVersion);
                }

                List<string> dockerfileOses = dockerfiles
                    .Select(dockerfile => dockerfile.Os)
                    .Distinct()
                    .ToList();

                dockerfileOses.Should().ContainSingle(
                    "all dockerfiles for tag " + tag + " should have the same os");
            }
        }

        private static bool IsWindows(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.Os.Contains("windowsservercore") || dockerfileInfo.Os.Contains("nanoserver");

        private static bool IsApplianceVersionUsingOldSchema(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.Repo.Contains("monitor") && GetMajorVersion(dockerfileInfo.MajorMinor) <= 8;

        private static bool IsApplianceVersionUsingNewSchema(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            !IsApplianceVersionUsingOldSchema(dockerfileInfo);

        private static bool IsDotNet6(ManifestHelper.DockerfileInfo dockerfileInfo) =>
            dockerfileInfo.MajorMinor.StartsWith("6");

        private static bool IsExpectedMajorMinorVersion(Repo repo, string version)
        {
            IEnumerable<string> productVersions = ManifestHelper.GetResolvedProductVersions(repo);

            Assert.NotEmpty(productVersions);

            List<double> majorMinorVersions = productVersions
                .GroupBy(GetMajorVersion)
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
                .Select(group => group.Select(GetMajorMinorVersion).Max())
                .Distinct()
                .ToList();

            return majorMinorVersions.Contains(GetMajorMinorVersion(version));
        }

        private static bool IsTagOfFormat(
            string tag,
            int versionParts,
            string? majorMinor = null,
            string? os = null,
            string? architecture = null)
        {
            string tagRegex = versionParts switch
            {
                1 => majorMinor != null ? GetMajorVersion(majorMinor).ToString() : SingleNumberRegex,
                2 => @$"{majorMinor ?? DoubleNumberRegex}(-preview)?",
                3 => @$"{majorMinor ?? DoubleNumberRegex}\.{SingleNumberRegex}(-{SingleNumberRegex})?(?:-(alpha|beta|preview|rc)\.{SingleNumberRegex})?",
                _ => throw new ArgumentException("Invalid version parts", nameof(versionParts)),
            };

            string patternRegex = $"^{tagRegex}" + (os != null ? $"-{os}" : string.Empty) + (architecture != null ? $"-{architecture}" : string.Empty) + "$";
            return Regex.IsMatch(tag, patternRegex);
        }

        private static string GetAlpineVersion(ManifestHelper.DockerfileInfo dockerfileInfo)
        {
            Match match = Regex.Match(dockerfileInfo.Os, @$"{AlpineOs}(?<version>{DoubleNumberRegex})");
            if (!match.Success)
            {
                return string.Empty;
            }
            return match.Groups["version"].Value;
        }

        private static int GetExpectedMajorVersion(Repo repo)
        {
            IEnumerable<string> productVersions = ManifestHelper.GetResolvedProductVersions(repo);

            Assert.NotEmpty(productVersions);

            if (productVersions.Count() == 1)
            {
                // Use the first product version if there is only one
                return GetMajorVersion(productVersions.First());
            }

            return productVersions
                .Where(version => 
                {
                    if (!Config.IsNightlyRepo)
                    {
                        // Use the latest GA version on the main branch
                        // Assumes that non-GA versions have a hyphen in them
                        // e.g. non GA: 5.0.0-preview.1, GA: 5.0.0
                        return !version.Contains('-');
                    }
                    // Use the latest version on the nightly branch
                    return true;
                })
                .Select(GetMajorVersion)
                .Max();
        }

        private static int GetMajorVersion(string input) =>
            int.Parse(ParseVersion(input, 1));

        private static double GetMajorMinorVersion(string input) =>
            double.Parse(ParseVersion(input, 2));

        private static string ParseVersion(string input, int versionParts)
        {
            if (versionParts < 1 || versionParts > 2)
            {
                throw new ArgumentException("Invalid version parts", nameof(versionParts));
            }

            Match match = Regex.Match(input, versionParts == 1 ? SingleNumberRegex : DoubleNumberRegex);
            if (!match.Success)
            {
                throw new ArgumentException($"Failed to parse version: {input}");
            }
            return match.Value;
        }

        private static object[] GetTagTestInput(
            TestType testType,
            Repo repo,
            int? versionParts = null,
            bool? checkOs = false,
            bool? checkArchitecture = false,
            Func<ManifestHelper.DockerfileInfo, bool>? skipDockerfileOn = null)
        {
            switch (testType)
            {
                case TestType.Latest:
                    return [ repo ];

                case TestType.Platform:
                    if (versionParts == null || checkOs == null || checkArchitecture == null)
                    {
                        throw new ArgumentException("'versionParts', 'checkOs' and 'checkArchitecture' must be specified", nameof(testType));
                    }
                    return [ repo, versionParts, checkOs, checkArchitecture, skipDockerfileOn ?? (_ => false) ];

                case TestType.Alpine:
                    if (versionParts == null || checkArchitecture == null)
                    {
                        throw new ArgumentException("'versionParts' and 'checkArchitecture' must be specified", nameof(testType));
                    }
                    return [ repo, versionParts, checkArchitecture ];

                case TestType.Version:
                    if (versionParts == null)
                    {
                        throw new ArgumentException("'versionParts' must be specified", nameof(testType));
                    }
                    return [ repo, versionParts ];

                default:
                    throw new ArgumentException("Invalid tag type", nameof(testType));
            }
        }
    }
}
