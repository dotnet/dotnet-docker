// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SampleImageData : ImageData
    {
        /// <summary>
        /// Gets or sets a value indicating that this sample is published as a Docker image.
        /// </summary>
        public bool IsPublished { get; set; }

        public string DockerfileSuffix { get; set; }

        public string GetImage(SampleImageType imageType, DockerHelper dockerHelper, bool allowPull = false)
        {
            string tagPrefix = Enum.GetName(typeof(SampleImageType), imageType).ToLowerInvariant();
            string tag = GetTagName(tagPrefix, OS);
            if (!IsPublished)
            {
                tag += "-local";
            }

            string imageName = GetImageName(tag);

            if (IsPublished)
            {
                PullImageIfNecessary(imageName, dockerHelper, allowPull);
            }
            
            return imageName;
        }

        public static string GetImageName(string tag)
        {
            return GetImageName(tag, "samples", string.Empty);
        }
    }
}
