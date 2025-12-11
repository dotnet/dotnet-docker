{{
    _ ARGS:
      top-header: The string to use as the top-level header.
}}{{ARGS["top-header"]}} Featured Tags

{{if match(SHORT_REPO, "samples")
:* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp-chiseled`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled`^
elif match(SHORT_REPO, "monitor"):* `10` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:10`
* `9` (Standard Support)
  * `docker pull {{FULL_REPO}}:9`
* `8` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8`^
elif match(REPO, "monitor/base"):* `9` (Standard Support)
  * `docker pull {{FULL_REPO}}:9`
* `8` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8`^
elif match(REPO, "aspire-dashboard"):* `13`
  * `docker pull {{FULL_REPO}}:13`^
elif match(REPO, "yarp"):* `2.3-preview`
  * `docker pull {{FULL_REPO}}:2.3-preview`^
else:* `10.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:10.0`
* `9.0` (Standard Support)
  * `docker pull {{FULL_REPO}}:9.0`
* `8.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8.0`}}
