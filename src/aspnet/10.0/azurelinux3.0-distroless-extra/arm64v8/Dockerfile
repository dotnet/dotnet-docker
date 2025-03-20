ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.3.25169.11 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='0c4a8d28f32f8e90bf41c11f94d642aa3e45d076fc4d09b35732cc13d0edc8a4f22b3d13d7f7c8eaa5a9bc09e3a301ddc434cc536298a246af61ef0914897fc4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-azurelinux3.0-distroless-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.3.25169.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
