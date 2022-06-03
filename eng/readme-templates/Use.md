{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}} Usage

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

{{InsertTemplate(join(["Use", when(IS_PRODUCT_FAMILY, "product-family", SHORT_REPO), "md"], "."),
  [ "top-header": ARGS["top-header"], "readme-host": ARGS["readme-host"]])}}
