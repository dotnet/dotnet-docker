// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will scan a Dockerfile for the .NET Core artifacts that are installed.
    /// The updater will then retrieve and update the checksum sha used to validate the downloaded artifacts.
    /// </summary>
    public class DockerfileShaUpdater : FileRegexUpdater
    {
        public DockerfileShaUpdater(string dockerfilePath) : base()
        {
            Path = dockerfilePath;
            string envShaPattern = "ENV (?<name>DOTNET_[\\S]*DOWNLOAD_SHA) (?<value>[\\S]*)";
            string varShaPattern = "[ \\$](?<name>(dotnet_|aspnetcore_)sha512)( )*=( )*'(?<value>[^'\\s]*)'";
            Regex = new Regex($"({envShaPattern})|({varShaPattern})");
            VersionGroupName = "value";
        }

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyBuildInfos,
            out IEnumerable<IDependencyInfo> usedBuildInfos)
        {
            string sha = null;
            usedBuildInfos = Enumerable.Empty<IDependencyInfo>();

            Trace.TraceInformation($"DockerfileShaUpdater is processing '{Path}'.");
            string dockerfile = File.ReadAllText(Path);

            Regex versionRegex = new Regex($"ENV (?<name>(DOTNET_|ASPNETCORE_)[\\S]*VERSION) (?<value>[\\S]*)");
            Match versionMatch = versionRegex.Match(dockerfile);
            if (versionMatch.Success)
            {
                string versionEnvName = versionMatch.Groups["name"].Value;
                string version = versionMatch.Groups["value"].Value;

                string envUrlPattern = "ENV (DOTNET_[\\S]*DOWNLOAD_URL) (?<value>[\\S]*)";
                string inlineUrlPattern = "(?<value>https://dotnetcli.blob.core.windows.net/[^;\\s]*)";
                Regex shaRegex = new Regex($"({envUrlPattern})|{inlineUrlPattern}");
                Match shaMatch = shaRegex.Match(dockerfile);
                if (shaMatch.Success)
                {
                    // TODO:  Cleanup differences in sha extensions - https://github.com/dotnet/cli/issues/6724
                    string shaExt = versionEnvName.Contains("SDK") ? ".sha" : ".sha512";
                    string shaUrl = shaMatch.Groups["value"].Value
                        .Replace("dotnetcli", "dotnetclichecksums")
                        .Replace($"${versionEnvName}", version)     // *nix ENV var reference format
                        .Replace($"$Env:{versionEnvName}", version) // Windows ENV var reference format
                        + shaExt;

                    Trace.TraceInformation($"Downloading '{shaUrl}'.");
                    using (Stream shaStream = new HttpClient().GetStreamAsync(shaUrl).Result)
                    using (StreamReader reader = new StreamReader(shaStream))
                    {
                        sha = reader.ReadToEnd().ToLowerInvariant();
                    }
                }
                else
                {
                    Trace.TraceInformation($"DockerfileShaUpdater no-op - checksum url not found.");
                }
            }
            else
            {
                Trace.TraceInformation($"DockerfileShaUpdater no-op - dotnet url not found.");
            }

            return sha;
        }
    }
}
