// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.DotNet.Docker.Tests.TestScenarios;

namespace Microsoft.DotNet.Docker.Tests
{
    public abstract class CommonRuntimeImageTests : ProductImageTests
    {
        protected CommonRuntimeImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> GetImageData(
            DotNetImageRepo imageRepo,
            DotNetImageVariant variant = DotNetImageVariant.None)
        {
            return TestData.GetImageData(imageRepo, variant)
                .Select(imageData => new object[] { imageData });
        }

        protected void VerifyCommonEnvironmentVariables(
            ProductImageData imageData, IEnumerable<EnvironmentVariableInfo> customVariables = null)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(GetCommonEnvironmentVariables());

            if (!imageData.IsWindows)
            {
                variables.Add(new EnvironmentVariableInfo("APP_UID", imageData.NonRootUID?.ToString()));
            }

            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_HTTP_PORTS", imageData.DefaultPort.ToString()));

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

        protected async Task VerifyGlobalizationScenarioBase(ProductImageData imageData)
        {
            using var testScenario = new GlobalizationScenario(imageData, ImageRepo, DockerHelper);
            await testScenario.ExecuteAsync();
        }

        protected async Task VerifyNlsScenarioBase(ProductImageData imageData)
        {
            using var testScenario = new NlsScenario(imageData, ImageRepo, DockerHelper);
            await testScenario.ExecuteAsync();
        }
    }
}
