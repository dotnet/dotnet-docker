using System.Text.Json.Serialization;
using ReleaseJson;
using ReleaseValues;
using ReportJson;
using Version = ReportJson.Version;

namespace ReleaseReport;

public static class Generator
{
    public static async Task<Report> MakeReportAsync()
    {
        Report report = new(DateTime.Today.ToShortDateString(), await GetVersionsAsync().ToListAsync());
        return report;
    }

    public static async IAsyncEnumerable<Version> GetVersionsAsync()
    {
        await foreach(MajorRelease release in GetMajorReleasesAsync())
        {
            int supportDays = release.EolDate is null ? 0 : GetDaysAgo(release.EolDate);
            bool supported = release.SupportPhase is "active" or "maintainence";
            Version version = new(release.ChannelVersion, supported, release.EolDate ?? "", supportDays, GetReleases(release).ToList());
            yield return version;
        }

        yield break;
    }

    public static async IAsyncEnumerable<MajorRelease> GetMajorReleasesAsync()
    {
        HttpClient httpClient = new();
        string loadError = "Failed to load release information.";
        ReleaseIndex releases = await httpClient.GetFromJsonAsync<ReleaseIndex>(Values.RELEASE_INDEX_URL, ReleaseJsonSerializerContext.Default.ReleaseIndex) ?? throw new Exception(loadError);

        foreach (MajorRelease releaseSummary in releases.ReleasesIndex)
        {
            // Only show releases in support or < 1 year EOL
            if (DateOnly.TryParse(releaseSummary.EolDate, out DateOnly eolDate) &&
                DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)).DayNumber > eolDate.DayNumber)
            {
                continue;
            }

            MajorRelease release = await httpClient.GetFromJsonAsync<MajorRelease>(releaseSummary.ReleasesJson, ReleaseJsonSerializerContext.Default.MajorRelease) ?? throw new Exception(loadError);
            yield return release;
        }

        yield break;
    }

    // Get first and first security release
    public static IEnumerable<Release> GetReleases(MajorRelease release)
    {
        bool securityOnly = false;
        
        foreach (ReleaseDetail releaseDetail in release.Releases)
        {
            if (!releaseDetail.Security && securityOnly)
            {
                continue;
            }
            
            yield return new Release(releaseDetail.ReleaseDate, GetDaysAgo(releaseDetail.ReleaseDate, true), releaseDetail.ReleaseVersion, releaseDetail.Security, releaseDetail.Cves);

            if (releaseDetail.Security)
            {
                yield break;
            }
            else if (!securityOnly)
            {
                securityOnly = true;
            }
        }

        yield break;
    } 

    public static int GetDaysAgo(string date, bool isPositive = false)
    {
        bool success = DateTime.TryParse(date, out DateTime day);
        int daysAgo = success ? (int)(day - DateTime.Now).TotalDays : 0;
        return isPositive ? Math.Abs(daysAgo) : daysAgo;
    }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ReleaseIndex))]
internal partial class ReleaseJsonSerializerContext : JsonSerializerContext
{
}
