// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

public static class VariableHelper
{
    public const string AspNetVersionName = "aspnet_version";
    public const string AspNetCoreVersionName = "aspnetcore_version";
    public const string DotnetVersionName = "dotnet_version";
    public const string DotnetSdkVersionName = "dotnet_sdk_version";
    public const string MonitorVersionName = "dotnet_monitor_version";
    public const string ValueGroupName = "value";
    public const string VariableGroupName = "variable";

    public static Regex GetValueRegex(params string[] variableNames)
    {
        string variableGroupFormat = $"(?<{VariableGroupName}>({{0}}))";
        string joinedVariableNames = string.Join('|', variableNames);
        string upperCasedVariableGroup = string.Format(variableGroupFormat, joinedVariableNames.ToUpperInvariant());
        string lowerCasedVariableGroup = string.Format(variableGroupFormat, joinedVariableNames.ToLowerInvariant());

        string argEnvPattern = $" {upperCasedVariableGroup}[= ](?<{ValueGroupName}>[^\\s$]+)";
        string linuxPattern = $"\\b{lowerCasedVariableGroup}=(?<{ValueGroupName}>[^\\s]+)";
        string windowsPowerShellPattern = $"\\${lowerCasedVariableGroup} = '(?<{ValueGroupName}>[^\\s]+)'";
        string windowsCmdPattern = $"\\bset \"{lowerCasedVariableGroup}=(?<{ValueGroupName}>[^\\s]+)\"";

        return new Regex($"{argEnvPattern}|{linuxPattern}|{windowsPowerShellPattern}|{windowsCmdPattern}");
    }
}
