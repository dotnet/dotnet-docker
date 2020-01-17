// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SampleImageData : ImageData
    {
        /// <summary>
        /// Gets or sets a value indicating that this represents a local sample only and is not published.
        /// </summary>
        public bool LocalOnly { get; set; }

        public string Dockerfile { get; set; }

        public string GetImage(SampleImageType imageType, DockerHelper dockerHelper)
        {
            string tagPrefix = Enum.GetName(typeof(SampleImageType), imageType).ToLowerInvariant();
            string tag = GetTagName(tagPrefix, OS);
            if (LocalOnly)
            {
                tag += "-local";
            }

            string imageName = GetImageName(tag, "samples");

            if (!LocalOnly)
            {
                PullImageIfNecessary(imageName, dockerHelper);
            }
            
            return imageName;
        }
    }
}
