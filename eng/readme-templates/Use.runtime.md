{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}}# Container sample: Run a simple application

You can quickly run a container with a pre-built [.NET Docker image]({{InsertTemplate("Url.md", [ "repo": "dotnet/samples" "readme-host": ARGS["readme-host"] ])}}), based on the [.NET console sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/README.md).

Type the following command to run a sample console application:

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```
