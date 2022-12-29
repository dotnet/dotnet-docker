{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}}# Container sample: Run a simple application

Enter the following command to run a console app in a container with a pre-built [.NET Docker image]({{InsertTemplate("Url.md", [ "repo": "dotnet/samples" "readme-host": ARGS["readme-host"] ])}}):

```console
docker run --rm mcr.microsoft.com/dotnet/samples
```

{{ARGS["top-header"]}}# Container sample: Run a web app

Enter the following command to run a console app in a container with a pre-built [.NET Docker image]({{InsertTemplate("Url.md",  [ "repo": "dotnet/samples" "readme-host": ARGS["readme-host"] ])}}):

```console
docker run -it --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

When the app starts, navigate to `http://localhost:8000` in a browser.

See [Hosting ASP.NET Core Images with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md) to use HTTPS with this image.
