{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}You can run this image to launch a YARP instance.

YARP expects the config file to be in `/etc/yarp.config`, and listens by default on port 5000.

Example of configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ReverseProxy": {
    "Routes": {
      "route1": {
        "ClusterId": "cluster1",
        "Match": {
          "Path": "/aspnetapp/{**catch-all}"
        },
        "Transforms": [
            { "PathRemovePrefix": "/aspnetapp" }
        ]
      }
    },
    "Clusters": {
      "cluster1": {
        "Destinations": {
          "destination1": {
            "Address": "http://aspnetapp1:8080"
          }
        }
      }
    }
  }
}
```

It can then be used with the following command (where `my-config.config` is a file containing this configuration):

```bash
docker run --rm --name myaspnetapp -d -t  mcr.microsoft.com/dotnet/samples:aspnetapp 
docker run --rm -v $(pwd)/my-config.config:/etc/yarp.config -p 5000:5000 --link myaspnetapp:aspnetapp1 mcr.microsoft.com/dotnet/yarp:latest
```

This example will proxy every requests from `http://localhost:5000/aspnetapp` to the `mcr.microsoft.com/dotnet/samples:aspnetapp` container deployed.

The [YARP GitHub repository](https://github.com/dotnet/yarp/tree/main/samples/) contains more configuration samples.

For more details, see the [documentation](https://aka.ms/YarpDocumentation) for how to configure the image and documentation for the reverse proxy configuration.