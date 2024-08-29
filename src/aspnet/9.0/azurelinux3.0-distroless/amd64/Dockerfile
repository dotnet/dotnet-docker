ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.1.24416.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='66046f03b22746c93e6ed88df83f4d5eb28166f8cfb70932279e65f816478200c86c2dde2ede784975309a16828b4eda14f6d4a7fdca8c79d37ec13a56b4f59d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.1-azurelinux3.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.1.24416.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
