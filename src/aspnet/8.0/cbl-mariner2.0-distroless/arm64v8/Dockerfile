ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-rc.2.23468.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='8fe54d9e2e50d1ee19a6ebda331bd2f28b12630a6b6d91cbc0394c80412a78d0c83da3597830737a82906cbd0b38dacc54213fca8d21f02182b3ceb7f5d12f86' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-rc.2-cbl-mariner2.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rc.2.23468.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
