// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace Dotnet.Docker
{
    public record SpecificCommandOptions : CreatePullRequestOptions, IOptions
    {
        public string GitHubProject { get; } = "dotnet-docker";
        public string GitHubUpstreamOwner { get; } = "dotnet";

        // .NET Version options
        public required string DockerfileVersion { get; init; } = "";
        public IDictionary<string, string?> ProductVersions { get; init; } = new Dictionary<string, string?>();
        public ReleaseState? ReleaseState { get; init; } = null;
        public bool StableBranding { get; init; } = false;
        public string ChecksumsFile { get; init; } = "";

        // Tool/image component version options
        public IEnumerable<string> Tools { get; init; } = [];

        public bool UpdateOnly =>
            string.IsNullOrEmpty(Email)
            || string.IsNullOrEmpty(Password)
            || string.IsNullOrEmpty(User)
            || string.IsNullOrEmpty(TargetBranch);

        // Internal build options
        public string InternalBaseUrl { get; init; } = "";
        public string InternalAccessToken { get; init; } = "";
        public bool IsInternal => !string.IsNullOrEmpty(InternalBaseUrl);

        public static new List<Argument> Arguments =>
        [
            new Argument<string>("dockerfile-version"),
            ..CreatePullRequestOptions.Arguments,
        ];

        public static new List<Option> Options =>
        [
            ..GetOptions(),
            ..CreatePullRequestOptions.Options,
        ];

        private static List<Option> GetOptions()
        {
            var toolsOption = new Option<IEnumerable<string>>("--tools", "--tool")
            {
                Description = "Image tools or Dockerfile components to update",
                Arity = ArgumentArity.ZeroOrMore,
                DefaultValueFactory = _ => [],
            };
            toolsOption.AcceptOnlyFromAmong(Docker.Tools.SupportedTools);

            List<Option> options =
            [
                new Option<IDictionary<string, string?>>("--product-versions", "--product-version")
                {
                    Description = "Product versions to update (<product-name>=<version>)",
                    Arity = ArgumentArity.ZeroOrMore,
                    DefaultValueFactory = _ => new Dictionary<string, string?>(),
                    CustomParser = argumentResult =>
                    {
                        var productVersions = argumentResult.Tokens
                            .Select(token => token.Value)
                            .Select(value => value.Split('=', 2))
                            .ToDictionary(
                                keySelector: pair => pair[0],
                                elementSelector: pair => pair.Length > 1 ? pair[1] : null);

                        // Special case for handling the shared dotnet product version variables.
                        if (productVersions.TryGetValue("runtime", out string? runtimeVersion))
                        {
                            productVersions["dotnet"] = runtimeVersion;
                        }
                        else if (productVersions.TryGetValue("aspnet", out string? aspnetVersion))
                        {
                            productVersions["dotnet"] = aspnetVersion;
                        }

                        return productVersions;
                    },
                },

                new Option<ReleaseState?>("--release-state") { Description = "The release state of the product assets" },
                new Option<bool>("--stable-branding") { Description = "Use stable branding version numbers to compute paths" },
                new Option<string>("--checksums-file") { Description = "File containing a list of checksums for each product asset" },
                toolsOption,

                new Option<string>("--internal-base-url") { Description = "Base Url for internal build artifacts" },
                new Option<string>("--internal-access-token") { Description = "PAT for accessing internal build artifacts" },
            ];

            return options;
        }
    }
}
