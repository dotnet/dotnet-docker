# Hosting ASP.NET Core Images with Docker over HTTPS

ASP.NET Core uses [HTTPS by
default](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl).
[HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on
[certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust,
identity, and encryption.

Documentation for [HTTPS](https://en.wikipedia.org/wiki/HTTPS) scenarios are now located at:
[Hosting ASP.NET Core images with Docker over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https).

## Getting certificates into your container

The above instructions [bind mount](https://docs.docker.com/engine/storage/bind-mounts/)
certificates into containers. While you could also add your certificates into the
image with a `COPY` command in a Dockerfile, we don't recommend that approach.
It makes it harder to use the same image for testing with dev certificates and
hosting with production certificates. There is also a risk of certificate
disclosure if certificates are made part of your container image.
