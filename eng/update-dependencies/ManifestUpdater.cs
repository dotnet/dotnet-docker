// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will update the full versioned tags within the manifest to align with the
    /// current product version.
    /// </summary>
    public class ManifestUpdater : FileRegexUpdater
    {
        private const string TagVersionValueGroupName = "tagVersionValue";
        private readonly static string[] ExcludedMonikers = { "servicing", "rtm" };

        private string _tagVersion;

        public ManifestUpdater(string imageVariantName, string version, string repoRoot) : base()
        {
            // Derive the Docker tag version from the product build version.
            // 5.0.0-preview.2.19530.9 => 5.0.0-preview.2
            string versionRegexPattern = "[\\d]+.[\\d]+.[\\d]+(-[\\w]+(.[\\d]+)?)?";
            Match versionMatch = Regex.Match(version, versionRegexPattern);
            _tagVersion = versionMatch.Success ? versionMatch.Value : version;

            foreach (string excludedMoniker in ExcludedMonikers)
            {
                int monikerIndex = _tagVersion.IndexOf($"-{excludedMoniker}", StringComparison.OrdinalIgnoreCase);
                if (monikerIndex != -1)
                {
                    _tagVersion = _tagVersion.Substring(0, monikerIndex);
                }
            }

            string productVersion = version.Split('-')[0];
            string dockerfileVersion = productVersion.Substring(0, productVersion.LastIndexOf('.'));
            string versionVariableName = $"{dockerfileVersion}-{imageVariantName}Version";
            Trace.TraceInformation($"Updating {versionVariableName} to {_tagVersion}");

            Path = System.IO.Path.Combine(repoRoot, "manifest.json");
            Regex = new Regex($"\"{versionVariableName}\": \"(?<{TagVersionValueGroupName}>{versionRegexPattern})\"");
            VersionGroupName = TagVersionValueGroupName;
        }

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyBuildInfos, out IEnumerable<IDependencyInfo> usedBuildInfos)
        {
            usedBuildInfos = Enumerable.Empty<IDependencyInfo>();

            return _tagVersion;
        }
    }
}
