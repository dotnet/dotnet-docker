{{
    _ ARGS:
      top-header: The string to use as the top-level header.
}}{{ARGS["top-header"]}} Featured Tags

{{if match(SHORT_REPO, "samples")
:* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`^
elif match(SHORT_REPO, "monitor"):* `8` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8`
* `7` (Standard Support)
  * `docker pull {{FULL_REPO}}:7`
* `6` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:6`^
elif match(REPO, "monitor/base"):* `8` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8`^
else:* `8.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8.0`
* `7.0` (Standard Support)
  * `docker pull {{FULL_REPO}}:7.0`
* `6.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:6.0`}}
