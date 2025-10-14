# .NET Release Report App

This app demonstrates publishing an app published with [single file](https://learn.microsoft.com/dotnet/core/deploying/single-file/) deployment in containers.

A similar [web API sample](../releasesapi/README.md) supports native AOT deployment. This app could also be deployed that way.

## Usage

You can build and run the sample:

```bash
docker build --pull -t releasesapp .
docker run --rm releasesapp
```

It will produce output similar to this:

<details>
<summary>Command output</summary>

```json
{
  "reportDate": "09/10/2025",
  "versions": [
    {
      "version": "10.0",
      "supported": false,
      "eolDate": "",
      "supportEndsInDays": 0,
      "releases": [
        {
          "releaseDate": "2025-09-09",
          "releasedDaysAgo": 2,
          "releaseVersion": "10.0.0-rc.1",
          "security": false,
          "cveList": []
        }
      ]
    },
    {
      "version": "9.0",
      "supported": true,
      "eolDate": "2026-05-12",
      "supportEndsInDays": 242,
      "releases": [
        {
          "releaseDate": "2025-09-09",
          "releasedDaysAgo": 2,
          "releaseVersion": "9.0.9",
          "security": false,
          "cveList": []
        },
        {
          "releaseDate": "2025-06-10",
          "releasedDaysAgo": 93,
          "releaseVersion": "9.0.6",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2025-30399",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2025-30399"
            }
          ]
        }
      ]
    },
    {
      "version": "8.0",
      "supported": true,
      "eolDate": "2026-11-10",
      "supportEndsInDays": 424,
      "releases": [
        {
          "releaseDate": "2025-09-09",
          "releasedDaysAgo": 2,
          "releaseVersion": "8.0.20",
          "security": false,
          "cveList": []
        },
        {
          "releaseDate": "2025-06-10",
          "releasedDaysAgo": 93,
          "releaseVersion": "8.0.17",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2025-30399",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2025-30399"
            }
          ]
        }
      ]
    },
    {
      "version": "6.0",
      "supported": false,
      "eolDate": "2024-11-12",
      "supportEndsInDays": -303,
      "releases": [
        {
          "releaseDate": "2024-11-12",
          "releasedDaysAgo": 303,
          "releaseVersion": "6.0.36",
          "security": false,
          "cveList": []
        },
        {
          "releaseDate": "2024-10-08",
          "releasedDaysAgo": 338,
          "releaseVersion": "6.0.35",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2024-43483",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2024-43483"
            },
            {
              "cveId": "CVE-2024-43485",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2024-43485"
            },
            {
              "cveId": "CVE-2024-43484",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2024-43484"
            }
          ]
        }
      ]
    }
  ]
}
```

</details>

## App

The app is intended as a sort of compliance report for .NET. The report includes supported major releases and those recently out of support. It includes the latest and latest security patch versions for each of those major releases.

This same information is available from the [release JSON](https://github.com/dotnet/core/blob/main/release-notes/releases-index.json) files that the team maintains, but that requires a bit of code to provide the same report.

## Dockerfiles

The sample includes support for the following distributions:

- [Alpine](Dockerfile.alpine)
- [Azure Linux Distroless](Dockerfile.azurelinux-distroless)
- [Ubuntu](Dockerfile)
- [Ubuntu Chiseled](Dockerfile.chiseled)
