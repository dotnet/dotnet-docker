# Hosting ASP.NET Core with Docker over HTTPS

ASP.NET Core 2.1 uses [HTTPS by default](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl). [HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on [certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust, identity and encryption.

ASP.NET Core uses [self-signed dev certificates](https://en.wikipedia.org/wiki/Self-signed_certificate) for development. Self-signed certificates are easy and free to create. For [production hosting](https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/), you need a certificate from a [certificate authority](https://en.wikipedia.org/wiki/Certificate_authority). [Let's Encrypt](https://letsencrypt.org/) is a certificate authority that offers free certificates.

This document explains how to configure Docker containers to use certificates, for both development and production. The instructions are based on the [ASP.NET Core Docker Sample](README.md). These instructions are more breif. Consult the [ASP.NET Core Docker Sample](README.md) for help if these instructions are not working.

The instructions volume mount certificates into containers. You can add certificates into container images with a `COPY` command in a Dockerfile. This approach is not recommended. It prevents using the same image for testing with dev certificates and hosting with production certificates. There is also a  significant risk of certificate disclosure if certificates are made part of container images.

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with git, using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Host ASP.NET Core over HTTP with Docker

The sample can be run over HTTP to test the basic function of ASP.NET Core. Testing HTTP first is a good idea to make sure that the sample is working correctly before testing with cerificates. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp .
docker run --name aspnetcore_sample --rm -it -p 8000:80 aspnetapp
```

You can also test the same sample as a pre-built image with the following instructions:

```console
docker pull microsoft/dotnet-samples:aspnetapp
docker run --rm -it -p 8000:80 microsoft/dotnet-samples:aspnetapp
```

Note: The sample includes a banner to accept a cookie policy. When switching between HTTP and HTTPS, you may see the banner repeatedly. Switch to "InPrivate"/"incognito" mode in that case.

## Host ASP.NET Core over HTTPS with Dev Certs

In development, you can use dev certs to [Use HTTPS with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl). There are three steps to follow:

1. Generate a dev cert for your site, with a password.
2. Generate a user secret for your site, which includes the password.
3. Configure the container to volume mount the cert and user secrets and correctly configure ASP.NET Core to use them.

The following instructions perform these three steps.

**Windows** using **Linux containers**

```console
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p crypticpassword
dotnet user-secrets set "Kestrel:Certificates:Development:Password" "rich"

```

## Host ASP.NET Core over HTTPS with Production Certs



dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p rich
dotnet user-secrets set "Kestrel:Certificates:Development:Password" "rich"
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v %APPDATA%\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v %USERPROFILE%\.aspnet\https:/root/.aspnet/https/ aspnetapp



You can build the sample based on the instructions provided. The sample is also hosted at [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/). The instructions that follow will use the images hosted in that repo.



## Getting the sample

These instructions are based on the [ASP.NET Core Docker Sample](README.md). You can build the sample based on the instructions provided. The sample is also hosted at [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/). The instructions that follow will use the images hosted in that repo.
