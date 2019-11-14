// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace Dotnet.Docker
{
    public static class VariableHelper
    {
        public const string AspNetVersionName = "aspnetcore_version";
        public const string DotnetVersionName = "dotnet_version";
        public const string DotnetSdkVersionName = "dotnet_sdk_version";
        public const string ValueGroupName = "value";
        public const string VariableGroupName = "variable";

        public static Regex GetValueRegex(params string[] variableNames)
        {
            string variableGroupFormat = $"(?<{VariableGroupName}>({{0}}))";
            string joinedVariableNames = string.Join('|', variableNames);
            string upperCasedVariableGroup = string.Format(variableGroupFormat, joinedVariableNames.ToUpperInvariant());
            string lowerCasedVariableGroup = string.Format(variableGroupFormat, joinedVariableNames.ToLowerInvariant());

            string envPattern = $"ENV {upperCasedVariableGroup} (?<{ValueGroupName}>[^\r\n]*)";
            string linuxPattern = $"\\b{lowerCasedVariableGroup}=(?<{ValueGroupName}>[^\\s]*)";
            string windowsPowerShellPattern = $"\\${lowerCasedVariableGroup} = '(?<{ValueGroupName}>[^\\s]*)'";
            string windowsCmdPattern = $"\\bset \"{lowerCasedVariableGroup}=(?<{ValueGroupName}>[^\\s]*)\"";

            return new Regex($"{envPattern}|{linuxPattern}|{windowsPowerShellPattern}|{windowsCmdPattern}");
        }
    }
}
