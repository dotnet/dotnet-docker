# Run applications in as .NET Core SDK container

You can use containers to establish a .NET Core development environment with only Docker and an editor. The environment can be made to match your local machine, production or both.

The following examples demonstrate using `dotnet run` in a .NET Core SDK container. It builds an application from source and then launches it. You have to re-launch the container every time you want to observe source code changes.

Alternatively, you can use `dotnet watch run`. This command reruns the application within a running container, with every local code change.

## Requirements

The instructions assume that you have cloned the [repository](https://github.com/dotnet/dotnet-docker) locally.

You may need to enable [shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

Container scenarios that use volume mounting can produce conflicts between the `bin` and `obj` directories in local and container environments.  To avoid that, you need to use a different set of `obj` and `bin` folders for your container environment. The easiest way to do that is to copy a custom [Directory.Build.props](Directory.Build.props) into the directory you are using (like the `dotnetapp` directory in the following example), either via copying from this repo or downloading with the following command:

```console
curl -o Directory.Build.props https://raw.githubusercontent.com/dotnet/dotnet-docker/master/samples/Directory.Build.props
```

## Console app

The following example demonstrates using `dotnet watch run` with a console app in a .NET Core SDK container. This initial example is demonstrated on macOS. Instructions for all OSes follow.

The instructions assume you are in the `samples/dotnetapp` directory (due to the [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) `-v` syntax).

```console
% docker run --rm -it -v $(pwd):/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run

      Hello from .NET Core!
      __________________
                        \
                        \
                            ....
                            ....'
                            ....
                          ..........
                      .............'..'..
                  ................'..'.....
                .......'..........'..'..'....
                ........'..........'..'..'.....
              .'....'..'..........'..'.......'.
              .'..................'...   ......
              .  ......'.........         .....
              .                           ......
              ..    .            ..        ......
            ....       .                 .......
            ......  .......          ............
              ................  ......................
              ........................'................
            ......................'..'......    .......
          .........................'..'.....       .......
      ........    ..'.............'..'....      ..........
    ..'..'...      ...............'.......      ..........
    ...'......     ...... ..........  ......         .......
  ...........   .......              ........        ......
  .......        '...'.'.              '.'.'.'         ....
  .......       .....'..               ..'.....
    ..       ..........               ..'........
            ............               ..............
          .............               '..............
          ...........'..              .'.'............
        ...............              .'.'.............
        .............'..               ..'..'...........
        ...............                 .'..............
        .........                        ..............
          .....
  
Environment:
.NET Core 3.1.0
Linux 4.9.184-linuxkit #1 SMP Tue Jul 2 22:58:16 UTC 2019
```

You can test this working by simply editing [Program.cs](dotnetapp/Program.cs). If you make an observable change, you will see it. If you make a syntax error, you will see compiler errors.

The following instructions demonstrate this scenario in various environments.

## Linux or macOS

```console
docker run --rm -it -v $(pwd):/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run
```

## Windows using Linux containers

```console
docker run --rm -it -v %cd%:/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run
```

## Windows using Windows containers

```console
docker run --rm -it -v %cd%:c:\app\ -w \app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run
```

## ASP.NET Core App

The following example demonstrates using `dotnet watch run` with an ASP.NET Core app in a .NET Core SDK container. This initial example is demonstrated on macOS. Instructions for all OSes follow.

The instructions assume you are in the `samples/aspnetapp/aspnetapp` directory (due to the [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) `-v` syntax).

```console
% docker run --rm -it -p 8000:80 -v $(pwd):/app/ -w /app -e ASPNETCORE_URLS=http://+:80 -e ASPNETCORE_ENVIRONMENT=Development mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run --no-launch-profile

info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app
```

You can test this working by editing one of the [source files](aspnetapp/aspnetapp). If you make an observable change, you will see it. If you make a syntax error, you will see compiler errors.

Note: This example (and those in the instructions that follow) configure ASP.NET Core via environment variables and disable the use of a launch profile (none of the launch profiles are compatible with this scenario). Instructions are provided later in this document that add and use a new launch profile, which removes the need for specifying environment variables with the Docker CLI.

The following instructions demonstrate this scenario in various environments:

### Linux or macOS

```console
docker run --rm -it -p 8000:80 -v $(pwd):/app/ -w /app -e ASPNETCORE_URLS=http://+:80 -e ASPNETCORE_ENVIRONMENT=Development mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run --no-launch-profile
```

### Windows using Linux containers

```console
docker run --rm -it -p 8000:80 -v %cd%:/app/ -w /app -e ASPNETCORE_URLS=http://+:80 -e ASPNETCORE_ENVIRONMENT=Development mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run --no-launch-profile
```

You can use CTRL-C to terminate `dotnet watch`. Navigate to the site at `http://localhost:8000` in your browser.

### Windows using Windows containers

```console
docker run --rm -it -p 8000:80 -v %cd%:C:\app\ -w /app -e ASPNETCORE_URLS=http://+:80 -e ASPNETCORE_ENVIRONMENT=Development mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run --no-launch-profile
```

You can use CTRL-C to terminate `dotnet watch`.

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

### Using a launch profile to configure ASP.NET Core

The examples above use environment variables to configure ASP.NET Core. You can instead [configure ASP.NET Core with a launchSettings.json file](https://docs.microsoft.com/aspnet/core/fundamentals/environments). The [launchSettings.json file](aspnetapp/aspnetapp/Properties/launchSettings.json) in this app has been updated with a `container` profile that can be used instead of specifying environment variables with the docker CLI. You can see this profile used in the following example:

```console
% docker run --rm -it -p 8000:80 -v $(pwd):/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run --launch-profile container
```

The following JSON segment shows the `container` profile that was added to enable the previous command.

```json
"container": {
  "commandName": "Project",
  "launchBrowser": true,
  "applicationUrl": "http://+:80",
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  }
```

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
