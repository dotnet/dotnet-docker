{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme ^
    set templateQualifier to when(IS_PRODUCT_FAMILY,
        "samples",
        when(PARENT_REPO = "monitor", cat("monitor-", SHORT_REPO), SHORT_REPO))
}}{{ARGS["top-header"]}} Usage
{{ _ Special case while aspire-dashboard image doesn't have any samples. Remove when https://github.com/dotnet/dotnet-docker/issues/5335 is resolved. ^
if templateQualifier != "aspire-dashboard":
The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.
}}
{{InsertTemplate(join(["Use", templateQualifier, "md"], "."),
  [ "top-header": ARGS["top-header"], "readme-host": ARGS["readme-host"]])}}
