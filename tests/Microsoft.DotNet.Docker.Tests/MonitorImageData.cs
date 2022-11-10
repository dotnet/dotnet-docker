// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.Docker.Tests
{
    public class MonitorImageData : VersionedImageData
    {
        private ImageVersion _runtimeVersion;

        public override ImageVersion RuntimeVersion
        {
            get => _runtimeVersion;
            set => _runtimeVersion = value;
        }

        public string OSTag { get; set; }

        public string GetImage(DockerHelper dockerHelper)
        {
            string tag = GetTagName(Version.GetTagName(), OSTag);

            string imageName = GetImageName(tag, "monitor");

            PullImageIfNecessary(imageName, dockerHelper);

            return imageName;
        }
    }
}
