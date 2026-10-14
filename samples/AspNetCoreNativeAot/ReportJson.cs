namespace ReportJson;

public record Report(string ReportDate, IList<MajorVersion> Versions);

public record MajorVersion(string Version,  bool Supported, string EolDate, int SupportEndsInDays, IList<PatchRelease> Releases);

public record PatchRelease(string ReleaseDate, int ReleasedDaysAgo,string ReleaseVersion, bool Security, IList<ReleaseJson.Cve> CveList);
