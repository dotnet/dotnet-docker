// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
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

            if (imageData.GlobalizationInvariantMode)
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
    }
}
