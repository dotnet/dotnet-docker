// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ProductImageData : ImageData
    {
        private string _sdkOS;

        public Version Version { get; set; }
        public string VersionString => Version.ToString(2);
        public bool HasCustomSdk => _sdkOS != null;

        public string SdkOS
        {
            get
            {
                if (_sdkOS != null)
                {
                    return _sdkOS;
                }

                if (Version.Major >= 5)
                {
                    return OS;
                }

                return OS.TrimEnd(Tests.OS.SlimSuffix);
            }
            set { _sdkOS = value; }
        }

        public override string GetIdentifier(string type) => $"{VersionString}-{base.GetIdentifier(type)}";

        public string GetProductImageName(string tag, string variantName)
        {
            return GetImageName(tag, variantName);
        }

        public string GetImage(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string variantName = Enum.GetName(typeof(DotNetImageType), imageType).ToLowerInvariant().Replace('_', '-');
            string tag = GetTagName(imageType);
            string imageName = GetProductImageName(tag, variantName);

            PullImageIfNecessary(imageName, dockerHelper);

            return imageName;
        }

        public string GetProductVersion(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string imageName = GetImage(imageType, dockerHelper);
            string containerName = GetIdentifier($"GetProductVersion-{imageType}");

            return imageType switch
            {
                DotNetImageType.SDK => dockerHelper.Run(imageName, containerName, "dotnet --version"),
                DotNetImageType.Runtime => GetRuntimeVersion(imageName, containerName, "Microsoft.NETCore.App", dockerHelper),
                DotNetImageType.Aspnet => GetRuntimeVersion(imageName, containerName, "Microsoft.AspNetCore.App", dockerHelper),
                _ => throw new NotSupportedException($"Unsupported image type '{imageType}'"),
            };
        }

        private string GetRuntimeVersion(string imageName, string containerName, string runtimeName, DockerHelper dockerHelper)
        {
            const string versionGroupName = "Version";

            string runtimeListing = dockerHelper.Run(imageName, containerName, "dotnet --list-runtimes");
            Regex versionRegex = new Regex($"{runtimeName} (?<{versionGroupName}>[^\\s]+) ");
            Match match = versionRegex.Match(runtimeListing);
            return match.Success ? match.Groups[versionGroupName].Value : string.Empty;
        }

        private string GetTagName(DotNetImageType imageType)
        {
            Version imageVersion;
            string os;
            switch (imageType)
            {
                case DotNetImageType.Runtime:
                case DotNetImageType.Aspnet:
                case DotNetImageType.Runtime_Deps:
                    imageVersion = Version;
                    os = OS;
                    break;
                case DotNetImageType.SDK:
                    imageVersion = Version;
                    os = SdkOS;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported image type '{imageType}'");
            }

            return GetTagName(imageVersion.ToString(2), os);
        }

        protected override string GetArchTagSuffix()
        {
            if (Arch == Arch.Amd64 && Version.Major < 5)
            {
                return string.Empty;
            }

            return base.GetArchTagSuffix();
        }
    }
}
