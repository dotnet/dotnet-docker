// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine;

namespace Dotnet.Docker
{
    public class Options
    {
        public Uri BuildInfoUrl { get; private set; }
        public string GitHubEmail { get; private set; }
        public string GitHubPassword { get; private set; }
        public string GitHubProject => "dotnet-docker";
        public string GitHubUpstreamBranch => "nightly";
        public string GitHubUpstreamOwner => "dotnet";
        public string GitHubUser { get; private set; }
        public bool UpdateOnly => GitHubEmail == null || GitHubPassword == null || GitHubUser == null;

        public void Parse(string[] args)
        {
            ArgumentSyntax argSyntax = ArgumentSyntax.Parse(args, syntax =>
            {
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

                Uri buildInfoUrl = null;
                syntax.DefineParameter(
                    "build-info",
                    ref buildInfoUrl,
                    (value) => new Uri(value),
                    "URL of the build info to update the Dockerfiles with (http(s):// or file://)");
                BuildInfoUrl = buildInfoUrl;
            });

            // Workaround for https://github.com/dotnet/corefxlab/issues/1689
            foreach (Argument arg in argSyntax.GetActiveArguments())
            {
                if (arg.IsParameter && !arg.IsSpecified)
                {
                    Console.Error.WriteLine($"error: `{arg.Name}` must be specified.");
                    Environment.Exit(1);
                }
            }
        }
    }
}
