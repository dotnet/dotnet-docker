// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "monitor")]
    public class MonitorImageTests
    {
        private static readonly string s_samplesPath = Path.Combine(Config.SourceRepoRoot, "samples");

        public MonitorImageTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
            DockerHelper = new DockerHelper(outputHelper);
        }

        protected DockerHelper DockerHelper { get; }

        protected ITestOutputHelper OutputHelper { get; }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetMonitorImageData()
                .Select(imageData => new object[] { imageData });
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(MonitorImageData imageData)
        {
            ValidateEnvironmentVariables(imageData, imageData.GetImage(DockerHelper));
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public Task VerifyMetricsEndpoint(MonitorImageData imageData)
        {
            return VerifyAsync(imageData, async (image, containerName) =>
            {
                try
                {
                    DockerHelper.Run(
                        image: image,
                        name: containerName,
                        detach: true,
                        optionalRunArgs: "-p 52325");

                    if (!Config.IsHttpVerificationDisabled)
                    {
                        // Verify metrics endpoint is accessible
                        await ImageScenarioVerifier.VerifyHttpResponseFromContainerAsync(
                            containerName,
                            DockerHelper,
                            OutputHelper,
                            52325,
                            "metrics");
                    }

                    ValidateEnvironmentVariables(imageData, image);
                }
                finally
                {
                    DockerHelper.DeleteContainer(containerName);
                }
            });
        }

        private async Task VerifyAsync(
            MonitorImageData imageData,
            Func<string, string, Task> verifyImageAsync)
        {
            string image = imageData.GetImage(DockerHelper);

            string containerName = imageData.GetIdentifier("monitor");

            await verifyImageAsync(image, containerName);
        }

        private void ValidateEnvironmentVariables(MonitorImageData imageData, string image)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(ProductImageTests.GetCommonEnvironmentVariables());

            // ASPNETCORE_URLS has been unset to allow the default URL binding to occur.
            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", string.Empty));
            // Diagnostics should be disabled
            variables.Add(new EnvironmentVariableInfo("COMPlus_EnableDiagnostics", "0"));

            EnvironmentVariableInfo.Validate(
                variables,
                image,
                imageData,
                DockerHelper);
        }
    }
}
