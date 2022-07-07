// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class SkippableTheoryAttribute : DotNetTheoryAttribute
    {
        public SkippableTheoryAttribute(params string[] skippedOperatingSystems)
        {
            if (Config.OsNames.Any() && !Config.OsNames.Any(osName => osName != "*"))
            {
                foreach (string osName in Config.OsNames)
                {
                    string osPattern =
                        osName != null ? Config.GetFilterRegexPattern(osName) : null;
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
}
