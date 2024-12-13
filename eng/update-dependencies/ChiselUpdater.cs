using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker;

#nullable enable
internal static class ChiselUpdater
{
    public const string ToolName = Repo;

    private const string Owner = "canonical";

    private const string Repo = "chisel";

    private static readonly string[] s_supportedArchitectures = ["amd64", "arm", "arm64"];

    public static IEnumerable<IDependencyUpdater> GetUpdaters(string repoRoot)
    {
        return s_supportedArchitectures
            .SelectMany<string, IDependencyUpdater>(arch =>
                [
                    new GitHubReleaseUrlUpdater(
                        repoRoot: repoRoot,
                        toolName: ToolName,
                        variableName: GetChiselManifestVariable(ToolName, arch, "url", "latest"),
                        owner: Owner,
                        repo: Repo,
                        assetRegex: GetAssetRegex(arch)),
                ]);
    }

    public static async Task<GitHubReleaseInfo> GetBuildInfoAsync() =>
        new GitHubReleaseInfo(
            SimpleName: ToolName,
            Release: await GitHubHelper.GetLatestRelease(Owner, Repo));

    public static string GetChiselManifestVariable(string product, string arch, string type, string dockerfileVersion = "latest")
    {
        // Workaround for ambiguous method call, will be fixed with https://github.com/dotnet/csharplang/issues/8374
        return string.Join('|', new string[] { product, dockerfileVersion, ToManifestArch(arch), type });
    }

    private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;
}
