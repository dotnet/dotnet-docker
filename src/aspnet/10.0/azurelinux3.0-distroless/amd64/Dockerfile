ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25224.106 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='f1481fb4779a377213bdcc4eae31f33adc974827f196a1a5b0eefe28a0dd5511bebd4df11627959aed887d4c7a720acca1b7b3a65c9abb7e402eb300f4442ed0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-azurelinux3.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25224.106

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
