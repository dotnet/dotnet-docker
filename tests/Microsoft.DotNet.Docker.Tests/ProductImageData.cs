// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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
            get => _sdkOS ?? OS.TrimEnd(Tests.OS.SlimSuffix);
            set { _sdkOS = value; }
        }

        public override string GetIdentifier(string type) => $"{VersionString}-{base.GetIdentifier(type)}";

        public string GetImage(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string variantName = Enum.GetName(typeof(DotNetImageType), imageType).ToLowerInvariant().Replace('_', '-');
            string tag = GetTagName(imageType);
            string imageName = GetImageName(tag, variantName);

            PullImageIfNecessary(imageName, dockerHelper);

            return imageName;
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

            return this.GetTagName(imageVersion.ToString(2), os);
        }
    }
}
