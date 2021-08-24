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
            new ProductImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.Focal,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.Alpine313,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ProductImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Arm },
            new ProductImageData { Version = V3_1, OS = OS.Focal,        Arch = Arch.Arm },
            new ProductImageData { Version = V3_1, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_1, OS = OS.Bionic,       Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_1, OS = OS.Focal,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V3_1, OS = OS.Alpine313,    Arch = Arch.Arm64,   SdkOS = OS.Buster },
            new ProductImageData { Version = V3_1, OS = OS.Mariner10,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.BusterSlim,   Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.Focal,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.Alpine313,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.Mariner10,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.BusterSlim,   Arch = Arch.Arm },
            new ProductImageData { Version = V5_0, OS = OS.BusterSlim,   Arch = Arch.Arm64 },
            new ProductImageData { Version = V5_0, OS = OS.Focal,        Arch = Arch.Arm },
            new ProductImageData { Version = V5_0, OS = OS.Focal,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V5_0, OS = OS.Alpine313,    Arch = Arch.Arm64,   SdkOS = OS.BusterSlim },
            new ProductImageData { Version = V6_0, OS = OS.BullseyeSlim, Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Focal,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Alpine313,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Mariner10,    Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.BullseyeSlim, Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.BullseyeSlim, Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.Focal,        Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.Focal,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.Alpine313,    Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.Alpine313,    Arch = Arch.Arm64 },
        };
        private static readonly ProductImageData[] s_windowsTestData =
        {
            new ProductImageData { Version = V3_1, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.NanoServer2004,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.NanoServer20H2,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V3_1, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.NanoServer2004,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.NanoServer20H2,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V5_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.NanoServer2004,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.NanoServer20H2,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
        };
        
        private static readonly SampleImageData[] s_linuxSampleTestData =
        {
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Arm,   IsPublished = true },
            new SampleImageData { OS = OS.BusterSlim, Arch = Arch.Arm64, IsPublished = true },

            new SampleImageData { OS = OS.Alpine,     Arch = Arch.Arm64, DockerfileSuffix = "alpine-arm64" },
            new SampleImageData { OS = OS.Alpine,     Arch = Arch.Amd64, DockerfileSuffix = "alpine-x64" },
            new SampleImageData { OS = OS.Alpine,     Arch = Arch.Amd64, DockerfileSuffix = "alpine-x64-slim" },
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
            new SampleImageData { OS = OS.NanoServer1809,     Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServer2004,     Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServer20H2,     Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, IsPublished = true },

            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver-x64" },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver-x64-slim" },

            // Use Nano Server as the OS even though the Dockerfiles are for Windows Server Core. This is because the OS value
            // needs to match the filter set by the build/test job. We only produce builds jobs based on what's in the manifest
            // and the manifest only defines Nano Server-based Dockerfiles. So we just need to piggyback on the Nano Server
            // jobs in order to test the Windows Server Core samples.
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-x64" },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-iis-x64" },

            // Disabling the slim sample due to https://github.com/dotnet/dotnet-docker/issues/2938
            //new SampleImageData { OS = OS.NanoServer1809, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-x64-slim" },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-x64-slim" },
        };

        private static readonly MonitorImageData[] s_linuxMonitorTestData =
        {
            new MonitorImageData { Version = V5_0, RuntimeVersion = V3_1, OS = OS.Alpine313, OSTag = OS.Alpine, Arch = Arch.Amd64 },
        };

        private static readonly MonitorImageData[] s_windowsMonitorTestData =
        {
        };

        public static IEnumerable<ProductImageData> GetImageData()
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxTestData : s_windowsTestData)
                .FilterImagesByVersion()
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<ProductImageData>();
        }

        public static IEnumerable<SampleImageData> GetAllSampleImageData() =>
            DockerHelper.IsLinuxContainerModeEnabled ? s_linuxSampleTestData : s_windowsSampleTestData;

        public static IEnumerable<SampleImageData> GetSampleImageData()
        {
            return GetAllSampleImageData()
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<SampleImageData>();
        }

        public static IEnumerable<MonitorImageData> GetMonitorImageData()
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxMonitorTestData : s_windowsMonitorTestData)
                .FilterMonitorImagesByRuntimeVersion()
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<MonitorImageData>();
        }

        public static IEnumerable<ImageData> FilterImagesByVersion(this IEnumerable<ProductImageData> imageData)
        {
            string versionFilterPattern = GetFilterRegexPattern("IMAGE_VERSION");
            return imageData
                .Where(imageData => versionFilterPattern == null
                    || Regex.IsMatch(imageData.VersionString, versionFilterPattern, RegexOptions.IgnoreCase));
        }

        public static IEnumerable<ImageData> FilterMonitorImagesByRuntimeVersion(this IEnumerable<MonitorImageData> imageData)
        {
            string versionFilterPattern = GetFilterRegexPattern("IMAGE_VERSION");
            return imageData
                .Where(imageData => versionFilterPattern == null
                    || Regex.IsMatch(imageData.RuntimeVersionString, versionFilterPattern, RegexOptions.IgnoreCase));
        }

        public static IEnumerable<ImageData> FilterImagesByArch(this IEnumerable<ImageData> imageData)
        {
            string archFilterPattern = GetFilterRegexPattern("IMAGE_ARCH");
            return imageData
                .Where(imageData => archFilterPattern == null
                    || Regex.IsMatch(Enum.GetName(typeof(Arch), imageData.Arch), archFilterPattern, RegexOptions.IgnoreCase));
        }

        public static IEnumerable<ImageData> FilterImagesByOs(this IEnumerable<ImageData> imageData)
        {
            string osFilterPattern = GetFilterRegexPattern("IMAGE_OS");
            return imageData
                .Where(imageData => osFilterPattern == null
                    || Regex.IsMatch(imageData.OS, osFilterPattern, RegexOptions.IgnoreCase));
        }

        private static string GetFilterRegexPattern(string filterEnvName)
        {
            string filter = Environment.GetEnvironmentVariable(filterEnvName);
            return Config.GetFilterRegexPattern(filter);
        }
    }
}
