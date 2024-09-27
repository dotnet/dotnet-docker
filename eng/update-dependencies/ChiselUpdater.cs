using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            [
                new ChiselReleaseShaUpdater(repoRoot, GetChiselManifestVariable("chisel", arch, "sha", "latest"), chiselRelease, arch),
                new ChiselReleaseShaUpdater(repoRoot, GetChiselManifestVariable("chisel", arch, "sha", dockerfileVersion), chiselRelease, arch),
                new GitHubReleaseUrlUpdater(repoRoot, GetChiselManifestVariable("chisel", arch, "url", "latest"), chiselRelease, DependencyInfoToUse, GetAssetRegex(arch)),
                new GitHubReleaseUrlUpdater(repoRoot, GetChiselManifestVariable("chisel", arch, "url", dockerfileVersion), chiselRelease, DependencyInfoToUse, GetAssetRegex(arch))
            ]);

        GitHubReleaseVersionUpdater rocksToolboxUpdater = new(repoRoot, "rocks-toolbox|latest|version", rocksToolboxRelease, DependencyInfoToUse);

        return [ ..chiselUpdaters, rocksToolboxUpdater ];
    }

    public static string GetChiselManifestVariable(string product, string arch, string type, string dockerfileVersion = "latest")
    {
        // Workaround for ambiguous method call, will be fixed with https://github.com/dotnet/csharplang/issues/8374
        return string.Join('|', new string() { product, dockerfileVersion, ToManifestArch(arch), type });
    }

    private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;

    private sealed class ChiselReleaseShaUpdater(string repoRoot, string variableName, Release release, string arch)
        : GitHubReleaseUrlUpdater(repoRoot, variableName, release, DependencyInfoToUse, GetAssetRegex(arch))
    {
        private static readonly HttpClient s_httpClient = new();

        protected override string? GetValue()
        {
            string? downloadUrl = base.GetValue();
            if (downloadUrl is null)
            {
                return null;
            }

            return ChecksumHelper.ComputeChecksumShaAsync(s_httpClient, downloadUrl).Result;
        }
    }
}
