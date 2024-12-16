# Hosting ASP.NET Core Images with Docker over HTTPS

ASP.NET Core uses [HTTPS by
default](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl).
[HTTPS](https://en.wikipedia.org/wiki/HTTPS) relies on
[certificates](https://en.wikipedia.org/wiki/Public_key_certificate) for trust,
identity, and encryption.

This document explains how to run pre-built container images with HTTPS.

## Getting the sample image

Pull the sample image:

```console
docker pull mcr.microsoft.com/dotnet/samples:aspnetapp
```

Alternatively, you can build the sample image locally:

```console
docker build --pull -t mcr.microsoft.com/dotnet/samples:aspnetapp 'https://github.com/dotnet/dotnet-docker.git#:samples/aspnetapp'
```


## Certificates

You need a certificate from a [certificate authority](https://en.wikipedia.org/wiki/Certificate_authority)
for [production hosting](https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/)
for your domain. You may already have one. [Let's Encrypt](https://letsencrypt.org/)
is a certificate authority that offers free certificates.

### Using HTTPS with certificates during development

You can set up development certificates to run your app locally with HTTPS. See
[create and trust a development certificate](./run-aspnetcore-https-development.md#create-and-trust-a-development-certificate)
for guidance.

## Running container images with HTTPS

The instructions [bind mount](https://docs.docker.com/engine/storage/bind-mounts/)
certificates into containers. While you could add certificates into container
images with a `COPY` command in a Dockerfile, we don't recommend that approach.
It makes it harder to use the same image for testing with dev certificates and
hosting with production certificates. There is also a risk of certificate
disclosure if certificates are made part of container images.

### Enable HTTPS using environment variables

If you want to put your certificate in a different location, or if you want to
use a certificate name that doesn't match the assembly name of your app, set
the `ASPNETCORE_Kestrel__Certificates__Default__Path` variable to your
certificate's path in the container image. Be sure to also set the
`ASPNETCORE_ENVIRONMENT` environment variable to `"Development"` if you're
developing and running your container locally.

For Linux containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}/.aspnet/https/:/https/ `
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx `
    # Replace the placeholder with the same password that you used when exporting the certificate
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="<CREDENTIAL_PLACEHOLDER>" `
    # Uncomment if you are running in Development instead of production
    # -e ASPNETCORE_ENVIRONMENT=Development `
    mcr.microsoft.com/dotnet/samples:aspnetapp
```

Linux containers on macOS or Linux:

```bash
docker run --rm -it \
    -p 8001:8001 \
    -e ASPNETCORE_HTTPS_PORTS=8001 \
    # Replace the placeholder with the same password that you used when exporting the certificate
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="<CREDENTIAL_PLACEHOLDER>" \
    # Bind mount the location of your exported certificate
    -v ${HOME}/.aspnet/https/:/home/app/.aspnet/https/ \
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx \
    # Uncomment if you are running in Development instead of production
    # -e ASPNETCORE_ENVIRONMENT=Development \
    mcr.microsoft.com/dotnet/samples:aspnetapp
```

Windows containers on Windows:

```pwsh
docker run --rm -it `
    -p 8001:8001 `
    -e ASPNETCORE_HTTPS_PORTS=8001 `
    # Replace the placeholder with the same password that you used when exporting the certificate
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="<CREDENTIAL_PLACEHOLDER>" `
    # Bind mount the location of your exported certificate
    -v ${env:USERPROFILE}\.aspnet\https:C:\https `
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=\https\aspnetapp.pfx `
    # Uncomment if you are running in Development instead of production
    # -e ASPNETCORE_ENVIRONMENT=Development `
    --user ContainerAdministrator `
    mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-ltsc2022
```

> [!NOTE]
> The password must match the password used for the certificate. Running as ContainerAdministrator is not recommended in production scenarios. The `--user ContainerAdministrator` flag has been added in this example since there is a potential and intermittent race condition that throws a `WindowsCryptographicException` at the container/app startup when using self-signed certificates. This is a known bug and it has been [reported here](https://github.com/dotnet/runtime/issues/70386).

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

- Check that the certificate you're using is trusted on the host.
- Check that the certificate you're using is not expired.
- Restart your browser (browsers can cache certificate information).
- If you're looking to test https with a domain name (e.g.
`https://contoso.com:8001`), the certificate would also need the appropiate
Subject Alternative Name included, and the DNS settings on the host would need
to be updated. In the case of using the generated dev certificate, the trusted
certificate will be issued from localhost and will not have the SAN added.
