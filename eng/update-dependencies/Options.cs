// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace Dotnet.Docker
{
    public class Options
    {
        public string InternalBaseUrl { get; }
        public string InternalAccessToken { get; }
        public bool ComputeChecksums { get; }
        public string DockerfileVersion { get; }
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
        public string[] Tools { get; }
        public string VersionSourceName { get; }
        public bool UseStableBranding { get; }
        public bool UpdateOnly => Email == null || Password == null || User == null || TargetBranch == null;
        public bool IsInternal => !string.IsNullOrEmpty(InternalBaseUrl);
        public string ChecksumsFile { get; }
        public ReleaseState? ReleaseState { get; }

        public Options(
            string dockerfileVersion,
            string[] productVersion,
            string[] tool,
            string versionSourceName,
            string email,
            string password,
            string user,
            bool computeShas,
            bool stableBranding,
            string binarySas,
            string checksumSas,
            string sourceBranch,
            string targetBranch,
            string org,
            string project,
            string repo,
            string checksumsFile,
            ReleaseState? releaseState,
            string internalBaseUrl,
            string internalAccessToken)
        {
            DockerfileVersion = dockerfileVersion;
            ProductVersions = productVersion
                .Select(pair => pair.Split(new char[] { '=' }, 2))
                .ToDictionary(split => split[0].ToLower(), split => split.Skip(1).FirstOrDefault());
            Tools = tool;
            VersionSourceName = versionSourceName;
            Email = email;
            Password = password;
            User = user;
            ComputeChecksums = computeShas;
            ChecksumsFile = checksumsFile;
            UseStableBranding = stableBranding;
            SourceBranch = sourceBranch;
            InternalBaseUrl = internalBaseUrl;
            InternalAccessToken = internalAccessToken;

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

            ReleaseState = releaseState;
        }

        public static IEnumerable<Symbol> GetCliSymbols() =>
            new Symbol[]
            {
                new Argument<string>("dockerfile-version", "Version of the Dockerfiles to update"),
                new Option<string[]>("--product-version", "Product versions to update (<product-name>=<version>)"),
                new Option<string[]>("--tool", "Tool to update.").FromAmong(Docker.Tools.SupportedTools),
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
                new Option<string>("--checksums-file", "File containing a list of checksums for each product asset"),
                new Option<ReleaseState?>("--release-state", "The release state of the product assets"),
                new Option<string>("--internal-base-url", "Base Url for internal build artifacts"),
                new Option<string>("--internal-access-token", "PAT for accessing internal build artifacts")
            };
    }

    public enum ReleaseState
    {
        Prerelease,
        Release
    }
}
