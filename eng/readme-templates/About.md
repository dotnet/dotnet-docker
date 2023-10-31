{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme ^
    set templateQualifier to when(IS_PRODUCT_FAMILY,
        "product-family",
        when(PARENT_REPO = "monitor", cat("monitor-", SHORT_REPO), SHORT_REPO))
}}{{ARGS["top-header"]}} About
{{if ARGS["readme-host"] = "mar":{{InsertTemplate("Announcement.md",
  [
    "leading-line-break": "true",
    "readme-host": ARGS["readme-host"]
  ])}}}}
{{InsertTemplate(join(["About", templateQualifier, "md"], "."), [ "top-header": ARGS["top-header"] ])}}

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.{{if templateQualifier != "sdk":

{{InsertTemplate("About.chiseled.md", [ "top-header": ARGS["top-header"] ])}}}}{{if templateQualifier = "aspnet":

{{InsertTemplate("About.composite-aspnet.md", [ "top-header": ARGS["top-header"] ])}}}}
