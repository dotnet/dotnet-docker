// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public abstract class CommonRuntimeImageTests : ProductImageTests
    {
        protected CommonRuntimeImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> GetImageData(DotNetImageRepo imageRepo)
        {
            return TestData.GetImageData(imageRepo)
                .Select(imageData => new object[] { imageData });
        }

        /// <summary>
        /// Verifies that the packages installed are correct and scannable by security tools.
        /// </summary>
        protected void VerifyInstalledPackages(ProductImageData imageData)
        {
            IEnumerable<string> basePackages = imageData.IsDistroless
                ? GetDistrolessBasePackages(imageData).Concat(GetRuntimeDepsPackages(imageData))
                : GetRuntimeDepsPackages(imageData);

            IEnumerable<string> expectedPackages = imageData.ImageVariant.HasFlag(DotNetImageVariant.Extra)
                ? basePackages.Concat(GetExtraPackages(imageData))
                : basePackages;

            expectedPackages = expectedPackages.Distinct().OrderBy(s => s);

            IEnumerable<string> actualPackages = GetInstalledPackages(imageData);

            OutputHelper.WriteLine($"Expected Packages: [ {string.Join(", ", expectedPackages)} ]");

            if (imageData.IsDistroless)
            {
                OutputHelper.WriteLine($"Actual Packages: [ {string.Join(", ", actualPackages)} ]");
                Assert.Equal(expectedPackages, actualPackages);
            }
            else
            {
                IEnumerable<string> missingPackages = expectedPackages.Except(actualPackages);
                if (missingPackages.Count() > 0)
                {
                    OutputHelper.WriteLine($"Missing packages: [ {string.Join(", ", missingPackages)} ]");
                }
                Assert.Empty(missingPackages);
            }
        }

        protected void VerifyCommonEnvironmentVariables(
            ProductImageData imageData, IEnumerable<EnvironmentVariableInfo> customVariables = null)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(GetCommonEnvironmentVariables());

            if (!imageData.IsWindows && imageData.Version.Major != 6 && imageData.Version.Major != 7)
            {
                variables.Add(new EnvironmentVariableInfo("APP_UID", imageData.NonRootUID?.ToString()));
            }

            if (imageData.VersionFamily.Major <= 7)
            {
                variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", $"http://+:{imageData.DefaultPort}"));
            }
            else
            {
                variables.Add(new EnvironmentVariableInfo("ASPNETCORE_HTTP_PORTS", imageData.DefaultPort.ToString()));
            }

            if (customVariables != null)
            {
                variables.AddRange(customVariables);
            }

            if (imageData.globalizationInvariantMode)
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "true"));
            }

            string imageTag = imageData.GetImage(ImageRepo, DockerHelper);

            EnvironmentVariableInfo.Validate(variables, imageTag, imageData, DockerHelper);
        }

        protected void VerifyCommonShellNotInstalledForDistroless(ProductImageData imageData)
        {
            if (!imageData.IsDistroless)
            {
                OutputHelper.WriteLine("Skipping test for non-distroless platform.");
                return;
            }

            if (imageData.OS == OS.Mariner20Distroless)
            {
                OutputHelper.WriteLine("Temporarily disable due to bash being installed. See https://github.com/dotnet/dotnet-docker/issues/3526");
                return;
            }

            string imageTag = imageData.GetImage(ImageRepo, DockerHelper);

            // Attempting to execute the container's shell should result in an exception.
            // There should be no shell installed in distroless containers.
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() =>
                DockerHelper.Run(
                        image: imageTag,
                        name: imageData.GetIdentifier($"env"),
                        optionalRunArgs: $"--entrypoint /bin/sh")
                );

            Assert.Contains("Exit code: 127", ex.Message);
        }

        protected IEnumerable<string> GetInstalledPackages(ProductImageData imageData)
        {
            JsonNode output = GetSyftOutput("package-info", imageData);
            return ((JsonArray)output["artifacts"])
                .Select(artifact => artifact["name"]?.ToString());
        }

        protected JsonNode GetSyftOutput(string name, ProductImageData imageData)
        {
            const string SyftImage = "anchore/syft:v0.87.1";
            DockerHelper.Pull(SyftImage);

            string imageName = imageData.GetImage(ImageRepo, DockerHelper);
            string output = DockerHelper.Run(
                SyftImage, name, $"packages docker:{imageName} -o json",
                useMountedDockerSocket: true);

            return JsonNode.Parse(output)
                    ?? throw new JsonException($"Unable to parse the output as JSON:{Environment.NewLine}{output}");
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
    }
}
