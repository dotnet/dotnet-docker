// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;

namespace Dotnet.Docker
{
    public class SpecificCommandOptions : IOptions
    {
        public string GitHubProject { get; } = "dotnet-docker";
        public string GitHubUpstreamOwner { get; } = "dotnet";

        public required string InternalBaseUrl { get; init; }
        public required string InternalAccessToken { get; init; }
        public required bool ComputeShas { get; init; }
        public required string DockerfileVersion { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public string SourceBranch { get; init; } = "nightly";
        public required string TargetBranch { get; init; }
        public required string User { get; init; }
        public required string AzdoOrganization { get; init; }
        public required string AzdoProject { get; init; }
        public required string AzdoRepo { get; init; }
        public required IDictionary<string, string?> ProductVersions { get; set; }
        public required IEnumerable<string> Tools { get; init; }
        public required string VersionSourceName { get; init; }
        public required bool StableBranding { get; init; }
        public required string ChecksumsFile { get; init; }
        public required ReleaseState? ReleaseState { get; init; }

        public bool UpdateOnly => Email == null || Password == null || User == null || TargetBranch == null;
        public bool IsInternal => !string.IsNullOrEmpty(InternalBaseUrl);

        public static List<Argument> Arguments =>
        [
            new Argument<string>("dockerfile-version"),
        ];

        public static List<Option> Options => GetOptions();

        private static List<Option> GetOptions()
        {
            var toolOption = new Option<IEnumerable<string>>("--tool", "--tools")
            {
                Description = "Tool to update.",
                Arity = ArgumentArity.ZeroOrMore,
                DefaultValueFactory = _ => [],
            };
            toolOption.AcceptOnlyFromAmong(Docker.Tools.SupportedTools);

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
                toolOption,
                new Option<string>("--version-source-name") { Description = "The name of the source from which the version information was acquired." },
                new Option<string>("--email") { Description = "GitHub or AzDO email used to make PR (if not specified, a PR will not be created)" },
                new Option<string>("--password") { Description = "GitHub or AzDO password used to make PR (if not specified, a PR will not be created)" },
                new Option<string>("--user") { Description = "GitHub or AzDO user used to make PR (if not specified, a PR will not be created)" },
                new Option<bool>("--compute-shas") { Description = "Compute the checksum if a published checksum cannot be found" },
                new Option<bool>("--stable-branding") { Description = "Use stable branding version numbers to compute paths" },
                new Option<string>("--source-branch") { Description = "Branch where the Dockerfiles are hosted" },
                new Option<string>("--target-branch")
                {
                    Description = "Target branch of the generated PR (defaults to value of source-branch)",
                    DefaultValueFactory = argumentResult =>
                    {
                        var thisOption = argumentResult.Parent as OptionResult;
                        var command = thisOption?.Parent as CommandResult;

                        OptionResult? sourceBranchOption = command?.Children?
                            .Where(child => child is OptionResult)
                            .Cast<OptionResult>()
                            .Where(option => option.IdentifierToken?.Value == "--source-branch")
                            .FirstOrDefault();

                        string? sourceBranchValue = sourceBranchOption?.Tokens
                            .FirstOrDefault()?
                            .Value;

                        return sourceBranchValue ?? "";
                    },
                },
                new Option<string>("--azdo-organization", "--org") { Description = "Name of the AzDO organization" },
                new Option<string>("--azdo-project", "--project") { Description = "Name of the AzDO project" },
                new Option<string>("--azdo-repo", "--repo") { Description = "Name of the AzDO repo" },
                new Option<string>("--checksums-file") { Description = "File containing a list of checksums for each product asset" },
                new Option<ReleaseState?>("--release-state") { Description = "The release state of the product assets" },
                new Option<string>("--internal-base-url") { Description = "Base Url for internal build artifacts" },
                new Option<string>("--internal-access-token") { Description = "PAT for accessing internal build artifacts" }
            ];

            return options;
        }
    }
}
