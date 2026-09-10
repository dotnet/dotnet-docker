// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will update the specified version variables within the manifest to align with the
    /// current product version.
    /// </summary>
    internal partial class VersionUpdater : VariableUpdaterBase
    {
        private static readonly string[] s_excludedMonikers = { "servicing", "rtm" };

        private readonly string _productName;
        private readonly SpecificCommandOptions _options;
        private readonly VersionType _versionType;

        public VersionUpdater(
            VersionType versionType,
            string productName,
            string dockerfileVersion,
            SpecificCommandOptions options,
            ManifestVariables variables)
            : base(variables, ManifestHelper.GetVersionVariableName(versionType, productName, dockerfileVersion))
        {
            _productName = productName;
            _options = options;
            _versionType = versionType;
        }

        // Preserve the old selection rule: edit literal versions, not aliases or empty values.
        protected override bool ShouldUpdate(string currentValue) => VersionValueRegex.IsMatch(currentValue);

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

        public static string GetBuildVersion(string productName, string dockerfileVersion, ManifestVariables variables)
        {
            // Special case for handling the lzma NuGet package cache.
            if (productName == "lzma")
            {
                productName = "sdk";
            }

            string versionVariableName = ManifestHelper.GetVersionVariableName(VersionType.Build, productName, dockerfileVersion);
            string version = variables.GetRawValue(versionVariableName);
            if (!VersionValueRegex.IsMatch(version))
            {
                throw new InvalidOperationException($"Unable to retrieve {versionVariableName}");
            }

            return version;
        }

        private string GetProductVersion(IDependencyInfo productInfo)
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

            return VersionHelper.ResolveProductVersion(version, _options.StableBranding);
        }

        [GeneratedRegex(@"\Av?[\d]+.[\d]+.[\d]+(-[\w]+(.[\d]+)*)?\z")]
        private static partial Regex VersionValueRegex { get; }
    }
}
