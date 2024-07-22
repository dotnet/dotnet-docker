using System.Net.Http;
using Octokit;

namespace Dotnet.Docker;

#nullable enable
/// <summary>
/// Updates the checksum when runtime dependencies are being updated.
/// </summary>
internal sealed class ChiselReleaseShaUpdater(string repoRoot, string variableName, Release release, string arch)
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
