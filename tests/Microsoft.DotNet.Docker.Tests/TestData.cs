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
            new ProductImageData { Version = V6_0, OS = OS.BullseyeSlim,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.BookwormSlim,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Focal,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Jammy,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V6_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V6_0, OS = OS.Alpine318,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Alpine319,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Mariner20,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20 },

            new ProductImageData { Version = V6_0, OS = OS.Mariner20,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20 },
            new ProductImageData { Version = V6_0, OS = OS.BullseyeSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.BookwormSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.Focal,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.Jammy,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V6_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V6_0, OS = OS.Alpine318,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V6_0, OS = OS.Alpine319,           Arch = Arch.Arm64 },

            new ProductImageData { Version = V6_0, OS = OS.BullseyeSlim,        Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.BookwormSlim,        Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.Focal,               Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.Jammy,               Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy },
            new ProductImageData { Version = V6_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V6_0, OS = OS.Alpine318,           Arch = Arch.Arm },
            new ProductImageData { Version = V6_0, OS = OS.Alpine319,           Arch = Arch.Arm },


            new ProductImageData { Version = V7_0, OS = OS.BullseyeSlim,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.BookwormSlim,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.Jammy,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V7_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V7_0, OS = OS.Alpine318,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.Alpine319,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.Mariner20,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20 },

            new ProductImageData { Version = V7_0, OS = OS.Mariner20,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V7_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20 },
            new ProductImageData { Version = V7_0, OS = OS.BullseyeSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V7_0, OS = OS.BookwormSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V7_0, OS = OS.Jammy,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V7_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V7_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V7_0, OS = OS.Alpine318,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V7_0, OS = OS.Alpine319,           Arch = Arch.Arm64 },

            new ProductImageData { Version = V7_0, OS = OS.BullseyeSlim,        Arch = Arch.Arm },
            new ProductImageData { Version = V7_0, OS = OS.BookwormSlim,        Arch = Arch.Arm },
            new ProductImageData { Version = V7_0, OS = OS.Jammy,               Arch = Arch.Arm },
            new ProductImageData { Version = V7_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy },
            new ProductImageData { Version = V7_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V7_0, OS = OS.Alpine318,           Arch = Arch.Arm },
            new ProductImageData { Version = V7_0, OS = OS.Alpine319,           Arch = Arch.Arm },


            new ProductImageData { Version = V8_0, OS = OS.BookwormSlim,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Jammy,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Amd64,   SdkOS = OS.Alpine318,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Amd64,   SdkOS = OS.Alpine318,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Amd64,   SdkOS = OS.Alpine319,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Amd64,   SdkOS = OS.Alpine319,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },

            new ProductImageData { Version = V8_0, OS = OS.Mariner20,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.BookwormSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Jammy,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Arm64,   SdkOS = OS.Alpine318,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Arm64,   SdkOS = OS.Alpine318,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Arm64,   SdkOS = OS.Alpine319,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Arm64,   SdkOS = OS.Alpine319,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },

            new ProductImageData { Version = V8_0, OS = OS.BookwormSlim,        Arch = Arch.Arm },
            new ProductImageData { Version = V8_0, OS = OS.Jammy,               Arch = Arch.Arm },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm,     SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Arm },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Arm,     SdkOS = OS.Alpine318,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine318,           Arch = Arch.Arm,     SdkOS = OS.Alpine318,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Arm },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Arm,     SdkOS = OS.Alpine319,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine319,           Arch = Arch.Arm,     SdkOS = OS.Alpine319,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
        };

        private static readonly ProductImageData[] s_windowsTestData =
        {
            new ProductImageData { Version = V6_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V6_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V7_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
        };

        private static readonly SampleImageData[] s_linuxSampleTestData =
        {
            new SampleImageData { OS = OS.Alpine,    Arch = Arch.Amd64, DockerfileSuffix = "alpine", IsPublished = true },
            new SampleImageData { OS = OS.Alpine,    Arch = Arch.Arm,   DockerfileSuffix = "alpine", IsPublished = true },
            new SampleImageData { OS = OS.Alpine,    Arch = Arch.Arm64, DockerfileSuffix = "alpine", IsPublished = true },

            new SampleImageData { OS = OS.BookwormSlim,     Arch = Arch.Amd64 },
            new SampleImageData { OS = OS.BookwormSlim,     Arch = Arch.Arm },
            new SampleImageData { OS = OS.BookwormSlim,     Arch = Arch.Arm64 },
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Arm64, DockerfileSuffix = "alpine" },
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Amd64, DockerfileSuffix = "alpine" },
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Arm64, DockerfileSuffix = "alpine-icu" },
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Amd64, DockerfileSuffix = "alpine-icu" },
            new SampleImageData { OS = OS.BookwormSlim,     Arch = Arch.Arm,   DockerfileSuffix = "debian" },
            new SampleImageData { OS = OS.BookwormSlim,     Arch = Arch.Arm64, DockerfileSuffix = "debian" },
            new SampleImageData { OS = OS.BookwormSlim,     Arch = Arch.Amd64, DockerfileSuffix = "debian" },
            new SampleImageData { OS = OS.Jammy,            Arch = Arch.Arm,   DockerfileSuffix = "ubuntu" },
            new SampleImageData { OS = OS.Jammy,            Arch = Arch.Arm64, DockerfileSuffix = "ubuntu" },
            new SampleImageData { OS = OS.Jammy,            Arch = Arch.Amd64, DockerfileSuffix = "ubuntu" },
        };

        private static readonly SampleImageData[] s_windowsSampleTestData =
        {
            new SampleImageData { OS = OS.NanoServer1809,     Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, IsPublished = true },

            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver" },

            // Use Nano Server as the OS even though the Dockerfiles are for Windows Server Core. This is because the OS value
            // needs to match the filter set by the build/test job. We only produce builds jobs based on what's in the manifest
            // and the manifest only defines Nano Server-based Dockerfiles. So we just need to piggyback on the Nano Server
            // jobs in order to test the Windows Server Core samples.
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore" },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-iis" },
        };

        private static readonly ProductImageData[] s_linuxMonitorTestData =
        {
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.Alpine318,           OSTag = OS.Alpine,            Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.Alpine318,           OSTag = OS.Alpine,            Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.JammyChiseled,       OSTag = OS.UbuntuChiseled,    Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.JammyChiseled,       OSTag = OS.UbuntuChiseled,    Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.Mariner20,           OSTag = OS.Mariner,           Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.Mariner20,           OSTag = OS.Mariner,           Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.Mariner20Distroless, OSTag = OS.MarinerDistroless, Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V6_3, VersionFamily = V6_0, OS = OS.Mariner20Distroless, OSTag = OS.MarinerDistroless, Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.Alpine318,           OSTag = OS.Alpine,            Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.Alpine318,           OSTag = OS.Alpine,            Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.JammyChiseled,       OSTag = OS.UbuntuChiseled,    Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.JammyChiseled,       OSTag = OS.UbuntuChiseled,    Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.Mariner20,           OSTag = OS.Mariner,           Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.Mariner20,           OSTag = OS.Mariner,           Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.Mariner20Distroless, OSTag = OS.MarinerDistroless, Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V7_3, VersionFamily = V7_0, OS = OS.Mariner20Distroless, OSTag = OS.MarinerDistroless, Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V8_0, VersionFamily = V8_0, OS = OS.JammyChiseled,       OSTag = OS.UbuntuChiseled,    Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V8_0, VersionFamily = V8_0, OS = OS.JammyChiseled,       OSTag = OS.UbuntuChiseled,    Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V8_0, VersionFamily = V8_0, OS = OS.Mariner20Distroless, OSTag = OS.MarinerDistroless, Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor },
            new ProductImageData { Version = V8_0, VersionFamily = V8_0, OS = OS.Mariner20Distroless, OSTag = OS.MarinerDistroless, Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor }
        };

        private static readonly ProductImageData[] s_windowsMonitorTestData =
        {
        };

        public static IEnumerable<ProductImageData> GetImageData(DotNetImageRepo imageRepo)
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxTestData : s_windowsTestData)
                .FilterImagesBySupportedRepo(imageRepo)
                .FilterImagesByPath(imageRepo)
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

        public static IEnumerable<ProductImageData> GetMonitorImageData()
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxMonitorTestData : s_windowsMonitorTestData)
                .FilterImagesByPath(DotNetImageRepo.Monitor)
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<ProductImageData>();
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
            IEnumerable<string> osFilterPatterns = Config.OsNames
                .Select(osName => Config.GetFilterRegexPattern(osName));

            return imageData
                .Where(imageData => !osFilterPatterns.Any()
                    || osFilterPatterns.Any(osFilterPattern => Regex.IsMatch(imageData.OS, osFilterPattern, RegexOptions.IgnoreCase)));
        }

        public static IEnumerable<ImageData> FilterImagesByPath(this IEnumerable<ProductImageData> imageData, DotNetImageRepo imageRepo)
        {
            IEnumerable<string> pathPatterns = Config.Paths
                .Select(path => Config.GetFilterRegexPattern(path));

            return imageData
                .Where(imageData => !pathPatterns.Any()
                    || pathPatterns.Any(pathPattern => Regex.IsMatch(imageData.GetDockerfilePath(imageRepo), pathPattern, RegexOptions.IgnoreCase)));
        }

        public static IEnumerable<ProductImageData> FilterImagesBySupportedRepo(this IEnumerable<ProductImageData> imageData, DotNetImageRepo imageRepo)
        {
            var images = imageData.Where(imageData => imageData.ImageRepoIsSupported(imageRepo));
            return images;
        }

        private static string GetFilterRegexPattern(string filterEnvName)
        {
            string filter = Environment.GetEnvironmentVariable(filterEnvName);
            return Config.GetFilterRegexPattern(filter);
        }
    }
}
