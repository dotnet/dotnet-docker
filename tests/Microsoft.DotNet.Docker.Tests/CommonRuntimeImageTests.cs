// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetImageData()
                .Select(imageData => new object[] { imageData });
        }

        protected void VerifyCommonEnvironmentVariables(
            ProductImageData imageData, IEnumerable<EnvironmentVariableInfo> customVariables = null)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(GetCommonEnvironmentVariables());
            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", "http://+:80"));

            if (customVariables != null)
            {
                variables.AddRange(customVariables);
            }

            if (imageData.OS.StartsWith(OS.AlpinePrefix))
            {
                variables.Add(new EnvironmentVariableInfo("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "true"));
            }

            EnvironmentVariableInfo.Validate(variables, imageData.GetImage(ImageType, DockerHelper), imageData, DockerHelper);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }
    }
}
