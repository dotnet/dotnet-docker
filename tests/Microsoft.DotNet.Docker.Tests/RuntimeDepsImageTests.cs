// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class RuntimeDepsImageTests
    {
        private readonly DockerHelper _dockerHelper;
        private readonly ITestOutputHelper _outputHelper;

        public RuntimeDepsImageTests(ITestOutputHelper outputHelper)
        {
            _dockerHelper = new DockerHelper(outputHelper);
            _outputHelper = outputHelper;
        }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetImageData()
                .Distinct(new DefaultImageDataEqualityComparer())
                .Select(imageData => new object[] { imageData });
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(ImageData imageData)
        {
            EnvironmentVariableInfo.VerifyCommonRuntimeEnvironmentVariables(DotNetImageType.Runtime_Deps, imageData, _dockerHelper);
        }
    }
}
