# .NET Release Report App

This app demonstrates publishing an app published with [single file](https://learn.microsoft.com/dotnet/core/deploying/single-file/) deployment in containers.

A similar [web API sample](../AspNetCoreNativeAot/README.md) supports native AOT deployment. This app could also be deployed that way.

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
  "reportDate": "09/11/2026",
  "versions": [
    {
      "version": "11.0",
      "supported": false,
      "eolDate": "",
      "supportEndsInDays": 0,
      "releases": [
        {
          "releaseDate": "2026-09-08",
          "releasedDaysAgo": 3,
          "releaseVersion": "11.0.0-rc.1",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2026-69439",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69439"
            },
            {
              "cveId": "CVE-2026-71328",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-71328"
            },
            {
              "cveId": "CVE-2026-69522",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69522"
            },
            {
              "cveId": "CVE-2026-69304",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69304"
            },
            {
              "cveId": "CVE-2026-58649",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-58649"
            },
            {
              "cveId": "CVE-2026-69806",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69806"
            }
          ]
        }
      ]
    },
    {
      "version": "10.0",
      "supported": true,
      "eolDate": "2028-11-14",
      "supportEndsInDays": 794,
      "releases": [
        {
          "releaseDate": "2026-09-08",
          "releasedDaysAgo": 3,
          "releaseVersion": "10.0.12",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2026-69439",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69439"
            },
            {
              "cveId": "CVE-2026-71328",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-71328"
            },
            {
              "cveId": "CVE-2026-69522",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69522"
            },
            {
              "cveId": "CVE-2026-69304",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69304"
            },
            {
              "cveId": "CVE-2026-58649",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-58649"
            },
            {
              "cveId": "CVE-2026-69806",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69806"
            }
          ]
        }
      ]
    },
    {
      "version": "9.0",
      "supported": true,
      "eolDate": "2026-11-10",
      "supportEndsInDays": 59,
      "releases": [
        {
          "releaseDate": "2026-09-08",
          "releasedDaysAgo": 3,
          "releaseVersion": "9.0.20",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2026-69439",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69439"
            },
            {
              "cveId": "CVE-2026-71328",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-71328"
            },
            {
              "cveId": "CVE-2026-69522",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69522"
            },
            {
              "cveId": "CVE-2026-69304",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69304"
            },
            {
              "cveId": "CVE-2026-58649",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-58649"
            },
            {
              "cveId": "CVE-2026-69806",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69806"
            }
          ]
        }
      ]
    },
    {
      "version": "8.0",
      "supported": true,
      "eolDate": "2026-11-10",
      "supportEndsInDays": 59,
      "releases": [
        {
          "releaseDate": "2026-09-08",
          "releasedDaysAgo": 3,
          "releaseVersion": "8.0.31",
          "security": true,
          "cveList": [
            {
              "cveId": "CVE-2026-69439",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69439"
            },
            {
              "cveId": "CVE-2026-71328",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-71328"
            },
            {
              "cveId": "CVE-2026-69522",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69522"
            },
            {
              "cveId": "CVE-2026-69304",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-69304"
            },
            {
              "cveId": "CVE-2026-58649",
              "cveUrl": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2026-58649"
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
