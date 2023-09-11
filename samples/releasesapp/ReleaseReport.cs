namespace Report;

public record ReleaseReport(string ReportDate, IList<DotnetVersion> Versions);

public record DotnetVersion(string MajorVersion, bool Supported, string EolDate, int SupportEndsInDays, IList<Release> Releases);

public record Release(string BuildVersion, bool Security, string ReleaseDate, int ReleasedDaysAgo, IList<ReleaseJson.Cve> Cves);