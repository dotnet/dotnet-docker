// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

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
            IEnumerable<EnvironmentVariableInfo> variables,
            string imageName,
            ImageData imageData,
            DockerHelper dockerHelper)
        {
            IDictionary<string, string> actualValues = dockerHelper.GetEnvironmentVariables(imageName);

            foreach (EnvironmentVariableInfo variable in variables)
            {
                bool isFound = actualValues.TryGetValue(variable.Name, out string actualValue);
                Assert.True(isFound, $"Variable '{variable.Name}' is not defined in image '{imageName}'.");

                if (variable.AllowAnyValue)
                {
                    Assert.NotEmpty(actualValue);
                }
                else
                {
                    // If we're validating a product version environment variable for an internal build
                    // we need to trim off the "servicing" or "rtm" part of the version value.
                    if (variable.IsProductVersion && !string.IsNullOrEmpty(Config.SasQueryString))
                    {
                        int servicingIndex = actualValue.IndexOf("-servicing.");
                        if (servicingIndex != -1)
                        {
                            actualValue = actualValue.Substring(0, servicingIndex);
                        }
                        else
                        {
                            int rtmIndex = actualValue.IndexOf("-rtm.");
                            if (rtmIndex != -1)
                            {
                                actualValue = actualValue.Substring(0, rtmIndex);
                            }
                        }
                    }

                    Assert.Equal(variable.ExpectedValue, actualValue);
                }
            }
        }
    }
}
