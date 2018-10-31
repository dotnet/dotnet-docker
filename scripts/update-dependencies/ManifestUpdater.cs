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
            _tagVersion = version;

            // Derive the Docker tag version from the product build version.
            // Example: Product build version 2.2.0-rtm-35586 => Docker tag version 2.2.0.
            int firstDashIndex = version.IndexOf('-');
            if (firstDashIndex != -1)
            {
                int secondDashIndex = version.IndexOf('-', firstDashIndex + 1);
                if (secondDashIndex != -1)
                {
                    _tagVersion = version.Substring(0, secondDashIndex);
                }

                foreach (string excludedMoniker in ExcludedMonikers)
                {
                    if (_tagVersion.EndsWith($"-{excludedMoniker}", StringComparison.OrdinalIgnoreCase))
                    {
                        _tagVersion = _tagVersion.Substring(0, _tagVersion.Length - (excludedMoniker.Length + 1));
                    }
                }
            }

            string versionVariableName = $"{version.Substring(0, version.LastIndexOf('.'))}-{imageVariantName}Version";
            Trace.TraceInformation($"Updating {versionVariableName} to {_tagVersion}");

            Path = System.IO.Path.Combine(repoRoot, "manifest.json");
            Regex = new Regex($"\"{versionVariableName}\": \"(?<{TagVersionValueGroupName}>[\\d]+.[\\d]+.[\\d]+(-[\\S]+)?)\"");
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
