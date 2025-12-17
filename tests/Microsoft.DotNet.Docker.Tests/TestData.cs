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
            new ProductImageData { Version = V8_0, OS = OS.Noble,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Amd64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Amd64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Amd64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Amd64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30 },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Amd64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },

            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30 },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20 },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Mariner20Distroless, Arch = Arch.Arm64,   SdkOS = OS.Mariner20,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.BookwormSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Jammy,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.JammyChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Jammy,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Noble,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Arm64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Arm64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Arm64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Arm64,   SdkOS = OS.Alpine323,
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
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Arm },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Arm,     SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine322,           Arch = Arch.Arm,     SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Arm },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Arm,     SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V8_0, OS = OS.Alpine323,           Arch = Arch.Arm,     SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },


            new ProductImageData { Version = V9_0, OS = OS.BookwormSlim,        Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.Noble,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Amd64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Amd64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Amd64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Amd64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30 },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },

            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30 },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.BookwormSlim,        Arch = Arch.Arm64 },
            new ProductImageData { Version = V9_0, OS = OS.Noble,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Arm64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Arm64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Arm64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Arm64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },

            new ProductImageData { Version = V9_0, OS = OS.BookwormSlim,        Arch = Arch.Arm },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Arm },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Arm,     SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine322,           Arch = Arch.Arm,     SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Arm },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Arm,     SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.Alpine323,           Arch = Arch.Arm,     SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V9_0, OS = OS.Noble,               Arch = Arch.Arm },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V9_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },


            new ProductImageData { Version = V10_0, OS = OS.Noble,               Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Amd64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Amd64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Amd64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Amd64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Amd64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30,           Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30 },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Amd64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },

            new() { Version = V10_0, Arch = Arch.Amd64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.Alpine322 },
            new() { Version = V10_0, Arch = Arch.Amd64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.Alpine323 },
            new() { Version = V10_0, Arch = Arch.Amd64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.AzureLinux30Distroless, SdkOS = OS.AzureLinux30 },
            new() { Version = V10_0, Arch = Arch.Amd64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.NobleChiseled, SdkOS = OS.Noble },

            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30 },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.AzureLinux30Distroless, Arch = Arch.Arm64,   SdkOS = OS.AzureLinux30,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Noble,               Arch = Arch.Arm64 },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm64,   SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Arm64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Arm64,   SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Arm64 },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Arm64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Arm64,   SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },

            new() { Version = V10_0, Arch = Arch.Arm64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.Alpine322 },
            new() { Version = V10_0, Arch = Arch.Arm64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.Alpine323 },
            new() { Version = V10_0, Arch = Arch.Arm64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.AzureLinux30Distroless, SdkOS = OS.AzureLinux30 },
            new() { Version = V10_0, Arch = Arch.Arm64, SdkImageVariant = DotNetImageVariant.AOT, SupportedImageRepos = DotNetImageRepo.Runtime_Deps, OS = OS.NobleChiseled, SdkOS = OS.Noble },

            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Arm },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Arm,     SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine322,           Arch = Arch.Arm,     SdkOS = OS.Alpine322,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Arm },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Arm,     SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.Alpine323,           Arch = Arch.Arm,     SdkOS = OS.Alpine323,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps },
            new ProductImageData { Version = V10_0, OS = OS.Noble,               Arch = Arch.Arm },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Composite | DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Aspnet },
            new ProductImageData { Version = V10_0, OS = OS.NobleChiseled,       Arch = Arch.Arm,     SdkOS = OS.Noble,
                    ImageVariant = DotNetImageVariant.Extra, SupportedImageRepos = DotNetImageRepo.Runtime_Deps | DotNetImageRepo.Runtime | DotNetImageRepo.Aspnet },
        };

        private static readonly ProductImageData[] s_windowsTestData =
        {
            new ProductImageData { Version = V8_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V8_0, OS = OS.ServerCoreLtsc2025, Arch = Arch.Amd64 },

            new ProductImageData { Version = V9_0, OS = OS.NanoServer1809,     Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.ServerCoreLtsc2019, Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V9_0, OS = OS.ServerCoreLtsc2025, Arch = Arch.Amd64 },

            new ProductImageData { Version = V10_0, OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.ServerCoreLtsc2022, Arch = Arch.Amd64 },
            new ProductImageData { Version = V10_0, OS = OS.ServerCoreLtsc2025, Arch = Arch.Amd64 },
        };

        private static readonly SampleImageData[] s_linuxSampleTestData =
        {
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Amd64, DockerfileSuffix = "alpine",   IsPublished = true },
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Arm,   DockerfileSuffix = "alpine",   IsPublished = true },
            new SampleImageData { OS = OS.Alpine,           Arch = Arch.Arm64, DockerfileSuffix = "alpine",   IsPublished = true },
            new SampleImageData { OS = OS.JammyChiseled,    Arch = Arch.Arm,   DockerfileSuffix = "chiseled", IsPublished = true },
            new SampleImageData { OS = OS.JammyChiseled,    Arch = Arch.Arm64, DockerfileSuffix = "chiseled", IsPublished = true },
            new SampleImageData { OS = OS.JammyChiseled,    Arch = Arch.Amd64, DockerfileSuffix = "chiseled", IsPublished = true },

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
            new SampleImageData { OS = OS.JammyChiseled,    Arch = Arch.Arm,   DockerfileSuffix = "chiseled" },
            new SampleImageData { OS = OS.JammyChiseled,    Arch = Arch.Arm64, DockerfileSuffix = "chiseled" },
            new SampleImageData { OS = OS.JammyChiseled,    Arch = Arch.Amd64, DockerfileSuffix = "chiseled" },
        };

        private static readonly SampleImageData[] s_windowsSampleTestData =
        {
            new SampleImageData { OS = OS.NanoServer1809,     Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, IsPublished = true },
            new SampleImageData { OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64, IsPublished = true },

            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver" },
            new SampleImageData { OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64, DockerfileSuffix = "nanoserver" },

            // Use Nano Server as the OS even though the Dockerfiles are for Windows Server Core. This is because the OS value
            // needs to match the filter set by the build/test job. We only produce builds jobs based on what's in the manifest
            // and the manifest only defines Nano Server-based Dockerfiles. So we just need to piggyback on the Nano Server
            // jobs in order to test the Windows Server Core samples.
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore" },
            new SampleImageData { OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore" },
            new SampleImageData { OS = OS.NanoServerLtsc2022, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-iis" },
            new SampleImageData { OS = OS.NanoServerLtsc2025, Arch = Arch.Amd64, DockerfileSuffix = "windowsservercore-iis" },
        };

        private static readonly ProductImageData[] s_linuxMonitorTestData =
        [
            new ProductImageData { Version = V8_1, VersionFamily = V8_0, OS = OS.JammyChiseled,          OSTag = OS.UbuntuChiseled,    Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            new ProductImageData { Version = V8_1, VersionFamily = V8_0, OS = OS.JammyChiseled,          OSTag = OS.UbuntuChiseled,    Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            // OSTag does not correspond to OS because platform tags for Azure Linux were not added to the images
            // Use CBL-Mariner distroless for OSTag since those platform tags exist and won't require tests to understand the difference in tagging.
            new ProductImageData { Version = V8_1, VersionFamily = V8_0, OS = OS.AzureLinux30Distroless, OSDir = OS.AzureLinuxDistroless, OSTag = OS.MarinerDistroless, Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            new ProductImageData { Version = V8_1, VersionFamily = V8_0, OS = OS.AzureLinux30Distroless, OSDir = OS.AzureLinuxDistroless, OSTag = OS.MarinerDistroless, Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            new ProductImageData { Version = V9_0, VersionFamily = V9_0, OS = OS.AzureLinux30Distroless, OSDir = OS.AzureLinuxDistroless, OSTag = "", Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            new ProductImageData { Version = V9_0, VersionFamily = V9_0, OS = OS.AzureLinux30Distroless, OSDir = OS.AzureLinuxDistroless, OSTag = "", Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            new ProductImageData { Version = V10_0, VersionFamily = V10_0, OS = OS.AzureLinux30Distroless, OSDir = OS.AzureLinuxDistroless, OSTag = "", Arch = Arch.Amd64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
            new ProductImageData { Version = V10_0, VersionFamily = V10_0, OS = OS.AzureLinux30Distroless, OSDir = OS.AzureLinuxDistroless, OSTag = "", Arch = Arch.Arm64,  SupportedImageRepos = DotNetImageRepo.Monitor | DotNetImageRepo.Monitor_Base },
        ];

        private static readonly ProductImageData[] s_windowsMonitorTestData =
        {
        };

        private static readonly ProductImageData[] s_AspireDashboardTestData =
        {
            new() {
                Version = V13_1,
                VersionFamily = V9_0,
                OS = OS.AzureLinux30Distroless,
                OSTag = "",
                OSDir = OS.AzureLinuxDistroless,
                Arch = Arch.Amd64,
                SupportedImageRepos = DotNetImageRepo.Aspire_Dashboard,
            },
            new() {
                Version = V13_1,
                VersionFamily = V9_0,
                OS = OS.AzureLinux30Distroless,
                OSTag = "",
                OSDir = OS.AzureLinuxDistroless,
                Arch = Arch.Arm64,
                SupportedImageRepos = DotNetImageRepo.Aspire_Dashboard
            },
        };

        private static readonly ProductImageData[] s_YarpTestData =
        [
        ];

        public static IEnumerable<ProductImageData> AllImageData =>
        [
            ..s_linuxTestData,
            ..s_windowsTestData,
            ..s_AspireDashboardTestData,
            ..s_linuxMonitorTestData,
            ..s_windowsMonitorTestData,
            ..s_YarpTestData,
        ];

        public static IEnumerable<ProductImageData> GetImageData(
            DotNetImageRepo imageRepo,
            DotNetImageVariant variant = DotNetImageVariant.None)
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxTestData : s_windowsTestData)
                .FilterImagesBySupportedRepo(imageRepo)
                .FilterImagesByVariant(variant)
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

        public static IEnumerable<ProductImageData> GetAspireDashboardImageData()
        {
            if (!DockerHelper.IsLinuxContainerModeEnabled)
            {
                return [];
            }

            return s_AspireDashboardTestData
                .FilterImagesByPath(DotNetImageRepo.Aspire_Dashboard)
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<ProductImageData>();
        }

        public static IEnumerable<ProductImageData> GetMonitorImageData()
        {
            return (DockerHelper.IsLinuxContainerModeEnabled ? s_linuxMonitorTestData : s_windowsMonitorTestData)
                .FilterImagesByPath(DotNetImageRepo.Monitor)
                .FilterImagesByArch()
                .FilterImagesByOs()
                .Cast<ProductImageData>();
        }

        public static IEnumerable<ProductImageData> GetYarpImageData()
        {
            if (!DockerHelper.IsLinuxContainerModeEnabled)
            {
                return [];
            }

            return s_YarpTestData
                .FilterImagesByPath(DotNetImageRepo.Yarp)
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
                .Select(Config.GetFilterRegexPattern);

            return imageData
                .Where(imageData => !osFilterPatterns.Any()
                    || osFilterPatterns.Any(osFilterPattern => Regex.IsMatch(imageData.OS, osFilterPattern, RegexOptions.IgnoreCase)));
        }

        public static IEnumerable<ImageData> FilterImagesByPath(this IEnumerable<ProductImageData> imageData, DotNetImageRepo imageRepo)
        {
            IEnumerable<string> pathPatterns = Config.Paths
                .Select(Config.GetFilterRegexPattern);

            IEnumerable<ProductImageData> filteredImageData = imageData
                .Where(imageData =>
                {
                    return !pathPatterns.Any() || pathPatterns.Any(pathPattern =>
                        {
                            string dockerfilePath = imageData.GetDockerfilePath(imageRepo);
                            return Regex.IsMatch(dockerfilePath, pathPattern, RegexOptions.IgnoreCase);
                        });
                });

            return filteredImageData;
        }

        public static IEnumerable<ProductImageData> FilterImagesBySupportedRepo(
            this IEnumerable<ProductImageData> imageData,
            DotNetImageRepo imageRepo)
        {
            IEnumerable<ProductImageData> images = imageData.Where(imageData =>
                imageData.ImageRepoIsSupported(imageRepo));
            return images;
        }

        public static IEnumerable<ProductImageData> FilterImagesByVariant(
            this IEnumerable<ProductImageData> imageData,
            DotNetImageVariant variant)
        {
            IEnumerable<ProductImageData> images = imageData.Where(imageData =>
                imageData.ImageVariant.HasFlag(variant));
            return images;
        }

        private static string GetFilterRegexPattern(string filterEnvName)
        {
            string filter = Environment.GetEnvironmentVariable(filterEnvName);
            return Config.GetFilterRegexPattern(filter);
        }
    }
}
