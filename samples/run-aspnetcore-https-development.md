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

See [Hosting ASP.NET Core Images with Docker over
HTTPS](host-aspnetcore-https.md) for production scenarios.

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

ASP.NET Core uses [self-signed development certificates](https://learn.microsoft.com/aspnet/core/security/enforcing-ssl#trust-the-aspnet-core-https-development-certificate) for development.

The following instructions create a .NET development certificate (if one doesn't
already exist), export it to a `.pfx` file, and trust the certificate locally.

If you encounter any issues with your deveolpment certificate (for example, if
it is out of date), you can run `dotnet dev-certs https --clean` to clear all
development certificates from your machine.

> [!NOTE]
>
> - `<CREDENTIAL_PLACEHOLDER>` is used as a placeholder for a secure password
>   of your own choosing.
> - If the console returns "A valid HTTPS certificate is already present.", a
>   trusted certificate already exists on your machine. It will stll be
>   exported with the following commands.

### Windows

```pwsh
dotnet dev-certs https -ep ${env:USERPROFILE}\.aspnet\https\aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER> --trust
```

### Linux

See [Linux-specific considerations](https://learn.microsoft.com/aspnet/core/security/enforcing-ssl#linux-specific-considerations)
for trusting development certificates on Linux.

### macOS

Create a certificate directory with appropriate permissions:

```console
mkdir -p -m 700 ${HOME}/.aspnet/https
```

Generate, export, and trust the certificate:

```console
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER> --trust
```

## Building and running the sample with HTTPS

The instructions [bind mount](https://docs.docker.com/engine/storage/bind-mounts/)
certificates into containers. While you could add certificates into container
images with a `COPY` command in a Dockerfile, we don't recommend that approach.
It makes it harder to use the same image for testing with dev certificates and
hosting with production certificates. There is also a risk of certificate
disclosure if certificates are made part of container images.

Use the following instructions for your operating system configuration. The
commands assume that you are in the root of the repository.

### Using environment variables

ASP.NET Core looks for certificates in `~/.aspnet/https/` by default. This
directory is different depending on whether you are running the container image
as a non-root user or the `root` user. Since our images come with a non-root
user (`app`) by default, we'll use that user's home directory for the following
commands.

ASP.NET Core will also look in that directory for a certificate file that
matches the assembly name of your app. If you want to put your certificate in a
different location, or if you want to use a certificate name that doesn't match
the assembly name of your app, set the
`ASPNETCORE_Kestrel__Certificates__Default__Path` variable to your
certificate's path in the container image.

On Linux, ASP.NET Core looks for certificates in `~/.aspnet/https/`. This
directory is different depending on whether you are running the container image
as a non-root user or the `root` user. Since our images come with a non-root
user (`app`) by default, we'll use that user's home directory for the following
commands. If you are running your container as the `root` user, replace
`/home/app/` with the `root` user's home directory, `/root/`.

For Linux containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    -e ASPNETCORE_ENVIRONMENT=Development `
    # This should be the same password that you used when exporting the certificate
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="<CREDENTIAL_PLACEHOLDER>" `
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}/.aspnet/https:/home/app/.aspnet/https/ `
    # Uncomment if you are putting your cert somewhere besides ~/.aspnet/https/
    # -e ASPNETCORE_Kestrel__Certificates__Default__Path=/path/to/your/cert.pfx `
    mcr.microsoft.com/dotnet/samples:aspnetapp
```

Linux containers on macOS or Linux:

```bash
docker run --rm -it \
    -p 8001:8001 \
    -e ASPNETCORE_HTTPS_PORTS=8001 \
    -e ASPNETCORE_ENVIRONMENT=Development \
    # This should be the same password that you used when exporting the certificate
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="<CREDENTIAL_PLACEHOLDER>" \
    # Bind mount the location of your exported certificate
    -v ${HOME}/.aspnet/https/:/home/app/.aspnet/https/ \
    mcr.microsoft.com/dotnet/samples:aspnetapp
```

Windows containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    -e ASPNETCORE_ENVIRONMENT=Development `
    # This should be the same password that you used when exporting the certificate
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="<CREDENTIAL_PLACEHOLDER>" \
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}\.aspnet\https:C:\Users\ContainerUser\AppData\Roaming\ASP.NET\Https `
    mcr.microsoft.com/dotnet/samples:aspnetapp
```

### Using user secrets for certificate password

Instead of using the `ASPNETCORE_Kestrel__Certificates__Default__Password`, you
can use [.NET user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=linux)
to store the certificate password on your machine.

This will require modifications to the sample app, so make sure you've cloned
this repo or [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/main.zip).

```console
git clone https://github.com/dotnet/dotnet-docker/
```

First, initialize user secrets for your app, and set the certificate password.

```console
cd samples/aspnetapp
dotnet user-secrets init -p aspnetapp/aspnetapp.csproj
dotnet user-secrets -p aspnetapp/aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "<CREDENTIAL_PLACEHOLDER>"
```

Initializing user-secrets for the first time on a project will modify the
project file, so we need to re-build the sample image:

```pwsh
docker build --pull -t aspnetapp .
```

In Linux containers, .NET looks under the `~/.microsoft/usersecrets/` directory
for user secrets data. All you need to do is bind-mount your host machine's user
secrets directory to the container's filesystem in the correct location, similar
to what we did with the certificate above.

> [!NOTE]
>
> Similar to with certificates above, if you are running your container as the
> `root` user, replace `/home/app/` with the `root` user's home directory:
> `/root/`.

For Linux containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    -e ASPNETCORE_ENVIRONMENT=Development `
    # Use user secrets instead of certificate password environment variable
    -v ${env:APPDATA}/microsoft/UserSecrets/:/home/app/.microsoft/usersecrets `
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}/.aspnet/https/:/home/app/.aspnet/https/ `
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
    -v ${HOME}/.aspnet/https/:/home/app/.aspnet/https/ \
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
    -v ${env:USERPROFILE}\.aspnet\https:C:\Users\ContainerUser\AppData\Roaming\ASP.NET\Https `
    aspnetapp
```

After the application starts, navigate to `https://localhost:8001` in your web
browser.

## Troubleshooting

Be sure to check that the certificate you're using is trusted on the host, and
that it is not expired. You can start with navigating to
`https://localhost:8001` in the browser. If you're looking to test https with a
domain name (e.g. `https://contoso.com:8001`), the certificate would also need
the appropiate Subject Alternative Name included, and the DNS settings on the
host would need to be updated. In the case of using the generated dev
certificate, the trusted certificate will be issued from localhost and will not
have the SAN added.
