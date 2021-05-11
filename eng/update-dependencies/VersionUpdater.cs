// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

#nullable enable
namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will update the specified version variables within the manifest to align with the
    /// current product version.
    /// </summary>
    public class VersionUpdater : FileRegexUpdater
    {
        private static readonly string[] s_excludedMonikers = { "servicing", "rtm" };
        private static readonly string s_versionGroupName = "versionValue";

        private readonly string _productName;
        private readonly VersionType _versionType;

        public VersionUpdater(VersionType versionType, string productName, string dockerfileVersion, string repoRoot) : base()
        {
            _productName = productName;
            _versionType = versionType;
            string versionVariableName = GetVersionVariableName(versionType, productName, dockerfileVersion);

            Trace.TraceInformation($"Updating {versionVariableName}");

            Path = System.IO.Path.Combine(repoRoot, Program.VersionsFilename);
            VersionGroupName = s_versionGroupName;
            Regex = GetVersionVariableRegex(versionVariableName);
        }

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyBuildInfos, out IEnumerable<IDependencyInfo> usedBuildInfos)
        {
            IDependencyInfo productInfo = dependencyBuildInfos.First(info => info.SimpleName == _productName);

            usedBuildInfos = new IDependencyInfo[] { productInfo };

            return _versionType switch
            {
                VersionType.Build => GetBuildVersion(productInfo),
                VersionType.Product => GetProductVersion(productInfo),
                _ => throw new NotSupportedException($"Unsupported VersionType: {_versionType}"),
            };
        }

        private static string GetBuildVersion(IDependencyInfo productInfo) => productInfo.SimpleVersion ?? string.Empty;

        public static string GetBuildVersion(string productName, string dockerfileVersion, string variables)
        {
            // Special case for handling the lzma NuGet package cache.
            if (productName == "lzma")
            {
                productName = "sdk";
            }

            string versionVariableName = GetVersionVariableName(VersionType.Build, productName, dockerfileVersion);
            Regex regex = GetVersionVariableRegex(versionVariableName);
            Match match = regex.Match(variables);
            if (!match.Success)
            {
                throw new InvalidOperationException($"Unable to retrieve {versionVariableName}");
            }

            return match.Groups[s_versionGroupName].Value;
        }

        private static string GetProductVersion(IDependencyInfo productInfo)
        {
            if (productInfo.SimpleVersion is null)
            {
                return string.Empty;
            }

            // Derive the Docker tag version from the product build version.
            // 5.0.0-preview.2.19530.9 => 5.0.0-preview.2
            string versionRegexPattern = "[\\d]+.[\\d]+.[\\d]+(-[\\w]+(.[\\d]+)?)?";
            Match versionMatch = Regex.Match(productInfo.SimpleVersion, versionRegexPattern);
            string version = versionMatch.Success ? versionMatch.Value : productInfo.SimpleVersion;

            foreach (string excludedMoniker in s_excludedMonikers)
            {
                int monikerIndex = version.IndexOf($"-{excludedMoniker}", StringComparison.OrdinalIgnoreCase);
                if (monikerIndex != -1)
                {
                    version = version.Substring(0, monikerIndex);
                }
            }

            return version;
        }

        private static Regex GetVersionVariableRegex(string versionVariableName) =>
            new Regex($"\"{Regex.Escape(versionVariableName)}\": \"(?<{s_versionGroupName}>[\\d]+.[\\d]+.[\\d]+(-[\\w]+(.[\\d]+)*)?)\"");

        private static string GetVersionVariableName(VersionType versionType, string productName, string dockerfileVersion) =>
            $"{productName}|{dockerfileVersion}|{versionType.ToString().ToLowerInvariant()}-version";
    }
}
#nullable disable
