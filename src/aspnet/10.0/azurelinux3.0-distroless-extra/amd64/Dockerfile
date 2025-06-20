ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && curl --fail --show-error --location --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='6e69a85f7e18b8eebb5f99a7e8099db2fa5da34bcf078becbb123c0863d4be7b4252c7cfc6b21b9585f4f800c058a12cae55ef2a63b9bea886ca3d1d8a0ec113' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-distroless-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
