// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public class MonitorImageData : ImageData
    {
        public Version RuntimeVersion { get; set; }
        public string RuntimeVersionString => RuntimeVersion.ToString(2);
        public Version Version { get; set; }
        public string VersionString => Version.ToString(2);
        public string OSTag { get; set; }

        public string GetImage(DockerHelper dockerHelper)
        {
            string tag = GetTagName(VersionString, OSTag);

            string imageName = GetImageName(tag, "monitor");

            PullImageIfNecessary(imageName, dockerHelper);

            return imageName;
        }

        protected override string GetArchTagSuffix() => string.Empty;
    }
}
