# Developing ASP.NET Core Applications with Docker over HTTPS

ASP.NET Core uses [HTTPS by default](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl). [HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on [certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust, identity, and encryption.

This document demonstrates how to develop ASP.NET Core applications with HTTPS in Docker containers. It's recommended to try the [ASP.NET Core Docker Sample](README.md) first, which is simpler because the container only exposes HTTP. This more basic tutorial will help you validate that you have the sample working correctly, before adding the complication of certificates.

See [Hosting ASP.NET Core Images with Docker over HTTPS](host-aspnetcore-https.md) for production scenarios.

The samples are written for `cmd.exe`. PowerShell users will need to special case the environment variables that are used in the instructions.

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with git, using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/main.zip).

## Certificates

ASP.NET Core uses [self-signed development certificates](https://en.wikipedia.org/wiki/Self-signed_certificate) for development. Self-signed certificates are easy and free to create.

The instructions volume mount certificates into containers. You can add certificates into container images with a `COPY` command in a Dockerfile. This approach isn't recommended. It makes it harder to use the same image for testing with dev certificates and hosting with production certificates. There's also a  significant risk of certificate disclosure if certificates are made part of container images.

## Application Secrets

These instructions assume that your project is configured for [application secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets). The primary requirement is a [UserSecretsId](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/aspnetapp/aspnetapp.csproj#L5) element in your project file. If you're using the ASP.NET Core sample in this repo, you don't need to do anything. It's already correctly configured. If you're using your own project file, add an `UserSecretsId` element.

You can add the element manually or use Visual Studio to do it for you. The following image demonstrates the experience in Visual Studio.

![Manage user secrets in Visual Studio](https://user-images.githubusercontent.com/7681382/39641521-85d4a7b4-4f9c-11e8-9466-d1ff56db33cb.png)

The format of the `UserSecretsId` content doesn't matter. The sample in this repo used [Random String Generator](https://www.random.org/strings/?num=6&len=20&digits=on&unique=on&format=html&rnd=new) to produce a unique string.

> Note: `User Secrets` and `Application Secrets` terms are used interchangebly.

## Building and Running the Sample with HTTPS

Use the following instructions, for your operating system configuration. The commands assume that you are in the root of the repository.

> Note: The sample includes a banner to accept a cookie policy. When switching between HTTP and HTTPS, you may see the banner repeatedly. Delete the cookie for the site in `Developer Tools` in this case.

![Developer Tools -- Delete cookie](https://user-images.githubusercontent.com/2608468/40246148-875fee5a-5a7c-11e8-9728-7da89a491014.png)

Further, if you're loading SSL certificates and trimming assemblies as part of the publish, you'll also need to update the project file for the sample.  See details for how you can [support SSL certificates](https://docs.microsoft.com/en-us/dotnet/core/deploying/trim-self-contained#support-for-ssl-certificates).

### Windows using Linux containers

The following example uses PowerShell.

Navigate to sample:

```console
cd samples\aspnetapp
```

Generate cert and configure local machine:

```console
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

> Note: The certificate name, in this case *aspnetapp*.pfx must match the project assembly name.

> Note: `<CREDENTIAL_PLACEHOLDER>` is used as a stand-in for a password of your own choosing.

> Note: If console returns "A valid HTTPS certificate is already present.", a trusted certificate already exists in your store. It can be exported using MMC Console.

Configure application secrets, for the certificate:

```console
dotnet user-secrets -p aspnetapp\aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "<CREDENTIAL_PLACEHOLDER>"
```

> Note: The password must match the password used for the certificate.

Build a container image:

```console
docker build --pull -t aspnetapp .
```

Run the container image with ASP.NET Core configured for HTTPS:

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v $env:APPDATA\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v $env:USERPROFILE\.aspnet\https:/root/.aspnet/https/ aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

### macOS

```console
cd samples\aspnetapp
```

Generate cert and configure local machine:

```console
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

> Note: The certificate name, in this case *aspnetapp*.pfx must match the project assembly name.

> Note: `<CREDENTIAL_PLACEHOLDER>` is used as a stand-in for a password of your own choosing.

Configure application secrets, for the certificate:

```console
dotnet user-secrets -p aspnetapp/aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "<CREDENTIAL_PLACEHOLDER>"
```

> Note: The password must match the password used for the certificate.

Build a container image:

```console
docker build --pull -t aspnetapp .
```

Run the container image with ASP.NET Core configured for HTTPS:

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v ${HOME}/.microsoft/usersecrets/:/root/.microsoft/usersecrets -v ${HOME}/.aspnet/https:/root/.aspnet/https/ aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

### Linux

```console
cd samples\aspnetapp
```

Generate cert and configure local machine:

```console
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
```

> Note: `dotnet dev-certs https --trust` is only supported on macOS and Windows. You need to trust certs on Linux in the way that is supported by your distro. It is likely that you need to trust the certificate in your browser.

> Note: The certificate name, in this case *aspnetapp*.pfx must match the project assembly name.

> Note: `<CREDENTIAL_PLACEHOLDER>` is used as a stand-in for a password of your own choosing.

Configure application secrets, for the certificate:

```console
dotnet user-secrets -p aspnetapp/aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "<CREDENTIAL_PLACEHOLDER>"
```

Build a container image:

```console
docker build --pull -t aspnetapp .
```

Run the container image with ASP.NET Core configured for HTTPS:

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_Kestrel__Certificates__Development__Password="<CREDENTIAL_PLACEHOLDER>" -v ${HOME}/.microsoft/usersecrets/:/root/.microsoft/usersecrets -v ${HOME}/.aspnet/https:/root/.aspnet/https/ aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

### Windows using Windows containers

The following example uses PowerShell.

Navigate to sample:

```console
cd samples\aspnetapp
```

Generate cert and configure local machine:

```console
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

> Note: The certificate name, in this case *aspnetapp*.pfx must match the project assembly name.

> Note: `<CREDENTIAL_PLACEHOLDER>` is used as a stand-in for a password of your own choosing.

> Note: If console returns "A valid HTTPS certificate is already present.", a trusted certificate already exists in your store. It can be exported using MMC Console.

Configure application secrets, for the certificate:

```console
dotnet user-secrets -p aspnetapp\aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "<CREDENTIAL_PLACEHOLDER>"
```

> Note: The password must match the password used for the certificate.

Build a container image:

```console
docker build --pull -t aspnetapp .
```

Run the container image with ASP.NET Core configured for HTTPS.

```console
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v $env:APPDATA\microsoft\UserSecrets\:C:\Users\ContainerUser\AppData\Roaming\microsoft\UserSecrets -v $env:USERPROFILE\.aspnet\https:C:\Users\ContainerUser\AppData\Roaming\ASP.NET\Https aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

> In the case of using https, be sure to check the certificate you're using is trusted on the host. You can start with navigating to https://localhost:8001 in the browser. If you're looking to test https with a domain name (e.g. https://contoso.com:8001), the certificate would also need the appropiate Subject Alternative Name included, and the DNS settings on the host would need to be updated. In the case of using the generated dev certificate, the trusted certificate will be issued from localhost and will not have the SAN added.

