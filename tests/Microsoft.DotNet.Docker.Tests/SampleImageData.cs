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
            string tagPrefix = GetTagNameBase(imageType);
            string os = OS;

            string tag = GetTagName(tagPrefix, os);
            if (!IsPublished)
            {
                tag += "-local";
            }

            return GetImage(tag, dockerHelper, allowPull);
        }

        public string GetImage(string tag, DockerHelper dockerHelper, bool allowPull = false)
        {
            string imageName = GetImageName(tag);

            if (IsPublished)
            {
                PullImageIfNecessary(imageName, dockerHelper, allowPull);
            }

            return imageName;
        }

        public string GetTagNameBase(SampleImageType imageType) =>
            Enum.GetName(typeof(SampleImageType), imageType).ToLowerInvariant();

        public static string GetImageName(string tag) =>
            GetImageName(tag, "samples", string.Empty);
    }
}
