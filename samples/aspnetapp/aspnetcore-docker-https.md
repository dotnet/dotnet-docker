# Hosting ASP.NET Core with Docker over HTTPS

ASP.NET Core 2.1 uses [HTTPS by default](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl). [HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on [certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust, identity and encryption.

ASP.NET Core uses [self-signed dev certificates](https://en.wikipedia.org/wiki/Self-signed_certificate) for development. Self-signed certificates are easy and free to create. For [production hosting](https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/), you need a certificate from a [certificate authority](https://en.wikipedia.org/wiki/Certificate_authority). [Let's Encrypt](https://letsencrypt.org/) is a certificate authority that offers free certificates.

This document explains how to configure Docker containers to use certificates. The instructions are based on the [ASP.NET Core Docker Sample](README.md). These instructions use the same sample but are more brief. Consult the [ASP.NET Core Docker Sample](README.md) for help if these instructions are not working.

The instructions volume mount certificates into containers. You can add certificates into container images with a `COPY` command in a Dockerfile. This approach is not recommended. It makes it harder to use the same image for testing with dev certificates and hosting with production certificates. There is also a  significant risk of certificate disclosure if certificates are made part of container images.

The samples are written for `cmd.exe`. PowerShell users will need to special case the environment variables that are used in the instructions.

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with git, using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Host ASP.NET Core over HTTP with Docker

The sample can be run over HTTP to test the basic function of ASP.NET Core. Testing with HTTP first is a good idea to make sure that the sample is working correctly before testing with cerificates. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp .
docker run --name aspnetcore_sample --rm -it -p 8000:80 aspnetapp
```

You can also test the same sample as a pre-built image with the following instructions:

```console
docker pull microsoft/dotnet-samples:aspnetapp
docker run --name aspnetcore_sample --rm -it -p 8000:80 microsoft/dotnet-samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

> Note: The `-p` argument maps port 8000 on your local machine to port 80 in the container (the form of the port mapping is `host:container`). See the [Docker run reference](https://docs.docker.com/engine/reference/commandline/run/) for more information on commandline parameters. In some cases, you might see an error because the host port you select is already in use. Choose a different port in that case.

> Note: The sample includes a banner to accept a cookie policy. When switching between HTTP and HTTPS, you may see the banner repeatedly. Switch to *InPrivate*/*incognito* mode in that case.

## Host ASP.NET Core over HTTPS with Dev Certs

In development, you can use dev certs to [Use HTTPS with ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl). There are three steps to follow:

1. Generate a dev cert for your site, with a password.
2. Generate a user secret for your site, which includes the dev cert password.
3. Configure the container to volume mount the cert and user secrets and correctly configure ASP.NET Core to use them.

### Application secrets

These instructions assume that your project is configured for [application secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets). The primary requirement is a [UserSecretsId](https://github.com/dotnet/dotnet-docker/blob/update-sample-to-latest/samples/aspnetapp/aspnetapp/aspnetapp.csproj#L5) element in your project file. If you are using your own project file, please add an `UserSecretsId` element. If you are using the ASP.NET Core sample in this repo, you don't need to do anything,since it is already correctly configured.

You can add the element manually or use Visual Studio to do it for you. The following image demonstrates the user experience in Visual Studio.

![Manage user secrets in Visual Studio](https://user-images.githubusercontent.com/7681382/39641521-85d4a7b4-4f9c-11e8-9466-d1ff56db33cb.png)

The format of the `UserSecretsId` doesn't matter. The sample in this repo used [Random String Generator](https://www.random.org/strings/?num=6&len=20&digits=on&unique=on&format=html&rnd=new) to produce a unique string.

> Note: `User Secrets` and `Application Secrets` terms are used interchangebly.

### Testing with Dev Certs

**Windows** using **Linux containers**

Configure and build image:

```console
cd samples\aspnetapp
dotnet user-secrets -p aspnetapp\aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "crypticpassword"
docker build --pull -t aspnetapp .
```

> Note: `crypticpassword` is used as a stand-in for a password of your own choosing.

Generate cert and configure local machine:

```console
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p crypticpassword
dotnet dev-certs https --trust
```

Run the container image with ASP.NET Core configured for HTTPS:

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v %APPDATA%\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v %USERPROFILE%\.aspnet\https:/root/.aspnet/https/ aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

**macOS or Linux** using **Linux containers**

Configure and build image:

```console
cd samples/aspnetapp
dotnet user-secrets -p aspnetapp/aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "crypticpassword"
docker build --pull -t aspnetapp .
```

Generate cert and configure local machine:

```console
dotnet dev-certs https --trust
```

Run the container image with ASP.NET Core configured for HTTPS:

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v ${HOME}/.microsoft/UserSecrets/:/root/.microsoft/usersecrets -v ${HOME}/.aspnet/https:/root/.aspnet/https/ aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

**Windows** using **Windows containers**

Configure and build image:

```console
cd samples\aspnetapp
dotnet user-secrets -p aspnetapp\aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "crypticpassword"
docker build --pull -t aspnetapp .
```

> Note: `crypticpassword` is used as a stand-in for a password of your own choosing.

Generate cert and configure local machine:

```console
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p crypticpassword
dotnet dev-certs https --trust
```

Run the container image with ASP.NET Core configured for HTTPS, for Windows Server 2016 (as is the case in [Dockerfile.nanoserver-sac2016](Dockerfile.nanoserver-sac2016)):

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v %APPDATA%\microsoft\UserSecrets\:C:\Users\ContainerAdministrator\AppData\Roaming\microsoft\UserSecrets -v %USERPROFILE%\.aspnet\https:C:\Users\ContainerAdministrator\AppData\Roaming\ASP.NET\Https aspnetapp
```

Run the container image with ASP.NET Core configured for HTTPS, for Windows Server 2016, version 1709 or higher:

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v %APPDATA%\microsoft\UserSecrets\:C:\Users\ContainerUser\AppData\Roaming\microsoft\UserSecrets -v %USERPROFILE%\.aspnet\https:C:\Users\ContainerUser\AppData\Roaming\ASP.NET\Https aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.
