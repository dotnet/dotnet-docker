// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace Dotnet.Docker
{
    public class Options
    {
        public bool ComputeChecksums { get; private set; }
        public string DockerfileVersion { get; private set; }
        public string GitHubEmail { get; private set; }
        public string GitHubPassword { get; private set; }
        public string GitHubProject => "dotnet-docker";
        public string GitHubUpstreamBranch => "nightly";
        public string GitHubUpstreamOwner => "dotnet";
        public string GitHubUser { get; private set; }
        public IDictionary<string, string> ProductVersions { get; set; } = new Dictionary<string, string>();
        public string VersionSourceName { get; private set; }
        public bool UpdateOnly => GitHubEmail == null || GitHubPassword == null || GitHubUser == null;

        public void Parse(string[] args)
        {
            ArgumentSyntax argSyntax = ArgumentSyntax.Parse(args, syntax =>
            {
                IReadOnlyList<string> productVersions = Array.Empty<string>();
                syntax.DefineOptionList(
                    "product-version",
                    ref productVersions,
                    "Product versions to update (<product-name>=<version>)");
                ProductVersions = productVersions
                    .Select(pair => pair.Split(new char[] { '=' }, 2))
                    .ToDictionary(split => split[0].ToLower(), split => split[1]);

                string versionSourceName = null;
                syntax.DefineOption(
                    "version-source-name",
                    ref versionSourceName,
                    "The name of the source from which the version information was acquired.");
                VersionSourceName = versionSourceName;

                string gitHubEmail = null;
                syntax.DefineOption(
                    "email",
                    ref gitHubEmail,
                    "GitHub email used to make PR (if not specified, a PR will not be created)");
                GitHubEmail = gitHubEmail;

                string gitHubPassword = null;
                syntax.DefineOption(
                    "password",
                    ref gitHubPassword,
                    "GitHub password used to make PR (if not specified, a PR will not be created)");
                GitHubPassword = gitHubPassword;

                string gitHubUser = null;
                syntax.DefineOption(
                    "user",
                    ref gitHubUser,
                    "GitHub user used to make PR (if not specified, a PR will not be created)");
                GitHubUser = gitHubUser;

                bool computeChecksums = false;
                syntax.DefineOption(
                    "compute-shas",
                    ref computeChecksums,
                    "Compute the checksum if a published checksum cannot be found");
                ComputeChecksums = computeChecksums;

                string dockerfileVersion = null;
                syntax.DefineParameter(
                    "dockerfile-version",
                    ref dockerfileVersion,
                    "Version to the Dockerfiles to update");
                DockerfileVersion = dockerfileVersion;
            });
        }
    }
}
