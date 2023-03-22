// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.DotNet.Docker.Tests
{
    public abstract class ImageData
    {
        private readonly List<string> _pulledImages = new List<string>();

        public Arch Arch { get; set; }
        public bool IsArm => Arch == Arch.Arm || Arch == Arch.Arm64;
        public string OS { get; set; }
        public bool IsDistroless => OS.Contains("distroless") || OS.Contains("chiseled");
        public virtual int DefaultPort => IsDistroless ? 8080 : 80;
        public virtual int? NonRootUID => IsWindows ? null : 64198;

        private static readonly Lazy<JObject> s_imageInfoData;

        static ImageData()
        {
            s_imageInfoData = new Lazy<JObject>(() =>
            {
                string imageInfoPath = Environment.GetEnvironmentVariable("IMAGE_INFO_PATH");
                if (!string.IsNullOrEmpty(imageInfoPath))
                {
                    string imageInfoContents = File.ReadAllText(imageInfoPath);
                    return JsonConvert.DeserializeObject<JObject>(imageInfoContents);
                }

                return null;
            });
        }

        public string Platform
        {
            get
            {
                string os = IsWindows ? "windows" : "linux";
                string arch = Arch.ToString().ToLowerInvariant();
                if (ArchVariant.Length > 0)
                {
                    arch += $"/{ArchVariant}";
                }

                return $"{os}/{arch}";
            }
        }

        public string ArchVariant =>
            Arch switch
            {
                Arch.Amd64 => string.Empty,
                Arch.Arm => "v7",
                Arch.Arm64 => "v8",
                _ => throw new NotImplementedException()
            };

        public bool IsWindows => OS.StartsWith(Tests.OS.NanoServer) || OS.StartsWith(Tests.OS.ServerCore);

        public string Rid
        {
            get
            {
                string rid;

                if (IsWindows)
                {
                    rid = "win-x64";
                }
                else
                {
                    string arch = Arch switch
                    {
                        Arch.Arm => "arm",
                        Arch.Arm64 => "arm64",
                        Arch.Amd64 => "x64",
                        _ => throw new NotImplementedException()
                    };
                    string modifier = OS.StartsWith(Tests.OS.Alpine) ? "musl-" : "";
                    rid = $"linux-{modifier}{arch}";
                }

                return rid;
            }
        }

        public string OsVersion
        {
            get
            {
                const string PrefixGroup = "Prefix";
                const string VersionGroup = "Version";
                const string LtscPrefix = "ltsc";
                string versionNumber = string.Empty;
                Match match = Regex.Match(OS, @$"(-(?<{PrefixGroup}>[a-zA-Z_]*))?(?<{VersionGroup}>\d+.\d+)");

                if (match.Groups[PrefixGroup].Success && match.Groups[PrefixGroup].Value == LtscPrefix)
                {
                    versionNumber = LtscPrefix;
                }

                versionNumber += match.Groups[VersionGroup].Value;
                return versionNumber;
            }
        }

        public virtual string GetIdentifier(string type) => GenerateContainerName(type);

        public static string GenerateContainerName(string prefix) => $"{prefix}-{DateTime.Now.ToFileTime()}";

        protected void PullImageIfNecessary(string imageName, DockerHelper dockerHelper, bool allowPull = false)
        {
            if ((Config.PullImages || allowPull) && !_pulledImages.Contains(imageName))
            {
                dockerHelper.Pull(imageName);
                _pulledImages.Add(imageName);
            }
            else
            {
                Assert.True(DockerHelper.ImageExists(imageName), $"`{imageName}` could not be found on disk.");
            }
        }

        public static string GetRepoNameModifier() => $"{(Config.IsNightlyRepo ? "/nightly" : string.Empty)}";

        public static string GetImageName(string tag, string variantName, string repoNameModifier = null)
        {
            string repo = $"dotnet{repoNameModifier ?? GetRepoNameModifier()}/{variantName}";
            string registry = GetRegistryName(repo, tag);

            return $"{registry}{repo}:{tag}";
        }

        protected string GetTagName(string tagPrefix, string os) =>
            $"{tagPrefix}-{os}{GetArchTagSuffix()}";

        protected virtual string GetArchTagSuffix()
        {
            if (Arch == Arch.Amd64 && !DockerHelper.IsLinuxContainerModeEnabled)
            {
                return string.Empty;
            }

            return $"-{GetArchLabel()}";
        }

        protected string GetArchLabel() =>
            Arch switch
            {
                Arch.Amd64 => "amd64",
                Arch.Arm => "arm32v7",
                Arch.Arm64 => "arm64v8",
                _ => throw new NotSupportedException()
            };

        private static string GetRegistryName(string repo, string tag)
        {
            bool imageExistsInStaging = true;

            // In the case of running this in a local development environment, there would likely be no image info file
            // provided. In that case, the assumption is that the images exist in the staging location.

            if (ImageData.s_imageInfoData.Value != null)
            {
                JObject repoInfo = (JObject)ImageData.s_imageInfoData.Value
                    .Value<JArray>("repos")
                    .FirstOrDefault(imageInfoRepo => imageInfoRepo["repo"].ToString() == repo);

                if (repoInfo?["images"] != null)
                {
                    imageExistsInStaging = repoInfo.Value<JArray>("images")
                        .SelectMany(imageInfo => imageInfo.Value<JArray>("platforms"))
                        .Cast<JObject>()
                        .Any(platformInfo => platformInfo.Value<JArray>("simpleTags")?.Any(imageTag => imageTag.ToString() == tag) == true);
                }
                else
                {
                    imageExistsInStaging = false;
                }
            }

            return imageExistsInStaging ? $"{Config.Registry}/{Config.RepoPrefix}" : "mcr.microsoft.com/";
        }

        public override string ToString()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Select(propInfo => $"{propInfo.Name}='{propInfo.GetValue(this) ?? "<null>"}'")
                .Aggregate((working, next) => $"{working}, {next}");
        }
    }
}
