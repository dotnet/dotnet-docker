using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.Dependencies;
using Octokit;

namespace Dotnet.Docker;

#nullable enable
internal static class ChiselUpdater
{
    private static readonly string[] s_chiselArchitectures = ["amd64", "arm", "arm64"];

    private const string DependencyInfoToUse = "runtime";

    public static async Task<IEnumerable<IDependencyUpdater>> GetChiselUpdatersAsync(string repoRoot, string dockerfileVersion)
    {
        Release chiselRelease = await GitHubHelper.GetLatestRelease("canonical", "chisel");
        Release rocksToolboxRelease = await GitHubHelper.GetLatestRelease("canonical", "rocks-toolbox");

        IEnumerable<IDependencyUpdater> chiselUpdaters = s_chiselArchitectures.SelectMany<string, IDependencyUpdater>(arch =>
        {
            string manifestVar = GetChiselManifestVariable("chisel", arch, "sha", dockerfileVersion);
            return [
                new ChiselReleaseShaUpdater(repoRoot, manifestVar, chiselRelease, arch),
                new GitHubReleaseUrlUpdater(repoRoot, manifestVar, chiselRelease, DependencyInfoToUse, GetAssetRegex(arch))
            ];
        });

        GitHubReleaseVersionUpdater rocksToolboxUpdater = new(repoRoot, "rocks-toolbox|latest|version", rocksToolboxRelease, DependencyInfoToUse);

        return [ ..chiselUpdaters, rocksToolboxUpdater ];
    }

    private static string GetChiselManifestVariable(string product, string arch, string type, string dockerfileVersion = "latest")
    {
        return string.Join('|', [product, dockerfileVersion, ToManifestArch(arch), type]);
    }

    private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;
}
