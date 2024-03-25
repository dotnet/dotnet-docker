ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.4.24173.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='15dc9fd68c71237e220ef771c006cdca545b9853d354843db92b4fceabbbcaab5e5ce6ff78b21e8c1dbf00f79baaeb486a672f9d089d75b09782daa65a536a85' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-cbl-mariner2.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24173.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
