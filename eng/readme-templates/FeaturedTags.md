{{
    _ ARGS:
      top-header: The string to use as the top-level header.
}}{{ARGS["top-header"]}} Featured Tags

{{if match(SHORT_REPO, "samples")
:* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`^
elif match(SHORT_REPO, "monitor"):{{if VARIABLES["branch"] = "nightly":* `7` (Preview)
  * `docker pull {{FULL_REPO}}:7`
* `6` (Preview)
  * `docker pull {{FULL_REPO}}:6`^
else:* `6` (Current)
  * `docker pull {{FULL_REPO}}:6`}}^
else:{{if VARIABLES["branch"] = "nightly":* `7.0` (Preview)
  * `docker pull {{FULL_REPO}}:7.0`
}}* `6.0` (Current, LTS)
  * `docker pull {{FULL_REPO}}:6.0`}}
