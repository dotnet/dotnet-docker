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
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='8f11518d84f00f258b2ae54091307799213952b8b1ec7d25a2194d8e29e5f9546ecc1007fd233912657eefc07b3fb03584c873144efbe99203fd4439aeccb130' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.5-azurelinux3.0-distroless-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.6.24277.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
