ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM azurelinuxpreview.azurecr.io/public/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.6.24277.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='7329c20b14ff676bf51783afa8a74706e01b1c87cebe5eea2ba01958960505fbe196b1fc7c53c1aec2cb4d61f4316c4d38a43850c76a42cb77f983c797149dfa' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.5-azurelinux3.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.6.24277.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
