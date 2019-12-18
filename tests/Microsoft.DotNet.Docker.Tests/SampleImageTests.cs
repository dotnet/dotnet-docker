// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SampleImageTests : ImageTests
    {
        public SampleImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetSampleImageData()
                .Select(imageData => new object[] { imageData });
        }

        [Theory]
        [MemberData(nameof(GetImageData))]
        public void VerifyConsoleSampleImage(SampleImageData imageData)
        {
            string image = imageData.GetImage(SampleImageType.Console, DockerHelper);
            string containerName = imageData.GetIdentifier("sample-dotnetapp");
            string output = DockerHelper.Run(image, containerName);
            Assert.StartsWith("Hello from .NET Core!", output);
        }
    }
}
