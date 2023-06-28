ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23327.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='033762fcec5b4a4ee4fa1850c5056fa026d4ac31321e30457b755a9aded3d10c179ec7875656115d8c853d4e6740d8efc23391a4fb18f3c3fb1411457bdb27d9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23327.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
