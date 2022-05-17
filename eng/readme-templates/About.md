{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      is-mcr: Indicates whether the readme is being generated for the MCR portal
}}{{ARGS["top-header"]}} About
{{if ARGS["is-mcr"]:
{{InsertTemplate("Announcement.md")}}}}{{
InsertTemplate(join(["About", when(IS_PRODUCT_FAMILY, "product-family", SHORT_REPO), "md"], "."))}}

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.
