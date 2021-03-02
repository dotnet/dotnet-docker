// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SkippableTheoryAttribute : DotNetTheoryAttribute
    {
        public SkippableTheoryAttribute(params string[] skippedOperatingSystems)
        {
            if (!string.IsNullOrEmpty(Config.Os) && Config.Os != "*")
            {
                string osPattern =
                    Config.Os != null ? Config.GetFilterRegexPattern(Config.Os) : null;
                foreach (string skippedOperatingSystem in skippedOperatingSystems)
                {
                    if (Regex.IsMatch(skippedOperatingSystem, osPattern, RegexOptions.IgnoreCase))
                    {
                        Skip = $"{skippedOperatingSystem} is unsupported";
                        break;
                    }
                }
            }
        }
    }
}
