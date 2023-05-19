{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}}# Container sample: Run a web application

You can quickly run a container with a pre-built [.NET Docker image]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/samples"])}}), based on the [ASP.NET Core sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/README.md).

Type the following command to run a sample web application:

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.

{{ARGS["top-header"]}}# Composite images containers

Starting from .NET 8, a composite version of the ASP.NET images is being offered alongside the regular image. The main characteristics of these images are their smaller size on disk while keeping the performance of the default R2R setting. The caveat is that the composite images have tighter version coupling. This means the final app run on them cannot use handpicked custom versions of the framework and/or ASP.NET assemblies that are in-built into the composite binary.

For a full technical description on how the composites work, we have a [feature doc here](https://github.com/dotnet/runtime/blob/main/docs/design/features/readytorun-composite-format-design.md).
