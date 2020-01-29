// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Dotnet.Docker
{
    public class Options
    {
        public string AspnetVersion { get; private set; }
        public string GitHubEmail { get; private set; }
        public string GitHubPassword { get; private set; }
        public string GitHubProject => "dotnet-docker";
        public string GitHubUpstreamBranch => "nightly";
        public string GitHubUpstreamOwner => "dotnet";
        public string GitHubUser { get; private set; }
        public string RuntimeVersion { get; private set; }
        public string SdkVersion { get; private set; }
        public bool UpdateOnly => GitHubEmail == null || GitHubPassword == null || GitHubUser == null;

        public void Parse(string[] args)
        {
            ArgumentSyntax argSyntax = ArgumentSyntax.Parse(args, syntax =>
            {
                string aspnetVersion = null;
                Argument<string> aspnetVersionArg = syntax.DefineOption(
                    "aspnet-version",
                    ref aspnetVersion,
                    "ASP.NET version to update the Dockerfiles with");
                AspnetVersion = aspnetVersion;

                string runtimeVersion = null;
                Argument<string> runtimeVersionArg = syntax.DefineOption(
                    "runtime-version",
                    ref runtimeVersion,
                    ".NET runtime version to update the Dockerfiles with");
                RuntimeVersion = runtimeVersion;

                string sdkVersion = null;
                Argument<string> sdkVersionArg = syntax.DefineOption(
                    "sdk-version",
                    ref sdkVersion,
                    "SDK version to update the Dockerfiles with");
                SdkVersion = sdkVersion;

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
            });
        }
    }
}
