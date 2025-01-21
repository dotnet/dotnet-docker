{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}You can run this image to launch a YARP instance.

See [Basic YARP Sample](https://github.com/microsoft/reverse-proxy/tree/main/samples/BasicYarpSample) for a sample config to use with this container.

YARP expects the config file to be in `/etc/reverse-proxy.config`, and listens by default on port 5000.

It can be run with this command:

```console
docker run -v $(pwd)/my-config.config:/etc/reverse-proxy.config -p 5000:5000 mcr.microsoft.com/dotnet/reverse-proxy:latest
```

See [documentation](https://microsoft.github.io/reverse-proxy/articles/index.html) for how to configure the image and documentation for the reverse proxy configuration.