# Developing ASP.NET Core Applications with Docker over HTTPS

ASP.NET Core uses [HTTPS by
default](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl).
[HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on
[certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust,
identity, and encryption.

This document demonstrates how to develop ASP.NET Core applications with HTTPS
in Docker containers. It's recommended to try the [ASP.NET Core Docker
Sample](README.md) first, which is simpler because the container only exposes
HTTP. This more basic tutorial will help you validate that you have the sample
working correctly, before adding the complication of certificates.

See [Hosting ASP.NET Core images with Docker over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https)
for production scenarios.

The Windows examples below are written for PowerShell. CMD users will need to
change the format of the environment variables in the instructions from
`$env:USERPROFILE` to `%USERPROFILE%`.

This example requires [Docker
Desktop](https://www.docker.com/products/docker-desktop/). Make sure you have
the latest version.

## Getting the sample image

Pull the sample image:

```console
docker pull mcr.microsoft.com/dotnet/samples:aspnetapp
```

Alternatively, you can build the sample image locally:

```console
docker build --pull -t mcr.microsoft.com/dotnet/samples:aspnetapp 'https://github.com/dotnet/dotnet-docker.git#:samples/aspnetapp'
```

## Create and trust a development certificate

Follow the instructions for creating a [.NET development certificate](https://learn.microsoft.com/dotnet/core/tools/dotnet-dev-certs)
from [Running pre-built container images with HTTPS](https://learn.microsoft.com/aspnet/core/security/docker-https#running-pre-built-container-images-with-https).

## Enable HTTPS using environment variables

See [Hosting ASP.NET Core images with Docker over HTTPS](https://learn.microsoft.com/aspnet/core/security/docker-https).
If you don't want to use environment variables to store your development
certificate password, continue reading.

## Using user secrets for certificate password

If you don't want to use environment variables to store your development
certificate password, you can use [.NET user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=linux)
instead to store the certificate password on your machine.

This will require modifications to the `aspnetapp` sample, so make sure you've cloned
this repo or [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/main.zip).

```console
git clone https://github.com/dotnet/dotnet-docker/
```

First, initialize user secrets for your app, and set the certificate password.

```console
cd samples/aspnetapp
dotnet user-secrets init -p aspnetapp/aspnetapp.csproj
dotnet user-secrets -p aspnetapp/aspnetapp.csproj set "Kestrel:Certificates:Default:Password" "<CREDENTIAL_PLACEHOLDER>"
```

Initializing user-secrets for the first time on a project will modify the
project file, so we need to re-build the sample image:

```pwsh
docker build --pull -t aspnetapp .
```

In Linux containers, .NET looks under the `~/.microsoft/usersecrets/` directory
for user secrets data. All you need to do is bind-mount your host machine's
user secrets directory to the container's filesystem in the correct location,
similar to what we did with the certificate above. if you are running your
container as the `root` user, replace `/home/app/` with the `root` user's home
directory, `/root/`.

For Linux containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    -e ASPNETCORE_ENVIRONMENT=Development `
    # Use user secrets instead of certificate password environment variable
    -v ${env:APPDATA}/microsoft/UserSecrets/:/home/app/.microsoft/usersecrets `
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}/.aspnet/https/:/https/ `
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx `
    aspnetapp
```

Linux containers on macOS or Linux:

```bash
docker run --rm -it \
    -p 8001:8001 \
    -e ASPNETCORE_HTTPS_PORTS=8001 \
    -e ASPNETCORE_ENVIRONMENT=Development \
    # Use user secrets instead of certificate password environment variable
    -v ${HOME}/.microsoft/usersecrets/:/home/app/.microsoft/usersecrets \
    # Bind mount the location of your exported certificate
    -v ${HOME}/.aspnet/https/:/https/ \
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx \
    aspnetapp
```

Windows containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    -e ASPNETCORE_ENVIRONMENT=Development `
    # Use user secrets instead of certificate password environment variable
    -v ${env:APPDATA}\microsoft\UserSecrets\:C:\Users\ContainerUser\AppData\Roaming\microsoft\UserSecrets `
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}\.aspnet\https:C:\https `
    aspnetapp
```

After the application starts, navigate to `https://localhost:8001` in your web
browser.
