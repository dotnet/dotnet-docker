// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

#nullable enable
namespace Dotnet.Docker
{
    public class Options
    {
        public bool ComputeChecksums { get; }
        public string DockerfileVersion { get; }
        public string ChannelName { get; }
        public string GitHubEmail { get; }
        public string GitHubPassword { get; }
        public string GitHubProject => "dotnet-docker";
        public string GitHubUpstreamBranch => "nightly";
        public string GitHubUpstreamOwner => "dotnet";
        public string GitHubUser { get; }
        public IDictionary<string, string?> ProductVersions { get; set; } = new Dictionary<string, string?>();
        public string VersionSourceName { get; }
        public bool UseStableBranding { get; }
        public bool UpdateOnly => GitHubEmail == null || GitHubPassword == null || GitHubUser == null;

        public Options(string dockerfileVersion, string[] productVersion, string channelName, string versionSourceName, string email, string password, string user,
            bool computeShas, bool stableBranding)
        {
            DockerfileVersion = dockerfileVersion;
            ProductVersions = productVersion
                .Select(pair => pair.Split(new char[] { '=' }, 2))
                .ToDictionary(split => split[0].ToLower(), split => split.Skip(1).FirstOrDefault());
            ChannelName = channelName;
            VersionSourceName = versionSourceName;
            GitHubEmail = email;
            GitHubPassword = password;
            GitHubUser = user;
            ComputeChecksums = computeShas;
            UseStableBranding = stableBranding;

            // Special case for handling the shared dotnet product version variables.
            if (ProductVersions.ContainsKey("runtime"))
            {
                ProductVersions["dotnet"] = ProductVersions["runtime"];
            }
            else if (ProductVersions.ContainsKey("aspnet"))
            {
                ProductVersions["dotnet"] = ProductVersions["aspnet"];
            }
        }

        public static IEnumerable<Symbol> GetCliSymbols() =>
            new Symbol[]
            {
                new Argument<string>("dockerfile-version", "Version of the Dockerfiles to update"),
                new Option<string[]>("--product-version", "Product versions to update (<product-name>=<version>)"),
                new Option<string>("--channel-name", "The name of the channel from which to find product files."),
                new Option<string>("--version-source-name", "The name of the source from which the version information was acquired."),
                new Option<string>("--email", "GitHub email used to make PR (if not specified, a PR will not be created)"),
                new Option<string>("--password", "GitHub password used to make PR (if not specified, a PR will not be created)"),
                new Option<string>("--user", "GitHub user used to make PR (if not specified, a PR will not be created)"),
                new Option<bool>("--compute-shas", "Compute the checksum if a published checksum cannot be found"),
                new Option<bool>("--stable-branding", "Use stable branding version numbers to compute paths")
            };
    }
}
#nullable disable
