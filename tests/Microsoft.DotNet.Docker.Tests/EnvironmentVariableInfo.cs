// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Microsoft.DotNet.Docker.Tests
{
    public class EnvironmentVariableInfo
    {
        public bool AllowAnyValue { get; private set; }
        public string ExpectedValue { get; private set; }
        public string Name { get; private set; }
        public bool IsProductVersion { get; set; }

        public EnvironmentVariableInfo(string name, string expectedValue)
        {
            Name = name;
            ExpectedValue = expectedValue;
        }

        public EnvironmentVariableInfo(string name, bool allowAnyValue)
        {
            Name = name;
            AllowAnyValue = allowAnyValue;
        }

        public static void Validate(
            IEnumerable<EnvironmentVariableInfo> expectedVariables,
            string imageName,
            ImageData imageData,
            DockerHelper dockerHelper)
        {
            IDictionary<string, string> environmentVariables = dockerHelper.GetEnvironmentVariables(imageName);

            using (new AssertionScope())
            {
                foreach (EnvironmentVariableInfo variable in expectedVariables)
                {
                    string environmentVariable = environmentVariables.Should()
                        .ContainKey(
                            variable.Name,
                            because: $"{imageName} should have the environment variable '{variable.Name}' defined")
                        .WhoseValue;

                    if (variable.AllowAnyValue)
                    {
                        environmentVariable.Should().NotBeNullOrEmpty(
                            because: $"environment variable {variable.Name} is allowed to have any value");
                    }
                    else
                    {
                        // If we're validating a product version environment variable for a stable build
                        // we need to trim off the "servicing" or "rtm" part of the version value.
                        if (variable.IsProductVersion && Config.IsInternal)
                        {
                            environmentVariable = ImageVersion.TrimBuildVersionForRelease(environmentVariable);
                        }

                        environmentVariable.Should().Be(variable.ExpectedValue,
                            because: $"{imageName} should have the environment variable "
                                + $"'{variable.Name}' set to '{variable.ExpectedValue}'");
                    }
                }
            }

        }
    }
}
