// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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

        public EnvironmentVariableInfo(string name, string expectedValue)
        {
            this.Name = name;
            this.ExpectedValue = expectedValue;
        }

        public EnvironmentVariableInfo(string name, bool allowAnyValue)
        {
            this.Name = name;
            this.AllowAnyValue = allowAnyValue;
        }

        public static void Validate(
            IEnumerable<EnvironmentVariableInfo> variables,
            DotNetImageType imageType,
            ImageData imageData,
            DockerHelper dockerHelper)
        {
            const char delimiter = '|';
            IEnumerable<string> echoParts;
            string invokeCommand;
            char delimiterEscape;

            if (DockerHelper.IsLinuxContainerModeEnabled)
            {
                echoParts = variables.Select(envVar => $"${envVar.Name}");
                invokeCommand = $"/bin/sh -c";
                delimiterEscape = '\\';
            }
            else
            {
                echoParts = variables.Select(envVar => $"%{envVar.Name}%");
                invokeCommand = $"CMD /S /C";
                delimiterEscape = '^';
            }

            string combinedValues = dockerHelper.Run(
                image: imageData.GetImage(imageType, dockerHelper),
                name: imageData.GetIdentifier($"env"),
                command: $"{invokeCommand} \"echo {String.Join($"{delimiterEscape}{delimiter}", echoParts)}\"");

            string[] values = combinedValues.Split(delimiter);
            Assert.Equal(variables.Count(), values.Count());

            for (int i = 0; i < values.Count(); i++)
            {
                EnvironmentVariableInfo variable = variables.ElementAt(i);

                string actualValue;
                // Process unset variables in Windows
                if (!DockerHelper.IsLinuxContainerModeEnabled
                    && string.Equals(values[i], $"%{variable.Name}%", StringComparison.Ordinal))
                {
                    actualValue = string.Empty;
                }
                else
                {
                    actualValue = values[i];
                }

                if (variable.AllowAnyValue)
                {
                    Assert.NotEmpty(actualValue);
                }
                else
                {
                    Assert.Equal(variable.ExpectedValue, actualValue);
                }
            }
        }
    }
}
