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
    public class ImageData
    {
        private List<string> _pulledImages = new List<string>();
        private Version _runtimeDepsVersion;
        private string _sdkOS;

        public Arch Arch { get; set; }
        public Version Version { get; set; }
        public string VersionString => Version.ToString(2);
        public bool HasCustomSdk => _sdkOS != null;
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

        public Version RuntimeDepsVersion
        {
            get { return _runtimeDepsVersion ?? Version; }
            set { _runtimeDepsVersion = value; }
        }

        public string SdkOS
        {
            get => _sdkOS ?? OS.TrimEnd(Tests.OS.SlimSuffix);
            set { _sdkOS = value; }
        }

        public string GetIdentifier(string type) => $"{VersionString}-{type}-{DateTime.Now.ToFileTime()}";

        public string GetImage(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string imageName = GetImageName(imageType, dockerHelper);

            if (!Config.IsLocalRun && !_pulledImages.Contains(imageName))
            {
                dockerHelper.Pull(imageName);
                _pulledImages.Add(imageName);
            }
            else
            {
                Assert.True(DockerHelper.ImageExists(imageName), $"`{imageName}` could not be found on disk.");
            }

            return imageName;
        }

        private string GetImageName(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string repoSuffix = Config.IsNightlyRepo ? "-nightly" : string.Empty;
            string variantName = Enum.GetName(typeof(DotNetImageType), imageType).ToLowerInvariant().Replace('_', '-');
            string tag = GetTagName(imageType, variantName);
            string repo = $"dotnet/core{repoSuffix}/{variantName}";
            string registry = GetRegistryName(repo, tag);

            return $"{registry}{repo}:{tag}";
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

                if (repoInfo["images"] != null)
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

        private string GetTagName(DotNetImageType imageType, string variantName)
        {
            Version imageVersion;
            string os;
            switch (imageType)
            {
                case DotNetImageType.Runtime:
                case DotNetImageType.Aspnet:
                    imageVersion = Version;
                    os = OS;
                    break;
                case DotNetImageType.Runtime_Deps:
                    imageVersion = RuntimeDepsVersion;
                    os = OS;
                    break;
                case DotNetImageType.SDK:
                    imageVersion = Version;
                    os = SdkOS;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported image type '{variantName}'");
            }

            string arch = string.Empty;
            if (Arch == Arch.Arm)
            {
                arch = "-arm32v7";
            }
            else if (Arch == Arch.Arm64)
            {
                arch = "-arm64v8";
            }

            return $"{imageVersion.ToString(2)}-{os}{arch}";
        }

        public override string ToString()
        {
            return typeof(ImageData).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(propInfo => $"{propInfo.Name}='{propInfo.GetValue(this) ?? "<null>"}'")
                .Aggregate((working, next) => $"{working}, {next}");
        }
    }
}
