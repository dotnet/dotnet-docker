// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public string Rid
        {
            get {
                string rid;

                if (Arch == Arch.Arm)
                {
                    if (OS.StartsWith(Tests.OS.AlpinePrefix))
                    {
                        rid = "linux-musl-arm";
                    }
                    else
                    {
                        rid = "linux-arm";
                    }
                }
                else if (Arch == Arch.Arm64)
                {
                    if (OS.StartsWith(Tests.OS.AlpinePrefix))
                    {
                        rid = "linux-musl-arm64";
                    }
                    else
                    {
                        rid = "linux-arm64";
                    }
                }
                else if (OS.StartsWith(Tests.OS.AlpinePrefix))
                {
                    rid = "linux-musl-x64";
                }
                else
                {
                    rid = "linux-x64";
                }

                return rid;
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
            if (Arch == Arch.Amd64 && DockerHelper.IsLinuxContainerModeEnabled)
            {
                return "-amd64";
            }
            else if (Arch == Arch.Arm)
            {
                return "-arm32v7";
            }
            else if (Arch == Arch.Arm64)
            {
                return "-arm64v8";
            }

            return string.Empty;
        }

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
                        .Any(platformInfo => platformInfo.Value<JArray>("simpleTags").Any(imageTag => imageTag.ToString() == tag));
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
