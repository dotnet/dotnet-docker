ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25358.103 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512 \
    && sha512sum -c aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512 \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-azurelinux3.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25358.103

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
