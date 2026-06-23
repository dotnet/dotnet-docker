// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;

#nullable enable
namespace Microsoft.DotNet.Docker.Tests
{
    public record EnvironmentVariableInfo
    {
        public bool AllowAnyValue { get; init; } = false;
        public string? ExpectedValue { get; init; } = null;
        public string Name { get; init; }
        public bool IsProductVersion { get; init; } = false;

        /// <summary>
        /// When true, the variable is expected to NOT be set on the image.
        /// </summary>
        public bool ShouldNotExist { get; init; } = false;

        private EnvironmentVariableInfo(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Requires the named environment variable to be set to <paramref name="expectedValue"/>.
        /// </summary>
        public static EnvironmentVariableInfo Require(string name, string expectedValue) =>
            new(name) { ExpectedValue = expectedValue };

        /// <summary>
        /// Requires the named environment variable to be set to any non-empty value.
        /// </summary>
        public static EnvironmentVariableInfo Require(string name) =>
            new(name) { AllowAnyValue = true };

        /// <summary>
        /// Requires the named environment variable to NOT be set on the image.
        /// </summary>
        public static EnvironmentVariableInfo Forbid(string name) =>
            new(name) { ShouldNotExist = true };

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
                    if (variable.ShouldNotExist)
                    {
                        environmentVariables.Should().NotContainKey(
                            variable.Name,
                            because: $"{imageName} should not have the environment variable '{variable.Name}' defined");
                        continue;
                    }

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
