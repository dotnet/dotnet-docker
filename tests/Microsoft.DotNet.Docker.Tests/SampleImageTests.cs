// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "sample")]
    public class SampleImageTests
    {
        public SampleImageTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
            DockerHelper = new DockerHelper(outputHelper);
        }

        protected DockerHelper DockerHelper { get; }

        protected ITestOutputHelper OutputHelper { get; }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetSampleImageData()
                .Select(imageData => new object[] { imageData });
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyDotnetSample(SampleImageData imageData)
        {
            await VerifySampleAsync(imageData, SampleImageType.Dotnetapp, (image, containerName) =>
            {
                string output = DockerHelper.Run(image, containerName);
                Assert.StartsWith("Hello from .NET Core!", output);
                return Task.CompletedTask;
            });
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAspnetSample(SampleImageData imageData)
        {
            if (imageData.OS == OS.Bionic && imageData.Dockerfile != "Dockerfile.ubuntu-x64")
            {
                return;
            }

            await VerifySampleAsync(imageData, SampleImageType.Aspnetapp, async (image, containerName) =>
            {
                try
                {
                    DockerHelper.Run(
                        image: image,
                        name: containerName,
                        detach: true,
                        optionalRunArgs: "-p 80");

                    if (!Config.IsHttpVerificationDisabled)
                    {
                        await ImageScenarioVerifier.VerifyHttpResponseFromContainerAsync(containerName, DockerHelper, OutputHelper);
                    }
                }
                finally
                {
                    DockerHelper.DeleteContainer(containerName);
                }
            });
        }

        [Fact]
        public void VerifyComplexAppSample()
        {
            string appTag = ImageData.GetImageName("complexapp-local-app", "sample");
            string testTag = ImageData.GetImageName("complexapp-local-test", "sample");
            string sampleFolder = $"{Config.SamplesPath}/complexapp";
            string dockerfilePath = $"{sampleFolder}/Dockerfile";
            string containerName = ImageData.GenerateContainerName("sample-complex");
            string tempDir = null;
            try
            {
                DockerHelper.Build(appTag, dockerfilePath, contextDir: sampleFolder, pull: Config.PullImages);
                DockerHelper.Build(testTag, dockerfilePath, target: "test", contextDir: sampleFolder);

                DockerHelper.Run(testTag, containerName, skipAutoCleanup: true);

                tempDir = Directory.CreateDirectory(
                    Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())).FullName;
                DockerHelper.Copy($"{containerName}:/source/tests/TestResults", tempDir);
                string testLogFile = new DirectoryInfo($"{tempDir}/TestResults").GetFiles("*.trx").First().FullName;

                // Open the test log file and verify the tests passed
                XDocument doc = XDocument.Load(testLogFile);
                var summary = doc.Root.Element(XName.Get("ResultSummary", doc.Root.Name.NamespaceName));
                Assert.Equal("Completed", summary.Attribute("outcome").Value);
                var counters = summary.Element(XName.Get("Counters", doc.Root.Name.NamespaceName));
                Assert.Equal("2", counters.Attribute("total").Value);
                Assert.Equal("2", counters.Attribute("passed").Value);
            }
            finally
            {
                if (tempDir != null)
                {
                    Directory.Delete(tempDir, true);
                }

                DockerHelper.DeleteContainer(containerName);
                DockerHelper.DeleteImage(testTag);
                DockerHelper.DeleteImage(appTag);
            }
        }

        private async Task VerifySampleAsync(
            SampleImageData imageData,
            SampleImageType sampleImageType,
            Func<string, string, Task> verifyImageAsync)
        {
            string image = imageData.GetImage(sampleImageType, DockerHelper);
            string imageType = Enum.GetName(typeof(SampleImageType), sampleImageType).ToLowerInvariant();
            try
            {
                if (imageData.LocalOnly)
                {
                    string sampleFolder = $"{Config.SamplesPath}/{imageType}";
                    string dockerfilePath = $"{sampleFolder}/{imageData.Dockerfile}";

                    DockerHelper.Build(image, dockerfilePath, contextDir: sampleFolder, pull: Config.PullImages);
                }

                string containerName = imageData.GetIdentifier($"sample-{imageType}");
                await verifyImageAsync(image, containerName);
            }
            finally
            {
                if (imageData.LocalOnly)
                {
                    DockerHelper.DeleteImage(image);
                }
            }
        }
    }
}
