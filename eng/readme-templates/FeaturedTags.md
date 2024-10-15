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
elif match(SHORT_REPO, "monitor"):* `9` (Standard Support)
  * `docker pull {{FULL_REPO}}:9`
* `8` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8`
* `6` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:6`^
elif match(REPO, "monitor/base"):* `9` (Standard Support)
  * `docker pull {{FULL_REPO}}:9`
* `8` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8`^
elif match(REPO, "aspire-dashboard"):* `8.2`
  * `docker pull {{FULL_REPO}}:8.2`^
else:* `9.0` (Standard Support)
  * `docker pull {{FULL_REPO}}:9.0`
* `8.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:8.0`
* `6.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:6.0`}}
