ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-rtm.23468.24 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='6510dffb6d1995992e2590e45d3e024093296d3529eac3c297861cd5f592696c548a408b9b2a775466ed132c9f418b9d41e7356d2e297380e1377f85f06b2bc2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-rc.2-cbl-mariner2.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rtm.23468.24

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
