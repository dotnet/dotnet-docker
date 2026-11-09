{{
    _ ARGS:
      top-header: The string to use as the top-level header.
}}{{ARGS["top-header"]}} Featured Tags

{{if match(SHORT_REPO, "samples")
:* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/ConsoleApp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp-chiseled`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/AspNetCoreRazorApp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled`^
elif match(SHORT_REPO, "monitor") || match(REPO, "monitor/base"):* `10` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:10`^
elif match(REPO, "yarp"):* `2.3-preview`
  * `docker pull {{FULL_REPO}}:2.3-preview`^
else:* `11.0` (Standard Support)
  * `docker pull {{FULL_REPO}}:11.0`
* `10.0` (Long-Term Support)
  * `docker pull {{FULL_REPO}}:10.0`}}
