ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.4.24223.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='bc129682e5ce9a83aead21d8342bfe7a495d8e5079b12d33606a8c7cfcf3ad32b3a702e2cda4e4cff23e7b1297288021f35269d5df048cb6760fc5d6698d4bc2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-bookworm-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24223.1

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
