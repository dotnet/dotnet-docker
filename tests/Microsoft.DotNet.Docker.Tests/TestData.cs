// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Microsoft.DotNet.Docker.Tests.ImageVersion;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class TestData
    {
        private static readonly ProductImageData[] s_linuxTestData =
        {
            new ProductImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Amd64 },
            new ProductImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ProductImageData { Version = V2_1, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V2_1, OS = OS.StretchSlim,  Arch = Arch.Arm },
            new ProductImageData { Version = V2_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ProductImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_0, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ProductImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm },
            new ProductImageData { Version = V3_0, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_0, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_0, OS = OS.Alpine310,    Arch = Arch.Arm64,   SdkOS = OS.Buster },
            new ProductImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.Alpine310,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ProductImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ProductImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_1, OS = OS.Alpine310,    Arch = Arch.Arm64,   SdkOS = OS.Buster },
        };
        private static readonly ProductImageData[] s_windowsTestData =
        {
            new ProductImageData { Version = V2_1, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ProductImageData { Version = V2_1, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ProductImageData { Version = V2_1, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_0, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_0, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ProductImageData { Version = V3_0, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_0, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.NanoServer1809, Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.NanoServer1809, Arch = Arch.Arm },
            new ProductImageData { Version = V3_1, OS = OS.NanoServer1903, Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.NanoServer1909, Arch = Arch.Amd64 },
        };
        
        private static readonly SampleImageData[] s_linuxSampleTestData =
        {
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Arm,   IsPublished = true },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Arm64, IsPublished = true },

            new SampleImageData { OS = OS.Alpine310,  Arch = Arch.Arm64, DockerfileSuffix = "alpine-arm64" },
            new SampleImageData { OS = OS.Alpine310,  Arch = Arch.Amd64, DockerfileSuffix = "alpine-x64" },
            new SampleImageData { OS = OS.Alpine310,  Arch = Arch.Amd64, DockerfileSuffix = "alpine-x64-slim" },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Arm,   DockerfileSuffix = "debian-arm32" },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Arm64, DockerfileSuffix = "debian-arm64" },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Amd64, DockerfileSuffix = "debian-x64" },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Amd64, DockerfileSuffix = "debian-x64-slim" },
            new SampleImageData { OS = OS.Bionic,     Arch = Arch.Arm,   DockerfileSuffix = "ubuntu-arm32" },
            new SampleImageData { OS = OS.Bionic,     Arch = Arch.Arm64, DockerfileSuffix = "ubuntu-arm64" },
            new SampleImageData { OS = OS.Bionic,     Arch = Arch.Amd64, DockerfileSuffix = "ubuntu-x64" },
            new SampleImageData { OS = OS.Bionic,     Arch = Arch.Amd64, DockerfileSuffix = "ubuntu-x64-slim" },
        };

        private static readonly SampleImageData[] s_windowsSampleTestData =
        {
            new SampleImageData { OS = OS.NanoServer1809, Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServer1809, Arch = Arch.Arm,   IsPublished = true },
            new SampleImageData { OS = OS.NanoServer1903, Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServer1909, Arch = Arch.Amd64, IsPublished = true },

            new SampleImageData { OS = OS.NanoServer1809, Arch = Arch.Arm,   DockerfileSuffix = "nanoserver-arm32" },
            new SampleImageData { OS = OS.NanoServer1909, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver-x64" },
            new SampleImageData { OS = OS.NanoServer1909, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver-x64-slim" },
        };

        public static IEnumerable<ProductImageData> GetImageData()
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxTestData : s_windowsTestData)
                .FilterImagesByVersion()
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<ProductImageData>();
        }
        
        public static IEnumerable<SampleImageData> GetSampleImageData()
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxSampleTestData : s_windowsSampleTestData)
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<SampleImageData>();
        }

        private static IEnumerable<ImageData> FilterImagesByVersion(this IEnumerable<ProductImageData> imageData)
        {
            string versionFilterPattern = GetFilterRegexPattern("IMAGE_VERSION_FILTER");
            return imageData
                .Where(imageData => versionFilterPattern == null
                    || Regex.IsMatch(imageData.VersionString, versionFilterPattern, RegexOptions.IgnoreCase));
        }

        private static IEnumerable<ImageData> FilterImagesByArch(this IEnumerable<ImageData> imageData)
        {
            string archFilterPattern = GetFilterRegexPattern("IMAGE_ARCH_FILTER");
            return imageData
                .Where(imageData => archFilterPattern == null
                    || Regex.IsMatch(Enum.GetName(typeof(Arch), imageData.Arch), archFilterPattern, RegexOptions.IgnoreCase));
        }

        private static IEnumerable<ImageData> FilterImagesByOs(this IEnumerable<ImageData> imageData)
        {
            string osFilterPattern = GetFilterRegexPattern("IMAGE_OS_FILTER");
            return imageData
                .Where(imageData => osFilterPattern == null
                    || Regex.IsMatch(imageData.OS, osFilterPattern, RegexOptions.IgnoreCase));
        }

        private static string GetFilterRegexPattern(string filterEnvName)
        {
            string filter = Environment.GetEnvironmentVariable(filterEnvName);
            return filter != null ? $"^{Regex.Escape(filter).Replace(@"\*", ".*").Replace(@"\?", ".")}$" : null;
        }
    }
}
