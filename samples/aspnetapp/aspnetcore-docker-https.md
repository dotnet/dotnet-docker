# Hosting ASP.NET Core Images with Docker over HTTPS

ASP.NET Core 2.1 uses [HTTPS by default](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl). [HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on [certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust, identity and encryption.

This document explains how to run pre-built container images with HTTPS. [Developing ASP.NET Core Applications with Docker over HTTPS](aspnetcore-docker-https-development.md) explains how to develop applications in Docker with HTTPS.

## Getting a Certificate

For [production hosting](https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/), you need a certificate from a [certificate authority](https://en.wikipedia.org/wiki/Certificate_authority) for your domain. You may already have one. [Let's Encrypt](https://letsencrypt.org/) is a certificate authority that offers free certificates.

This document uses development certificates for hosting pre-built images over `localhost`. The instructions are the same as using production certificates. This approach is also required to enable instructions that everyone can try without needing to use actual production certificates.

You need to generate a development certificate in order to run a pre-built image with HTTPS. The required certificate does not come with the image. Including a certification within a container image is considered an anti-pattern in most cases.

You need the .NET Core 2.1 SDK in order to generate and trust a deveopment certificate on your machine. You can do that with the following instructions:

**On Windows**

```console
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p crypticpassword
dotnet dev-certs https --trust
```

**On macOS or Linux**

```console
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p crypticpassword
dotnet dev-certs https --trust
```

> Note: `crypticpassword` is used as a stand-in for a password of your own choosing.
