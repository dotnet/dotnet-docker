{{
    _ ARGS:
      top-header: The string to use as the top-level header.
}}{{ARGS["top-header"]}} About

{{if match(PARENT_REPO, "nightly") || VARIABLES["branch"] = "nightly"
:The images from the dotnet/{{if IS_PRODUCT_FAMILY:nightly^else:{{PARENT_REPO}}}} repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).

See [dotnet](https://hub.docker.com/_/microsoft-dotnet/) for images with official releases of [.NET](https://github.com/dotnet/core).

}}{{InsertTemplate(join(["About", when(IS_PRODUCT_FAMILY, "product-family", SHORT_REPO), "md"], "."))}}

Watch [discussions](https://github.com/dotnet/dotnet-docker/discussions/categories/announcements) for Docker-related .NET announcements.
