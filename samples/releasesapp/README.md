# .NET Release Report App

This app demonstrates publishing an app published with [single file](https://learn.microsoft.com/dotnet/core/deploying/single-file/) deployment in containers.

A similar [web API sample](../releasesapi/README.md) supports native AOT deployment. This app could also be deployed that way.

## Usage

You can build and run the sample:

```bash
docker build --pull -t app .
docker run --rm app
```

It will produce output similar to: https://gist.github.com/richlander/4701a33592abd021f767644974c0ced6

## App

The app is intended as a sort of compliance report for .NET. The report includes supported major releases and those recently out of support. It includes the latest and latest security patch versions for each of those major releases. 

This same information is available from the [release JSON](https://github.com/dotnet/core/blob/main/release-notes/releases-index.json) files that the team maintains, but that requires a bit of code to provide the same report.

## Dockerfiles

The sample includes support for three distributions:

- [Alpine](Dockerfile.alpine)
- [Debian](Dockerfile)
- [Ubuntu Chiseled](Dockerfile.chiseled)
