using System.Text.Json.Serialization;

namespace ReleaseJson;

public record ReleaseIndex(
    [property: JsonPropertyName("releases-index")] List<MajorRelease> ReleasesIndex
    );

public record MajorRelease(
    [property: JsonPropertyName("channel-version")] string ChannelVersion, 
    [property: JsonPropertyName("latest-release")] string LatestRelease, 
    [property: JsonPropertyName("latest-release-date")] string LatestReleaseDate, 
    [property: JsonPropertyName("security")] bool Security, 
    [property: JsonPropertyName("latest-runtime")] string LatestRuntime, 
    [property: JsonPropertyName("latest-sdk")] string LatestSdk, 
    [property: JsonPropertyName("release-type")] string ReleaseType, 
    [property: JsonPropertyName("support-phase")] string SupportPhase, 
    [property: JsonPropertyName("eol-date")] string EolDate, 
    [property: JsonPropertyName("releases.json")] string ReleasesJson, 
    [property: JsonPropertyName("releases")] List<ReleaseDetail> Releases
    );

public record ReleaseDetail(
    [property: JsonPropertyName("release-date")] string ReleaseDate, 
    [property: JsonPropertyName("release-version")] string ReleaseVersion, 
    [property: JsonPropertyName("security")] bool Security, 
    [property: JsonPropertyName("cve-list")] List<Cve> Cves
    );

public record Cve(
    [property: JsonPropertyName("cve-id")] string CveId,
    [property: JsonPropertyName("cve-url")] string CveUrl
    );
