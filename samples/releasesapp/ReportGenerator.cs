using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ReleaseJson;
using ReleaseValues;
using ReportJson;

namespace ReleaseReport;

public static class Generator
{
    public static async Task<Report> MakeReportAsync() =>
        new(DateTime.Today.ToShortDateString(), await GetVersionsAsync().ToListAsync());

    public static async IAsyncEnumerable<MajorVersion> GetVersionsAsync()
    {
        await foreach(MajorRelease release in GetMajorReleasesAsync())
        {
            int supportDays = release.EolDate is null ? 0 : GetDaysAgo(release.EolDate);
            bool supported = release.SupportPhase is "active" or "maintainence";
            MajorVersion version = new(release.ChannelVersion, supported, release.EolDate ?? "", supportDays, GetReleases(release).ToList());
            yield return version;
        }
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
    }

    public static MajorVersion GetVersion(MajorRelease release) =>
        new(release.ChannelVersion, 
            release.SupportPhase is "active" or "maintainence", 
            release.EolDate ?? "", 
            release.EolDate is null ? 0 : GetDaysAgo(release.EolDate), 
            GetReleases(release).ToList()
            );

    // Get first and first security release
    public static IEnumerable<PatchRelease> GetReleases(MajorRelease majorRelease)
    {
        bool securityOnly = false;
        
        foreach (Release release in majorRelease.Releases)
        {
            if (securityOnly && !release.Security)
            {
                continue;
            }
            
            yield return new(release.ReleaseDate, GetDaysAgo(release.ReleaseDate, true), release.ReleaseVersion, release.Security, release.CveList);

            if (release.Security)
            {
                yield break;
            }
            else if (!securityOnly)
            {
                securityOnly = true;
            }
        }
    }
   
    static int GetDaysAgo(string date, bool positiveNumber = false)
    {
        bool success = DateTime.TryParse(date, out var day);
        var daysAgo = success ? (int)(day - DateTime.Now).TotalDays : 0;
        return positiveNumber ? Math.Abs(daysAgo) : daysAgo;
    }

}

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.KebabCaseLower
)]
[JsonSerializable(typeof(ReleaseIndex))]
internal partial class ReleaseJsonSerializerContext : JsonSerializerContext
{
}
