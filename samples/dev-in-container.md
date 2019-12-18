# Develop .NET Core applications in a container

You can use containers to establish a .NET Core development environment with only Docker and an editor. The environment can be made to match your local machine, production or both.

The following example demonstrates using `dotnet watch run` in a .NET Core SDK container. This command reruns the application with every local code change. This scenario works for both console applications and websites. The syntax differs for Windows and Linux containers.

## Requirements

The instructions assume that you have cloned the repository locally.

You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

To avoid conflicts between container usage and your local environment, you need to use a different set of `obj` and `bin` folders for your container environment. The easiest way to do that is to copy a custom [Directory.Build.props](Directory.Build.props) into the directory you are using, either via copying from this repo or downloading with the following command:

```console
curl -o Directory.Build.props https://raw.githubusercontent.com/dotnet/dotnet-docker/master/samples/dotnetapp/Directory.Build.props
```

## Console app

The following example demonstrates using `dotnet watch run` with a console app in a .NET Core SDK container. This initial example is demonstrated on macOS and the the following 

The instructions assume you are in the `samples/dotnetapp` directory (due to the [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) `-v` syntax).

```console
% docker run --rm -it -v $(pwd):/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet watch run
watch : Polling file watcher is enabled
watch : Started

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
docker run --rm -it -v $(pwd):/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet watch run
```

## Windows using Linux containers

```console
docker run --rm -it -v %cd%:/app/ -w /app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet watch run
```

## Windows using Windows containers

```console
docker run --rm -it -v %cd%:c:\app\ -w \app mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet watch run
```

## ASP.NET Core App

The following example demonstrates using `dotnet watch run` with a console app in a .NET Core SDK container on macOS. 

The instructions assume you are in the `samples/aspnetapp` directory (due to the [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) `-v` syntax).

```console
rich@thundera samples % docker run --rm -it -p 8000:80 -v ~/git/dotnet-docker/samples/aspnetapp:/app/ -w /app/aspnetapp -e ASPNETCORE_URLS=http://+:80 mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
watch : Polling file watcher is enabled
watch : Started
warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
      Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {d6c0ae93-2f64-481c-908e-b163dd5c0163} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app/aspnetapp
```

You can test this working by simply editing [Program.cs](dotnetapp/Program.cs). If you make an observable change, you will see it. If you make a syntax error, you will see compiler errors.

The following instructions demonstrate this scenario in various configurations.

### Linux or macOS

```console
docker run --rm -it -p 8000:80 -v ~/git/dotnet-docker/samples/aspnetapp:/app/ -w /app/aspnetapp -e ASPNETCORE_URLS=http://+:80 mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
```


### Windows using Linux containers

```console
docker run --rm -it -p 8000:80 -v c:\git\dotnet-docker\samples\aspnetapp:/app/ -w /app/aspnetapp -e ASPNETCORE_URLS=http://+:80 mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
```

You can use CTRL-C to terminate `dotnet watch`. Navigate to the site at `http://localhost:8000` in your browser.

### Windows using Windows containers

```console
docker run --rm -it -p 8000:80 -v c:\git\dotnet-docker\samples\aspnetapp:c:\app\ -w \app\aspnetapp -e ASPNETCORE_URLS=http://+:80 --name aspnetappsample mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch run
```

You can use CTRL-C to terminate `dotnet watch`.

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

## Updating the site while the container is running

You can demo a relaunch of the site by changing the About controller method in `HomeController.cs`, waiting a few seconds for the site to recompile and then visit `http://localhost:8000/Home/About`

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
