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
        public string BinarySasQueryString { get; }
        public string ChecksumSasQueryString { get; }
        public bool ComputeChecksums { get; }
        public string DockerfileVersion { get; }
        public string ChannelName { get; }
        public string Email { get; }
        public string Password { get; }
        public string GitHubProject => "dotnet-docker";
        public string SourceBranch { get; }
        public string TargetBranch { get; }
        public string GitHubUpstreamOwner => "dotnet";
        public string User { get; }
        public string AzdoOrganization { get; }
        public string AzdoProject { get; }
        public string AzdoRepo { get; }
        public IDictionary<string, string?> ProductVersions { get; set; } = new Dictionary<string, string?>();
        public string VersionSourceName { get; }
        public bool UseStableBranding { get; }
        public bool UpdateOnly => Email == null || Password == null || User == null || TargetBranch == null;
        public bool IsInternal => !string.IsNullOrEmpty(BinarySasQueryString) || !string.IsNullOrEmpty(ChecksumSasQueryString);

        public Options(string dockerfileVersion, string[] productVersion, string channelName, string versionSourceName, string email, string password, string user,
            bool computeShas, bool stableBranding, string binarySas, string checksumSas, string sourceBranch, string targetBranch, string org, string project, string repo)
        {
            DockerfileVersion = dockerfileVersion;
            ProductVersions = productVersion
                .Select(pair => pair.Split(new char[] { '=' }, 2))
                .ToDictionary(split => split[0].ToLower(), split => split.Skip(1).FirstOrDefault());
            ChannelName = channelName;
            VersionSourceName = versionSourceName;
            Email = email;
            Password = password;
            User = user;
            ComputeChecksums = computeShas;
            UseStableBranding = stableBranding;
            BinarySasQueryString = binarySas;
            ChecksumSasQueryString = checksumSas;
            SourceBranch = sourceBranch;

            // Default TargetBranch to SourceBranch if it's not explicitly provided
            TargetBranch = string.IsNullOrEmpty(targetBranch) ? sourceBranch : targetBranch;

            AzdoOrganization = org;
            AzdoProject = project;
            AzdoRepo = repo;

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
                new Option<string>("--email", "GitHub or AzDO email used to make PR (if not specified, a PR will not be created)"),
                new Option<string>("--password", "GitHub or AzDO password used to make PR (if not specified, a PR will not be created)"),
                new Option<string>("--user", "GitHub or AzDO user used to make PR (if not specified, a PR will not be created)"),
                new Option<bool>("--compute-shas", "Compute the checksum if a published checksum cannot be found"),
                new Option<bool>("--stable-branding", "Use stable branding version numbers to compute paths"),
                new Option<string>("--source-branch", () => "nightly", "Branch where the Dockerfiles are hosted"),
                new Option<string>("--target-branch", "Target branch of the generated PR (defaults to value of source-branch)"),
                new Option<string>("--binary-sas", "SAS query string used to access binary files in blob storage"),
                new Option<string>("--checksum-sas", "SAS query string used to access checksum files in blob storage"),
                new Option<string>("--org", "Name of the AzDO organization"),
                new Option<string>("--project", "Name of the AzDO project"),
                new Option<string>("--repo", "Name of the AzDO repo"),
            };
    }
}
#nullable disable
