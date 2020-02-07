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
        private List<string> _pulledImages = new List<string>();

        public Arch Arch { get; set; }
        public bool IsArm => Arch == Arch.Arm || Arch == Arch.Arm64;
        public string OS { get; set; }

        private static Lazy<JArray> ImageInfoData;

        static ImageData()
        {
            ImageInfoData = new Lazy<JArray>(() =>
            {
                string imageInfoPath = Environment.GetEnvironmentVariable("IMAGE_INFO_PATH");
                if (!String.IsNullOrEmpty(imageInfoPath))
                {
                    string imageInfoContents = File.ReadAllText(imageInfoPath);
                    return JsonConvert.DeserializeObject<JArray>(imageInfoContents);
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
                    rid = "linux-arm";
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

        protected void PullImageIfNecessary(string imageName, DockerHelper dockerHelper)
        {
            if (Config.PullImages && !_pulledImages.Contains(imageName))
            {
                dockerHelper.Pull(imageName);
                _pulledImages.Add(imageName);
            }
            else
            {
                Assert.True(DockerHelper.ImageExists(imageName), $"`{imageName}` could not be found on disk.");
            }
        }

        public static string GetImageName(string tag, string variantName)
        {
            string repoSuffix = Config.IsNightlyRepo ? "-nightly" : string.Empty;
            return GetImageName(tag, variantName, repoSuffix);
        }

        protected static string GetImageName(string tag, string variantName, string repoSuffix)
        {
            string repo = $"dotnet/core{repoSuffix}/{variantName}";
            string registry = GetRegistryName(repo, tag);

            return $"{registry}{repo}:{tag}";
        }

        protected string GetTagName(string tagPrefix, string os)
        {
            string arch = string.Empty;
            if (Arch == Arch.Arm)
            {
                arch = "-arm32v7";
            }
            else if (Arch == Arch.Arm64)
            {
                arch = "-arm64v8";
            }

            return $"{tagPrefix}-{os}{arch}";
        }

        private static string GetRegistryName(string repo, string tag)
        {
            bool imageExistsInStaging = true;

            // In the case of running this in a local development environment, there would likely be no image info file
            // provided. In that case, the assumption is that the images exist in the staging location.

            if (ImageData.ImageInfoData.Value != null)
            {
                JObject repoInfo = (JObject)ImageData.ImageInfoData.Value
                    .FirstOrDefault(imageInfoRepo => imageInfoRepo["repo"].ToString() == repo);

                if (repoInfo?["images"] != null)
                {
                    imageExistsInStaging = repoInfo["images"]
                        .Cast<JProperty>()
                        .Any(imageInfo => imageInfo.Value["simpleTags"].Any(imageTag => imageTag.ToString() == tag));
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
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Select(propInfo => $"{propInfo.Name}='{propInfo.GetValue(this) ?? "<null>"}'")
                .Aggregate((working, next) => $"{working}, {next}");
        }
    }
}
