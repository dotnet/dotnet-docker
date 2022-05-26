{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}} About
{{if ARGS["readme-host"] = "mar":{{InsertTemplate("Announcement.md",
  [
    "leading-line-break": "true",
    "readme-host": ARGS["readme-host"]
  ])}}}}
{{InsertTemplate(join(["About", when(IS_PRODUCT_FAMILY, "product-family", SHORT_REPO), "md"], "."))}}

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.
