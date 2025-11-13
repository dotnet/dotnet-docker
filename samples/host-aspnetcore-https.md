# Hosting ASP.NET Core Images with Docker over HTTPS

ASP.NET Core uses [HTTPS by default](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl). [HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on [certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust, identity, and encryption.

See [Hosting ASP.NET Core images with Docker over HTTPS](https://learn.microsoft.com/aspnet/core/security/docker-https) for [HTTPS](https://en.wikipedia.org/wiki/HTTPS) scenarios

## Get certificates into a container

The instructions show how to [bind-mount](https://docs.docker.com/engine/storage/bind-mounts/) certificates into containers. We recommend certificates ***NOT*** be added into the image with a `COPY` command in a Dockerfile for the following reasons:

* It makes it harder to use the same image for testing with dev certificates and hosting with production certificates.
* It increases the risk of certificate disclosure if certificates are made part of your container image.
