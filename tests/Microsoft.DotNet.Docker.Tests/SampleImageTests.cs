// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void VerifyDotnetSample(SampleImageData imageData)
        {
            string image = imageData.GetImage(SampleImageType.Dotnetapp, DockerHelper);
            string containerName = imageData.GetIdentifier("sample-dotnetapp");
            string output = DockerHelper.Run(image, containerName);
            Assert.StartsWith("Hello from .NET Core!", output);
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public async Task VerifyAspnetSample(SampleImageData imageData)
        {
            string image = imageData.GetImage(SampleImageType.Aspnetapp, DockerHelper);
            string containerName = imageData.GetIdentifier("sample-aspnetapp");

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
        }
    }
}
