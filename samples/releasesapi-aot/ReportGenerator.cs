using System.Text.Json.Serialization;
using Report;

namespace ReleaseJson;

public static class ReportGenerator
{
    public static async Task<ReleaseReport> MakeReport()
    {
        List<DotnetVersion> versions= new();
        await foreach (var release in GetReleases())
        {
            versions.Add(GetVersionDetail(release));
        }

        ReleaseReport report = new(DateTime.Today.ToShortDateString(), versions);
        return report;
    }

    public static async IAsyncEnumerable<Release> GetReleases()
    {
        HttpClient  httpClient = new();
        var loadError = "Failed to load release information.";
        var releases = await httpClient.GetFromJsonAsync<ReleaseIndex>(ReleaseValues.RELEASE_INDEX_URL, ReleaseJsonSerializerContext.Default.ReleaseIndex) ?? throw new Exception(loadError);

        foreach (var releaseSummary in releases.ReleasesIndex)
        {
            // Only show releases in support or < 1 year EOL
            if (DateOnly.TryParse(releaseSummary.EolDate, out DateOnly eolDate)
             && DateOnly.FromDateTime(DateTime.Now.AddYears(-1)).DayNumber > eolDate.DayNumber)
            {
                continue;
            }

            var release = await httpClient.GetFromJsonAsync<Release>(releaseSummary.ReleasesJson, ReleaseJsonSerializerContext.Default.Release);
            if (release is not null)
            {
                yield return release;
            }
            else
            {
                yield break;
            }
        }

        yield break;
    }

    public static DotnetVersion GetVersionDetail(Release release)
    {
        int supportDays = release.EolDate is null ? 0 : GetDaysAgo(release.EolDate);
        List<Report.Release> releases = new();
        DotnetVersion version = new(release.ChannelVersion, release.SupportPhase is "active" or "maintainence", release.EolDate ?? "unknown", supportDays, releases);
        bool hasSecurity = false;
        bool hasLatest = false;
        
        // Include latest and latest security release (which is often a single release)
        // It is important to include the latest CVE fixes
        foreach (ReleaseDetail releaseDetail in release.Releases)
        {
            if (hasSecurity)
            {
                break;
            }
            else if (hasLatest && !releaseDetail.Security)
            {
                continue;
            }
            
            var r = new Report.Release(releaseDetail.ReleaseVersion, releaseDetail.Security, releaseDetail.ReleaseDate, GetDaysAgo(releaseDetail.ReleaseDate, true), releaseDetail.Cves);
            releases.Add(r);

            hasSecurity = releaseDetail.Security;
            hasLatest = true;
        }

        return version;
    }   

    public static int GetDaysAgo(string date, bool isPositive = false)
    {
        bool success = DateTime.TryParse(date, out var day);
        var daysAgo = success ? (int)(day - DateTime.Now).TotalDays : 0;
        return isPositive ? Math.Abs(daysAgo) : daysAgo;
    }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ReleaseIndex))]
internal partial class ReleaseJsonSerializerContext : JsonSerializerContext
{
}