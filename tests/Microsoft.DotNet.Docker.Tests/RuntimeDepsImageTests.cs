// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "runtime-deps")]
    public class RuntimeDepsImageTests : CommonRuntimeImageTests
    {
        public RuntimeDepsImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Runtime_Deps;

        public static IEnumerable<object[]> GetImageData() => GetImageData(DotNetImageRepo.Runtime_Deps);

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            base.VerifyCommonEnvironmentVariables(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyPackageInstallation(ProductImageData imageData)
        {
            if (!imageData.OS.Contains("cbl-mariner") || imageData.IsDistroless || imageData.Version.Major > 6)
            {
                return;
            }

            VerifyExpectedInstalledRpmPackages(
                imageData,
                GetExpectedRpmPackagesInstalled(imageData));
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyShellNotInstalledForDistroless(ProductImageData imageData)
        {
            base.VerifyCommonShellNotInstalledForDistroless(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyNoSasToken(ProductImageData imageData)
        {
            base.VerifyCommonNoSasToken(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDefaultUser(ProductImageData imageData)
        {
            VerifyCommonDefaultUser(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyDistrolessOSReleaseInfo(ProductImageData imageData)
        {
            if (!imageData.IsDistroless)
            {
                return;
            }
            Assert.NotEmpty(GetOSReleaseInfo(imageData));
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyRuntimeDepsPackages(ProductImageData imageData)
        {
            base.VerifyInstalledPackages(imageData, ImageRepo);
        }

        private string GetOSReleaseInfo(ProductImageData imageData)
        {
            JsonNode output = GetSyftOutput("os-release-info", imageData);
            JsonObject distro = (JsonObject)output["distro"];
            return (string)distro["version"];
        }

        internal static IEnumerable<string> GetDistrolessBasePackages(ProductImageData imageData) => imageData switch
            {
                { OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "distroless-packages-minimal",
                        "filesystem",
                        "mariner-release",
                        "prebuilt-ca-certificates",
                        "tzdata"
                    },
                { OS: OS.JammyChiseled } => new[]
                    {
                        "base-files"
                    },
                _ => throw new NotSupportedException()
            };

        internal static IEnumerable<string> GetRuntimeDepsPackages(ProductImageData imageData) => imageData switch
            {
                { OS: OS.Mariner20Distroless, Version: ImageVersion version }
                        when version.Major == 6 || version.Major == 7 => new[]
                    {
                        "e2fsprogs-libs",
                        "glibc",
                        "krb5",
                        "libgcc",
                        "libstdc++",
                        "openssl",
                        "openssl-libs",
                        "prebuilt-ca-certificates",
                        "zlib"
                    },
                { OS: OS.Mariner20, Version: ImageVersion version }
                        when version.Major == 6 || version.Major == 7 => new[]
                    {
                        "glibc",
                        "icu",
                        "krb5",
                        "libgcc",
                        "libstdc++",
                        "openssl-libs",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "glibc",
                        "libgcc",
                        "libstdc++",
                        "openssl-libs",
                        "zlib"
                    },
                { OS: string os } when os.Contains(OS.Jammy) => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libssl3",
                        "libstdc++6",
                        "zlib1g"
                    },
                { OS: OS.Focal } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1",
                        "libgssapi-krb5-2",
                        "libicu66",
                        "libssl1.1",
                        "libstdc++6",
                        "zlib1g"
                    },
                { OS: OS.Alpine318 } => new[]
                    {
                        "ca-certificates-bundle",
                        "libgcc",
                        "libssl3",
                        "libstdc++",
                        "zlib"
                    },
                { OS: OS.BookwormSlim } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1", 
                        "libicu72",
                        "libssl3",
                        "libstdc++6",
                        "tzdata",
                        "zlib1g"
                    },
                { OS: OS.BullseyeSlim } => new[]
                    {
                        "ca-certificates",
                        "libc6",
                        "libgcc-s1", // Listed as libgcc1 in the Dockerfile
                        "libgssapi-krb5-2",
                        "libicu67",
                        "libssl1.1",
                        "libstdc++6",
                        "zlib1g"
                    },
                _ => throw new NotSupportedException()
            };

        internal static IEnumerable<string> GetExtraPackages(ProductImageData imageData) => imageData switch
            {
                { IsDistroless: true, OS: string os } when os.Contains(OS.Mariner) => new[]
                    {
                        "icu",
                        "tzdata"
                    },
                { OS: OS.JammyChiseled } => new[]
                    {
                        "libicu70",
                        "tzdata"
                    },
                { OS: string os } when os.Contains(OS.Alpine) => new[]
                    {
                        "icu-data-full",
                        "icu-libs",
                        "tzdata"
                    },
                _ => new string[0]
            };

        internal static string[] GetExpectedRpmPackagesInstalled(ProductImageData imageData) =>
            new string[]
                {
                    $"dotnet-runtime-deps-{imageData.VersionString}"
                };
    }
}
