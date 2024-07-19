using System;
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

    public static async Task<IEnumerable<IDependencyUpdater>> GetChiselUpdatersAsync(string repoRoot, string dockerfileVersion)
    {
        Release chiselRelease = await GitHubHelper.GetLatestReleaseWithOctokitAsync("canonical", "chisel");
        Release rocksToolboxRelease = await GitHubHelper.GetLatestReleaseWithOctokitAsync("canonical", "rocks-toolbox");

        IEnumerable<IDependencyUpdater> chiselShaUpdaters = s_chiselArchitectures
            .Select(arch => new ChiselReleaseShaUpdater(repoRoot, GetChiselManifestVariable("chisel", arch, "sha"), chiselRelease, arch));
        IEnumerable<IDependencyUpdater> chiselUrlUpdaters = s_chiselArchitectures
            .Select(arch => new ChiselReleaseUrlUpdater(repoRoot, GetChiselManifestVariable("chisel", arch, "url"), chiselRelease, arch));
        var rocksToolboxUpdater = new ChiselReleaseVersionUpdater(repoRoot, "rocks-toolbox|latest|version", rocksToolboxRelease);

        return [ ..chiselUrlUpdaters, ..chiselShaUpdaters, rocksToolboxUpdater ];
    }

    private static string GetChiselManifestVariable(string product, string arch, string type)
    {
        return string.Join('|', [product, "latest", ToManifestArch(arch), type]);
    }

    private static string ToManifestArch(string arch) => arch == "amd64" ? "x64" : arch;

    /// <summary>
    /// Updates the checksum when runtime dependencies are being updated.
    /// </summary>
    private class ChiselReleaseShaUpdater(string repoRoot, string variableName, Release release, string arch)
        : ChiselReleaseUrlUpdater(repoRoot, variableName, release, arch)
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

    /// <summary>
    /// Updates to the latest download URL when runtime dependencies are being updated.
    /// </summary>
    private class ChiselReleaseUrlUpdater(string repoRoot, string variableName, Release release, string arch)
        : ChiselReleaseVersionUpdater(repoRoot, variableName, release)
    {
        private readonly string _variableName = variableName;

        protected override string? GetValue()
        {
            Regex archSpecificAssetRegex = GetAssetRegex(arch);

            ReleaseAsset releaseAsset = Release.Assets.FirstOrDefault(asset => archSpecificAssetRegex.IsMatch(asset.Name))
                ?? throw new Exception($"Could not find release asset for {_variableName} matching regex {archSpecificAssetRegex}");

            return releaseAsset.BrowserDownloadUrl;
        }

        private static Regex GetAssetRegex(string arch) => new(@"chisel_v\d+\.\d+\.\d+_linux_" + arch + @"\.tar\.gz");
    }

    /// <summary>
    /// Updates to the latest release tag name when runtime dependencies are being updated.
    /// </summary>
    private class ChiselReleaseVersionUpdater(string repoRoot, string variableName, Release release)
        : GitHubReleaseUpdaterBase(repoRoot, variableName, release)
    {
        protected override string? GetValue()
        {
            return Release.TagName;
        }

        protected sealed override IDependencyInfo? GetDependencyInfosToUse(IEnumerable<IDependencyInfo> dependencyInfos)
        {
            return dependencyInfos.FirstOrDefault(info => info.SimpleName == "runtime");
        }
    }
}
