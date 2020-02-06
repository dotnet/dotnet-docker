// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Xunit;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SkippableFactAttribute : FactAttribute
    {
        public SkippableFactAttribute(string requiredVersion)
        {
            string versionFilterPattern =
                Config.VersionFilter != null ? Config.GetFilterRegexPattern(Config.VersionFilter) : null;
            if (!Regex.IsMatch(requiredVersion, versionFilterPattern, RegexOptions.IgnoreCase))
            {
                Skip = $"Skipping test because it requires .NET Core {requiredVersion}";
            }
        }
    }
}
