using System.Text.Json.Serialization;

namespace ReleaseJson;

public record ReleaseIndex(List<MajorRelease> ReleasesIndex);

public record MajorRelease(string ChannelVersion, string LatestRelease, string LatestReleaseDate, bool Security, string LatestRuntime, string LatestSdk, string ReleaseType, string SupportPhase, string EolDate, [property: JsonPropertyName("releases.json")] string ReleasesJson, List<Release> Releases);

public record Release(string ReleaseDate, string ReleaseVersion, bool Security, List<Cve> CveList);

public record Cve(string CveId,string CveUrl);
