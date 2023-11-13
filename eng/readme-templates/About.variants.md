{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme ^
    set repo to when(IS_PRODUCT_FAMILY,
        "product-family",
        when(PARENT_REPO = "monitor", cat("monitor-", SHORT_REPO), SHORT_REPO))
}}{{ARGS["top-header"]}} Image Variants

.NET container images have several variants that offer different combinations of flexibility and deployment size.
The [Image Variants documentation](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-variants.md) contains a summary of the image variants and their use-cases.
