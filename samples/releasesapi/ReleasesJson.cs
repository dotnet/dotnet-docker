using System.Text.Json.Serialization;

namespace ReleaseJson;

public static class ReleaseValues
{
    public const string RELEASE_INDEX_URL = "https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/releases-index.json";
}

public record ReleaseIndex([property: JsonPropertyName("releases-index")]List<Release> ReleasesIndex);

public record Release([property: JsonPropertyName("channel-version")] string ChannelVersion, [property: JsonPropertyName("latest-release")] string LatestRelease, [property: JsonPropertyName("latest-release-date")] string LatestReleaseDate, bool Security, [property: JsonPropertyName("latest-runtime")] string LatestRuntime, [property: JsonPropertyName("latest-sdk")] string LatestSdk, [property: JsonPropertyName("release-type")] string ReleaseType, [property: JsonPropertyName("support-phase")] string SupportPhase, [property: JsonPropertyName("eol-date")] string EolDate, [property: JsonPropertyName("releases.json")] string ReleasesJson, [property: JsonPropertyName("releases")] List<ReleaseDetail> Releases);

public record ReleaseDetail([property: JsonPropertyName("release-date")] string ReleaseDate, [property: JsonPropertyName("release-version")] string ReleaseVersion, bool Security, [property: JsonPropertyName("cve-list")] List<Cve> Cves);

public record Cve([property: JsonPropertyName("cve-id")] string CveId,[property: JsonPropertyName("cve-url")] string CveUrl);
